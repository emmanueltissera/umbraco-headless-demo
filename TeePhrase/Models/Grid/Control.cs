using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeePhrase.Models.Grid
{
    public class Control
    {
        public bool Active { get; set; }

        public string Value { get; set; }

        public Editor Editor { get; set; }
    }
}
