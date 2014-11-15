namespace Yashmak.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Yashmak.Data.Common.Models;

    public class File : DeletableEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; }

        public string PathToFile { get; set; }

        public int Size { get; set; }

        public bool IsDirectory { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual File Parent { get; set; }

        public string UserId { get; set; }

        public virtual AppUser User { get; set; }

        public int? PermissionId { get; set; }

        [ForeignKey("PermissionId")]
        public virtual Permission Permission { get; set; }
    }
}