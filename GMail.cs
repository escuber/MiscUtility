using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace CardCells.utility
{
	public static class GMail
	{


		public static void SendEmail(string address, string subject, string message)
		{
			string email = "escuber@gmail.com";
			string password = "jimgjimg333";

			var loginInfo = new NetworkCredential(email, password);
			var msg = new MailMessage();
			var smtpClient = new SmtpClient("smtp.gmail.com", 587);

			msg.From = new MailAddress(email);
			msg.To.Add(new MailAddress(address));
			msg.Subject = subject;
			msg.Body = message;
			msg.IsBodyHtml = true;

			smtpClient.EnableSsl = true;
			smtpClient.UseDefaultCredentials = false;
			smtpClient.Credentials = loginInfo;
			smtpClient.Send(msg);
		}


	}
}