using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DocumentUploadApp.Server.Models.ConData
{
    [Table("DocumentUploads", Schema = "dbo")]
    public partial class DocumentUpload
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long DocumentID { get; set; }

        [Required]
        public string DocumentName { get; set; }

        [Required]
        public string DocumentType { get; set; }

        [Required]
        public byte[] DocumentData { get; set; }

    }
}