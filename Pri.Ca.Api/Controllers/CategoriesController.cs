using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pri.Ca.Api.DTOs;
using Pri.Ca.Core.Interfaces.Services;

namespace Pri.Ca.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _categoryService.GetAllAsync();
            if (categories.IsSuccess)
            {
            
            }
            return Ok(new CategoryResponseDto
            {
                Categories = categories.Items.Select(
                    c => new BaseResponseDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                    })
            });
        }
    }
}
