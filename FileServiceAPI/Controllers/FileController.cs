using Common.CommonEntities;
using Common.CommonRepo;
using Common.Utility;
using FileService.Entities;
using FileService.Model;
using FileService.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FileServiceAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        readonly NDRFileRepo fileRepo;
        readonly ValidationSummaryRepo errorLog;
        public FileController()
        {
            fileRepo = new NDRFileRepo();
            errorLog = new ValidationSummaryRepo();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFileStatus(FileUpdateModel model)
        {
            try
            {
                await fileRepo.UpdateFileStatusAsync(model);
                 
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            fileRepo.CloseSession();
            return Ok("Success!");
        }

        [HttpPost]
        public IActionResult SaveValidationSummary(IEnumerable<ValidationSummary> validationSummary)
        {
            try
            {
                if (validationSummary.Count() == 0)
                {
                    return NoContent();
                }

                errorLog.SaveValidationResult(validationSummary as List<ValidationSummary>);
                if (validationSummary.Any())
                    fileRepo.MarkBatchAsCompletedAsync(validationSummary.FirstOrDefault().FileUploadBacthNumber).Wait();
                 
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }

            fileRepo.CloseSession();
            return Ok("Success!");
        }

        public async Task<string> GenerateReport(string queryString) // (int userId)
        {
            UserProfile currentuser = await new UserRepo().RetrieveAsync(1);
 
            string[] param = queryString.Split('|');
            ZipFileSearchModel searchModel = new ZipFileSearchModel
            {
                IP = currentuser.RoleName == "ip" ? currentuser.IP.ShortName : "",
                StartIndex = Convert.ToInt32(param[1]),
                MaxRows = Convert.ToInt32(param[2])
            };

            List<FileUploadViewModel> mydata = fileRepo.RetrieveUsingPagingAsync(searchModel, out int total);

            foreach (var dr in mydata)
            {
                dr.ViewErrorbutton = string.Format("<a style='text-transform: capitalize;' class='btn btn-sm {2} viewvalidationsummary' id='{0}' {1}>View Validation Summary</a>&nbsp;&nbsp;&nbsp; <i style='display:none' id='loadImg2'><img class='center' src='/images/spinner.gif' width='40'> please wait ...</i>", dr.Id, dr.Status == "Pending" ? "disabled" : "", dr.Status == "Pending" ? "btn-default" : "btn-info");
            }

            fileRepo.CloseSession();
 
            //return mydata;
            return JsonConvert.SerializeObject(
                        new
                        {
                            sEcho = Convert.ToInt32(param[3]),
                            iTotalRecords = total,
                            iTotalDisplayRecords = total,
                            aaData = mydata
                        });
        }


        [HttpPost]
        public async Task<IActionResult> UploadFile()
        {
            Request.Form.TryGetValue("userId", out StringValues vs);
            int userId = Convert.ToInt32(vs[0]);
            UserProfile uploadedBy = await new UserRepo().RetrieveAsync(userId);
            var fileRepo = new NDRFileRepo();
            var bRepo = new FileZipUploadRepo();

            string directory = Utils.GetAppConfigItem("uploadPath") + DateTime.Now.ToString("MMM yyyy") + "/" + DateTime.Now.ToString("dd MMM yyyy") + "/";
            if (Directory.Exists(directory) == false)
            {
                Directory.CreateDirectory(directory);
            }
            
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
                        string filePath = directory + DateTime.Now.ToString("dd hh_mm_ss") + "_" + ndrfile.FileName;

                        using (var inputStream = new FileStream(filePath, FileMode.Create))
                        {
                            await ndrfile.CopyToAsync(inputStream); 
                        }

                        long lastId = await fileRepo.RunScalarSQLAsync("SELECT MAX(Id) FROM filezipupload");
                        string batchNumber = string.Format("{0}_{1:MMM_yyyy}_{2}", uploadedBy.IP.ShortName, DateTime.Now, lastId < 1 ? 1 : lastId + 1);

                        try
                        {
                            await bRepo.SaveAsync(
                                new FileZipUpload
                                {
                                    BatchNumber = batchNumber,
                                    DateUploaded = DateTime.Now,
                                    FilePath = filePath,
                                    Status = FileBatchStatus.Pending,
                                    UploadedBy = uploadedBy,
                                    UploadedFileName = ndrfile.FileName
                                });
                            await bRepo.CommitChangesAsync();
                        }
                        catch (Exception exp)
                        {
                            Logger.LogError(exp);
                            return BadRequest(exp);
                        }
                    }
                    else
                        return BadRequest("wrong file type uploaded. Please upload zip files only");
                }
            }

            fileRepo.CloseSession();
            return Ok("success");
        }


        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value ...", "value2 . . ." };
        }
    }


}
