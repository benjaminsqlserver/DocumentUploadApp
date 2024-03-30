using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using DocumentUploadApp.Server.Data;

namespace DocumentUploadApp.Server
{
    public partial class ConDataService
    {
        ConDataContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly ConDataContext context;
        private readonly NavigationManager navigationManager;

        public ConDataService(ConDataContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public void ApplyQuery<T>(ref IQueryable<T> items, Query query = null)
        {
            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }
        }


        public async Task ExportDocumentUploadsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/documentuploads/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/documentuploads/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportDocumentUploadsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/documentuploads/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/documentuploads/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnDocumentUploadsRead(ref IQueryable<DocumentUploadApp.Server.Models.ConData.DocumentUpload> items);

        public async Task<IQueryable<DocumentUploadApp.Server.Models.ConData.DocumentUpload>> GetDocumentUploads(Query query = null)
        {
            var items = Context.DocumentUploads.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                ApplyQuery(ref items, query);
            }

            OnDocumentUploadsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnDocumentUploadGet(DocumentUploadApp.Server.Models.ConData.DocumentUpload item);
        partial void OnGetDocumentUploadByDocumentId(ref IQueryable<DocumentUploadApp.Server.Models.ConData.DocumentUpload> items);


        public async Task<DocumentUploadApp.Server.Models.ConData.DocumentUpload> GetDocumentUploadByDocumentId(long documentid)
        {
            var items = Context.DocumentUploads
                              .AsNoTracking()
                              .Where(i => i.DocumentID == documentid);

 
            OnGetDocumentUploadByDocumentId(ref items);

            var itemToReturn = items.FirstOrDefault();

            OnDocumentUploadGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnDocumentUploadCreated(DocumentUploadApp.Server.Models.ConData.DocumentUpload item);
        partial void OnAfterDocumentUploadCreated(DocumentUploadApp.Server.Models.ConData.DocumentUpload item);

        public async Task<DocumentUploadApp.Server.Models.ConData.DocumentUpload> CreateDocumentUpload(DocumentUploadApp.Server.Models.ConData.DocumentUpload documentupload)
        {
            OnDocumentUploadCreated(documentupload);

            var existingItem = Context.DocumentUploads
                              .Where(i => i.DocumentID == documentupload.DocumentID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.DocumentUploads.Add(documentupload);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(documentupload).State = EntityState.Detached;
                throw;
            }

            OnAfterDocumentUploadCreated(documentupload);

            return documentupload;
        }

        public async Task<DocumentUploadApp.Server.Models.ConData.DocumentUpload> CancelDocumentUploadChanges(DocumentUploadApp.Server.Models.ConData.DocumentUpload item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnDocumentUploadUpdated(DocumentUploadApp.Server.Models.ConData.DocumentUpload item);
        partial void OnAfterDocumentUploadUpdated(DocumentUploadApp.Server.Models.ConData.DocumentUpload item);

        public async Task<DocumentUploadApp.Server.Models.ConData.DocumentUpload> UpdateDocumentUpload(long documentid, DocumentUploadApp.Server.Models.ConData.DocumentUpload documentupload)
        {
            OnDocumentUploadUpdated(documentupload);

            var itemToUpdate = Context.DocumentUploads
                              .Where(i => i.DocumentID == documentupload.DocumentID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            Reset();

            Context.Attach(documentupload).State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterDocumentUploadUpdated(documentupload);

            return documentupload;
        }

        partial void OnDocumentUploadDeleted(DocumentUploadApp.Server.Models.ConData.DocumentUpload item);
        partial void OnAfterDocumentUploadDeleted(DocumentUploadApp.Server.Models.ConData.DocumentUpload item);

        public async Task<DocumentUploadApp.Server.Models.ConData.DocumentUpload> DeleteDocumentUpload(long documentid)
        {
            var itemToDelete = Context.DocumentUploads
                              .Where(i => i.DocumentID == documentid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnDocumentUploadDeleted(itemToDelete);

            Reset();

            Context.DocumentUploads.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterDocumentUploadDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}