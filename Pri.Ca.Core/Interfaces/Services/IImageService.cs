using Microsoft.AspNetCore.Http;
using Pri.Ca.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.Ca.Core.Interfaces.Services
{
    public interface IImageService
    {
        Task<ImageResultModel> StoreImageAsync<T>(IFormFile image);
        ImageResultModel DeleteImage<T>(string fileName);
        Task<ImageResultModel> UpdateImageAsync<T>(IFormFile image, string fileName);
        ImageResultModel GetImagePath<T>(string fileName);

    }
}
