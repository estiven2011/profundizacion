using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace gestionReservas.Services
{
    public class EmailService
    {
        // Cambia esto por tu Gmail real
        private readonly string correoOrigen = "estivenposadarua@gmail.com";

        // Contraseña de aplicación generada desde la configuración de seguridad de Gmail
        private readonly string contraseñaApp = "kfdv tvmf xmlp lamw";

        private readonly string smtpHost = "smtp.gmail.com";
        private readonly int smtpPort = 587;

        public async Task EnviarCorreoAsync(string destino, string asunto, string cuerpo)
        {
            var mensaje = new MailMessage
            {
                From = new MailAddress(correoOrigen),
                Subject = asunto,
                Body = cuerpo,
                IsBodyHtml = false
            };

            mensaje.To.Add(destino);

            using var smtp = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(correoOrigen, contraseñaApp),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mensaje);
        }
    }
}
