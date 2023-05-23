namespace Wba.MyfirstWebapp.Web.ViewModels
{
    public class BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
