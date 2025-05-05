using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;
using Pustaksathi.Interface.Shared.Email;
using Pustaksathi.Model.Application.Members;
using Pustaksathi.Model.Shared.Email;
using Serilog;

namespace Pustaksathi.Services.Shared.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _emailOptions;

        public EmailService(IOptions<EmailOptions> options)
        {
            _emailOptions = options.Value;
        }

        public async Task SendOrderConfirmationAsync(Orders order, string userName, string userEmail, string? invoiceUrl = null)
        {
            try
            {
                Log.Information("==========================> Sending Email: SendOrderConfirmationAsync");
                var msg = new MimeMessage();
                msg.From.Add(new MailboxAddress(_emailOptions.Username, _emailOptions.Email));
                msg.To.Add(new MailboxAddress(userName, userEmail));

                msg.Subject = $"Order #{order.OrderId} Confirmation";
                var body = GetEmailBody(order, userName);
                msg.Body = new TextPart("html") { Text = body };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_emailOptions.SmtpServer, _emailOptions.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailOptions.Email, _emailOptions.AppPassword);
                await smtp.SendAsync(msg);
                await smtp.DisconnectAsync(true);

                Log.Information("==========================> Email Sent Successfully: SendOrderConfirmationAsync");
            }
            catch (Exception ex)
            {

                Log.Error("==========================> Error: SendOrderConfirmationAsync", ex.Message.ToString());
            }
        }

        //=================================
        //      Body Generator
        //=================================
        public string GetEmailBody(Orders orders, string userName)
        {
            return $@"
            <!DOCTYPE html>
            <html lang=""en"">
              <head>
                <meta charset=""UTF-8"">
                <style>
                  body {{
                    font-family: 'Helvetica', sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                  }}

                  .container {{
                    max-width: 600px;
                    margin: 20px auto;
                    background: #ffffff;
                    padding: 20px;
                    border-radius: 8px;
                  }}

                  .header {{
                    text-align: center;
                    padding-bottom: 10px;
                  }}

                  .header h1 {{
                    margin: 0;
                    color: #333;
                  }}

                  .content {{
                    line-height: 1.6;
                    color: #555;
                  }}

                  .btn {{
                    display: inline-block;
                    margin: 20px 0;
                    padding: 12px 20px;
                    background-color: #0078d7;
                    color: #fff;
                    text-decoration: none;
                    border-radius: 4px;
                  }}

                  .footer {{
                    font-size: 12px;
                    color: #999;
                    text-align: center;
                    margin-top: 30px;
                  }}
                </style>
              </head>
              <body>
                <div class=""container"">
                  <div class=""header"">
                    <h1>Thank you for your order!</h1>
                  </div>
                  <div class=""content"">
                    <p>Hi <strong>{userName}</strong>, </p>
                    <p>Your order has been successfully placed. Here are the details:</p>
                    <ul>
                      <li>
                         <strong>Membership Id:</strong> {orders.UserId}
                      </li>
                      <li>
                        <strong>Order Date:</strong> {orders.OrderDate:MMMM dd, yyyy}
                      </li>
                      <li>
                        <strong>Claim Code:</strong>
                        <code>{orders.ClaimCode}</code>
                      </li>
                      <li>
                        <strong>Total Amount:</strong> Rs {orders.TotalAmount}
                      </li>
                    </ul>
                    <p> Please present your membership ID and this claim code at our store to complete your pickup. </p>
                    <p> You can download or view your detailed invoice by clicking the button below: </p>
                    <p style=""text-align:center;"">
                      <a href="" class=""btn"">View Invoice</a>
                    </p>
                    <p>If you have any questions, reply to this email or contact our support team.</p>
                  </div>
                  <div class=""footer""> &copy; {DateTime.Now:yyyy} Your Book Store. All rights reserved. </div>
                </div>
              </body>
            </html>";
        }
    }
}
