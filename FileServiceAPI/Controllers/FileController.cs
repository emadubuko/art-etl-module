using FileService.Entities;
using FileService.Model;
using FileService.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
                return Ok("Success!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public IActionResult SaveValidationSummary(IEnumerable<ValidationSummary> validationSummary)
        {
            try
            {
                if(validationSummary.Count() == 0)
                {
                    return NoContent();
                }
                
                errorLog.SaveValidationResult(validationSummary as List<ValidationSummary>);
                return Ok("Success!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }


}
