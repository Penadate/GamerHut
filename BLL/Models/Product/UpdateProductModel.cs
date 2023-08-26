using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.Product
{
    public class UpdateProductModel
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public string Nickname { get; set; }
        public string Contact { get; set; }
        public string Password { get; set; } = string.Empty!;
    }
}
