using System.Buffers.Text;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Net.Mime.MediaTypeNames;
using System.Net.NetworkInformation;

namespace DocumentUploadApp.Server.Models.ConData
{
    public partial class DocumentUpload
    {
        [NotMapped]
        public string FilePath { 
            get
            {
                return $"data:image/png;base64,{Convert.ToBase64String(DocumentData)}";
            } 
        }
    }
}
