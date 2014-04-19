using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PQM.Models;

namespace PQM.Models
{
    public class Users
    {
        private project_infoEntities db = new project_infoEntities();

        [Required]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 6)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Remember on this computer")]
        public bool RememberMe { get; set; }

        //Check match user & password in Database
        public bool IsValid(string _username, string _password)
        {
            var q = from m in db.users
                    where m.email.Equals(_username) && m.password.Equals(_password)
                    select m;

            if (q.Count() != 1) return false;
            return true;
        }
    }

    public class ManagerUsers
    {
        private project_infoEntities db = new project_infoEntities();

        [Required]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength=6)]
        [Display(Name="Mật khẩu cũ")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 6)]
        [Display(Name="Mật khẩu mới")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 6)]
        [Display(Name="Xác nhận mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage="Mật khẩu mới không khớp nhau")]
        public string CofirmNewPass { get; set; }

        //Check match user & password in Database
        public bool IsValid(string _username, string _password)
        {
            var q = from m in db.users
                    where m.email.Equals(_username) && m.password.Equals(_password)
                    select m;

            if (q.Count() != 1) return false;
            return true;
        }
    }
}