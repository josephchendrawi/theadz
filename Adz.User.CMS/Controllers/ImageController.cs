using Adz.BLL.Lib;
using Adz.User.CMS.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Adz.User.CMS.Controllers
{
    public class ImageController : BaseAdminController
    {
        public IImageService imageservice = new ImageService();
        
        public ActionResult ImageMulti()
        {
            return View();
        }

        public ActionResult ImageSingle()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ImageListUploadImageMulti(HttpPostedFileBase imageUpload)
        {
            bool isUpload = false;
            string message = "";
            string imageurl = "";
            string imagename = "";
            int result = 0;
            if (imageUpload != null)
            {
                try
                {
                    if (!Helper.IsValidImage(imageUpload.FileName))
                    {
                        message = "File is not an image file";
                    }
                    string name = DateTime.UtcNow.ToString("yyyyMMddhhmmss");
                    imagename = Helper.uploadImageWithName(imageUpload, name);
                    imageurl = ConfigurationManager.AppSettings["uploadpath"] + imagename;

                    System.Drawing.Image img = System.Drawing.Image.FromStream(imageUpload.InputStream);
                    Adz.BLL.Lib.Image pro = new Adz.BLL.Lib.Image();
                    pro.Name = imagename;
                    pro.Width = img.Width;
                    pro.Height = img.Height;

                    result = imageservice.CreateImageId(pro).Result;
                    isUpload = true;
                    message = "Image uploaded successfully!";
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    message = string.Format("File upload failed: {0}", ex.Message);
                }
            }
            return Json(new { isUpload = isUpload, message = message, imageurl = imageurl, imagename = imagename, imageid = result }, "text/html");
        }
        [HttpPost]
        public virtual ActionResult ImageListUploadImage()//name and strsubimage come from post jquery
        {
            HttpPostedFileBase imageUpload = Request.Files["imagefile"];
            bool isUpload = false;
            string message = "";
            string imageurl = "";
            string imagename = "";
            int result = 0;
            if (imageUpload != null)
            {
                try
                {
                    if (!Helper.IsValidImage(imageUpload.FileName))
                    {
                        message = "File is not an image file";
                    }
                    string name = DateTime.UtcNow.ToString("yyyyMMddhhmmss");
                    imagename = Helper.uploadImageWithName(imageUpload, name);
                    imageurl = ConfigurationManager.AppSettings["uploadpath"] + imagename;

                    Adz.BLL.Lib.Image pro = new Adz.BLL.Lib.Image();
                    pro.Name = imagename;

                    result = imageservice.CreateImageId(pro).Result;
                    isUpload = true;
                    message = "Image uploaded successfully!";
                }
                catch (Exception ex)
                {
                    CustomException(ex);
                    message = string.Format("File upload failed: {0}", ex.Message);
                }
            }
            return Json(new { isUpload = isUpload, message = message, imageurl = imageurl, imagename = imagename, imageid = result }, "text/html");
        }
        public JsonResult ImageListDataDraft(string imageid)
        {
            List<Adz.BLL.Lib.Image> listR = imageservice.GetImageListDescByImageId(imageid).Result;
            List<ImageModel.Image> newlistR = new List<ImageModel.Image>();
            foreach (var v in listR)
            {
                newlistR.Add(new ImageModel.Image() { ImageId = v.ImageId, Name = v.Name, Url = v.Url, LastAction = v.LastAction });
            }
            IEnumerable<ImageModel.Image> ienuList = newlistR;
            return Json(ienuList, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ImageListData(int skip, int take)
        {
            List<Adz.BLL.Lib.Image> listR = imageservice.GetImageListDescBySkipTake(skip, take).Result;
            List<ImageModel.Image> newlistR = new List<ImageModel.Image>();
            foreach (var v in listR)
            {
                newlistR.Add(new ImageModel.Image() { ImageId = v.ImageId, Name = v.Name, Url = v.Url, LastAction = v.LastAction });
            }
            IEnumerable<ImageModel.Image> ienuList = newlistR;
            return Json(ienuList, JsonRequestBehavior.AllowGet);
        }

        ////Image Crop Upload
        
        public ActionResult ImageCropUpload()
        {
            //Just to distinguish between ajax request (for: modal dialog) and normal request
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }

            return View();
        }

        public ActionResult ImageCropUploadSuccess(int ImageId = 0, string ImageName = "", string ImageURL = "")
        {
            if (ImageId != 0 && ImageName != "" && ImageURL != "")
            {
                ViewBag.ImageId = ImageId;
                ViewBag.ImageName = ImageName;
                ViewBag.ImageURL = ImageURL;
            }

            return View();
        }
        
        [HttpPost]
        public ActionResult ImageCropUpload(Adz.User.CMS.Models.ImageModel.UploadImageModel model)
        {
            if (ModelState.IsValid)
            {
                Bitmap original = null;
                var name = "newimagefile";
                var extension = "";
                var errorField = string.Empty;

                if (model.IsUrl)
                {
                    errorField = "Url";
                    name = GetUrlFileName(model.Url);
                    extension = GetUrlFileExtension(model.Url);
                    original = GetImageFromUrl(model.Url);
                }
                else if (model.File != null)
                {
                    errorField = "File";
                    name = Path.GetFileNameWithoutExtension(model.File.FileName);
                    extension = Path.GetExtension(model.File.FileName);
                    original = Bitmap.FromStream(model.File.InputStream) as Bitmap;
                }

                //name with datetime [optional]
                name = DateTime.UtcNow.ToString("yyyyMMddhhmmss");
                extension = extension == "" ? ".jpg" : extension.ToLower();

                if (original != null)
                {
                    ////save image to folder & compression
                    var img = CreateImage(original, model.X, model.Y, model.Width, model.Height);
                    var fn = Server.MapPath("~/Content/upload/" + name + extension);
                    ////jpg compression
                    if (extension == ".jpg" || extension == ".jpeg")
                    {
                        ImageCodecInfo myImageCodecInfo = GetEncoder(ImageFormat.Jpeg);
                        Encoder myEncoder = Encoder.Quality;
                        EncoderParameters myEncoderParameters = new EncoderParameters(1);
                        EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 80L);
                        myEncoderParameters.Param[0] = myEncoderParameter;
                        img.Save(fn, myImageCodecInfo, myEncoderParameters);
                    }
                    else
                        img.Save(fn);

                    ////register image to db
                    string imagename = name + extension;
                    string imageurl = ConfigurationManager.AppSettings["uploadpath"] + imagename;

                    Adz.BLL.Lib.Image pro = new Adz.BLL.Lib.Image();
                    pro.Name = imagename;
                    pro.Width = model.Width;
                    pro.Height = model.Height;
                    pro.Url = fn;

                    int imageid = imageservice.CreateImageId(pro).Result;

                    ////when succeed
                    return RedirectToAction("ImageCropUploadSuccess", "Image", new { ImageId = imageid, ImageName = imagename, ImageURL = imageurl });
                }
                else
                    ModelState.AddModelError(errorField, "Your upload did not seem valid. Please try again using only correct images!");
            }

            return View(model);
        }

        //image compression by encoding
        public ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        Bitmap GetImageFromUrl(string url)
        {
            var buffer = 1024;
            Bitmap image = null;

            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                return image;

            using (var ms = new MemoryStream())
            {
                var req = WebRequest.Create(url);

                using (var resp = req.GetResponse())
                {
                    using (var stream = resp.GetResponseStream())
                    {
                        var bytes = new byte[buffer];
                        var n = 0;

                        while ((n = stream.Read(bytes, 0, buffer)) != 0)
                            ms.Write(bytes, 0, n);
                    }
                }

                image = Bitmap.FromStream(ms) as Bitmap;
            }

            return image;
        }

        string GetUrlFileName(string url)
        {
            var parts = url.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var last = parts[parts.Length - 1];
            return Path.GetFileNameWithoutExtension(last);
        }

        string GetUrlFileExtension(string url)
        {
            Uri myUri = new Uri(url);
            string path = String.Format("{0}{1}{2}{3}", myUri.Scheme, Uri.SchemeDelimiter, myUri.Authority, myUri.AbsolutePath);

            return Path.GetExtension(path);
        }

        Bitmap CreateImage(Bitmap original, int x, int y, int width, int height)
        {
            var img = new Bitmap(width, height);

            using (var g = Graphics.FromImage(img))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(original, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);
            }

            //resize when image is too big
            if (width > 720 || height > 1280)
            {
                Bitmap resized = new Bitmap(img, new Size(720, 1280));
                return resized;
            }

            return img;
        }

	}
}