using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace DocumentUploadApp.Server.Controllers
{
    public partial class UploadController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly ConDataService _conDataService;

        public UploadController(IWebHostEnvironment environment,ConDataService conDataService)
        {
            this.environment = environment;
            _conDataService=conDataService;
        }

        // Single file upload
        [HttpPost("upload/single")]
        public IActionResult Single(IFormFile file)
        {
            try
            {
                // Put your code here
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Multiple files upload
        [HttpPost("upload/multiple")]
        public async Task<IActionResult> Multiple(IFormFile[] files)
        {
            try
            {
                // Put your code here
                foreach(IFormFile file in files)
                {
                    if (file == null || file.Length == 0)
                        return null; // or throw an exception, handle it accordingly
                    DocumentUploadApp.Server.Models.ConData.DocumentUpload documentUpload = new Models.ConData.DocumentUpload();
                    documentUpload.DocumentName = file.FileName;
                    documentUpload.DocumentType = file.ContentType;

                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        documentUpload.DocumentData = memoryStream.ToArray();
                    }
                    await _conDataService.CreateDocumentUpload(documentUpload);
                }

                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Multiple files upload with parameter
        [HttpPost("upload/{id}")]
        public IActionResult Post(IFormFile[] files, int id)
        {
            try
            {
                // Put your code here
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Image file upload (used by HtmlEditor components)
        [HttpPost("upload/image")]
        public IActionResult Image(IFormFile file)
        {
            try
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                using (var stream = new FileStream(Path.Combine(environment.WebRootPath, fileName), FileMode.Create))
                {
                    // Save the file
                    file.CopyTo(stream);

                    // Return the URL of the file
                    var url = Url.Content($"~/{fileName}");

                    return Ok(new { Url = url });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
