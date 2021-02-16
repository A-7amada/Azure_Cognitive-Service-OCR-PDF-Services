using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AltairTask.Models
{
    /// <summary>
    /// Json Deserialize object Model
    /// </summary>
    public class Root
    {
        public string status { get; set; }
        public List<RecognitionResult> recognitionResults { get; set; }
    }
}
