using BarcodeGenerator;
using BarcodeGenerator.UWP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Markup;
using Xamarin.Forms.Platform.UWP;


[assembly: ExportRenderer(typeof(QRGenerator), typeof(CustomTabbedPageRenderer))]
namespace BarcodeGenerator.UWP
{
    class CustomTabbedPageRenderer : TabbedPageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            

            base.OnElementChanged(e);
            Control.HeaderTemplate = GetStyledTitleTemplate();


            if (Control != null)
                Control.Loaded += Control_Loaded;

            if (e.OldElement == null) return;

            // Unhook when disposing control
            if (Control != null) Control.Loaded -= Control_Loaded;
            // Re-enable animations for everything else

        }
         private Windows.UI.Xaml.DataTemplate GetStyledTitleTemplate()
    {
        string dataTemplateXaml = @"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
            xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                <TextBlock
                    Text=""{Binding Title}""
                    FontWeight=""Normal""
                    FontSize =""25"" />
                  </DataTemplate>";

        return (Windows.UI.Xaml.DataTemplate)XamlReader.Load(dataTemplateXaml);
    }
        private void Control_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            // Disable UWP swipe gesture on tabbled page
            if (Control.ItemsPanelRoot != null)
                Control.ItemsPanelRoot.ManipulationMode = Windows.UI.Xaml.Input.ManipulationModes.None;
        }

    }

       
    }

