using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace BarcodeGenerator
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Text : ContentPage
    {
        String barcodetype;
        Boolean isValidCode;
        private ZXingBarcodeImageView barcode;
        private static Regex _gtinRegex = new Regex("^(\\d{8}|\\d{12,14})$");
        public Text()
        {
            this.Title = "Text";
            InitializeComponent();
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
            barcodeTypes.Items.Add("Code 2of5 Interleaved");
            barcodeTypes.Items.Add("Code 39");
            barcodeTypes.Items.Add("Code 93");
            barcodeTypes.Items.Add("EAN-13");
            barcodeTypes.Items.Add("EAN-8");
            barcodeTypes.Items.Add("UPC");

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

                if (entryText.Text != null)
                {
                    barcode = new ZXingBarcodeImageView
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                    };
                    try
                    {
                        switch (barcodetype)
                        {
                            case "QR Code":
                                barcode.BarcodeFormat = ZXing.BarcodeFormat.QR_CODE;
                                isValidCode = true;
                                break;

                            case "Code 128":
                                barcode.BarcodeFormat = ZXing.BarcodeFormat.CODE_128;
                                isValidCode = true;
                                break;

                            case "Datamatrix":
                                barcode.BarcodeFormat = ZXing.BarcodeFormat.DATA_MATRIX;
                                isValidCode = true;
                                break;

                            case "Code 2of5 Interleaved":
                                barcode.BarcodeFormat = ZXing.BarcodeFormat.ITF;
                                isValidCode=BarcodeUtil.IsNumber(entryText.Text);
                                break;

                            case "Code 39":
                                barcode.BarcodeFormat = ZXing.BarcodeFormat.CODE_39;
                                isValidCode = BarcodeUtil.IsValidCode39(entryText.Text);
                                break;

                            case "Code 93":
                                barcode.BarcodeFormat = ZXing.BarcodeFormat.CODE_93;
                                isValidCode = BarcodeUtil.IsValidCode39(entryText.Text);
                                break;

                            case "EAN-13":
                                barcode.BarcodeFormat = ZXing.BarcodeFormat.EAN_13;
                                isValidCode = BarcodeUtil.IsValidGtin(entryText.Text);

                                break;

                            case "EAN-8":
                                barcode.BarcodeFormat = ZXing.BarcodeFormat.EAN_8;
                                isValidCode = BarcodeUtil.IsValidGtin(entryText.Text);
                                break;

                            case "UPC":
                                barcode.BarcodeFormat = ZXing.BarcodeFormat.UPC_A;
                                isValidCode = BarcodeUtil.IsValidUPC(entryText.Text);

                                break;
                        }

                        barcode.BarcodeOptions.Width = 500;
                        barcode.BarcodeOptions.Height = 500;
                        if (isValidCode)
                        {
                            barcode.BarcodeValue = entryText.Text.Trim();
                        }
                        else
                        {
                            DisplayAlert("Alert", "Entered Text does not conform with seleted Barcode Type standards", "OK");
                            return;
                        }

                        qrResult.Content = barcode;





                    }

                    catch (ArgumentException io)
                    {
                        // TODO Auto-generated catch block
                        DisplayAlert("Alert", "Entered Text does not conform with seleted Barcode Type standards", "OK");
                    }

                    catch (Exception ex)
                    {
                        DisplayAlert("Alert", "Entered Text does not conform with seleted Barcode Type standards", "OK");
                    }

                }
                else
                {
                    DisplayAlert("Alert", "Please Enter Valid text", "OK");
                }
            }
              
            catch (ArgumentException io)
            {
                // TODO Auto-generated catch block
                DisplayAlert("Alert", "Entered Text does not conform with selected Barcode Type standards", "OK");
            }

        }








        private void DownloadQR(object sender1, EventArgs e1)
        {
            if (barcode != null)
            {
                DependencyService.Get<IDownloadImage>().SaveImage(barcode);
            }
            else
            {
                DisplayAlert("Alert", "Please Create Code", "OK");
            }


        }

        private void CopyImage(object sender1, EventArgs e1)
        {
            if (barcode != null)
            {
                DependencyService.Get<IDownloadImage>().CopyImage(barcode);
            }
            else
            {
                DisplayAlert("Alert", "Please Create Code", "OK");
            }

        }

    }
}

