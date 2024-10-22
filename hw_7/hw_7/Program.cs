using System.Net;
using System.Net.Mail;
using MailKit.Net.Pop3;
using MimeKit;

namespace hw_7;

class Program
{
    // Реализовать приложение по типу «Console Application», с возможность отправки письма нескольким получателям.
        
    // !!! Дописал врзу доп1 - Используя код из предыдущего задания, реализовать возможно прикреплять вложения, указывая путь к ним.
    private static EmailConfiguration _emailConfiguration = new EmailConfiguration()
    {
        From = "ushachovg324@gmail.com",
        SmtpServer = "smtp.gmail.com",
        Port = 587,
        UserName = "ushachovg324@gmail.com",
        Password = "udph dsed shvu scjw"
    };
    private static List<string> _emailList = new List<string> { "ushachovg324@gmail.com", "nenovgleb1234@gmail.com" };
    
    static void Main(string[] args)
    {
        Console.WriteLine("Would you like to attach a file? \nIf yes enter path to file or press \"enter\"");
        string pathToFile = Console.ReadLine();
        if (File.Exists(pathToFile))
        {
            Attachment attachment = new Attachment(pathToFile);
            SendMessage(_emailList, attachment);
        }
        else
        {
            SendMessage(_emailList);
        }
    }

    static void SendMessage(IEnumerable<string> emailList, Attachment? attachment = null)
    {
        MailMessage post = new MailMessage();
        post.From = new MailAddress(_emailConfiguration.From);
        foreach (var email in emailList)
        {
            post.To.Add(email);
        }
        post.Subject = "Hello";
        post.Body = "How are you ?";

        if (attachment is not null)
        {
            post.Attachments.Add(attachment);
        }

        SmtpClient smtpClient = new SmtpClient(_emailConfiguration.SmtpServer, _emailConfiguration.Port);
        smtpClient.EnableSsl = true;
        smtpClient.Credentials = new NetworkCredential(_emailConfiguration.UserName, _emailConfiguration.Password);     
        smtpClient.Send(post);
    }



    // Используя протокол «POP3», получить все письма с вашего почтового ящика.
    static void Dop2()
    {
        string pop3Server = "pop.gmail.com";
        int port = 995;
        string username = "ushachovg324@gmail.com";
        string password = "udph dsed shvu scjw";
    
        try
        {
            using (var client = new Pop3Client())
            {
                client.Connect(pop3Server, port, true);
                client.Authenticate(username, password);
    
                int count = client.Count;
                Console.WriteLine(count);
    
                for (int i = 0; i < 50; i++)
                {
                    MimeMessage message = client.GetMessage(i);
                    Console.WriteLine(message.Subject);
                }
    
                client.Disconnect(true);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}

public class EmailConfiguration
{
    public string From { get; set; }
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}