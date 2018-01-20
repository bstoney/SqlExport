namespace SqlExport.Logic
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Monads;
    using System.Runtime.InteropServices;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// Defines the ImageExtensions class.
    /// </summary>
    public static class ImageExtensions
    {
        /// <summary>
        /// Converts the image to a <see cref="MemoryStream"/>.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>
        /// A new <see cref="MemoryStream"/>.
        /// </returns>
        public static Stream ToStream(this Image image)
        {
            return image.With(
                i =>
                {
                    var ms = new MemoryStream();
                    i.Save(ms, ImageFormat.Png);
                    ms.Flush();
                    return ms;
                });
        }

        /// <summary>
        /// Converts the icon to a <see cref="MemoryStream"/>.
        /// </summary>
        /// <param name="icon">The icon.</param>
        /// <returns>
        /// A new <see cref="MemoryStream"/>.
        /// </returns>
        public static Stream ToStream(this Icon icon)
        {
            return icon.With(
                i =>
                {
                    var bitmap = i.ToBitmap();
                    var hbitmap = bitmap.GetHbitmap();

                    var wpfBitmap = Imaging.CreateBitmapSourceFromHBitmap(
                        hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                    if (!DeleteObject(hbitmap))
                    {
                        throw new Win32Exception();
                    }

                    var ms = new MemoryStream();
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(wpfBitmap));
                    encoder.Save(ms);
                    ms.Flush();

                    return ms;
                });
        }

        /// <summary>
        /// Converts the bitmap source to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="bitmapSource">The bitmap source.</param>
        /// <returns>
        /// A new <see cref="Stream"/>.
        /// </returns>
        public static Stream ToStream(this BitmapSource bitmapSource)
        {
            return bitmapSource.With(
                s =>
                {
                    var ms = new MemoryStream();
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(s));
                    encoder.Save(ms);
                    ms.Flush();

                    return ms;
                });
        }

        /// <summary>
        /// Converts the stream to a <see cref="BitmapSource"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// A new <see cref="BitmapSource"/>.
        /// </returns>
        public static BitmapSource ToBitmapImage(this Stream stream)
        {
            return stream.With(
                s =>
                {
                    var bitmapImage = new BitmapImage();

                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = s;
                    bitmapImage.EndInit();

                    return bitmapImage;
                });
        }

        /// <summary>
        /// Converts the stream to a <see cref="Image"/>.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// A new <see cref="Image"/>.
        /// </returns>
        public static Image ToImage(this Stream stream)
        {
            return stream.With(s => Image.FromStream(s));
        }

        /// <summary>
        /// Deletes the object.
        /// </summary>
        /// <param name="hObject">The h object.</param>
        [DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);
    }
}
