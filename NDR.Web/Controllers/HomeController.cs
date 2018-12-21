using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Common.CommonEntities;
using Common.CommonRepo;
using Common.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NDR.Web.Models;

namespace NDR.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        UserProfile uploadedBy;
        readonly UserRepo userRepo;

        public HomeController()
        {
            userRepo = new UserRepo();
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View("~/Views/Account/login.cshtml");
                //return RedirectToAction("LogOff", "AccountController"); 
            }
            else
            {
                uploadedBy = await userRepo.GetUserAsync(User.Identity.Name);

                string ipshortname = uploadedBy.RoleName.ToLower() == "ip" ? uploadedBy.IP.ShortName : "";
                IList<MacroReport> macro = new DashBoardRepo().GenerateMacroReportForDisplay(ipshortname);

                ViewBag.IPLocation = new FacilityRepo().GetDashBoardFilter(ipshortname);

                return View(macro);
            }
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
