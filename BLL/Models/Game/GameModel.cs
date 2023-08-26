using BLL.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models.Game
{
    public class GameModel : IMapFrom<DAL.Models.Game>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string Keyword { get; set; } = string.Empty;
    }
}
