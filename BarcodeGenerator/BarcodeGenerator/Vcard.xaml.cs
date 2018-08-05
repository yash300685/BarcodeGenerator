
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace BarcodeGenerator
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Vcard : ContentPage
    {
        private String stringVcard;
        private ZXingBarcodeImageView barcode;
        public Vcard ()
		{
            this.Title = "vCard";
            
            InitializeComponent ();
            ImageSource imageSource = ImageSource.FromFile(

                       "Images/qr.png");

            Image image = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = imageSource
            };
            qrResult.Content = image;
        }

        public String GenerateVCard()
        {
                VCard generateVcard = new VCard();
                generateVcard.FirstName = entryfirstname.Text;
                generateVcard.LastName = entrylastname.Text;
                generateVcard.Mobile = entryMobileNumber.Text;
                generateVcard.Organization = entryCompanyName.Text;
                generateVcard.StreetAddress = entryStreet.Text + ","+ entrystate.Text;
                generateVcard.City = entrycity.Text;
                generateVcard.Email = entryEmail.Text;
                generateVcard.JobTitle = entryJob.Text;
                generateVcard.CountryName = entryCountry.Text;
                generateVcard.HomePage = entryWebsite.Text;
                generateVcard.Zip = entryzip.Text;
                generateVcard.Phone = entryPhoneNumber.Text;
                return generateVcard.ToString();
        }
        private void GenerateQRCode(object sender, EventArgs e)
        {
            stringVcard = GenerateVCard();

            try
            {
                if (stringVcard != string.Empty)
                {
                    barcode = new ZXingBarcodeImageView
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                    };
                   
                    barcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
                    barcode.BarcodeOptions.Width = 500;
                    barcode.BarcodeOptions.Height = 500;
                    barcode.BarcodeValue = stringVcard;
                    qrResult.Content = barcode;




                }
                else
                {
                    DisplayAlert("Alert", "Please Enter valid info", "OK");
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
