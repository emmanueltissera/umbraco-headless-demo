using System.Collections.Generic;
using System.Linq;

namespace TeePhrase.Models
{
    public partial class Home
    {
        public List<string> PhraseSamplesList => PhraseSamples.Split('\n').ToList();
    }
}
