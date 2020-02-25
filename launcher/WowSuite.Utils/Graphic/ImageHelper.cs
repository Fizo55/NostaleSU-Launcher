using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Brush = System.Windows.Media.Brush;
using Drawing = System.Drawing;
using Size = System.Windows.Size;

namespace WowLauncher.Utils.Graphic
{
    public class ImageHelper
    {
        /// <summary>
        /// Конвертирует System.Windows.Media.Imaging.BitmapImage в System.Drawing.Bitmap.
        /// </summary>
        /// <param name="bitmapImage">Изображение для конвертирования</param>
        /// <returns></returns>
        public static Drawing.Bitmap BitmapImageToBitmap(BitmapImage bitmapImage)
        {
            using (var outStream = new MemoryStream())
            {
                BitmapEncoder enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                var bitmap = new Drawing.Bitmap(outStream);

                return new Drawing.Bitmap(bitmap);
            }
        }

        public static BitmapSource ToBitmapSource(Drawing.Bitmap bitmap)
        {
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Bmp);
            stream.Position = 0;

            BitmapImage result = new BitmapImage();
            result.BeginInit();
            result.CacheOption = BitmapCacheOption.OnLoad;
            result.StreamSource = stream;
            result.EndInit();

            return result;
        }

        /// <summary>
        /// Кисть, которую нужно преобразовать в System.Windows.Media.ImageSource
        /// </summary>
        /// <param name="brush">Кисть</param>
        /// <param name="size">Размер объекта</param>
        /// <param name="margin">Отступы</param>
        /// <param name="fileName">Имя файла, в который будет сохранено изображение в формате PNG</param>
        /// <returns></returns>
        public static ImageSource BrushToImageSource(Brush brush, Size size, Thickness margin, string fileName)
        {
            var bitmap = new RenderTargetBitmap(
                (int)(size.Width + margin.Left + margin.Right),
                (int)(size.Height + margin.Top + margin.Bottom),
                96, 96, PixelFormats.Pbgra32);

            var drawingVisual = new DrawingVisual();
            using (DrawingContext context = drawingVisual.RenderOpen())
            {
                var rect = new Rect(margin.Left, margin.Top, size.Width, size.Height);
                context.DrawRectangle(brush, null, rect);
            }

            bitmap.Render(drawingVisual);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                encoder.Save(fs);
            }

            return bitmap;
        }

        /// <summary>
        /// Кисть, которую нужно преобразовать в System.Windows.Media.ImageSource
        /// </summary>
        /// <param name="brush">Кисть</param>
        /// <param name="size">Размер объекта</param>
        /// <param name="fileName">Имя файла, в который будет сохранено изображение в формате PNG</param>
        /// <returns></returns>
        public static ImageSource BrushToImageSource(Brush brush, Size size, string fileName)
        {
            return BrushToImageSource(brush, size, new Thickness(0d), fileName);
        }

        #region Сохранение в формате JPEG
        /// <summary>
        /// Сохранить изображение в формате JPEG (Joint Photographics Experts Group).
        /// </summary>
        /// <param name="bmp">Сохраняемое изображение</param>
        /// <param name="path">Путь по которому будет сохранено изображение</param>
        /// <param name="quality">Качество JPEG изображения от 1 до 100</param>
        public static void SaveInJpeg(BitmapImage bmp, string path, int quality)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                JpegBitmapEncoder jpgEncoder = new JpegBitmapEncoder();
                jpgEncoder.QualityLevel = quality;
                jpgEncoder.Frames.Add(BitmapFrame.Create(bmp));
                jpgEncoder.Save(fileStream);
            }
        }

        /// <summary>
        /// Сохранить изображение в формате JPEG (Joint Photographics Experts Group).
        /// </summary>
        /// <param name="bmp">Сохраняемое изображение</param>
        /// <param name="path">Путь по которому будет сохранено изображение</param>
        /// <param name="quality">Качество JPEG изображения от 1 до 100</param>
        public static void SaveInJpeg(BitmapSource bmp, string path, int quality)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                JpegBitmapEncoder jpgEncoder = new JpegBitmapEncoder();
                jpgEncoder.QualityLevel = quality;
                jpgEncoder.Frames.Add(BitmapFrame.Create(bmp));
                jpgEncoder.Save(fileStream);
            }
        }
        #endregion

        #region Сохранение в формате PNG
        /// <summary>
        /// Сохранить изображение в формате PNG (Portable Network Graphics).
        /// </summary>
        /// <param name="bmp">Сохраняемое изображение</param>
        /// <param name="path">Путь по которому будет сохранено изображение</param>
        public static void SaveInPng(BitmapImage bmp, string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(bmp));
                pngEncoder.Save(fileStream);
            }
        }

        /// <summary>
        /// Сохранить изображение в формате PNG (Portable Network Graphics).
        /// </summary>
        /// <param name="bmp">Сохраняемое изображение</param>
        /// <param name="path">Путь по которому будет сохранено изображение</param>
        public static void SaveInPng(BitmapSource bmp, string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                pngEncoder.Frames.Add(BitmapFrame.Create(bmp));
                pngEncoder.Save(fileStream);
            }
        }
        #endregion

        #region Сохранение в формате WMP
        /// <summary>
        /// Сохранить изображение в формате WMP (Microsoft Windows Media Photo).
        /// </summary>
        /// <param name="bmp">Сохраняемое изображение</param>
        /// <param name="path">Путь по которому будет сохранено изображение</param>
        public static void SaveInWmp(BitmapImage bmp, string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                WmpBitmapEncoder wmpEncoder = new WmpBitmapEncoder();
                wmpEncoder.Frames.Add(BitmapFrame.Create(bmp));
                wmpEncoder.Save(fileStream);
            }
        }

        /// <summary>
        /// Сохранить изображение в формате WMP (Microsoft Windows Media Photo).
        /// </summary>
        /// <param name="bmp">Сохраняемое изображение</param>
        /// <param name="path">Путь по которому будет сохранено изображение</param>
        public static void SaveInWmp(BitmapSource bmp, string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                WmpBitmapEncoder wmpEncoder = new WmpBitmapEncoder();
                wmpEncoder.Frames.Add(BitmapFrame.Create(bmp));
                wmpEncoder.Save(fileStream);
            }
        }
        #endregion

        #region Сохранение в формате TIFF
        /// <summary>
        /// Сохранить изображение в формате TIFF (Tagged Image File Format).
        /// </summary>
        /// <param name="bmp">Сохраняемое изображение</param>
        /// <param name="path">Путь по которому будет сохранено изображение</param>
        public static void SaveInTiff(BitmapImage bmp, string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                TiffBitmapEncoder tiffEncoder = new TiffBitmapEncoder();
                tiffEncoder.Frames.Add(BitmapFrame.Create(bmp));
                tiffEncoder.Save(fileStream);
            }
        }

        /// <summary>
        /// Сохранить изображение в формате TIFF (Tagged Image File Format).
        /// </summary>
        /// <param name="bmp">Сохраняемое изображение</param>
        /// <param name="path">Путь по которому будет сохранено изображение</param>
        public static void SaveInTiff(BitmapSource bmp, string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                TiffBitmapEncoder tiffEncoder = new TiffBitmapEncoder();
                tiffEncoder.Frames.Add(BitmapFrame.Create(bmp));
                tiffEncoder.Save(fileStream);
            }
        }
        #endregion

        #region Сохранение в формате GIF
        /// <summary>
        /// Сохранить изображение в формате GIF (Graphics Interchange Format).
        /// </summary>
        /// <param name="bmp">Сохраняемое изображение</param>
        /// <param name="path">Путь по которому будет сохранено изображение</param>
        public static void SaveInGif(BitmapImage bmp, string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                GifBitmapEncoder gifEncoder = new GifBitmapEncoder();
                gifEncoder.Frames.Add(BitmapFrame.Create(bmp));
                gifEncoder.Save(fileStream);
            }
        }

        /// <summary>
        /// Сохранить изображение в формате GIF (Graphics Interchange Format).
        /// </summary>
        /// <param name="bmp">Сохраняемое изображение</param>
        /// <param name="path">Путь по которому будет сохранено изображение</param>
        public static void SaveInGif(BitmapSource bmp, string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                GifBitmapEncoder gifEncoder = new GifBitmapEncoder();
                gifEncoder.Frames.Add(BitmapFrame.Create(bmp));
                gifEncoder.Save(fileStream);
            }
        }
        #endregion

        #region Сохранение в формате BMP
        /// <summary>
        /// Сохранить изображение в формате BMP (bitmap — точечный рисунок).
        /// </summary>
        /// <param name="bmp">Сохраняемое изображение</param>
        /// <param name="path">Путь по которому будет сохранено изображение</param>
        public static void SaveInBmp(BitmapImage bmp, string path)
        {
            using (var fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                var bmpEncoder = new BmpBitmapEncoder();
                bmpEncoder.Frames.Add(BitmapFrame.Create(bmp));
                bmpEncoder.Save(fileStream);
            }
        }

        /// <summary>
        /// Сохранить изображение в формате BMP (bitmap — точечный рисунок).
        /// </summary>
        /// <param name="bmp">Сохраняемое изображение</param>
        /// <param name="path">Путь по которому будет сохранено изображение</param>
        public static void SaveInBmp(BitmapSource bmp, string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                BmpBitmapEncoder bmpEncoder = new BmpBitmapEncoder();
                bmpEncoder.Frames.Add(BitmapFrame.Create(bmp));
                bmpEncoder.Save(fileStream);
            }
        }
        #endregion

        #region Конвертер форматов (BitmapSource в BitmapImage)
        /// <summary>
        /// Конвертировать BitmapSource в BitmapImage используя PngBitmapEncoder
        /// </summary>
        /// <param name="bitmapSource">BitmapSource который нужно конвертировать</param>
        /// <returns></returns>
        public static BitmapImage BitmapSourceToBitmapImage(BitmapSource bitmapSource)
        {
            var encoder = new PngBitmapEncoder();
            using (var ms = new MemoryStream())
            {
                var bImg = new BitmapImage();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(ms);
                bImg.BeginInit();
                bImg.StreamSource = new MemoryStream(ms.ToArray());
                bImg.EndInit();
                return bImg;
            }
        }
        #endregion
    }
}
