

using System.ComponentModel.DataAnnotations;

namespace Pri.Ca.Api.DTOs.Games
{
    public class GameUpdateRequestDto : GameRequestDto
    {
        [Required(ErrorMessage = "Id required!")]
        public int Id { get; set; }
    }
}
