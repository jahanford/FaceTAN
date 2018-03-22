using System.Collections.Generic;

namespace FaceTAN.Core.Data.Models.Animetrics
{
    public class AnimetricsDetectImage
    {
        public long time { get; set; }
        public string status { get; set; }
        public string file { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string image_id { get; set; }
        public string image_expiration { get; set; }
        public string setpose_image { get; set; }
        public List<AnimetricsDetectFace> faces { get; set; }
    }
}
