using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobSysFinalsBase1.Models
{

    public class Recipe
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public int PrepTimeMinutes { get; set; }

        public List<string> Ingredients { get; set; } = new();
        public List<string> Instructions { get; set; } = new();
    }


}
