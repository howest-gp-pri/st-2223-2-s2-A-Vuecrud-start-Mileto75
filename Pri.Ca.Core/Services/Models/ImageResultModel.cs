using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.Ca.Core.Services.Models
{
    public class ImageResultModel
    {
        public bool IsSuccess { get; set; }
        public ValidationResult Error { get; set; }
        public string Image { get; set; }
    }
}
