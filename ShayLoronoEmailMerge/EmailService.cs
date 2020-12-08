// C# SMTP Configuration for Outlook.Com SMTP Host
// https://www.codeproject.com/Articles/700211/Csharp-SMTP-Configuration-for-Outlook-Com-SMTP-Hos
// Allowing less secure apps to access your account in Gmail
// https://support.google.com/accounts/answer/6010255

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;

public static class EmailService
{
    private const string ACCOUNT_EMAIL = "noreply@bernardgabon.com";
    private const string ACCOUNT_PASSWORD = "rlBqvdnxuta@jzfkm8gceZw6oyb&ih";
    private const string SMTP_HOST = "mail.bernardgabon.com"; // smtp-mail.outlook.com, smtp.mail.yahoo.com
    private const int SMTP_PORT = 587;
    private const bool REQUIRE_SSL = false;
    private const char MULTI_MAILTO_SEPARATOR = ','; // semi-colon or comma
    private const bool IS_HTML = true;
    private static string SUBJECT_LABEL = "[BernardGabon.com]";

    #region EmailServiceMethods
    public static bool SendEmail(string mailTo, string subject, string body, string attachmentFile = "", string mailCc = "", string mailBc = "", string mailReplyTo = "")
    {
        var smtp = new SmtpClient()
        {
            Host = SMTP_HOST,
            Port = SMTP_PORT,
            EnableSsl = REQUIRE_SSL,
            UseDefaultCredentials = false,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Credentials = new System.Net.NetworkCredential(ACCOUNT_EMAIL, ACCOUNT_PASSWORD)
        };

        var mail = new MailMessage();
        mail.From = new MailAddress(ACCOUNT_EMAIL);
        string[] mailTos = mailTo.Split(MULTI_MAILTO_SEPARATOR).ToArray();
        foreach (var mailto in mailTos)
        {
            mail.To.Add(mailto);
        }
        if (!String.IsNullOrEmpty(mailCc)) mail.CC.Add(mailCc);
        if (!String.IsNullOrEmpty(mailBc)) mail.Bcc.Add(mailBc);
        if (!String.IsNullOrEmpty(mailReplyTo)) mail.ReplyToList.Add(mailReplyTo);
        mail.Subject = SUBJECT_LABEL + " " + subject;
        mail.IsBodyHtml = IS_HTML;
        mail.Body = body;

        try
        {
			if (!string.IsNullOrEmpty(attachmentFile))
            {
                mail.Attachments.Add(new Attachment(attachmentFile));
            }

            smtp.Send(mail);
            return true;
        }
        catch
        {
            return false;
        }
    }


    #endregion
}
