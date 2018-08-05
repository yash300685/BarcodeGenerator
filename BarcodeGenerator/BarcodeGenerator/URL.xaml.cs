using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;

namespace BarcodeGenerator
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class URL : ContentPage
	{
        String barcodetype;
        private ZXingBarcodeImageView barcode;
        public URL ()
		{
            this.Title = "URL";
            InitializeComponent ();
            ImageSource imageSource = ImageSource.FromFile(
                   
                        "Images/qr.png");

            Image image = new Image
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
                if (entryWebsite.Text != string.Empty && BarcodeUtil.IsValidURL(entryWebsite.Text))
                {
                    barcode = new ZXingBarcodeImageView
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                    };
                    switch(barcodetype)
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
                    barcode.BarcodeValue = entryWebsite.Text.Trim();
                    qrResult.Content = barcode;
                   

                 

                }
                else
                {
                    DisplayAlert("Alert", "Please Enter Valid URL", "OK");
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
    }
}
