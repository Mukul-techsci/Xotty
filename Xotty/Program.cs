using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

#region Services

// Add MVC
builder.Services.AddControllersWithViews();

// Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// HTTP Client
builder.Services.AddHttpClient();

// Authentication & Authorization
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

// Response Caching
builder.Services.AddResponseCaching();

// Email Service
builder.Services.AddScoped<IEmailService, EmailService>();

#endregion

var app = builder.Build();

#region Middleware

// Error Handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

// CORS
app.UseCors("AllowAll");

// Session
app.UseSession();

// Response Cache
app.UseResponseCaching();

// Authentication
app.UseAuthentication();

app.UseAuthorization();

#endregion

#region Routing

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Portfolio}/{action=Portfolio}/{id?}");

#endregion

app.Run();

#region Email Service

/// <summary>
/// Email Service Interface
/// </summary>
public interface IEmailService
{
    Task<bool> SendEmailAsync(
        string to,
        string subject,
        string htmlContent,
        IFormFile? attachment = null
    );
}

/// <summary>
/// Email Service Implementation
/// </summary>
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(
        IConfiguration configuration,
        ILogger<EmailService> logger
    )
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(
        string to,
        string subject,
        string htmlContent,
        IFormFile? attachment = null
    )
    {
        try
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            string smtpServer = emailSettings["SmtpServer"]!;
            int smtpPort = Convert.ToInt32(emailSettings["SmtpPort"]);
            string senderEmail = emailSettings["SenderEmail"]!;
            string senderPassword = emailSettings["SenderPassword"]!;
            bool enableSSL = Convert.ToBoolean(emailSettings["EnableSSL"]);

            using (SmtpClient client = new SmtpClient(smtpServer, smtpPort))
            {
                client.EnableSsl = enableSSL;

                client.Credentials = new NetworkCredential(
                    senderEmail,
                    senderPassword
                );

                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(
                        senderEmail,
                        "Portfolio Website"
                    );

                    message.To.Add(to);

                    message.Subject = subject;

                    message.Body = htmlContent;

                    message.IsBodyHtml = true;

                    // File Attachment
                    if (attachment != null && attachment.Length > 0)
                    {
                        var stream = new MemoryStream();

                        await attachment.CopyToAsync(stream);

                        stream.Position = 0;

                        Attachment mailAttachment = new Attachment(
                            stream,
                            attachment.FileName
                        );

                        message.Attachments.Add(mailAttachment);
                    }

                    await client.SendMailAsync(message);
                }
            }

            _logger.LogInformation("Email sent successfully to {Email}", to);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Email sending failed");

            return false;
        }
    }
}

#endregion