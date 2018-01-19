namespace SqlExport.View.Converters
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;

    public class ImageConverter : IValueConverter
    {
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            MemoryStream ms = null;

            var image = value as Image;
            if (image != null)
            {
                ms = new MemoryStream();
                image.Save(ms, ImageFormat.Png);
            }

            var icon = value as Icon;
            if (icon != null)
            {
                var bitmap = icon.ToBitmap();
                var hbitmap = bitmap.GetHbitmap();

                var wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                    hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                if (!DeleteObject(hbitmap))
                {
                    throw new Win32Exception();
                }

                ms = new MemoryStream();
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(wpfBitmap));
                encoder.Save(ms);
            }

            if (ms != null)
            {
                BitmapImage myBitmapImage = new BitmapImage();

                myBitmapImage.BeginInit();
                myBitmapImage.StreamSource = ms;
                myBitmapImage.EndInit();

                return myBitmapImage;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
