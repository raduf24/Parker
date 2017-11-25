using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Parker.Controllers
{

    [Produces("application/json")]
    [Route("api/RecieveFiles")]
    public class RecieveFilesController : Controller
    {
        [HttpPost("{location}")]
        public IActionResult Post(string location)
        {
            var uploadedFile = Request.Form.Files[0];

            var currentDirectory = Directory.GetCurrentDirectory();
            var filePath = currentDirectory + "/Photos/" + location +"/"+ uploadedFile.FileName;
            (new FileInfo(filePath)).Directory.Create();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                uploadedFile.CopyToAsync(stream);
            }

            return Ok();
        }
    }
}
