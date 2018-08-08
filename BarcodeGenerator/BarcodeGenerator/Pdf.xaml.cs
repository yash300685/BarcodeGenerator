using Acr.UserDialogs;
using Microsoft.WindowsAzure.Storage;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace BarcodeGenerator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class Pdf : ContentPage
    {
       private String barcodetype;
        private ZXingBarcodeImageView barcode;
        private String uploadedpath;
       private FileData pickedFile;
        private Image image;
        public Pdf()
        {

            this.Title = "PDF";
            InitializeComponent();
      
            ImageSource imageSource = ImageSource.FromFile(

                        "Images/qr.png");

             image = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = imageSource
            };
            qrResult.Content = image;
            barcodeTypes.Items.Add("QR Code");
            barcodeTypes.Items.Add("Code 128");
            barcodeTypes.Items.Add("Datamatrix");
            barcodeTypes.SelectedIndex = 0;
            barcodetype = barcodeTypes.Items[barcodeTypes.SelectedIndex];
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            barcodetype = barcodeTypes.Items[barcodeTypes.SelectedIndex];
        }
        private void GenerateQRCode(object sender, EventArgs e)
        {
            try
            {
                if (uploadedpath != string.Empty&&uploadedpath != null)
                {
                    barcode = new ZXingBarcodeImageView
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                    };
                    switch (barcodetype)
                    {
                        case "QR Code":
                            barcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
                            break;

                        case "Code 128":
                            barcode.BarcodeFormat = ZXing.BarcodeFormat.CODE_128;
                            break;

                        case "Datamatrix":
                            barcode.BarcodeFormat = ZXing.BarcodeFormat.DATA_MATRIX;
                            break;
                    }

                    barcode.BarcodeOptions.Width = 500;
                    barcode.BarcodeOptions.Height = 500;
                    barcode.BarcodeValue = uploadedpath;
                    qrResult.Content = barcode;




                }
                else
                {
                    DisplayAlert("Alert", "Please Upload the file", "OK");
                }


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                DisplayAlert("Alert", "Something went wrong ,Please try again", "OK");
            }

        }

        private void DownloadQR(object sender1, EventArgs e1)
        {
            DependencyService.Get<IDownloadImage>().SaveImage(barcode);


        }

        private  void Upload(object sender1, EventArgs e1)
        {
            uploadedpath = "";
            qrResult.Content = image;
            UploadPdf(pickedFile.GetStream());
            
        }

        

        private async void ChooseFile(object sender1, EventArgs e1)
        {

             pickedFile = await CrossFilePicker.Current.PickFile();
        
            if (pickedFile != null)
            {
                


                if (pickedFile.FileName.EndsWith("pdf", StringComparison.Ordinal))

                {
                    
                    entryFilePath.Text = pickedFile.FilePath;
                    
                }
                else
                {
                    await DisplayAlert("Alert", "Please upload valid pdf", "OK");
                }
            }
        }

        private async void UploadPdf(Stream pdftoupload)
        {


            UserDialogs.Instance.ShowLoading("Uploading",MaskType.Clear);

            //! added using Microsoft.WindowsAzure.Storage;
            var account = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=barcodepdfstorage;AccountKey=7dZXH3RbSTKODKqCWGMlukiSedwbb4Gmu40yhBMBhOIXLLjjtiC3FbqnxEIpa8Q+LEGdg+fksgsMQJWnwEYlrg==;EndpointSuffix=core.windows.net");
            var blobClient = account.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("pdfs");

            string uniqueName = Guid.NewGuid().ToString();
            var blockBlob = container.GetBlockBlobReference($"{uniqueName}.pdf");
            
            await blockBlob.UploadFromStreamAsync(pdftoupload);
            await DisplayAlert("Alert", "Pdf uploaded", "OK");
            uploadedpath = blockBlob.Uri.OriginalString;
            UserDialogs.Instance.HideLoading();

        }
    }
}

