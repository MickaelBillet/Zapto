﻿using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MimeKit;
using Polly;
using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    internal class MailService : IMailService
    {
        #region Properties
        private MailSettings MailSettings { get; }
        #endregion

        #region Constructor

        public MailService(IServiceProvider serviceProvider)
        {
            this.MailSettings = serviceProvider.GetRequiredService<IOptions<MailSettings>>().Value;
        }

        #endregion

        #region Methods
        public async Task<bool> SendEmailAsync(MailRequest mailRequest, EventHandler<MessageSentEventArgs> smtp_sent, string mailPassword, string mailAddress)
        {
            MimeMessage email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(mailAddress)
            };
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.From.Add(MailboxAddress.Parse(mailAddress));
            email.Subject = mailRequest.Subject;
            BodyBuilder builder = new BodyBuilder();
            bool isSent = false;

            if (mailRequest.Attachments != null)
            {
                foreach (var attachment in mailRequest.Attachments.Where(attachment => attachment.Content != null))
                {
                    builder.Attachments.Add(attachment.Filename, attachment.Content, ContentType.Parse(attachment.ContentType));
                }
            }

            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            using (SmtpClient smtp = new SmtpClient())
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;
                cts.CancelAfter(30000);
                smtp.MessageSent += smtp_sent;

                try
                {
                    //Retry policy (5 times), if the process returns OperationCanceledException => retry
                    await Policy.Handle<OperationCanceledException>().RetryAsync(5, onRetry: async (exception, retryCount) =>
                    {
                        await Task.Run(() => Log.Information("Retry : " + retryCount));

                    }).ExecuteAsync(async (token) =>
                    {
                        if (this.MailSettings.SecureSocketOptions != null)
                        {
                            Task connectTask = smtp.ConnectAsync(this.MailSettings.Host, this.MailSettings.Port, SetOption(this.MailSettings.SecureSocketOptions), token);
                            Task completetdTask = await Task.WhenAny(connectTask, Task.Delay(5000));
                            if (completetdTask != connectTask)
                            {
                                throw new OperationCanceledException();
                            }
                        }
                    }, cts.Token);

                    if (smtp.IsConnected)
                    {
                        if (string.IsNullOrEmpty(mailAddress) == false && string.IsNullOrEmpty(mailPassword) == false)
                        {
                            await smtp.AuthenticateAsync(mailAddress, mailPassword, token);
                        }
                        await smtp.SendAsync(email, token);
                        isSent = true;
                        await smtp.DisconnectAsync(true);
                    }
                    else
                    {
                        Log.Error("MailService Connection Error");
                    }
                }
                catch (Exception ex)
                {
                    isSent = false;
                    Log.Error("MailService : " + ex.Message);
                }
                finally
                {
                    smtp.MessageSent -= smtp_sent;
                }
            }

            return isSent;
        }

        private SecureSocketOptions SetOption(string options)
        {
            SecureSocketOptions option = SecureSocketOptions.Auto;
            if (options.Equals("Auto"))
            {
                option = SecureSocketOptions.Auto;
            }
            else if (options.Equals("None"))
            {
                option = SecureSocketOptions.None;
            }
            else if (options.Equals("Tls"))
            {
                option = SecureSocketOptions.StartTlsWhenAvailable;
            }
            else if (option.Equals("Ssl"))
            {
                option = SecureSocketOptions.SslOnConnect;
            }

            return option;
        }
        #endregion
    }
}
