using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();

        void AddTrip(Trip trip);
        void AddStop(string tripName, Stop newStop, string name);

        Task<bool> SaveChangesAsync();

        Trip GetTripByTripName(string tripName);
        Trip GetUserTripByTripName(string username, string tripName);
        IEnumerable<Trip> GetTripsByUserName(string name);
        
        
    }
}