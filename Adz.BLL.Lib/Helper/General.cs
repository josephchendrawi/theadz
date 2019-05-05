using Adz.DAL.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    public class General
    {
        public static void LoggingException(string ip, string url, string exception, string message, string user)
        {
            using (var context = new TheAdzEntities())
            {
                try
                {
                    WebExceptionLogger log = new WebExceptionLogger();
                    log.last_created = DateTime.UtcNow;
                    log.url = url;
                    log.ip_address = ip;
                    if(user == "" || user == null)
                        user = Thread.CurrentPrincipal.Identity.Name;
                    log.logger = user;
                    log.err_exception = exception;
                    log.err_message = message;
                    context.WebExceptionLoggers.Add(log);
                    context.SaveChanges();
                }
                catch (Exception) { }
            }
        }

        public static User GetUserData(string email, string uniqueid)
        {
            using (var context = new TheAdzEntities())
            {
                User us = new User();

                var entityUser = from d in context.Users
                                 where d.email.ToLower() == email
                                 && d.locked_status == false
                                 && (d.userstatus_id == 1 || d.userstatus_id == 4)
                                 select d;
                if (entityUser.Count() > 0)
                {
                    var entityAPIKeys = from d in context.APIKeys
                                        where d.unique_id == uniqueid
                                        && d.user_id == entityUser.FirstOrDefault().id
                                        select d;
                    if (entityAPIKeys.Count() > 0)
                    {
                        return new User
                        {
                            UserId = entityUser.First().id,
                            FirstName = entityUser.First().first_name,
                            LastName = entityUser.First().last_name,
                            Email = entityUser.First().email,
                            Gender = (int)entityUser.First().gender,
                            DateOfBirth = entityUser.First().dateofbirth == null ? DateTime.MinValue : (DateTime)entityUser.First().dateofbirth,
                            Debug = (int)entityAPIKeys.First().flag_debug
                        };
                    }
                }

                throw new CustomException(CustomErrorType.UserServiceNotFound);
            }
        }

        public static User checkAuth(string key, string username, string uniqueid)
        {
            using (var context = new TheAdzEntities())
            {
                if (key == null || username == null || uniqueid == null)
                {
                    throw new CustomException(CustomErrorType.Unauthenticated);
                }
                else
                {
                    User user = checkApi(key, username, uniqueid);
                    if (user != null)
                    {
                        return user;
                    }
                    else
                    {
                        throw new CustomException(CustomErrorType.Unauthenticated);
                    }
                }
            }
        }

        public static User checkApi(string key, string username, string uniqueid)
        {
            try
            {
                using (var context = new TheAdzEntities())
                {
                    var entityUser = from d in context.Users
                                     where d.email.ToLower() == username.ToLower()
                                     && d.locked_status == false
                                     && (d.userstatus_id == 1 || d.userstatus_id == 4)
                                     select d;
                    if (entityUser.Count() > 0)
                    {
                        var entityAPIKeys = from d in context.APIKeys
                                            where (d.token == key /*&& d.token_expire >= DateTime.UtcNow*/
                                            || (d.token_old == key /*&& d.token_old_expire >= DateTime.UtcNow*/))
                                            && d.unique_id == uniqueid
                                            && d.user_id == entityUser.FirstOrDefault().id
                                            select d;
                        if (entityAPIKeys.Count() > 0)
                        {
                            return new User
                            {
                                FirstName = entityUser.First().first_name,
                                LastName = entityUser.First().last_name,
                                Email = entityUser.First().email,
                                Gender = (int)entityUser.First().gender,
                                DateOfBirth = entityUser.First().dateofbirth == null ? DateTime.MinValue : (DateTime)entityUser.First().dateofbirth,
                                Debug = (int)entityAPIKeys.First().flag_debug
                            };
                        }
                    }
                }
            }
            catch
            {
                throw new CustomException(CustomErrorType.Unauthenticated);
            }

            return null;
        }

        public static string RegenerateAPIKey(string key, string username, string uniqueid)
        {
            try
            {
                using (var context = new TheAdzEntities())
                {
                    var entityUser = from d in context.Users
                                     where d.email.ToLower() == username.ToLower()
                                     && d.locked_status == false
                                     && (d.userstatus_id == 1 || d.userstatus_id == 4)
                                     select d;
                    if (entityUser.Count() > 0)
                    {
                        var entityAPIKeys = from d in context.APIKeys
                                            where d.token == key && d.user_id == entityUser.FirstOrDefault().id
                                            //&& d.token_expire >= DateTime.UtcNow
                                            && d.unique_id == uniqueid
                                            select d;
                        if (entityAPIKeys.Count() > 0)
                        {
                            string tokensalt = Security.RandomString(60);
                            string token = Security.Encrypt(tokensalt, entityUser.First().email.ToLower());

                            entityAPIKeys.First().token = token;
                            entityAPIKeys.First().token_salt = tokensalt;
                            entityAPIKeys.First().token_expire = DateTime.UtcNow.AddMonths(1);

                            //save old-key
                            entityAPIKeys.First().token_old = key;
                            entityAPIKeys.First().token_old_expire = DateTime.UtcNow.AddHours(2);

                            context.SaveChanges();

                            return token;
                        }
                        else
                        {
                            //if using old-key, no need to generate new key.
                            var entityAPIKeys2 = from d in context.APIKeys
                                                 where d.token_old == key && d.token_expire >= DateTime.UtcNow
                                                 && d.user_id == entityUser.FirstOrDefault().id
                                                 && d.unique_id == uniqueid
                                                 select d;
                            if (entityAPIKeys2.Count() > 0)
                            {
                                return entityAPIKeys2.First().token;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw new CustomException(CustomErrorType.KeyGeneratingError);
            }

            return null;
        }

        public static void SendEmail(string subject, string to, string body, string cc = "")
        {
            System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
            email.From = new MailAddress("noreply@domain.com", "The Adz");///
            email.To.Add(to);
            if (cc != "")
            {
                email.CC.Add(cc);
            }
            email.Subject = subject;
            email.IsBodyHtml = true;
            email.Body += body;
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Credentials = new NetworkCredential("noreply@domain.com", "password");///
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.EnableSsl = true;
            try
            {
                //try to send port 587  first 
                smtpClient.Port = 587;
                smtpClient.Send(email);
                using (var context = new TheAdzEntities())
                {
                    Adz.DAL.EF.EmailSendLog em = new Adz.DAL.EF.EmailSendLog();
                    em.email = to;
                    em.status = 1;
                    em.subject = subject;
                    em.creationdate = DateTime.UtcNow;
                    context.EmailSendLogs.Add(em);
                    context.SaveChanges();
                }
            }
            catch
            {
                using (var context = new TheAdzEntities())
                {
                    Adz.DAL.EF.EmailSendLog em = new Adz.DAL.EF.EmailSendLog();
                    em.email = to;
                    em.status = 0;
                    em.subject = subject;
                    em.creationdate = DateTime.UtcNow;
                    context.EmailSendLogs.Add(em);
                    context.SaveChanges();
                }

                //try to send port 26  then 
                smtpClient.Port = 26;
                smtpClient.Send(email);
                using (var context = new TheAdzEntities())
                {
                    Adz.DAL.EF.EmailSendLog em = new Adz.DAL.EF.EmailSendLog();
                    em.email = to;
                    em.status = 1;
                    em.subject = subject;
                    em.creationdate = DateTime.UtcNow;
                    context.EmailSendLogs.Add(em);
                    context.SaveChanges();
                }
            }
        }

        public static string uploadImage(string ImageByte, string mimetype, string date, string userid)
        {
            if (ImageByte != "" && mimetype != "")
            {
                byte[] imgByte = Convert.FromBase64String(ImageByte);
                MemoryStream ms = new MemoryStream(imgByte);
                string filename = "profileimg" + date + "-" + userid;
                filename = filename.Replace(" ", "-") + "." + mimetype;
                string path = System.Web.Hosting.HostingEnvironment.MapPath("~/bin");
                string be = ConfigurationManager.AppSettings["upload-dir-before"];
                string af = ConfigurationManager.AppSettings["upload-dir-after"];
                path = path.Replace(be, af) + @"\" + filename;
                FileStream fs = new FileStream(path, FileMode.Create);
                ms.WriteTo(fs);

                return filename;
            }
            else
            {
                throw new CustomException(CustomErrorType.ImageError);
            }
        }

    }
}
