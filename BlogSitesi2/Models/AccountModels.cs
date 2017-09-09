using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;
using System.Web.Mvc;

namespace BlogSitesi2.Models
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }

    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class RegisterExternalLoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        public string ExternalLoginData { get; set; }
    }

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

    public partial class LoginModel
    {
        [Required]
        [Display(Name = "Kullanıcı adı")]
        public virtual string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public virtual string Password { get; set; }

        [Display(Name = "Beni hatırla")]
        public virtual bool RememberMe { get; set; }

        public virtual Int32 Id { get; set; }
        public virtual string Mail { get; set; }
        public virtual string Date { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "Kullanıcı adı")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} en az {2} karakter uzunluğunda olabilir.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifreyi doğrula")]
        [Compare("Password", ErrorMessage = "Şifreler birbirleriyle örtüşmüyor.")]
        public string ConfirmPassword { get; set; }
        
        [Required]
        [Display(Name = "Mail adresiniz")]
        public virtual string Mail { get; set; }
        public bool captchaValid { get; set; }
    }

    public class ExternalLogin
    {
        public string Provider { get; set; }
        public string ProviderDisplayName { get; set; }
        public string ProviderUserId { get; set; }
    }
}
