using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace DocumentUploadApp.Client.Pages
{
    public partial class EditDocumentUpload
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        public ConDataService ConDataService { get; set; }

        [Parameter]
        public long DocumentID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            documentUpload = await ConDataService.GetDocumentUploadByDocumentId(documentId:DocumentID);
        }
        protected bool errorVisible;
        protected DocumentUploadApp.Server.Models.ConData.DocumentUpload documentUpload;

        protected async Task FormSubmit()
        {
            try
            {
                await ConDataService.UpdateDocumentUpload(documentId:DocumentID, documentUpload);
                DialogService.Close(documentUpload);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}