using BusinessLogicLayer;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineAssessmentSystem.Controllers
{
    public class HomeController : Controller
    {
        IBLLService bllService;
        public HomeController()
        {
            bllService = new BLLService();
        }

        public HomeController(IBLLService bService)
        {
            bllService = bService;
        }

        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public IEnumerable<category> Get()
        {
            return bllService.GetCategories();
        }
    }
}
