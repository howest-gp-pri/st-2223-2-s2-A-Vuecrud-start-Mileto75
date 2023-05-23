using Pri.Ca.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.Ca.Core.Services.Models
{
    public class ItemResultModel<T>
    {
        public IEnumerable<T> Items { get; set; }
        public IEnumerable<ValidationResult> ValidationErrors { get; set; }
        public bool IsSuccess { get; set; }
    }
}
