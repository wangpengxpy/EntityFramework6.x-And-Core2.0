using EntityFrameworkBaseExample.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EntityFrameworkBaseExample.Controllers
{
    public class BlogController : Controller
    {
        /// <summary>
        /// 获取博客列表
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var blogs = new List<Blog>();
            using (var _context = new EFDbContext())
            {
                blogs = await _context.Blogs
                    .AsNoTracking()
                    .ToListAsync();
            };
            return View(blogs);
        }

        /// <summary>
        /// 博客站内搜索
        /// </summary>
        /// <param name="Owner"></param>
        /// <returns></returns>
        public ActionResult Search(string Owner)
        {
            var blogs = new List<Blog>();
            using (var _context = new EFDbContext())
            {
                blogs = _context.Blogs.ToList();
                blogs.All(b =>
                    {
                        b.Owner = Transfer(b._Owner);
                        return true;
                    });

            };
            if (!string.IsNullOrEmpty(Owner))
            {
                blogs = blogs
               .Where(d => d.Owner.Name == Owner || d.Owner.EnglishName == Owner)
               .ToList();
            }
            return View("Index", blogs);
        }

        Person Transfer(string p)
        {
            return JsonConvert.DeserializeObject<Person>(p);
        }

        public ActionResult UpInsert()
        {
            return View();
        }

        /// <summary>
        /// 获取单个博客实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Get(int? id)
        {
            if (ReferenceEquals(id, null) || id.Value <= 0)
            {
                return Content("<script  type='text/javascript'>alert('提交参数不正确!');location.href='/';</script>");
            }
            using (var _context = new EFDbContext())
            {
                var blog = await _context.Blogs.FindAsync(id);
                if (ReferenceEquals(blog, null))
                {
                    return Content("<script  type='text/javascript'>alert('该博客不存在!');location.href='/';</script>");
                }
                return View("UpInsert", blog);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Delete(int? id)
        {
            if (ReferenceEquals(id, null) || id.Value <= 0)
            {
                return Content("<script  type='text/javascript'>alert('提交参数不正确!');location.href='/';</script>");
            }
            using (var _context = new EFDbContext())
            {
                var dbBlog = await _context.Blogs.FindAsync(id);
                if (ReferenceEquals(dbBlog, null))
                {
                    return Content("<script  type='text/javascript'>alert('该博客不存在!');location.href='/';</script>");
                }
                else
                {
                    _context.Blogs.Remove(dbBlog);
                    var result = _context.SaveChanges();
                    if (result > 0)
                    {
                        return View("Index");
                    }
                    return Content("<script  type='text/javascript'>alert('删除失败!');location.href='/';</script>");
                }
            }
        }

        /// <summary>
        /// 添加或更新博客
        /// </summary>
        /// <param name="blog"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpInsert(Blog blog)
        {
            if (ModelState.IsValid)
            {
                using (var _context = new EFDbContext())
                {
                    if (blog.Id <= 0)
                    {
                        _context.Blogs.Add(blog);
                        blog.CreatedTime = DateTime.Now;
                        blog.ModifiedTime = DateTime.Now;
                    }
                    else
                    {
                        var dbBlog = await _context.Blogs.FindAsync(blog.Id);
                        if (ReferenceEquals(blog, null))
                        {
                            return Content("<script  type='text/javascript'>alert('提交参数不正确!');location.href='/';</script>");
                        }
                        else
                        {
                            dbBlog.Owner = blog.Owner;
                            dbBlog.Tags = blog.Tags;
                            dbBlog.Url = blog.Url;
                            dbBlog.ModifiedTime = DateTime.Now;
                        }
                    }
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                };
            }
            return View();
        }
    }
}