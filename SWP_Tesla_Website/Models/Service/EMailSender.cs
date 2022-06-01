using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP_Tesla_Website.Models.Service {
    public class EMailSender {
        public async Task SendEmailAsync(Email mail) {//Mail funktioniert nicht. Grund: ?
            MimeMessage msg = new MimeMessage();
            msg.From.Add(new MailboxAddress("Test", "teslanotify@gmx.at"));
            msg.To.Add(new MailboxAddress("Test1", "amuehlegger@tsn.at"));
            msg.Subject = mail.Subject;
            msg.Body = new TextPart("plain") {
                Text = mail.msg
            };

            using (SmtpClient client = new SmtpClient()) {
                client.Connect("mail.gmx.net", 587, false);
                client.Authenticate("teslanotify@gmx.at", "TeslaVogt2022");
                client.Send(msg);
                client.Disconnect(true);
            }
        }
    }
}
