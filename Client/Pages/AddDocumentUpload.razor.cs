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
    public partial class AddDocumentUpload
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

        protected override async Task OnInitializedAsync()
        {
            documentUpload = new DocumentUploadApp.Server.Models.ConData.DocumentUpload();
        }
        protected bool errorVisible;
        protected DocumentUploadApp.Server.Models.ConData.DocumentUpload documentUpload;

        protected async Task FormSubmit()
        {
            try
            {
                await ConDataService.CreateDocumentUpload(documentUpload);
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

         void OnProgress(UploadProgressArgs args, string name)
        {
            

            NotificationService.Notify(NotificationSeverity.Info, "Upload Progress", $"{args.Progress}% '{name}' / {args.Loaded} of {args.Total} bytes.", 10000);

            if (args.Progress == 100)//if upload is complete
            {
                foreach (var file in args.Files)
                {
                    // console.Log($"Uploaded: {file.Name} / {file.Size} bytes");
                    NotificationService.Notify(NotificationSeverity.Info, "Upload Complete", $"Uploaded: {file.Name} / {file.Size} bytes", 10000);
                }
            }
        }

    }
}