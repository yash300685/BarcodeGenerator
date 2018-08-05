using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BarcodeGenerator.Droid;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

[assembly: Dependency(typeof(SaveCode))]

namespace BarcodeGenerator.Droid
{
    class SaveCode : IDownloadImage
    {
        public void SaveImage(ZXingBarcodeImageView image)
        {
            throw new NotImplementedException();
        }
    }
}