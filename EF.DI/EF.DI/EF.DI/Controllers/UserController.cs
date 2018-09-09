using AutoMapper;
using AutoMapper.QueryableExtensions;
using EF.Core.Data;
using EF.DI.Models;
using EF.Service;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Ioc.Web.Controllers
{
    public class UserController : Controller
    {

        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var users = _userService
                .GetUsers()
                .ProjectTo<UserDTO>()
                .ToList();
            return View(users);
        }

        [HttpGet]
        public ActionResult CreateEditUser(int? id)
        {
            var model = new UserDTO();
            if (id.HasValue && id.Value > 0)
            {
                User userEntity = _userService.GetUser(id.Value);
                model = Mapper.Map<User, UserDTO>(userEntity);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateEditUser(UserDTO model)
        {
            if (model.ID <= 0)
            {
                var userEntity = Mapper.Map<UserDTO, User>(model);
                userEntity.IP = Request.UserHostAddress;
                userEntity.CreatedTime = DateTime.Now;
                userEntity.ModifiedTime = DateTime.Now;
                userEntity.UserProfile.CreatedTime = DateTime.Now;
                userEntity.UserProfile.ModifiedTime = DateTime.Now;
                _userService.InsertUser(userEntity);
                if (userEntity.ID > 0)
                {
                    return RedirectToAction("index");
                }
            }
            else
            {
                User userEntity = _userService.GetUser(model.ID);
                userEntity.UserName = model.UserName;
                userEntity.Email = model.Email;
                userEntity.Password = model.Password;
                userEntity.ModifiedTime = DateTime.UtcNow;
                userEntity.IP = Request.UserHostAddress;
                userEntity.UserProfile.FirstName = model.FirstName;
                userEntity.UserProfile.LastName = model.LastName;
                userEntity.UserProfile.Address = model.Address;
                userEntity.UserProfile.ModifiedTime = DateTime.UtcNow;
                userEntity.UserProfile.IP = Request.UserHostAddress;
                _userService.UpdateUser(userEntity);
                if (userEntity.ID > 0)
                {
                    return RedirectToAction("index");
                }

            }
            return View(model);
        }

        public ActionResult DetailUser(int? id)
        {
            var model = new UserDTO();
            if (id.HasValue && id > 0)
            {
                var userEntity = _userService.GetUser(id.Value);
                model.ID = userEntity.ID;
                model.FirstName = userEntity.UserProfile.FirstName;
                model.LastName = userEntity.UserProfile.LastName;
                model.Address = userEntity.UserProfile.Address;
                model.Email = userEntity.Email;
                model.CreatedTime = userEntity.CreatedTime;
                model.UserName = userEntity.UserName;
            }
            return View(model);
        }

        public ActionResult DeleteUser(int? id)
        {
            var model = new UserDTO();
            if (id.HasValue && id.Value > 0)
            {
                User userEntity = _userService.GetUser(id.Value);
                model.FirstName = userEntity.UserProfile.FirstName;
                model.LastName = userEntity.UserProfile.LastName;
                model.Address = userEntity.UserProfile.Address;
                model.Email = userEntity.Email;
                model.CreatedTime = userEntity.CreatedTime;
                model.UserName = userEntity.UserName;
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult DeleteUser(int? id, FormCollection collection)
        {
            try
            {
                if (id.HasValue && id.Value > 0)
                {
                    User userEntity = _userService.GetUser(id.Value);
                    _userService.DeleteUser(userEntity);
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
