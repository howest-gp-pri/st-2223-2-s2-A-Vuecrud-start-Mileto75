using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Pri.Ca.Core.Interfaces.Services;
using Pri.Ca.Core.Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.Ca.Core.Services
{
    //dependencies

    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageService(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor contextAccessor)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = contextAccessor;
        }

        public async Task<ImageResultModel> StoreImageAsync<T>(IFormFile image)
        {
            //create unique filename
            var fileName = $"{Guid.NewGuid()}_{image.FileName}";
            //generate correct directory
            var directory = Path.Combine(
                _webHostEnvironment.WebRootPath, "images",
                typeof(T).Name);
            //check if exists
            if(!Directory.Exists(directory))
            {
                //create directory
                Directory.CreateDirectory(directory);
            }
            //complete path to file
            var pathToFile = Path.Combine(directory, fileName);
            //copy the file
            using (FileStream fileStream = new(pathToFile,FileMode.CreateNew))
            {
                await image.CopyToAsync(fileStream);
            }
            //return filename to store in database
            return new ImageResultModel
            {
                IsSuccess = true,
                Image = fileName,
            };
        }
        public ImageResultModel DeleteImage<T>(string fileName)
        {
            //build the path
            var directoryPath = Path.Combine(_webHostEnvironment
                .WebRootPath,"images",typeof(T).Name);
            
            //}
            var fullPath = Path.Combine(directoryPath, fileName);
            try
            {
                File.Delete(fullPath);
            }
            catch(FileNotFoundException exception)
            {
                return new ImageResultModel
                {
                    Error = new ValidationResult(exception.Message)
                };
            }
            return new ImageResultModel { IsSuccess = true };
        }
    

        public ImageResultModel GetImagePath<T>(string fileName)
        {
            //get scheme
            var scheme = _httpContextAccessor.HttpContext.Request.Scheme;
            //get host
            var host = _httpContextAccessor.HttpContext.Request.Host.Value;
            //build url
            var url = $"{scheme}://{host}/images/{typeof(T).Name}/{fileName}";
            //return
            return new ImageResultModel
            {
                Image = url,
                IsSuccess = true,
            };
        }

        public Task<ImageResultModel> UpdateImageAsync<T>(IFormFile image, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
