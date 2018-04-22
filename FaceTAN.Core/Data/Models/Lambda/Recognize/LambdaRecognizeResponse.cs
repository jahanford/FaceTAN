using System.Collections.Generic;

namespace FaceTAN.Core.Data.Models.Lambda
{
    public class LambdaRecognizeResponse
    {
        public string status { get; set; }
        public List<LambdaRecognizePhotos> photos { get; set; }
    }
}
