using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    public class Email
    {
        public static string CreateResetPasswordEmail(string name, string url)
        {
            string body = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html>
<head>
        
        <meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
        
        <meta property=""og:title"" content=""*|MC:SUBJECT|*"" />
        
<title>*|MC:SUBJECT|*</title>
	
</head>
<body leftmargin=""0"" marginwidth=""0"" topmargin=""0"" marginheight=""0"" offset=""0"" style=""width: 100% !important; -webkit-text-size-adjust: none; margin-top: 0; margin-right: 0; margin-bottom: 0; margin-left: 0; padding-top: 0; padding-right: 0; padding-bottom: 0; padding-left: 0; background-color: #FFFFFF;"" bgcolor=""#FFFFFF"">
    <center>
        <table border=""0"" cellpadding=""0"" cellspacing=""0"" height=""100%"" width=""100%"" id=""backgroundTable"" style=""height: 100% !important; margin-bottom: 0; margin-top: 0; margin-right: 0; margin-left: 0; padding-left: 0; padding-top: 0; padding-bottom: 0; padding-right: 0; width: 100% !important;"">
            <tr>
                <td align=""center"" valign=""top"">
                    <table border=""0"" cellpadding=""20"" cellspacing=""0"" id=""templateContainer"" width=""760"" style=""font-family: Verdana; margin-top: 20px; font-size: 13px; border-top-color: #dddddd; border-right-color: #dddddd; border-bottom-color: #dddddd; border-left-color: #dddddd; border-top-style: solid; border-right-style: solid; border-bottom-style: solid; border-left-style: solid; border-top-width: 1px; border-right-width: 1px; border-bottom-width: 1px; border-left-width: 1px; background: #FAFAFA;"" bgcolor=""#FAFAFA"">
                        <tr>
							<td colspan=""2"" style=""background:#9AB2B2; text-align: left;"">
								<a href=""#"" style=""font-size:32px;font-weight:bold; color: white;"">The Adz</a>
							</td>
						</tr>
						<tr>
							<td colspan=""2"" style=""font-size:22px;"">
								Password Reset Request Received
							</td>
						</tr>
						<tr>
							<td colspan=""2"" style=""font-size:14px;"">
								Dear " + name + @",<br/><br/>
								We received a request to reset your account's password.<br/>
								To reset, please click on the button below.
								<br/><br/><br/>
								<div style=""text-align:center;"">
									<a href=""" + url + @""" style=""color: white;background: #72A1A1;padding: 10px;text-decoration: none;"">Reset Password</a><br/><br/>
								</div>
							</td>
						</tr>
                    </table>
                    <br />
                </td>
            </tr>
        </table>
    </center>
    
<style type=""text/css"">
body { width: 100% !important; }
body { -webkit-text-size-adjust: none !important; }
body { margin: 0 !important; padding: 0 !important; }
a{
	color:blue;
	text-decoration:none;
}
#backgroundTable { height: 100% !important; margin: 0 !important; padding: 0 !important; width: 100% !important; }
body { background-color: #FFFFFF !important; }
</style>
</body>
</html>";
            return body;
        }

        public static string CreateVerificationEmail(string VerificationLink)
        {
            string body = @"<html>
<body style=\""font-family: Tahoma, Geneva, sans-serif;\"">
<table width=""100%"" border=""0"" cellspacing=""0"" cellpadding=""0""><tr><td align=""center"">
<div style=""width: 420px; text-align: left;"">
	<img src=""http://theadzdemo.cloudapp.net/TheAdzProd/Content/theadz.jpg"" style=""max-height: 40px;"">
	<h2 style=""color: #6AA84F;"">Hello! <img goomoji=""1f600"" data-goomoji=""1f600"" style=""margin:0 0.2ex;vertical-align:middle;max-height:24px"" alt=""😀"" src=""https://mail.google.com/mail/e/1f600""></h2>
	<p style=""font-size: 13px;"">Thank you for signing up with Adz. We're can't wait to start your experience with us.</p>
	<h2 style=""color: #6AA84F;"">Let's get started!</h2>
	<p style=""font-size: 13px;"">Adz is the lockscreen app that rewards you the longer you use it. Earn points daily by running the app. When you have enough, cash out or spend it in our ever-growing rewards store!</p>
	<p style=""font-size: 13px;"">To get started, please verify your account at the link below:</p>
	<a href=""" + VerificationLink + @""" style=""font-size: 13px;"">" + VerificationLink + @"</a>
	<h2 style=""color: #6AA84F;"">Earn, Redeem, Enjoy!</h2>
	<p style=""font-size: 13px;"">We give as good as you earn. Our ever-growing reward store partners with the brands you love for great redeemable goodies.</p>
	<p style=""font-size: 13px;"">Here's a look at what you can get:</p>
	<ul>
		<li style=""font-size: 13px;""><img goomoji=""1f4f2"" data-goomoji=""1f4f2"" style=""margin:0 0.2ex;vertical-align:middle;max-height:24px"" alt=""📲"" src=""https://mail.google.com/mail/e/1f4f2""> <b>mobile reloads</b></li>
		<li style=""font-size: 13px;""><img goomoji=""1f3ac"" data-goomoji=""1f3ac"" style=""margin:0 0.2ex;vertical-align:middle;max-height:24px"" alt=""🎬"" src=""https://mail.google.com/mail/e/1f3ac""> <b>movie tickets</b></li>
		<li style=""font-size: 13px;""><img goomoji=""1f366"" data-goomoji=""1f366"" style=""margin:0 0.2ex;vertical-align:middle;max-height:24px"" alt=""🍦"" src=""https://mail.google.com/mail/e/1f366""> <b>food vouchers</b></li>
		<li style=""font-size: 13px;""><img goomoji=""1f6cd"" data-goomoji=""1f6cd"" style=""margin:0 0.2ex;vertical-align:middle;max-height:24px"" alt=""🛍"" src=""https://mail.google.com/mail/e/1f6cd""> <b>shopping vouchers</b></li>
		<li style=""font-size: 13px;""><img goomoji=""1f4b0"" data-goomoji=""1f4b0"" style=""margin:0 0.2ex;vertical-align:middle;max-height:24px"" alt=""💰"" src=""https://mail.google.com/mail/e/1f4b0""> <b>cash</b></li>
		<li style=""font-size: 13px;""><img goomoji=""1f484"" data-goomoji=""1f484"" style=""margin:0 0.2ex;vertical-align:middle;max-height:24px"" alt=""💄"" src=""https://mail.google.com/mail/e/1f484""><img goomoji=""1f4ff"" data-goomoji=""1f4ff"" style=""margin:0 0.2ex;vertical-align:middle;max-height:24px"" alt=""📿"" src=""https://mail.google.com/mail/e/1f4ff""> <b>and many more!</b> <img goomoji=""1f3a4"" data-goomoji=""1f3a4"" style=""margin:0 0.2ex;vertical-align:middle;max-height:24px"" alt=""🎤"" src=""https://mail.google.com/mail/e/1f3a4""><img goomoji=""2615"" data-goomoji=""2615"" style=""margin:0 0.2ex;vertical-align:middle;max-height:24px"" alt=""☕"" src=""https://mail.google.com/mail/e/2615""></li>
	</ul>
	<p style=""font-size: 13px;"">Plus! You'll also benefit from our partner companies with user exclusive discounts, freebies and flash deals.</p>
	<h2 style=""color: #6AA84F;"">Refer for more</h2>
	<p style=""font-size: 13px;"">Sharing Adz with friends and family earns you more points, and faster! Use your unique referral code (on the app) to get both you and a friend <b>200 points each</b> for the referral.</p>
	<h2 style=""color: #6AA84F;"">Lastly, don't forget to feedback!</h2>
	<p style=""font-size: 13px;"">Tell us how we're doing. Good or bad - we wan't to hear it. It may even earn you extra points because we're all about rewarding :)</p>
	<p style=""font-size: 13px;"">Send your feedback to this email or write to us at <a href=""mailto:hello@thadz.com"" style=""font-size: 13px;"">hello@thadz.com</a></p>
	<p style=""font-size: 13px;"">Let's start your Adz experience now!</p>
	<h2 style=""color: #6AA84F;"">Happy Earning!</h2>
	<p style=""font-size: 13px;"">- - </p>
	<p style=""font-size: 13px;"">The Adz Team</p>
	<a href=""www.facebook.com/AdzMalaysia"" style=""font-size: 13px;"">www.facebook.com/AdzMalaysia</a> 
	<p style=""font-size: 13px;"">P.S: We sometimes post exclusive flash deals on Adz. ""Like"" our page to not miss out!</p>
</div>
</td></tr></table>
</body>
</html>";
            return body;
        }
    }
}
