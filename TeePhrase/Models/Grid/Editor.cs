using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeePhrase.Models.Grid
{
    public class Editor
    {
        public string Alias { get; set; }

        public string Icon { get; set; }

        public string Name { get; set; }

        public string View { get; set; }

        public Config Config { get; set; }
    }
}
