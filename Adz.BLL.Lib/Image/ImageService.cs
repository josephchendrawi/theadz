using Adz.BLL.Lib.Helper;
using Adz.DAL.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    public class ImageService : IImageService
    {
        public Response<int> CreateImageId(Image Image)
        {
            Response<int> response = null;
            using (var context = new TheAdzEntities())
            {
                Adz.DAL.EF.Image mmentity = new Adz.DAL.EF.Image();
                mmentity.url = Image.Name;
                mmentity.last_action = "1";
                mmentity.width = Image.Width;
                mmentity.height = Image.Height;

                if (Image.Url != null)
                {
                    mmentity.checksum = CheckSum.GetChecksum(Image.Url, CheckSum.Algorithms.MD5);
                }

                context.Images.Add(mmentity);
                context.SaveChanges();
                response = Response<int>.Create(mmentity.id);
            }
            return response;
        }
        public Response<List<Image>> GetImageListDescByImageId(string imageid)
        {
            Response<List<Image>> response = null;
            List<Image> ImageList = new List<Image>();
            using (var context = new TheAdzEntities())
            {
                string[] _listimage = imageid.Split(',');
                List<int> ListImageId = new List<int>();
                foreach (var v in _listimage)
                {
                    if (v != "") ListImageId.Add(int.Parse(v));
                }
                var entityImage = (from d in context.Images
                                   where ListImageId.Any(id => d.id.Equals(id))
                                   orderby d.id descending
                                   select d);
                foreach (var v in entityImage)
                {
                    Image Image = new Image();
                    Image.ImageId = v.id;
                    Image.Url = ConfigurationManager.AppSettings["uploadpath"] + v.url;
                    Image.Name = v.url;
                    Image.LastAction = v.last_action;
                    ImageList.Add(Image);
                }
                response = Response<List<Image>>.Create(ImageList);
            }

            return response;
        }
        public Response<List<Image>> GetImageListDescBySkipTake(int skip, int take)
        {
            Response<List<Image>> response = null;
            List<Image> ImageList = new List<Image>();
            using (var context = new TheAdzEntities())
            {
                var entityImage = (from d in context.Images
                                   orderby d.id descending
                                   select d).Skip(skip).Take(take);
                foreach (var v in entityImage)
                {
                    Image Image = new Image();
                    Image.ImageId = v.id;
                    Image.Url = ConfigurationManager.AppSettings["uploadpath"] + v.url;
                    Image.Name = v.url;
                    Image.LastAction = v.last_action;
                    ImageList.Add(Image);
                }
                response = Response<List<Image>>.Create(ImageList);
            }

            return response;
        }

        public Response<Image> GetImageByImageId(int imageid)
        {
            Response<Image> response = null;
            using (var context = new TheAdzEntities())
            {
                var entityImage = from d in context.Images
                                  where d.id == imageid
                                  select d;
                if (entityImage.Count() > 0)
                {
                    var v = entityImage.First();
                    Image Image = new Image();
                    Image.ImageId = v.id;
                    Image.Url = ConfigurationManager.AppSettings["uploadpath"] + v.url;
                    Image.Name = v.url;
                    Image.LastAction = v.last_action;
                    response = Response<Image>.Create(Image);
                }
            }

            return response;
        }

    }
}
