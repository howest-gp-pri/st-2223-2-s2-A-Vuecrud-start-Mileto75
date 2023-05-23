namespace Pri.Ca.Api.DTOs.Games
{
    public class GameResponseDto : BaseResponseDto
    {
        public IEnumerable<BaseResponseDto> Categories { get; set; }
        public string Image { get; set; }
    }
}
