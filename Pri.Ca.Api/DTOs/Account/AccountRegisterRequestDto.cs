using System.ComponentModel.DataAnnotations;

namespace Pri.Ca.Api.DTOs.Account
{
    public class AccountRegisterRequestDto : AccountLoginRequestDto
    {
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Compare("Password")]
        [Required]
        public string CheckPassword { get; set; }
    }
}
