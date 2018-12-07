using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NDR.Web.Controllers
{
    [Authorize]
    public class FileuploadController : Controller
    {
        //this is for file upload
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult UploadFile(string connectionId)
        {

            return View();
        }
    }
}