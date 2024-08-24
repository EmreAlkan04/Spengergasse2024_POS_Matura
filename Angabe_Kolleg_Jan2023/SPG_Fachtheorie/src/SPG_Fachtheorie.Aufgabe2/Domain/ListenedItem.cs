using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPG_Fachtheorie.Aufgabe2.Domain
{
    public class ListenedItem
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }

        public int PlaylistId { get; set; }
        public Playlist Playlist { get; set; } = default!;

        public int ItemId { get; set; }
        public Item Item { get; set; } = default!;
    }
}
