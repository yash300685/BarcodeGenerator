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

[assembly: ExportRenderer(typeof(AdControlView), typeof(AdViewRenderer))]
namespace BarcodeGenerator.UWP
{
    class AdViewRenderer : ViewRenderer<AdControlView, AdControl>
    {
        string bannerId = "test";
        AdControl adView;
        string applicationID = "3f83fe91-d6be-434d-a0ae-7351c5a997f1";
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
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center,
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Bottom,
                Height = height,
                Width = width
            };

        }

        protected override void OnElementChanged(ElementChangedEventArgs<AdControlView> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                CreateNativeAdControl();
                SetNativeControl(adView);
            }
        }
    }
}

