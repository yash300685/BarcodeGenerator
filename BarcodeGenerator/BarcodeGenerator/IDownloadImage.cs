using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace BarcodeGenerator
{
    public interface IDownloadImage
    {
         void SaveImage(ZXingBarcodeImageView image);
        void CopyImage(ZXingBarcodeImageView image);
    }
}
