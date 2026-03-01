using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace ProjectVitour.Services.EmailServices
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendReservationSuccessEmailAsync(string toEmail, string userName, string tourName, string tourDate, string price, int personCount, string reservationCode)
        {
            // PROFESYONEL HTML MAİL ŞABLONU
            string htmlTemplate = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <style>
                    body {{ font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; background-color: #f4f7f6; margin: 0; padding: 0; }}
                    .container {{ max-width: 600px; margin: 40px auto; background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 10px 30px rgba(0,0,0,0.05); }}
                    .header {{ background-color: #1a6b4a; color: #ffffff; text-align: center; padding: 40px 20px; }}
                    .header h1 {{ margin: 0; font-size: 28px; letter-spacing: 1px; }}
                    .header p {{ margin: 10px 0 0 0; opacity: 0.8; font-size: 14px; }}
                    .content {{ padding: 40px; color: #444444; line-height: 1.6; }}
                    .content h2 {{ color: #1a6b4a; font-size: 22px; margin-top: 0; }}
                    .reservation-code-box {{ background-color: #e2f1e9; border: 2px dashed #1a6b4a; text-align: center; padding: 15px; margin: 20px 0; border-radius: 8px; }}
                    .reservation-code-box .code {{ font-size: 32px; font-weight: bold; color: #1a6b4a; letter-spacing: 2px; }}
                    .reservation-code-box .label {{ font-size: 14px; color: #64748b; text-transform: uppercase; margin-bottom: 5px; }}
                    .details-box {{ background-color: #f8fafc; border-radius: 8px; padding: 20px; margin-top: 25px; border-left: 4px solid #1a6b4a; }}
                    .details-table {{ width: 100%; border-collapse: collapse; }}
                    .details-table th, .details-table td {{ padding: 12px 0; border-bottom: 1px solid #e2e8f0; text-align: left; font-size: 15px; }}
                    .details-table th {{ color: #64748b; font-weight: 500; width: 45%; }}
                    .details-table td {{ font-weight: 700; color: #0f172a; text-align: right; }}
                    .details-table tr:last-child th, .details-table tr:last-child td {{ border-bottom: none; padding-bottom: 0; }}
                    .details-table tr:first-child th, .details-table tr:first-child td {{ padding-top: 0; }}
                    .footer {{ background-color: #ffffff; text-align: center; padding: 20px; font-size: 12px; color: #94a3b8; border-top: 1px solid #f1f5f9; }}
                    .button-container {{ text-align: center; margin-top: 35px; }}
                    .btn {{ background-color: #1a6b4a; color: #ffffff; text-decoration: none; padding: 14px 30px; border-radius: 50px; font-weight: bold; display: inline-block; transition: background 0.3s; }}
                    .support-msg {{ margin-top: 25px; font-size: 14px; background-color: #fffbeb; border-left: 4px solid #f59e0b; padding: 15px; border-radius: 4px; color: #78350f; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>Vitour Seyahat Acentası</h1>
                        <p>Dünyayı bizimle keşfedin</p>
                    </div>
                    <div class='content'>
                        <h2>Merhaba {userName},</h2>
                        <p>Harika haber! Rezervasyon işleminiz sistemimize başarıyla ulaştı. Hayalinizdeki tatil için ilk adımı attınız, gerisini bize bırakın.</p>
                        
                        <div class='reservation-code-box'>
                            <div class='label'>Rezervasyon Kodunuz</div>
                            <div class='code'>{reservationCode}</div>
                        </div>

                        <div class='details-box'>
                            <table class='details-table'>
                                <tr>
                                    <th>Seçilen Tur:</th>
                                    <td>{tourName}</td>
                                </tr>
                                <tr>
                                    <th>Kişi Sayısı:</th>
                                    <td>{personCount} Kişi</td>
                                </tr>
                                <tr>
                                    <th>Toplam Tutar:</th>
                                    <td style='color: #1a6b4a; font-size: 18px;'>{price}</td>
                                </tr>
                                <tr>
                                    <th>İşlem Tarihi:</th>
                                    <td>{tourDate}</td>
                                </tr>
                            </table>
                        </div>

                        <p style='margin-top: 30px;'>En kısa sürede müşteri temsilcilerimiz ödeme ve seyahat belgelerinizin onayı için sizinle iletişime geçecektir. Şimdiden iyi tatiller dileriz!</p>
                        
                        <div class='support-msg'>
                            <strong>💡 Bilgilendirme:</strong> Herhangi bir destek ihtiyacınızda veya rezervasyonunuzla ilgili değişiklik taleplerinizde, size özel oluşturulan bu <strong>{reservationCode}</strong> referans kodu ile müşteri hizmetlerimizden öncelikli ve kesintisiz yardım alabilirsiniz.
                        </div>
                        
                        <div class='button-container'>
                            <a href='#' class='btn' style='color:white;'>Müşteri Paneline Git</a>
                        </div>
                    </div>
                    <div class='footer'>
                        <p>© {DateTime.Now.Year} Vitour. Tüm hakları saklıdır.</p>
                        <p>Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayınız.</p>
                    </div>
                </div>
            </body>
            </html>";

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Vitour Rezervasyon", _config["SmtpSettings:Email"] ?? "no-reply@vitour.com"));
            email.To.Add(new MailboxAddress(userName, toEmail));
            email.Subject = "🎉 Rezervasyonunuz Başarıyla Alındı - Vitour";

            var builder = new BodyBuilder { HtmlBody = htmlTemplate };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            try
            {
                string smtpServer = _config["SmtpSettings:Server"] ?? "smtp.gmail.com";
                int smtpPort = int.TryParse(_config["SmtpSettings:Port"], out int port) ? port : 587;
                string senderEmail = _config["SmtpSettings:Email"];
                
                // GITHUB GÜVENLİĞİ: Şifre kesinlikle User Secrets (.env muadili) içinden okunmalıdır.
                string senderPassword = _config["SmtpSettings:Password"]; 

                if(string.IsNullOrEmpty(senderEmail) || string.IsNullOrEmpty(senderPassword))
                {
                    // Uygulama çökmesin diye log atıp geçiyoruz, eğer ayar yapılmadıysa mail atmaz ama rezervasyon başarılı olur.
                    Console.WriteLine("-----------------------------------------------------");
                    Console.WriteLine("UYARI: SMTP ayarları veya şifre eksik olduğu için mail gönderilemedi.");
                    Console.WriteLine("Lütfen User Secrets üzerinden şifrenizi tanımlayın.");
                    Console.WriteLine("-----------------------------------------------------");
                    return;
                }

                await smtp.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(senderEmail, senderPassword);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mail Gönderim Hatası: {ex.Message}");
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }
    }
}