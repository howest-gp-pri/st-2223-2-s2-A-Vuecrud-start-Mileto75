using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.Ca.Core.Entities
{
    public class Rating : BaseEntity
    {
        public int Score { get; set; }
        public Game Game { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
