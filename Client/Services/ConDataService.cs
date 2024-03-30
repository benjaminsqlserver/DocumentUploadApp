
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace DocumentUploadApp.Client
{
    public partial class ConDataService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public ConDataService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/ConData/");
        }


        public async System.Threading.Tasks.Task ExportDocumentUploadsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/documentuploads/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/documentuploads/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportDocumentUploadsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/condata/documentuploads/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/condata/documentuploads/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetDocumentUploads(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<DocumentUploadApp.Server.Models.ConData.DocumentUpload>> GetDocumentUploads(Query query)
        {
            return await GetDocumentUploads(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<DocumentUploadApp.Server.Models.ConData.DocumentUpload>> GetDocumentUploads(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"DocumentUploads");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetDocumentUploads(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<DocumentUploadApp.Server.Models.ConData.DocumentUpload>>(response);
        }

        partial void OnCreateDocumentUpload(HttpRequestMessage requestMessage);

        public async Task<DocumentUploadApp.Server.Models.ConData.DocumentUpload> CreateDocumentUpload(DocumentUploadApp.Server.Models.ConData.DocumentUpload documentUpload = default(DocumentUploadApp.Server.Models.ConData.DocumentUpload))
        {
            var uri = new Uri(baseUri, $"DocumentUploads");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(documentUpload), Encoding.UTF8, "application/json");

            OnCreateDocumentUpload(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DocumentUploadApp.Server.Models.ConData.DocumentUpload>(response);
        }

        partial void OnDeleteDocumentUpload(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteDocumentUpload(long documentId = default(long))
        {
            var uri = new Uri(baseUri, $"DocumentUploads({documentId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteDocumentUpload(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetDocumentUploadByDocumentId(HttpRequestMessage requestMessage);

        public async Task<DocumentUploadApp.Server.Models.ConData.DocumentUpload> GetDocumentUploadByDocumentId(string expand = default(string), long documentId = default(long))
        {
            var uri = new Uri(baseUri, $"DocumentUploads({documentId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetDocumentUploadByDocumentId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<DocumentUploadApp.Server.Models.ConData.DocumentUpload>(response);
        }

        partial void OnUpdateDocumentUpload(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateDocumentUpload(long documentId = default(long), DocumentUploadApp.Server.Models.ConData.DocumentUpload documentUpload = default(DocumentUploadApp.Server.Models.ConData.DocumentUpload))
        {
            var uri = new Uri(baseUri, $"DocumentUploads({documentId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(documentUpload), Encoding.UTF8, "application/json");

            OnUpdateDocumentUpload(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}