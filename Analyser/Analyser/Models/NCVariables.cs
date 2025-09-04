using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCFileCompare.Models
{
    public class NCVariables
    {
        // Оски
        public Dictionary<string, double> Axes { get; set; }
        // Параметри P#
        public Dictionary<string, double> Parameters { get; set; }
        // V.E.* вредности
        public Dictionary<string, object> ExternalValues { get; set; }
        // Feedrate вредности
        public Dictionary<string, double> FeedRate { get; set; }

        public NCVariables(Dictionary<string,double> axes, Dictionary<string,double> parameters, Dictionary<string,object> externalValues, Dictionary<string,double> feedRate)
        {
            Axes = axes;
            Parameters = parameters;
            ExternalValues = externalValues;
            FeedRate = feedRate;
        }
    }
}
