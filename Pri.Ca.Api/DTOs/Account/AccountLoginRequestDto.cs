using System.ComponentModel.DataAnnotations;

namespace Pri.Ca.Api.DTOs.Account
{
    public class AccountLoginRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
