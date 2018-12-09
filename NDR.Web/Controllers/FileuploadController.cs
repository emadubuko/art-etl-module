using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common.CommonEntities;
using Common.CommonRepo;
using Common.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NDR.Web.Services;
using Newtonsoft.Json;

namespace NDR.Web.Controllers
{
    [Authorize]
    public class FileuploadController : Controller
    {
        UserProfile uploadedBy;
        readonly UserRepo userRepo;
        public FileuploadController()
        {
            userRepo = new UserRepo();
        }

        //this is for file upload
        public IActionResult Index()
        {
            

            return View();
        }

        public IActionResult ViewUpload()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> UploadReport(int? draw, int? start, int? length)
        {
            string fileUrl = Utils.GetAppConfigItem("fileQueryURL");
            var search = string.Format("{0}|{1}|{2}|{3}", Request.Form["search[value]"], start, length,draw);

            fileUrl += "?queryString=" + search;

            string responseString = await new Utils().PostDateRemotelyWithStringResult(fileUrl, search);
            return responseString;
        }

        

        [HttpPost]
        public async Task<IActionResult> UploadFile(string connectionId)
        {
            uploadedBy = await userRepo.GetUserAsync(User.Identity.Name);

            string fileUrl = Utils.GetAppConfigItem("fileServiceURL");
            string directory = Utils.GetAppConfigItem("uploadPath") + DateTime.Now.ToString("MMM yyyy") + "/" + DateTime.Now.ToString("dd MMM yyyy") + "/";
            if (Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }

            // UserProfile uploadedBy = MembershipProvider.FormsAuthenticationService.LoggedinProfile;

            if (Request.Form.Files.Count == 0 || string.IsNullOrEmpty(Request.Form.Files[0].FileName))
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "No file uploaded");
            }
            else
            {
                Utils utils = new Utils();
                foreach (var ndrfile in Request.Form.Files)
                {
                    if (ndrfile.Length > 0 && Path.GetExtension(ndrfile.FileName).Substring(1).ToUpper() == "ZIP")
                    {
                        string filePath = "";
                        filePath = directory + DateTime.Now.ToString("dd hh_mm_ss") + "_" + ndrfile.FileName;

                        using (var inputStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ndrfile.CopyToAsync(inputStream);
                            byte[] array = new byte[inputStream.Length];
                            inputStream.Seek(0, SeekOrigin.Begin);
                            inputStream.Read(array, 0, array.Length);
                        }
                        bool status = await utils.PostFileRemotely(fileUrl, ndrfile, uploadedBy.Id);
                        if (status == false)
                            return BadRequest("system error occured. Please re-try at a later time");
                    }
                    else
                        return BadRequest("wrong file type uploaded. Please upload zip files only");
                }
                return Ok("success");
            }
        }


    }
}