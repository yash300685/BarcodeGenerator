using BarcodeGenerator;
using BarcodeGenerator.UWP;
using Microsoft.Advertising.WinRT.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Profile;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(AdControlViewUrl), typeof(AdViewRenderer))]
namespace BarcodeGenerator.UWP
{
    class AdViewRenderer : ViewRenderer<AdControlViewUrl, AdControl>
    {
        string bannerId = "1100029022";
        AdControl adView;
        string applicationID = "9pcv4wlj2q02";
        void CreateNativeAdControl()
        {
            if (adView != null)
                return;

            var width = 300;
            var height = 50;
            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Desktop")
            {
                width = 728;
                height = 90;
            }
            // Setup your BannerView, review AdSizeCons class for more Ad sizes. 
            adView = new AdControl
            {
                ApplicationId = applicationID,
                AdUnitId = bannerId,
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right,
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom,
                Height = height,
                Width = width
            };

        }

        protected override void OnElementChanged(ElementChangedEventArgs<AdControlViewUrl> e)
        {
            base.OnElementChanged(e);
            if (Control == null && adView==null)
            {
                CreateNativeAdControl();
                SetNativeControl(adView);
            }
        }
    }
}

