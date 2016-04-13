using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using System.IO;
using System.Net.Mail;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Message = Google.Apis.Gmail.v1.Data.Message;

namespace gmailDemo
{
    public partial class Form1 : Form
    {
        private GmailService gs;
        public Form1()
        {
            InitializeComponent();
        }

        public Form1(GmailService gs)
        {
            InitializeComponent();
            this.gs = gs;
        }

        private void toonEmail(GmailService gs)
        {
            UsersResource.MessagesResource.ListRequest request2 = gs.Users.Messages.List("me");
            IList<Google.Apis.Gmail.v1.Data.Message> mails = request2.Execute().Messages;
            textBox4.Text = "";
            foreach (var mail in mails)
            {
                textBox4.Text += mail.Id + "\n";
            }
           // textBox4.Text = mails.ToString();
        }

        private void zendEmail(GmailService gs, string to, string sub, string body)
        {
            var msg = new AE.Net.Mail.MailMessage
            {
                Subject = textBox2.Text,
                Body = textBox3.Text,
                From = new MailAddress("placeholder@gmail.com")
            };
            msg.To.Add(new MailAddress(textBox1.Text));
            msg.ReplyTo.Add(msg.From); // Bounces without this!!
            var msgStr = new StringWriter();
            msg.Save(msgStr);

          
            var result = gs.Users.Messages.Send(new Message
            {
                Raw = Base64UrlEncode(msgStr.ToString())
            }, "me").Execute();
            Console.WriteLine("Message ID {0} sent.", result.Id);

        }

        private static string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            // Special "url-safe" base64 encode.
            return Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            zendEmail(gs, textBox1.Text, textBox2.Text, textBox3.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            toonEmail(gs);
        }
    }

}
