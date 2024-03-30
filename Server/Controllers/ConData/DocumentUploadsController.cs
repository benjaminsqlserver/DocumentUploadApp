using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DocumentUploadApp.Server.Controllers.ConData
{
    [Route("odata/ConData/DocumentUploads")]
    public partial class DocumentUploadsController : ODataController
    {
        private DocumentUploadApp.Server.Data.ConDataContext context;

        public DocumentUploadsController(DocumentUploadApp.Server.Data.ConDataContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<DocumentUploadApp.Server.Models.ConData.DocumentUpload> GetDocumentUploads()
        {
            var items = this.context.DocumentUploads.AsQueryable<DocumentUploadApp.Server.Models.ConData.DocumentUpload>();
            this.OnDocumentUploadsRead(ref items);

            return items;
        }

        partial void OnDocumentUploadsRead(ref IQueryable<DocumentUploadApp.Server.Models.ConData.DocumentUpload> items);

        partial void OnDocumentUploadGet(ref SingleResult<DocumentUploadApp.Server.Models.ConData.DocumentUpload> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/ConData/DocumentUploads(DocumentID={DocumentID})")]
        public SingleResult<DocumentUploadApp.Server.Models.ConData.DocumentUpload> GetDocumentUpload(long key)
        {
            var items = this.context.DocumentUploads.Where(i => i.DocumentID == key);
            var result = SingleResult.Create(items);

            OnDocumentUploadGet(ref result);

            return result;
        }
        partial void OnDocumentUploadDeleted(DocumentUploadApp.Server.Models.ConData.DocumentUpload item);
        partial void OnAfterDocumentUploadDeleted(DocumentUploadApp.Server.Models.ConData.DocumentUpload item);

        [HttpDelete("/odata/ConData/DocumentUploads(DocumentID={DocumentID})")]
        public IActionResult DeleteDocumentUpload(long key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.DocumentUploads
                    .Where(i => i.DocumentID == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnDocumentUploadDeleted(item);
                this.context.DocumentUploads.Remove(item);
                this.context.SaveChanges();
                this.OnAfterDocumentUploadDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnDocumentUploadUpdated(DocumentUploadApp.Server.Models.ConData.DocumentUpload item);
        partial void OnAfterDocumentUploadUpdated(DocumentUploadApp.Server.Models.ConData.DocumentUpload item);

        [HttpPut("/odata/ConData/DocumentUploads(DocumentID={DocumentID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutDocumentUpload(long key, [FromBody]DocumentUploadApp.Server.Models.ConData.DocumentUpload item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.DocumentID != key))
                {
                    return BadRequest();
                }
                this.OnDocumentUploadUpdated(item);
                this.context.DocumentUploads.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.DocumentUploads.Where(i => i.DocumentID == key);
                
                this.OnAfterDocumentUploadUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/ConData/DocumentUploads(DocumentID={DocumentID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchDocumentUpload(long key, [FromBody]Delta<DocumentUploadApp.Server.Models.ConData.DocumentUpload> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.DocumentUploads.Where(i => i.DocumentID == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnDocumentUploadUpdated(item);
                this.context.DocumentUploads.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.DocumentUploads.Where(i => i.DocumentID == key);
                
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnDocumentUploadCreated(DocumentUploadApp.Server.Models.ConData.DocumentUpload item);
        partial void OnAfterDocumentUploadCreated(DocumentUploadApp.Server.Models.ConData.DocumentUpload item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] DocumentUploadApp.Server.Models.ConData.DocumentUpload item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnDocumentUploadCreated(item);
                this.context.DocumentUploads.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.DocumentUploads.Where(i => i.DocumentID == item.DocumentID);

                

                this.OnAfterDocumentUploadCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
