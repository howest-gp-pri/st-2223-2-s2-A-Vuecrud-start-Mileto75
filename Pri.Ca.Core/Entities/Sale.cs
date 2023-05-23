using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.Ca.Core.Entities
{
    public class Sale : BaseEntity
    {
        public int GameId { get; set; }
        public Game Game { get; set; }
        public int Quantity { get; set; }
    }
}
