using System.Net;
using System.Net.Mail;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    MailAddress fromAddress = new MailAddress(GetEnvironmentVariable("FROM_EMAIL")));
    MailAddress toAddress = new MailAddress(GetEnvironmentVariable("TEST_TO_EMAIL"));
    string fromPassword = GetEnvironmentVariable("FROM_PASSWORD");
    const string subject = "Merry Christmas - You accepted Neil's gift!"
    const string body = "";

    log.Info("C# HTTP trigger function processing the AcceptGift request.");

    var smtp = new SmtpClient
    {
        Host = "smtp.gmail.com",
        Port = 587,
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredentials(fromAddress.Address, fromPassword)        
    };

    using(var message = new MailMessage(fromAddress, toAddress))
    {
        Subject = subject,
        Body = body
    };
    try 
    {
        await smtp.SendMailAsync(Message);
    }
    catch(Exception ex)
    {
        return req.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
    }

    return req.CreateResponse(HttpStatusCode.OK, "Mail sent,");
}