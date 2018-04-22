using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace FaceTAN.Core.Data.Models.Lambda
{
    public class LambdaRecognizeTags
    {
        public double confidence { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public int tid { get; set; }
        public CoordinateTag eye_left { get; set; }
        public CoordinateTag eye_right { get; set; }
        public CoordinateTag center { get; set; }
        public CoordinateTag mouth_left { get; set; }
        public CoordinateTag mouth_center { get; set; }
        public CoordinateTag mouth_right { get; set; }
        public CoordinateTag nose { get; set; }
        public List<JObject> attributes { get; set; }
        public List<UidTag> uids { get; set; }
    }
}
