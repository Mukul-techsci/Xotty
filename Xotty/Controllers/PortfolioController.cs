using EntityLayer;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using Xotty.Models;

namespace Xotty.Controllers
{
    public class PortfolioController : Controller
    {
        private readonly ILogger<PortfolioController> _logger;

        public PortfolioController(ILogger<PortfolioController> logger)
        {
            _logger = logger;
        }
        public IActionResult Portfolio()
        {
            return View();
        }

        /// <summary>
        /// Handle contact form submission
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmail(enPortfolio.ContactFormModel model)
        {
            try
            {              
               

                // Validate email format
                if (!IsValidEmail(model.Email))
                {
                    return BadRequest(new { success = false, message = "Invalid email address" });
                }

                // Send email
                bool emailSent = await SendEmailAsync(model);

                if (emailSent)
                {
                    return Ok(new { success = true, message = "Email sent successfully! I'll get back to you soon." });
                }
                else
                {
                    return StatusCode(500, new { success = false, message = "Failed to send email. Please try again." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendEmail action");
                return StatusCode(500, new { success = false, message = "An error occurred. Please try again later." });
            }
        }

        /// <summary>
        /// Send email with contact form details
        /// </summary>
        private async Task<bool> SendEmailAsync(enPortfolio.ContactFormModel model)
        {
            try
            {
               
                string senderEmail = "no-reply@chemanalyst.com"; 
                string senderPassword = "TEchsc%$@"; 
                string recipientEmail = "mukul.kumar@techsciresearch.com";
                string smtpServer = "mail.techsciresearch.com";
                int smtpPort = 587;

                using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(senderEmail, senderPassword);

                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(senderEmail, "Portfolio Contact Form");
                        message.To.Add(recipientEmail);
                        message.Subject = $"New Contact Form Submission - {model.Subject}";
                        message.IsBodyHtml = true;

                        // Create HTML email body
                        string emailBody = GenerateEmailBody(model);
                        message.Body = emailBody;

                        // Add attachment if file was uploaded
                        if (model.Attachment != null && model.Attachment.Length > 0)
                        {
                            // Validate file size (max 5MB)
                            if (model.Attachment.Length > 5 * 1024 * 1024)
                            {
                                _logger.LogWarning("File size exceeds limit for submission from {0}", model.Email);
                                return false;
                            }

                            // Validate file type
                            string[] allowedExtensions = { ".pdf", ".doc", ".docx", ".xls", ".xlsx" };
                            string fileExtension = Path.GetExtension(model.Attachment.FileName).ToLower();

                            if (!Array.Exists(allowedExtensions, element => element == fileExtension))
                            {
                                _logger.LogWarning("Invalid file type uploaded: {0}", fileExtension);
                                return false;
                            }

                            // Create attachment from uploaded file
                            var memoryStream = new MemoryStream();
                            await model.Attachment.CopyToAsync(memoryStream);
                            memoryStream.Position = 0;
                            message.Attachments.Add(new Attachment(memoryStream, model.Attachment.FileName));
                        }

                        // Send email
                        await client.SendMailAsync(message);
                    }
                }

                _logger.LogInformation("Email sent successfully from {0}", model.Email);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
                return false;
            }
        }

        /// <summary>
        /// Generate HTML email body
        /// </summary>
        private string GenerateEmailBody(enPortfolio.ContactFormModel model)
        {
            return $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: 'Arial', sans-serif; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; background-color: #f9f9f9; }}
                        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 20px; border-radius: 5px 5px 0 0; }}
                        .header h2 {{ margin: 0; }}
                        .content {{ background-color: white; padding: 20px; border: 1px solid #ddd; }}
                        .field {{ margin-bottom: 15px; }}
                        .label {{ font-weight: bold; color: #667eea; }}
                        .value {{ padding: 10px; background-color: #f5f5f5; border-radius: 3px; }}
                        .footer {{ background-color: #f5f5f5; padding: 15px; text-align: center; font-size: 12px; color: #999; border-radius: 0 0 5px 5px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h2>📧 New Contact Form Submission</h2>
                        </div>
                        <div class='content'>
                            <div class='field'>
                                <div class='label'>Name:</div>
                                <div class='value'>{model.Name}</div>
                            </div>
                            <div class='field'>
                                <div class='label'>Email:</div>
                                <div class='value'><a href='mailto:{model.Email}'>{model.Email}</a></div>
                            </div>
                            <div class='field'>
                                <div class='label'>Phone:</div>
                                <div class='value'>{(string.IsNullOrEmpty(model.Phone) ? "Not provided" : model.Phone)}</div>
                            </div>
                            <div class='field'>
                                <div class='label'>Subject:</div>
                                <div class='value'>{model.Subject}</div>
                            </div>
                            <div class='field'>
                                <div class='label'>Message:</div>
                                <div class='value'>{model.Message.Replace(Environment.NewLine, "<br>")}</div>
                            </div>
                            <div class='field'>
                                <div class='label'>Submitted At:</div>
                                <div class='value'>{DateTime.Now:yyyy-MM-dd HH:mm:ss}</div>
                            </div>
                            {(model.Attachment != null ? $"<div class='field'><div class='label'>Attachment:</div><div class='value'>{model.Attachment.FileName}</div></div>" : "")}
                        </div>
                        <div class='footer'>
                            <p>This is an automated email. Please do not reply to this address.</p>
                            <p>&copy; 2024 Mukul Kumar Portfolio. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>";
        }

        /// <summary>
        /// Validate email address format
        /// </summary>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Error page
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
