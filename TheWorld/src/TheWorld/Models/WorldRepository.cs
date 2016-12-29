using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        private readonly WorldContext Context;

        private ILogger<WorldRepository> Logger;
        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            Context = context;
            Logger = logger;
        }

        public void AddStop(string tripName, Stop stop, string username)
        {
            var trip = GetUserTripByTripName(username,tripName);
            if (trip != null)
            {
                trip.Stops.Add(stop);
                Context.Stops.Add(stop);
            }
        }

        public void AddTrip(Trip trip)
        {
            Context.Add(trip);
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            Logger.LogInformation("Getting trips from Database");

            return Context.Trips.ToList();
        }

        public Trip GetTripByTripName(string tripName)
        {
            return Context.Trips.Include(t => t.Stops).Where(t => t.Name == tripName).FirstOrDefault();
        }

        public IEnumerable<Trip> GetTripsByUserName(string name)
        {
            return Context.Trips.Include(t => t.Stops).Where(user => user.UserName == name).ToList();
        }

        public Trip GetUserTripByTripName(string username, string tripName)
        {
            return Context.Trips.Include(t => t.Stops).Where(t => t.Name == tripName && t.UserName == username).FirstOrDefault();
        }

        public async Task<bool> SaveChangesAsync()
        {
          return ( await Context.SaveChangesAsync())>0;
        }
    }
}
