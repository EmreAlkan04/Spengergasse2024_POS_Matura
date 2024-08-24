using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPG_Fachtheorie.Aufgabe2.Domain
{
    public class Playlist
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string UserName { get; set; } = String.Empty;

        public List<ListenedItem> ListenedItems { get; set; } = new();
    }
}
