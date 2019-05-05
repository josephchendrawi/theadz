using Adz.BLL.Lib;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Adz
{
    public static class Helper
    {
        public static string currAdminName
        {
            get
            {
                try
                {
                    var user = new AdminService().GetCurrentAdmin().Result;

                    return user.FirstName + " " + user.LastName;
                }
                catch
                {
                    return null;
                }
            }
        }
        public static int defaultGMT
        {
            get
            {
                int gmt = int.Parse(ConfigurationManager.AppSettings["GMT"]);
                return gmt;
            }
        }

        public static bool IsValidImage(this string fileName)
        {
            Regex regex = new Regex(@"(.*?)\.(jpg|JPG|jpeg|JPEG|png|PNG|gif|GIF|bmp|BMP)$");
            return regex.IsMatch(fileName);
        }
        public static Bitmap ConvertCMYK(System.Drawing.Image MyBitmap)
        {
            Bitmap NewBit = new Bitmap(MyBitmap.Width, MyBitmap.Height, PixelFormat.Format24bppRgb);

            Graphics MyGraph = Graphics.FromImage(NewBit);
            MyGraph.CompositingQuality = CompositingQuality.HighQuality;
            MyGraph.SmoothingMode = SmoothingMode.HighQuality;
            MyGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;

            Rectangle MyRect = new Rectangle(0, 0, MyBitmap.Width, MyBitmap.Height);
            MyGraph.DrawImage(MyBitmap, MyRect);

            Bitmap ReturnBitmap = new Bitmap(NewBit);

            MyGraph.Dispose();
            NewBit.Dispose();
            MyBitmap.Dispose();

            return ReturnBitmap;
        }
        public static System.Drawing.Image resizeImage(HttpPostedFileBase FileModel, int heigth, int width, Boolean keepAspectRatio = true, Boolean getCenter = true)
        {

            int newheigth = heigth;
            System.Drawing.Image FullsizeImage = System.Drawing.Image.FromStream(FileModel.InputStream);
            //Fix CMYKs
            if (!FileModel.ContentType.ToLower().Contains("png"))
            {
                FullsizeImage = ConvertCMYK(FullsizeImage);
            }
            // Prevent using images internal thumbnail
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            if (keepAspectRatio || getCenter)
            {
                int bmpY = 0;
                double resize = (double)FullsizeImage.Width / (double)width;//get the resize vector
                if (getCenter)
                {
                    bmpY = (int)((FullsizeImage.Height - (heigth * resize)) / 2);// gives the Y value of the part that will be cut off, to show only the part in the center
                    Rectangle section = new Rectangle(new System.Drawing.Point(0, bmpY), new System.Drawing.Size(FullsizeImage.Width, (int)(heigth * resize)));// create the section to cut of the original image


                    Bitmap orImg = new Bitmap((Bitmap)FullsizeImage);//for the correct effect convert image to bitmap.
                    FullsizeImage.Dispose();//clear the original image
                    using (Bitmap tempImg = new Bitmap(section.Width, section.Height))
                    {
                        Graphics cutImg = Graphics.FromImage(tempImg);//              set the file to save the new image to.
                        cutImg.DrawImage(orImg, 0, 0, section, GraphicsUnit.Pixel);// cut the image and save it to tempImg
                        FullsizeImage = tempImg;//save the tempImg as FullsizeImage for resizing later
                        orImg.Dispose();
                        cutImg.Dispose();
                        return FullsizeImage.GetThumbnailImage(width, heigth, null, IntPtr.Zero);
                    }
                }
                else newheigth = (int)(FullsizeImage.Height / resize);//  set the new heigth of the current image
            }//return the image resized to the given heigth and width
            return FullsizeImage.GetThumbnailImage(width, newheigth, null, IntPtr.Zero);
        }
        public static string uploadImageWithName(HttpPostedFileBase FileModel, String name)
        {
            string uploadedPath = "";
            if (FileModel.ContentLength > 0)
            {
                var fileName = Path.GetFileName(FileModel.FileName);
                fileName = name + "-" + fileName;
                fileName = fileName.Replace(" ", "-");
                //upload to local
                uploadedPath = fileName;
                var path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/upload"), fileName);
                FileModel.SaveAs(path);
                System.Drawing.Image resize = resizeImage(FileModel, 200, 200);
                resize.Save(Path.Combine(HttpContext.Current.Server.MapPath("~/Content/upload"), "thumb-" + fileName));
            }
            else
            {
                throw new Exception("invalid filemodel");
            }

            return uploadedPath;
        }

        public static string GetCSV<T>(this List<T> list)
        {
            StringBuilder sb = new StringBuilder();

            //Get the properties for type T for the headers
            PropertyInfo[] propInfos = typeof(T).GetProperties();
            for (int i = 0; i <= propInfos.Length - 1; i++)
            {
                sb.Append(propInfos[i].Name);

                if (i < propInfos.Length - 1)
                {
                    sb.Append(",");
                }
            }

            sb.AppendLine();

            //Loop through the collection, then the properties and add the values
            for (int i = 0; i <= list.Count - 1; i++)
            {
                T item = list[i];
                for (int j = 0; j <= propInfos.Length - 1; j++)
                {
                    object o = item.GetType().GetProperty(propInfos[j].Name).GetValue(item, null);
                    if (o != null)
                    {
                        string value = o.ToString();

                        //Check if the value contans a comma and place it in quotes if so
                        if (value.Contains(","))
                        {
                            value = string.Concat("\"", value, "\"");
                        }

                        //Replace any \r or \n special characters from a new line with a space
                        if (value.Contains("\r"))
                        {
                            value = value.Replace("\r", " ");
                        }
                        if (value.Contains("\n"))
                        {
                            value = value.Replace("\n", " ");
                        }

                        sb.Append(value);
                    }

                    if (j < propInfos.Length - 1)
                    {
                        sb.Append(",");
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

    }
}