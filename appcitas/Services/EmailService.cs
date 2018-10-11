using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace appcitas.Services
{
    public class EmailService
    {
        //Asynchronous method to send emaill based on ContactForm object
        public async static Task EnviarEmail(string emailCliente, string nombreCliente, string tituloCorreo, string cuerpoCorreo, string asunto, string mensajeFinal,
            string mensajeFinal1, string mensajeFinal2, string mensajeFinal3, string mensajeFinal4, string mensajeFinal5, string mensajeFinal7)
        {

            
            string anio = System.DateTime.Now.ToString("yyyy");
            //Constructing the email
            string body = "<html>"+

                          "<meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>" +
                    "<body style='font-family: sans-serif; max-width: 750px; margin: auto;'>" +
                        "<div style='height: 60px; background: #E4002B;'>" +
                            "<img src='https://www1.sucursalelectronica.com/redir/redir2.0/images/common/bac-brand.png' style='max-height:70%; margin-top: 10px; margin-left: 10px;'>" +
                        "</div>" +
                        "<div style='padding: 30px; padding-top: 10px; color: rgb(31, 31, 31); background: rgb(250,250,250);'>" +
                            "<h2 style='text-align: center;color: rgb(228, 0, 43);font-size: 1.35rem;'>" + tituloCorreo + "</h2>" +
                            "<div>" + cuerpoCorreo + "</div>" +
                        "</div>" +
                        "<div style=' background: #E4002B;'>" +
                            "<p style='margin: 0px; color: rgb(255,255,255); text-align: center; font-size: 1.7rem; font-weight: bold; padding: 10px;'>" + mensajeFinal + "</p>" +
                        "</div>" +
                        "<div style='padding: 30px; padding-top: 4px; padding-bottom: 5px; background: rgb(250,250,250); color: rgb(150, 150, 150); font-size: 0.85rem; margin-bottom: 15px;'>" +
                            "<p>" + mensajeFinal1 + "</p>" +
                            "<p>" + mensajeFinal2 + "</p>" +
                            "<p style='display:none;'>" + mensajeFinal3 + "</p>" +
                            "<p style='display:none;'>" + mensajeFinal4 + "</p>" +
                            "<p style='display:none;'><b>" + mensajeFinal5 + "</p><br>" +
                            "<p style='width: 100%; border-top: 1px solid rgb(214, 214, 214); margin: auto;'></p>" +
                            "<p style='text-align: center; font-size: 90%;'>" + anio + " " + mensajeFinal7 + "</p>" +
                        "</div>" +
                    "</body>" +
                        "</html>";
            MailMessage message = new MailMessage();
            //if (directorioCita != "")
            //{
            //    Attachment citaArchivo = new Attachment(directorioCita);
            //    message.Attachments.Add(citaArchivo);
            //}
            message.To.Add(new MailAddress(emailCliente));
            message.Subject = asunto;
            message.Body = string.Format(body, nombreCliente, emailCliente);
            message.IsBodyHtml = true;
            //Attempting to send the email
            using (SmtpClient smtpClient = new SmtpClient())
            {
                        await smtpClient.SendMailAsync(message);
                }
           
        }
    }
}
