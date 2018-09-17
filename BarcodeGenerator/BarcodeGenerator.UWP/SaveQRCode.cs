using BarcodeGenerator.UWP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Email;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Notifications;

using Windows.UI.Xaml.Media.Imaging;
using Xamarin.Forms;
using ZXing;
using ZXing.Net;
using ZXing.Net.Mobile.Forms;

[assembly: Dependency(typeof(SaveQRCode))]
namespace BarcodeGenerator.UWP
{

    public class SaveQRCode : IDownloadImage
    {
        

        public async void  SaveImage(ZXingBarcodeImageView image)
        {
            var writer = new ZXing.Mobile.BarcodeWriter();

            if (image != null && image.BarcodeOptions != null)
                writer.Options = image.BarcodeOptions;
            if (image != null)
                writer.Format = image.BarcodeFormat;
            var value = image != null ? image.BarcodeValue : string.Empty;
            var wb = writer.Write(value);
         
            //var imageData = ImagetoBytes(wb);
            var filename = System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
            
            if (Device.Idiom == TargetIdiom.Desktop)
            {
                var savePicker = new Windows.Storage.Pickers.FileSavePicker();
                savePicker.SuggestedStartLocation =
                    Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                savePicker.SuggestedFileName = filename;
                savePicker.FileTypeChoices.Add("JPEG Image", new List<string>() { ".jpg" });

                var file = await savePicker.PickSaveFileAsync();

                if (file != null)
                {
                    CachedFileManager.DeferUpdates(file);
                    await ConvertToJPEGFileAsync(file, wb);
                    var status = await CachedFileManager.CompleteUpdatesAsync(file);

                    if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                        ShowToastNotification();
                }
            }
            else
            {
                StorageFolder storageFolder = KnownFolders.SavedPictures;
                StorageFile sampleFile = await storageFolder.CreateFileAsync(
                    filename + ".jpg", CreationCollisionOption.ReplaceExisting);
                await ConvertToJPEGFileAsync(sampleFile, wb);
            }
        }

        public async  Task<byte[]> ImagetoBytesAsync(WriteableBitmap wb)
        {
            Stream pixelStream = wb.PixelBuffer.AsStream();
            byte[] pixels = new byte[pixelStream.Length];
            await pixelStream.ReadAsync(pixels, 0, pixels.Length);
            return pixels;
        }

        public async Task ConvertToJPEGFileAsync(StorageFile file, WriteableBitmap WB)
        {
            Guid BitmapEncoderGuid = BitmapEncoder.JpegEncoderId;
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoderGuid, stream);
                Stream pixelStream = WB.PixelBuffer.AsStream();
                byte[] pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore,
                          (uint)WB.PixelWidth,
                          (uint)WB.PixelHeight,
                          96.0,
                          96.0,
                          pixels);
                await encoder.FlushAsync();
            }


        }

        private void ShowToastNotification()
        {
            ToastNotifier ToastNotifier = ToastNotificationManager.CreateToastNotifier();
            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            Windows.Data.Xml.Dom.XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
            toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode("Save Successful"));
            toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode("QR Code Successfully Saved"));
            Windows.Data.Xml.Dom.IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            Windows.Data.Xml.Dom.XmlElement audio = toastXml.CreateElement("audio");
            audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

            ToastNotification toast = new ToastNotification(toastXml);
            toast.ExpirationTime = DateTime.Now.AddSeconds(4);
            ToastNotifier.Show(toast);
        }

        public async Task<byte[]> DisplayImageAsync(ZXingBarcodeImageView image)
        {
            var writer = new ZXing.Mobile.BarcodeWriter();

            if (image != null && image.BarcodeOptions != null)
                writer.Options = image.BarcodeOptions;
            if (image != null)
                writer.Format = image.BarcodeFormat;
            var value = image != null ? image.BarcodeValue : string.Empty;
            var wb = writer.Write(value);
            byte [] x = await ImagetoBytesAsync(wb);
            return x;
        }

       

        

        async void IDownloadImage.CopyImage(ZXingBarcodeImageView image)
        {
            EmailMessage emailMessage = new EmailMessage();
            
            string messageBody = "Hello World";
            emailMessage.Body = messageBody;

            var writer = new ZXing.Mobile.BarcodeWriter();

            if (image != null && image.BarcodeOptions != null)
                writer.Options = image.BarcodeOptions;
            if (image != null)
                writer.Format = image.BarcodeFormat;
            var value = image != null ? image.BarcodeValue : string.Empty;
            var wb = writer.Write(value);
              StorageFile tempFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("Barcode_Temp.jpeg", CreationCollisionOption.ReplaceExisting);
            if (tempFile != null)
            {
                CachedFileManager.DeferUpdates(tempFile);
                await ConvertToJPEGFileAsync(tempFile, wb);
                var status = await CachedFileManager.CompleteUpdatesAsync(tempFile);

                var dataPackage = new DataPackage();
               
                dataPackage.SetStorageItems(new List<IStorageItem>() { tempFile });
                dataPackage.RequestedOperation = DataPackageOperation.Copy;
                Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);
                ToastNotifier ToastNotifier = ToastNotificationManager.CreateToastNotifier();
                Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
                Windows.Data.Xml.Dom.XmlNodeList toastNodeList = toastXml.GetElementsByTagName("text");
                toastNodeList.Item(0).AppendChild(toastXml.CreateTextNode("Image Copied"));
                toastNodeList.Item(1).AppendChild(toastXml.CreateTextNode("Image Copied Successfully"));
                Windows.Data.Xml.Dom.IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
                Windows.Data.Xml.Dom.XmlElement audio = toastXml.CreateElement("audio");
                audio.SetAttribute("src", "ms-winsoundevent:Notification.SMS");

                ToastNotification toast = new ToastNotification(toastXml);
                toast.ExpirationTime = DateTime.Now.AddSeconds(4);
                ToastNotifier.Show(toast);

            }

            
        }

        
        
    } 
}
