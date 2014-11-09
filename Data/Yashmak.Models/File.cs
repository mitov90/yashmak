namespace Yashmak.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class File
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
    }
}