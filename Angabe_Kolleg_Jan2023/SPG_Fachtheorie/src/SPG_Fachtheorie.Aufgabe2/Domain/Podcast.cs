using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPG_Fachtheorie.Aufgabe2.Domain
{
    public class Podcast : Item
    {
        public List<int> PositionForAd { get; set; } = new();
        public int MaxQuantityAds { get; set; }
    }
}
