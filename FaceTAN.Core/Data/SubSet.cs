using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceTAN.Core.Data
{
    public class SubSet
    {
        public SubSet(List<string> targets, List<string> sources)
        {
            SubSetId = new Guid();
            TargetImages = targets;
            SourceImages = sources;
        }

        public Guid SubSetId { get; }
        public List<string> TargetImages { get; }
        public List<string> SourceImages { get; }
    }
}
