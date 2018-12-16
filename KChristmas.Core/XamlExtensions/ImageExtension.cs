using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KChristmas.Core.XamlExtensions
{
    [ContentProperty("ImagePath")]
    public class ImageExtension : IMarkupExtension
    {
        public string ImagePath { get; set; }

        public object ProvideValue(IServiceProvider provider)
        {
            if(String.IsNullOrWhiteSpace(ImagePath))
            {
                return "";
            }

            if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
            {
                return Path.Combine("Assets/Images/", ImagePath);
            }
            else
            {
                return ImagePath;
            }
        }
    }
}
