
using MailKit.Net.Imap;
using MailKit;
using MailKit.Net.Pop3;
using System.Net.Mail;
using System.Net;
using EmailFetcherDemo.Models;
using EmailFetcherDemo.Enums;

var fetcherServer = new MailServer() { 
    Email="asd@gmail.com",
    Password= "sadfds",
    Port=993,
    HostAddress = "imap.gmail.com",
    ServerType=EnumMailServerType.IMAP
};

try
{



    using (var client = new ImapClient())
    {
        // IMAP sunucusuna bağlan
        client.Connect(fetcherServer.HostAddress, fetcherServer.Port, true);

        // Kimlik doğrulama
        client.Authenticate(fetcherServer.Email, fetcherServer.Password);

        Console.WriteLine("IMAP sunucusuna bağlanıldı ve kimlik doğrulandı.");

        // Gelen kutusunu aç
        var inbox = client.Inbox;
        inbox.Open(FolderAccess.ReadWrite);
        var unreadMessages = await inbox.SearchAsync(MailKit.Search.SearchQuery.All);

        Console.WriteLine($"Toplam Mail Sayısı: {inbox.Count}");
        Console.WriteLine($"Okunmamış Mail Sayısı: {inbox.Unread}");
        foreach (var messageIndex in unreadMessages)
        {
            var message = await inbox.GetMessageAsync(messageIndex);
            Console.WriteLine($"- Konu: {message.Subject}");
            Console.WriteLine($"- Body: {message.Body}");
            Console.WriteLine($"  Gönderen: {message.From}");
            Console.WriteLine($"  To: {message.To}");
            Console.WriteLine($"  CC: {message.Cc}");
            Console.WriteLine($"  BCC: {message.Bcc}");
            Console.WriteLine($" ReplyTo : {message.ReplyTo}");
            Console.WriteLine($" InReplyTo : {message.InReplyTo}");

            Console.WriteLine($"  MessageId: {message.MessageId}");
            Console.WriteLine($"  References: {message.References}");

            //Console.WriteLine($"  Attachments: {message.Attachments}");
            Console.WriteLine($"  Tarih: {message.Date}");
            await inbox.AddFlagsAsync(messageIndex, MessageFlags.Seen, true);
            Console.WriteLine(new string('-', 50));
        }
        client.Disconnect(true);
    }
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
