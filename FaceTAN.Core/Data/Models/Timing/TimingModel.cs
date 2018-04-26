namespace FaceTAN.Core.Data.Models.Timing
{
    public class TimingModel
    {
        public TimingModel(string method, string identifier, long time)
        {
            Method = method;
            Identifier = identifier;
            Time = time;
        }

        public string Method;
        public string Identifier;
        public long Time;
    }
}
