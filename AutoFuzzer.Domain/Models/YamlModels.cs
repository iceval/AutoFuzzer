using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoFuzzer.Domain.Models
{
    public class YamlRoot
    {
        public Dictionary<string, YamlContainer> Services { get; set; }
    }

    public class YamlContainer
    {
        public string Build { get; set; }
        public List<string> Environment { get; set; }
    }
}
