using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;
using System.Web.Mvc;

namespace Dto
{
        public class LocalPasswordModel
        {
            [Required]
            [StringLength(100, ErrorMessage = "{0} en az {2} karakter uzunluğunda olabilir.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Yeni şifre")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Yeni şifreyi doğrula")]
            [Compare("Password", ErrorMessage = "Şifreler birbirleriyle örtüşmüyor.")]
            public string ConfirmPassword { get; set; }
        }
}
