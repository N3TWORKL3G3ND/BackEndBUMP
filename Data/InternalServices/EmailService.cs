using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;

using MailKit.Net.Smtp;
using MimeKit;






namespace Data.InternalServices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_configuration["EmailSettings:SenderName"], _configuration["EmailSettings:SenderEmail"]));
                email.To.Add(new MailboxAddress("", to));
                email.Subject = subject;
                email.Body = new TextPart("plain") { Text = body };

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:SmtpPort"]), true);
                    await client.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
                    await client.SendAsync(email);
                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw new InternalApplicationException(ErrorCodigo.ErrorAlGenerarCorreo,
                    $"Error al enviar correo: {ex.Message}", ex);
            }
        }
    }




}
}
