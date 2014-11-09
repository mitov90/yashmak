namespace Yashmak.Models
{
    using System.ComponentModel.DataAnnotations;

    public class File
    {
        [Key]
        public int Id { get; set; }

        public string Filename { get; set; }

        public string PathToFile { get; set; }

        public int Size { get; set; }

        public bool IsDirectory { get; set; }

        public string UserId { get; set; }

        public virtual AppUser User { get; set; }
    }
}