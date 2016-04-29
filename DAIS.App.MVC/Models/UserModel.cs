using System.Collections.Generic;
using System.Web.Mvc;

namespace DAIS.App.MVC.Models
{
    public class UserModel
    {
        public long Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }

    public class ChangeUserModel
    {
        public long? CurrentUserId { get; set; }

        public long? UserId { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }
    }
}