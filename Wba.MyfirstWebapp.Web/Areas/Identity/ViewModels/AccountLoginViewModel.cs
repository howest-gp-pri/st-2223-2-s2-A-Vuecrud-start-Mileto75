using System.ComponentModel.DataAnnotations;

namespace Pri.Ca.Web.Areas.Identity.ViewModels
{
    public class AccountLoginViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
