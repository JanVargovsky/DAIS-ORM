using DAIS.App.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DAIS.App.MVC.Controllers
{
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("User/Change")]
        public ActionResult UserChange()
        {
            var changeUserModel = new ChangeUserModel
            {
                Users = new List<SelectListItem>(userRepository.Select().Select(u => new SelectListItem
                {
                    Text = $"{u.FirstName} {u.Surname}",
                    Value = u.Id.ToString(),
                    Selected = u.Id == SelectedUser?.Id
                })),
                CurrentUserId = SelectedUser?.Id
            };

            return View("UsersList", changeUserModel);
        }

        [HttpPost]
        [Route("User/Change")]
        [ValidateAntiForgeryToken]
        public ActionResult UserChange(FormCollection form)
        {
            try
            {
                long userId = long.Parse(form["UserId"]);
                var user = userRepository.Select(userId);
                SelectedUser = new UserModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.Surname,
                };
                ViewBag.Alerts = new AlertsListModel(new AlertModel { AlertType = AlertType.Success, Text = "User selected." });

                return UserChange();
            }
            catch (Exception ex)
            {
                SelectedUser = null;
                ViewBag.Alerts = new AlertsListModel(AlertModelFactory.Instance.Create(AlertCode.InvalidFormData));
                return UserChange();
            }
        }
    }
}