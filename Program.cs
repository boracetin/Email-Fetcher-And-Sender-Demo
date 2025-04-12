


using MailKit.Net.Imap;
using MailKit;
using MailKit.Net.Pop3;
using System.Net.Mail;
using System.Net;
using EmailFetcherDemo.Models;
using EmailFetcherDemo.Enums;
using MimeKit;
using System.IO;
using EmailFetcherDemo.Extentions;
using System.Reflection;
using EmailFetcherDemo.Helpers;

var fetcherServer = new MailServer()
{
    Email = "",
    Password = "",
    Port = 993,
    HostAddress = "imap.gmail.com",
    ServerType = EnumMailServerType.IMAP
};

try
{
    while (true)
    {
        try
        {
            var mailFetcher = new MailParser(fetcherServer);
            await mailFetcher.ConnectMailServerAsync();
            Console.WriteLine($"{DateTime.Now}: Mail kontrolü tamamlandı.");


            string inReplyToMessageId = "";

            var mailSender = new MailServer()
            {
                Email = "",
                Password = "",
                HostAddress = "smtp.gmail.com",
                ServerType = EnumMailServerType.SMTP,
                Port = 587
            };
            try
            {
                SmtpClient smtpClient = new SmtpClient(mailSender.HostAddress, mailSender.Port)
                {
                    Credentials = new NetworkCredential(mailSender.Email, mailSender.Password),
                    EnableSsl = true
                };


                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(mailSender.Email),
                    Subject = "RE: Bora İlk Gönderilen Mail",
                    Body = "Cevap Açıklaması",
                    IsBodyHtml = false
                };

                mail.To.Add("");
                mail.Headers.Add("In-Reply-To", "<" + inReplyToMessageId + ">");
                mail.Headers.Add("References", "<" + inReplyToMessageId + ">");

                smtpClient.Send(mail);
                Console.WriteLine("E-posta başarıyla gönderildi!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"E-posta gönderim hatası: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{DateTime.Now}: Bir hata oluştu: {ex.Message}");
        }

        // 1 dakika bekle
        await Task.Delay(TimeSpan.FromMinutes(1));
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Bir hata oluştu: {ex.Message}");
}


Console.ReadLine();
