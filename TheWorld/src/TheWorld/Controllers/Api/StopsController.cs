using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWorld.Models;
using Microsoft.Extensions.Logging;
using TheWorld.ViewModels;
using TheWorld.Services;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TheWorld.Controllers.Api
{
    [Route("/api/trips/{tripName}/stops")]
    [Authorize]
    public class StopsController : Controller
    {
        private IWorldRepository Repository;
        ILogger<StopsController> Logger;
        IGeoCordsService GeocordService;

        public StopsController(IWorldRepository  repository, ILogger<StopsController> logger, IGeoCordsService service)
        {
            Repository = repository;
            Logger = logger;
            GeocordService = service;
        }
        // GET: /<controller>/
        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = Repository.GetUserTripByTripName(User.Identity.Name, tripName);
                return Ok(AutoMapper.Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s=>s.Order).ToList()));
            }
            catch(Exception ex)
            {
                Logger.LogError($"Error getting stops {ex}");
            }
            return BadRequest("Failed to get the stops");
        }

        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName, [FromBody]StopViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newStop = AutoMapper.Mapper.Map<Stop>(vm);

                    var coordinateResult = await GeocordService.GetCoordsAsync(newStop.Name);

                    if (!coordinateResult.Success)
                    {
                        Logger.LogError($"Failed to get the coordinates {coordinateResult.Message}");
                    }
                    else
                    {
                        newStop.Latitude = coordinateResult.Latitude;
                        newStop.Longitude = coordinateResult.Longitude;

                        Repository.AddStop(tripName, newStop, User.Identity.Name);

                        if (await Repository.SaveChangesAsync())
                        {

                            return Created($"/api/trip/{tripName}/stops/{newStop.Name}.", AutoMapper.Mapper.Map<StopViewModel>(newStop));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error saving stop {ex}");
            }
            return BadRequest("Failed to save the stop");
        }
    }
}
