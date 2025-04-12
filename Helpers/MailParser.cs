using EmailFetcherDemo.Extentions;
using EmailFetcherDemo.Models;
using MailKit.Net.Imap;
using MailKit;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EmailFetcherDemo.Helpers
{
    public class MailParser
    {
        private readonly MailServer _mailServer;
        public MailParser(MailServer mailServer)
        {
            _mailServer = mailServer;
        }

        public async Task ConnectMailServerAsync()
        {
            if (_mailServer.ServerType == Enums.EnumMailServerType.IMAP)
            {
                using (var client = new ImapClient())
                {
                    // IMAP sunucusuna bağlan
                    client.Connect(_mailServer.HostAddress, _mailServer.Port, true);

                    // Kimlik doğrulama
                    client.Authenticate(_mailServer.Email, _mailServer.Password);

                    Console.WriteLine("IMAP sunucusuna bağlanıldı ve kimlik doğrulandı.");

                    // Gelen kutusunu aç
                    var inbox = client.Inbox;
                    var sentBox = client.GetFolder(SpecialFolder.Sent);


                    inbox.Open(FolderAccess.ReadWrite);
                    var unreadMessages = await inbox.SearchAsync(MailKit.Search.SearchQuery.NotSeen);

                    Console.WriteLine($"Toplam Mail Sayısı: {inbox.Count}");
                    Console.WriteLine($"Okunmamış Mail Sayısı: {inbox.Unread}");
                    foreach (var messageIndex in unreadMessages)
                    {
                        var message = await inbox.GetMessageAsync(messageIndex);
                        Console.WriteLine($"- Konu: {message.Subject}");
                        Console.WriteLine($"- Body: {message.Body}");
                        Console.WriteLine($"  Gönderen: {message.From}");
                        Console.WriteLine($"  Gönderenler Name: {message.From.Mailboxes.FirstOrDefault().Name}");
                        Console.WriteLine($"  Gönderenler Address: {message.From.Mailboxes.FirstOrDefault().Address}");
                        Console.WriteLine($"CC: {string.Join(", ", message.Cc.Mailboxes.Select(x => x.Address))}");
                        Console.WriteLine($"To: {string.Join(", ", message.To.Mailboxes.Select(x => x.Address))}");
                        Console.WriteLine($"Bcc: {string.Join(", ", message.Bcc.Mailboxes.Select(x => x.Address))}");

                        Console.WriteLine($" ReplyTo : {message.ReplyTo}");
                        Console.WriteLine($" InReplyTo : {message.InReplyTo}");
                        Console.WriteLine($"  MessageId: {message.MessageId}");
                        if (message.References != null && message.References.Any())
                        {
                            sentBox.Open(FolderAccess.ReadOnly);


                            var references = message.References
                                                    .ToList();
                            var summaries = sentBox.Fetch(0, -1, MessageSummaryItems.Envelope);
                            Console.WriteLine("References:");
                            foreach (var reference in references)
                            {
                                var targetMessage = summaries.FirstOrDefault(summary =>
                                            summary.Envelope.MessageId == reference);
                                Console.WriteLine($"  - {reference}");
                                if (!String.IsNullOrEmpty(targetMessage?.NormalizedSubject)) Console.WriteLine($"{reference}  Buldummm- {targetMessage.NormalizedSubject}");

                            }
                        }
                        else
                        {
                            Console.WriteLine("References: Bulunamadı veya boş.");
                        }


                        Console.WriteLine($"  Tarih: {message.Date}");
                        //SaveAttachments(message);
                        //await inbox.AddFlagsAsync(messageIndex, MessageFlags.Seen, true);
                        Console.WriteLine(new string('-', 50));
                    }
                    client.Disconnect(true);
                }
            }

        }
        public static bool SaveAttachments(MimeMessage message)
        {

            foreach (var attachment in message.Attachments)
            {
                if (attachment is MimePart mimePart)
                {
                    var fileName = mimePart.FileName;
                    byte[] attachmentBytes;
                    using (var memoryStream = new MemoryStream())
                    {
                        mimePart.Content.DecodeTo(memoryStream); // İçeriği MemoryStream'e yaz
                        attachmentBytes = memoryStream.ToArray(); // MemoryStream'i byte dizisine çevir
                        FileIOExtention.UploadFile(attachmentBytes, fileName);

                    }
                    return true;
                }
            }
            return false;
        }
    }
}
