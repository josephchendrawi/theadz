using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Adz.BLL.Lib
{
    [ServiceContract]
    public interface IImageService
    {
        [OperationContract]
        Response<int> CreateImageId(Image Image);
        [OperationContract]
        Response<List<Image>> GetImageListDescByImageId(string imageid);
        [OperationContract]
        Response<List<Image>> GetImageListDescBySkipTake(int skip, int take);
        Response<Image> GetImageByImageId(int imageid);
    }

    [DataContract]
    public class Image
    {
        [DataMember]
        public int ImageId { get; set; }
        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public string Name { get; set; }
        public string LastAction { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
