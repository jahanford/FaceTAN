namespace FaceTAN.Core.Data.Models.Animetrics
{
    public class AnimetricsRecognizeImage
    {
        public double time { get; set; }
        public AnimetricsRecognizeTransaction transaction { get; set; }
        public AnimetricsRecognizeCandidates candidates { get; set; }
    }
}
