using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerStats
{
    public class QueryParams
    {
        public string fullPath { get; set; }
        public byte maxNumberOfYearsPlayed { get; set; }
        public byte minimumRating { get; set; }
        public string pathToOutputFile { get; set; }
        public ushort filteredYear { get; set; }
    }
}
