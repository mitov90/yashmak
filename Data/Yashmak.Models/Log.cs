namespace Yashmak.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Log
    {
        [Key]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Ip { get; set; }

        public string Action { get; set; }

        public DateTime DateTime { get; set; }

        public virtual AppUser User { get; set; }
    }
}