using AutoMapper;
using AutoMapper.QueryableExtensions;
using EF.Core.Data;
using EF.Core.Exceptions;
using EF.Core.Helper;
using EF.Data;
using EFUnitOfWork.Models;
using EFUnitOfWork.UploadUtil;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace EF.Web.Controllers
{
    public class BookController : Controller
    {
        private static object _lock = new object();

        private UnitOfWork unitOfWork = new UnitOfWork();
        private Repository<Book> bookRepository;

        public BookController()
        {
            bookRepository = unitOfWork.Repository<Book>();
        }

        /// <summary>
        /// 获取书籍列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var books = bookRepository.Table
                .ProjectTo<BookDTO>().ToList();
            return View(books);
        }

        /// <summary>
        /// 获取编辑书籍信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CreateEditBook(int? id)
        {
            var bookDTO = new BookDTO();
            if (id.HasValue)
            {
                var entity = bookRepository.GetById(id.Value);
                bookDTO = Mapper.Map<Book, BookDTO>(entity);
            }
            return View(bookDTO);
        }

        /// <summary>
        /// 添加或编辑书籍信息
        /// </summary>
        /// <param name="bookDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateEditBook(BookDTO bookDTO)
        {
            if (bookDTO.ID == 0)
            {
                var model = Mapper.Map<BookDTO, Book>(bookDTO);

                model.ModifiedTime = DateTime.Now;
                model.CreatedTime = DateTime.Now;
                model.Url = string.Empty;
                model.IP = Request.UserHostAddress;

                bookRepository.Insert(model);
                unitOfWork.Commit();
            }
            else
            {
                var editModel = bookRepository.GetById(bookDTO.ID);

                bookDTO.ID = editModel.ID;
                editModel.Author = bookDTO.Author;
                editModel.ISBN = bookDTO.ISBN;
                editModel.Title = bookDTO.Title;
                editModel.Published = bookDTO.Published;
                editModel.ModifiedTime = DateTime.Now;
                editModel.IP = Request.UserHostAddress;
                bookRepository.Update(editModel);
                unitOfWork.Commit();
            }

            if (bookDTO.ID > 0)
            {
                return View("Upload", bookDTO);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 上传书籍
        /// </summary>
        /// <returns></returns>

        [HttpPost, ActionName("Upload")]
        public ActionResult Upload(Int64 id)
        {
            if (Request.Files.Count <= 0)
            {
                return Json(new { status = false, msg = "请选择要上传的书籍" });
            }

            HandleUploadFiles(Request.Files, id);

            return Json(new { status = true });
        }

        public void HandleUploadFiles(HttpFileCollectionBase files, Int64 id)
        {
            foreach (string file in Request.Files)
            {
                var fileDataContent = Request.Files[file];

                var stream = fileDataContent.InputStream;

                var fileName = Path.GetFileName(fileDataContent.FileName);

                var uploadPath = Server.MapPath("~/App_Data/uploads");

                if (!FileHelper.ExistDirectory(uploadPath))
                {
                    FileHelper.CreateDirectory(uploadPath);
                }

                var path = Path.Combine(uploadPath, fileName);

                //使用瞬态故障处理库Polly处理异常，采用等待重试策略
                PollyHelper.WaitAndRetry<IOException>(() =>
                {
                    if (FileHelper.Exist(path))
                    {
                        FileHelper.Delete(path);
                    }

                    using (var fileStream = System.IO.File.Create(path))
                    {
                        stream.CopyTo(fileStream);
                    }

                    // 当上传中断，已上传部分是否能合并？（待优化）
                    var ut = new Utils();
                    var storeFileName = string.Empty;

                    var result = false;
                    //Merge file
                    ut.MergeFile(path, out result, out storeFileName);

                    if (result)
                    {
                        var model = bookRepository.GetById(id);
                        model.Url = storeFileName;
                        bookRepository.Update(model);
                        unitOfWork.Commit();
                    }
                });
            }
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet, ActionName("Download")]
        public ActionResult Download(Int64? id)
        {
            if (!id.HasValue) { return View("Index"); }
            var book = bookRepository.GetById(id.Value);
            if (ReferenceEquals(book, null))
            {
                return RedirectToAction("Index");
            }
            var fileName = book.Url;
            if (string.IsNullOrEmpty(fileName))
            {
                return RedirectToAction("Index");
            }
            var uploadPath = Server.MapPath("~/App_Data/uploads");
            var fullPath = uploadPath + Path.DirectorySeparatorChar + fileName;
            if (!FileHelper.Exist(fullPath))
            {
                return Content("<script type='text/javaScript'>alert('未上传或已删除');location.href='/';</script>");
            }
            return File(new FileStream(fullPath, FileMode.Open, FileAccess.Read), "text/plain", fileName);
        }

        /// <summary>
        /// 获取删除书籍信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DeleteBook(int id)
        {
            var entity = bookRepository.GetById(id);
            if (ReferenceEquals(entity, null))
            {
                return RedirectToAction("Index");
            }

            var model = Mapper.Map<Book, BookDTO>(entity);

            return View(model);
        }

        /// <summary>
        /// 删除书籍
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ActionName("DeleteBook")]
        public ActionResult ConfirmDeleteBook(int id)
        {
            var model = bookRepository.GetById(id);
            bookRepository.Delete(model);
            unitOfWork.Commit();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 书籍概述
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DetailBook(int id)
        {
            var model = bookRepository.GetById(id);

            var bookDTO = Mapper.Map<Book, BookDTO>(model);

            return View(bookDTO);
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
