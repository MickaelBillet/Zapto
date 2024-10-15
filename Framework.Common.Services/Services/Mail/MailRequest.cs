using System.Collections.Generic;
#nullable disable

namespace Framework.Infrastructure.Services
{
    public class MailRequest
	{
		public string ToEmail { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }
        public IEnumerable<MailAttachment> Attachments { get; set; }
    }
}
