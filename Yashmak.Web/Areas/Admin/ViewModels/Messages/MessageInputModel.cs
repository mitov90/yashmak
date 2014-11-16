namespace Yashmak.Web.Areas.Admin.ViewModels.Messages
{
    using System.ComponentModel.DataAnnotations;

    using Yashmak.Data.Models;
    using Yashmak.Web.Infrastructure.Mapping;

    public class MessageInputModel : IMapFrom<Message>
    {
        [Required]
        public string ReceiverId { get; set; }

        [Required(ErrorMessage = "Content is required!")]
        [MinLength(1, ErrorMessage = "Minimum content lenght is 3 symbols")]
        public string Content { get; set; }
    }
}