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
            return GetPlatformIndependentPath(ImagePath);
        }

        public static string GetPlatformIndependentPath(string path)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                return "";
            }

            if (Device.RuntimePlatform == Device.UWP)
            {
                return Path.Combine("Assets/Images/", path);
            }
            else
            {
                return path;
            }
        }
    }   
}
