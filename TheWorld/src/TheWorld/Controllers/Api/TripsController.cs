using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWorld.Models;
using Microsoft.Extensions.Logging;
using TheWorld.ViewModels;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    [Authorize]
    public class TripsController : Controller
    {
        IWorldRepository Repository;
        ILogger Logging;

        public TripsController(IWorldRepository repository, ILogger<TripsController> logging)
        {
            Repository = repository;
            Logging = logging;
        }


        //[HttpGet("api/trips")]
        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var results = Repository.GetTripsByUserName(User.Identity.Name);

                return Ok(AutoMapper.Mapper.Map<IEnumerable<TripViewModel>>(results));
            }
            catch (Exception ex)
            {
                Logging.LogCritical($"Failed to get Trips{ex}");
                return BadRequest("Error occurred");
            }

        }

        //[HttpPost("api/trips")]
        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]TripViewModel theTrip)
        {
            if (ModelState.IsValid)
            {
                var newTrip = AutoMapper.Mapper.Map<Trip>(theTrip);

                newTrip.UserName = User.Identity.Name;

                Repository.AddTrip(newTrip);

                if (await Repository.SaveChangesAsync())
                {
                    return Created($"api/trips/{newTrip.Name}", AutoMapper.Mapper.Map<TripViewModel>(newTrip));
                }

                return BadRequest("Failed to save changes to db");
            }
            return BadRequest("Bad data");
        }
    }
}
