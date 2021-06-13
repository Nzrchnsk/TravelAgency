using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using static TravelAgency.Startup;

namespace TravelAgency.Helper
{
    public static class EmailHelper
    {
        public static async Task BuyTicket(string email, string ticketNumber, string[] tripName)
        {
            
            await SendEmailAsync(email, $"Билет турфирмы №{ticketNumber} ",
                $"Здравствуйте, вы купили билет у нашей турфирмы номер {ticketNumber}, по маршруту {string.Join(" - ", tripName)}");
        }
        
        private static async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(Configuration["SmtpSetting:Name"], Configuration["SmtpSetting:Address"]));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
             
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(Configuration["SmtpSetting:SmtpService"], Convert.ToInt32(Configuration["SmtpSetting:SmtpPort"]),
                    Convert.ToBoolean(Configuration["SmtpSetting:SmtpSSL"]));
                await client.AuthenticateAsync(Configuration["SmtpSetting:Address"], Configuration["SmtpSetting:SmtpPassword"]);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}