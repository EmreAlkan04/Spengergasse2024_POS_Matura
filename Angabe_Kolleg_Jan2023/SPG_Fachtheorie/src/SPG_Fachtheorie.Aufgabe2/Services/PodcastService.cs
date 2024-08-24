using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2.Domain;
using SPG_Fachtheorie.Aufgabe2.Infrastructure;

namespace SPG_Fachtheorie.Aufgabe2.Services
{
    public class PodcastService
    {
        public PodcastContext _db;

        public PodcastService(PodcastContext db)
        {
            _db = db;
        }

        public bool CalcTotalCosts(int customerId, DateTime begin, DateTime end)
        {
            var customer = _db.Customers
                .Include(c => c.Advertisements)
                .ThenInclude(a => a.ListenedItems)
                .FirstOrDefault(c => c.Id == customerId);

            if (customer == null)  return false;
            if(customer.TotalCosts != null) return false; //throw new ServiceException("hat value");  
            if (end <= begin) return false;
            if (customer.Advertisements == null) return false;

            var betweenDates = customer.Advertisements.Where(s => s.ListenedItems.All(s => s.Timestamp >= begin && s.Timestamp <= end)).ToList();
            if (betweenDates == null) return false;
            if(begin >= end) return false;

            var bigTotal = betweenDates.Sum(s => s.CostsPerPlay);

            customer.TotalCosts = bigTotal;
            return true;
        }

        public int CalcQuantityAdditionalAds(int playlistId)
        {
            var podcast = _db.Podcasts
                .Include(s => s.ListenedItems)
                .ThenInclude(s => s.Playlist)
                .Where(s => s.ListenedItems.All(s => s.Playlist.Id == playlistId))
                .ToList();

            if (podcast == null) return -2;

            var sumOfMaxAds = podcast.Sum(s => s.MaxQuantityAds);

            var advertisment = _db.Advertisements
                .Include(s => s.ListenedItems)
                .ThenInclude(s => s.Playlist)
                .Where(s => s.ListenedItems.All(s => s.Playlist.Id == playlistId))
                .Count();

            if (advertisment > sumOfMaxAds) return -1;

            return sumOfMaxAds - advertisment;

        }

        public bool AddPostionForAd(int itemId, int position)
        {
            var newAdd = _db.Items.FirstOrDefault(i => i.Id == itemId) as Podcast;
            if (newAdd == null)
            {
                throw new ServiceException("Podcast nicht gefunden");
                return false;
            }

            if(newAdd.Length < position)
            {
                throw new ServiceException("Position zu groß");
                return false;
            }
            
            if(newAdd.PositionForAd.Count() +1  < newAdd.MaxQuantityAds)
            {
                throw new ServiceException("Maximale Anzahl an Ads erreicht");
                return false;
            }

            newAdd.PositionForAd.Add(position);

            return true; 
            //throw new NotImplementedException("Noch keine Implementierung vorhanden");
        }
    }
}
