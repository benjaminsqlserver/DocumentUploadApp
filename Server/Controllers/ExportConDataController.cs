using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using DocumentUploadApp.Server.Data;

namespace DocumentUploadApp.Server.Controllers
{
    public partial class ExportConDataController : ExportController
    {
        private readonly ConDataContext context;
        private readonly ConDataService service;

        public ExportConDataController(ConDataContext context, ConDataService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/ConData/documentuploads/csv")]
        [HttpGet("/export/ConData/documentuploads/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportDocumentUploadsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetDocumentUploads(), Request.Query, false), fileName);
        }

        [HttpGet("/export/ConData/documentuploads/excel")]
        [HttpGet("/export/ConData/documentuploads/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportDocumentUploadsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetDocumentUploads(), Request.Query, false), fileName);
        }
    }
}
