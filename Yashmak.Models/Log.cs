namespace Yashmak.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Log
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        public string Ip { get; set; }

        public string Action { get; set; }

        public DateTime DateTime { get; set; }

        public virtual AppUser User { get; set; }
    }
}