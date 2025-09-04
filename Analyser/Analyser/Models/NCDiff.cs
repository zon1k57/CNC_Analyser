using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCFileCompare.Models
{
    public class NCDiff
    {
        public Dictionary<string, double> Values { get; set; }
        public Dictionary<string, object> ExternalValues { get; set; }
        public List<string> Commands;
        public bool InLayup { get; set; }

        public NCDiff()
        {
            Values = new Dictionary<string, double>();
            ExternalValues = new Dictionary<string, object>();
            Commands = new List<string>();
        }
    }
}
