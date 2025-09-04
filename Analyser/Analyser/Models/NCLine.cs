// NCLine.cs
using NCFileCompare.Models;
using NCFileCompare.Models.Interface;
using System.Collections.Generic;

namespace NCFileCompare.Models
{
    public class NCLine : INCLine
    {
        public int? SeqNo { get; set; }
        public string RawLine { get; set; }
        public List<string> Command { get; set; }
        public string Comment { get; set; }

        public NCVariables Variables { get; set; }

        public bool IsMove { get { return Command != null && Command.Exists(c => c.StartsWith("G")); } }
        public bool IsLayup { get; set; }
        public bool IsMCommand { get { return Command != null && Command.Exists(c => c.StartsWith("M")); } }
    }
}