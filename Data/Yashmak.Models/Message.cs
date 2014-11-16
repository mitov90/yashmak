namespace Yashmak.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Yashmak.Data.Common.Models;

    public class Message : DeletableEntity
    {
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        public string Content { get; set; }

        public bool IsSeen { get; set; }

        public string ReceiverId { get; set; }

        public virtual AppUser Receiver { get; set; }
    }
}