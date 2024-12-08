using MimeKit;
using System.IO;
#nullable disable

namespace Framework.Infrastructure.Services
{
    public class MailAttachment
    {
        private const string DefaultContentType = "application/octet-stream";

        /// <summary>Initializes a new instance of the <see cref="MailAttachment"/> class.</summary>
        /// <param name="filePath"></param>
        public MailAttachment(string filePath)
        {
            var content = File.ReadAllBytes(filePath);
            var filename = Path.GetFileName(filePath);
            var contentType = MimeTypes.GetMimeType(filename);

            this.MailAttachmentInternal(filename, content, contentType);
        }

        /// <summary>Initializes a new instance of the <see cref="MailAttachment"/> class.</summary>
        /// <param name="filename"></param>
        /// <param name="content"></param>
        public MailAttachment(string filename, byte[] content)
        {
            this.MailAttachmentInternal(filename, content, MimeTypes.GetMimeType(filename));
        }

        /// <summary>Initializes a new instance of the <see cref="MailAttachment"/> class.</summary>
        /// <param name="filename"></param>
        /// <param name="content"></param>
        /// <param name="contentType"></param>
        public MailAttachment(string filename, byte[] content, string contentType)
        {
            this.MailAttachmentInternal(filename, content, contentType);
        }

        public byte[] Content { get; set; }

        public string Filename { get; set; }

        public string ContentType { get; set; }

        private void MailAttachmentInternal(string filename, byte[] content, string contentType)
        {
            this.Filename = filename;
            this.Content = content;
            this.ContentType = contentType;
        }
    }
}
