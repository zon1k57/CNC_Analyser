using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace NCFileCompare.Models
{
    public class NCVisualization_Move
    {
        public Dictionary<string, double> StartPosition { get; }
        public Dictionary<string, double> EndPosition { get; }
        public string GCommand { get; }
        public double FeedRate { get; }

        public NCVisualization_Move(
            Dictionary<string, double> startPosition,
            Dictionary<string, double> endPosition,
            string gCommand,
            double feedRate)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
            GCommand = gCommand;
            FeedRate = feedRate;
        }
    }
}
