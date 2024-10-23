using System;
using System.Net;
using System.Net.Mail;
using System.Timers;
using Timer = System.Timers.Timer;

namespace FutureMeApp
{
    class Program
    {
        private static string _email; 
        private static string _subject; 
        private static string _body; 
        private static DateTime _dateForSend; 
        private static Timer _timer;
    
        static void Main(string[] args)
        {
            GetEmailData();
            WaitDate();

            Console.ReadKey();
        }

        static void GetEmailData()
        {
            Console.WriteLine("Enter your email:");
            _email = Console.ReadLine();
            
            Console.WriteLine("Enter email subject:");
            _subject = Console.ReadLine();
            
            Console.WriteLine("Enter email body:");
            _body = Console.ReadLine();
            
            Console.WriteLine("Enter date and time when you want to get the message in format \"dd.MM.yyyy\":");
            string date = Console.ReadLine();
            
            if (DateTime.TryParse(date, out _dateForSend))
            {
                Console.WriteLine($"Your message will be sent on {_dateForSend}");
            }
        }

        static void WaitDate()
        {
            _timer = new Timer(24 * 60 * 60 * 1000); 
            _timer.Elapsed += CheckDate;
            _timer.AutoReset = true;
            _timer.Start();
        }

        static void CheckDate(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now >= _dateForSend)
            {
                SendMessage();
                _timer.Stop();
            }
        }
    
        static void SendMessage()
        {
            MailMessage post = new MailMessage();
            post.From = new MailAddress("ushachovg324@gmail.com");
            post.To.Add(_email);
            post.Subject = _subject;
            post.Body = _body;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("ushachovg324@gmail.com", "udph dsed shvu scjw");
            smtpClient.Send(post);

            Console.WriteLine("Message sent successfully");
        }
    }
}
