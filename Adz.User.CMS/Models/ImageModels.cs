using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adz.User.CMS.Models
{
    public class ImageModel
    {
        public sealed class Image
        {
            public int ImageId { get; set; }

            [Display(Name = "Image")]
            [DataType(DataType.Text)]
            public string Name { get; set; }
            public string Url { get; set; }
            public string LastAction { get; set; }
        }

        public class UploadImageModel
        {
            [Display(Name = "Internet URL")]
            public string Url { get; set; }

            public bool IsUrl { get; set; }
            
            [Display(Name = "Local file")]
            public HttpPostedFileBase File { get; set; }

            public bool IsFile { get; set; }

            [Range(0, int.MaxValue)]
            public int X { get; set; }

            [Range(0, int.MaxValue)]
            public int Y { get; set; }

            [Range(1, int.MaxValue)]
            public int Width { get; set; }

            [Range(1, int.MaxValue)]
            public int Height { get; set; }
        }
    }
}