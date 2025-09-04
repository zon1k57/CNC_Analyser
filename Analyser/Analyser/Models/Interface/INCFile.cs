using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCFileCompare.Models.Interface
{
    public interface INCFile
    {
        string Filename { get; set; }
        void Parse();
    }
}
