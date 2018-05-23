using Amazon.Rekognition.Model;
using FaceTAN.Core.Data.Models.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceTAN.Core.Data
{
    public class ApiResults
    {

        public string IndexedFaces { get; set; }
        public string MatchResults { get; set; }
        public string TimingResults { get; set; }

        public ApiResults(string IndexedString, string MatchedString, string TimingString)
        {
            IndexedFaces = IndexedString;
            MatchResults = MatchedString;
            TimingResults = TimingString;
        }
    }
}
