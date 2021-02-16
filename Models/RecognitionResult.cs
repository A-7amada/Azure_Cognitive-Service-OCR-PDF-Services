using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltairTask.Models
{
    /// <summary>
    /// one of Json Deserialize object Model component
    /// </summary>
    public class RecognitionResult
    {
        public int page { get; set; }
        public double clockwiseOrientation { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public string unit { get; set; }
        public List<Line> lines { get; set; }
    }
}
