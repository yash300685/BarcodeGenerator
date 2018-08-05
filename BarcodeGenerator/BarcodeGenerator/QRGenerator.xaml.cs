using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BarcodeGenerator
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QRGenerator : TabbedPage
	{
		public QRGenerator ()
		{
			InitializeComponent ();
            this.Title = "QR Code Generator";
            NavigationPage.SetTitleIcon(this, "Images/logo.png");
            

        }


	}
}