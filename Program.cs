


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
    Email = "123@gmail.com",
    Password = "123213",
    Port = 993,
    HostAddress = "imap.gmail.com",
    ServerType = EnumMailServerType.IMAP
};

try
{
    var mailFetcher = new MailParser(fetcherServer);
    await mailFetcher.ConnectMailServerAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"Bir hata oluştu: {ex.Message}");
}

string toEmail = "bora.cetin@outlook.com"; // Alıcı e-posta adresi

var mailSender = new MailServer()
{
    Email = "9@gmail.com",
    Password = "asda",
    HostAddress = "smtp.gmail.com",
    ServerType = EnumMailServerType.SMTP,
    Port = 587
};
try
{
    // SMTP istemcisi oluştur
    SmtpClient smtpClient = new SmtpClient(mailSender.HostAddress, mailSender.Port)
    {
        Credentials = new NetworkCredential(mailSender.Email, mailSender.Password), // Kimlik doğrulama
        EnableSsl = true // Güvenli bağlantı
    };

    // E-posta mesajı oluştur
    MailMessage mail = new MailMessage
    {
        From = new MailAddress(mailSender.Email), // Gönderen
        Subject = "Test E-postası", // Konu
        Body = "Bu bir test e-postasıdır.", // İçerik
        IsBodyHtml = false // HTML içermiyor
    };

    mail.To.Add(toEmail); // Alıcıyı ekle

    // E-postayı gönder
    //smtpClient.Send(mail);
    Console.WriteLine("E-posta başarıyla gönderildi!");
}
catch (Exception ex)
{
    Console.WriteLine($"E-posta gönderim hatası: {ex.Message}");
}
