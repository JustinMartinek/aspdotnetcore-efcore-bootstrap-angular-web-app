using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TheWorld.ViewModels;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;
using TheWorld.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private IMailService MailService;

        public IConfigurationRoot Config { get; private set; }

        public IWorldRepository WorldRepository { get; set; }

        public ILogger Logger;

        public AppController(IMailService service, IConfigurationRoot config, IWorldRepository respository, ILogger<AppController> logger)
        {
            MailService = service;
            Config = config;
            WorldRepository = respository;
            Logger = logger;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {

            return View();

        }

        [Authorize]
        public IActionResult Trips()
        {
            return View();
            //try
            //{
            //    var data = WorldRepository.GetAllTrips();
            //    return View(data);
            //}
            //catch (Exception ex)
            //{
            //    Logger.LogError($"Failed to get trips: {ex.Message}");
            //    return Redirect("/error");
            //}

        }

        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(ContactViewModel vm)
        {
            if (ModelState.IsValid)
            {
                MailService.SendMail(Config["MailSettings:ToAddress"], vm.Name, "Contact", vm.Message);
                ModelState.Clear();
                ViewBag.UserMessage = "Mail Sent Successfully.";
            }
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
