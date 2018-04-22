using System.Collections.Generic;

namespace FaceTAN.Core.Data.Models.Lambda
{
    public class LambdaRecognizePhotos
    {
        public List<LambdaRecognizeTags> tags { get; set; }
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}
