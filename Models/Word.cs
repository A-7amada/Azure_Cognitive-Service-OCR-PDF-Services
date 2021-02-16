using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltairTask.Models
{
    /// <summary>
    /// one of Json Deserialize object Model component
    /// </summary>
    public class Word
    {
        public List<double> boundingBox { get; set; }
        public string text { get; set; }
        public string confidence { get; set; }
    }
}
