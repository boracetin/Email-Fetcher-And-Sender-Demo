
using MailKit.Net.Imap;
using MailKit;
using MailKit.Net.Pop3;
using System.Net.Mail;
using System.Net;

string email = "@gmail.com";
string password = "App Eposta";
string imapServer = "imap.gmail.com";
int imapPort = 993;
try
{



    using (var client = new ImapClient())
    {
        // IMAP sunucusuna bağlan
        client.Connect(imapServer, imapPort, true);

        // Kimlik doğrulama
        client.Authenticate(email, password);

        Console.WriteLine("IMAP sunucusuna bağlanıldı ve kimlik doğrulandı.");

        // Gelen kutusunu aç
        var inbox = client.Inbox;
        inbox.Open(FolderAccess.ReadOnly);
        var unreadMessages = await inbox.SearchAsync(MailKit.Search.SearchQuery.NotSeen);

        Console.WriteLine($"Toplam Mail Sayısı: {inbox.Count}");
        Console.WriteLine($"Okunmamış Mail Sayısı: {inbox.Unread}");
        foreach (var messageIndex in unreadMessages)
        {
            var message = await inbox.GetMessageAsync(messageIndex);
            Console.WriteLine($"- Konu: {message.Subject}");
            Console.WriteLine($"  Gönderen: {message.From}");
            Console.WriteLine($"  To: {message.To}");
            Console.WriteLine($"  CC: {message.Cc}");
            Console.WriteLine($"  BCC: {message.Bcc}");
            Console.WriteLine($"  MessageId: {message.MessageId}");
            Console.WriteLine($"  Attachments: {message.Attachments}");
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
string smtpServer = "smtp.gmail.com"; // SMTP sunucusu
int port = 587; // TLS portu
string toEmail = "demo adres"; // Alıcı e-posta adresi
try
{
    // SMTP istemcisi oluştur
    SmtpClient smtpClient = new SmtpClient(smtpServer, port)
    {
        Credentials = new NetworkCredential(email, password), // Kimlik doğrulama
        EnableSsl = true // Güvenli bağlantı
    };

    // E-posta mesajı oluştur
    MailMessage mail = new MailMessage
    {
        From = new MailAddress(email), // Gönderen
        Subject = "Test E-postası", // Konu
        Body = "Bu bir test e-postasıdır.", // İçerik
        IsBodyHtml = false // HTML içermiyor
    };

    mail.To.Add(toEmail); // Alıcıyı ekle

    // E-postayı gönder
    smtpClient.Send(mail);
    Console.WriteLine("E-posta başarıyla gönderildi!");
}
catch (Exception ex)
{
    Console.WriteLine($"E-posta gönderim hatası: {ex.Message}");
}
    