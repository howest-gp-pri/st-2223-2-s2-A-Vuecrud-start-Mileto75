

using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Pri.Ca.Api.DTOs.Games
{
    public class GameRequestDto
    { 
        [Required(ErrorMessage = "Please provide a name!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please provide categories")]
        public IEnumerable<int> Categories { get; set;}
        public IFormFile Image { get; set; }
    }
}
