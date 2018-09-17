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
	public partial class QRGenerator : MasterDetailPage
    {
		public QRGenerator ()
		{
			InitializeComponent ();
            masterPage.listView.ItemSelected += OnItemSelected;

            if (Device.RuntimePlatform == Device.UWP)
            {
                MasterBehavior = MasterBehavior.Popover;
                
                }
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
               var  page = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                page.BarBackgroundColor = Color.FromHex("#005493");
                page.BarTextColor = Color.White;
               Detail = page;
               
                //await Detail.Navigation.PushAsync(page);
                masterPage.listView.SelectedItem = null;
                IsPresented = false;

            }
        }


    }


	}
