namespace Yashmak.Data.Models
{
    using Yashmak.Data.Common.Models;

    public class Message : DeletableEntity
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public bool IsSeen { get; set; }

        public int ReceiverId { get; set; }

        public virtual AppUser Receiver { get; set; }
    }
}