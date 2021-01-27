using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Text;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net.Utilities
{
    static class ImageExtensions
    {
        /// <summary>
        /// Create rounded rectangle
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static GraphicsPath RoundedRectangle(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            path.AddArc(arc, 180, 90);

            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();

            return path;
        }

        /// <summary>
        /// Draw a rounded rectangle
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="pen"></param>
        /// <param name="bounds"></param>
        /// <param name="cornerRadius"></param>
        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle bounds, int cornerRadius)
        {
            using GraphicsPath path = RoundedRectangle(bounds, cornerRadius);

            graphics.DrawPath(pen, path);
        }

        /// <summary>
        /// Draw a solid rounded rectangle
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="brush"></param>
        /// <param name="bounds"></param>
        /// <param name="cornerRadius"></param>
        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle bounds, int cornerRadius)
        {
            using GraphicsPath path = RoundedRectangle(bounds, cornerRadius);

            graphics.FillPath(brush, path);
        }


        public static StringFormat TopLeft(bool rightToLeft) => new StringFormat()
        {
            Alignment = StringAlignment.Near,
            FormatFlags = rightToLeft ? StringFormatFlags.DirectionRightToLeft : 0,
            Trimming = StringTrimming.EllipsisCharacter
        };
        public static StringFormat CenterLeft(bool rightToLeft) => new StringFormat()
        {
            LineAlignment = StringAlignment.Center,
            FormatFlags = rightToLeft ? StringFormatFlags.DirectionRightToLeft : 0,
            Trimming = StringTrimming.EllipsisCharacter
        };
        public static StringFormat BottomLeft(bool rightToLeft) => new StringFormat()
        {
            LineAlignment = StringAlignment.Far,
            FormatFlags = rightToLeft ? StringFormatFlags.DirectionRightToLeft : 0,
            Trimming = StringTrimming.EllipsisCharacter
        };

        public static StringFormat TopCenter(bool rightToLeft) => new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Near,
            FormatFlags = rightToLeft ? StringFormatFlags.DirectionRightToLeft : 0,
            Trimming = StringTrimming.EllipsisCharacter
        };
        public static StringFormat CenterCenter(bool rightToLeft) => new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center,
            FormatFlags = rightToLeft ? StringFormatFlags.DirectionRightToLeft : 0,
            Trimming = StringTrimming.EllipsisCharacter
        };
        public static StringFormat BottomCenter(bool rightToLeft) => new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Far,
            FormatFlags = rightToLeft ? StringFormatFlags.DirectionRightToLeft : 0,
            Trimming = StringTrimming.EllipsisCharacter
        };

        public static StringFormat TopRight(bool rightToLeft) => new StringFormat()
        {
            Alignment = StringAlignment.Far,
            LineAlignment = StringAlignment.Near,
            FormatFlags = rightToLeft ? StringFormatFlags.DirectionRightToLeft : 0,
            Trimming = StringTrimming.EllipsisCharacter
        };
        public static StringFormat CenterRight(bool rightToLeft) => new StringFormat()
        {
            Alignment = StringAlignment.Far,
            LineAlignment = StringAlignment.Center,
            FormatFlags = rightToLeft ? StringFormatFlags.DirectionRightToLeft : 0,
            Trimming = StringTrimming.EllipsisCharacter
        };
        public static StringFormat BottomRight(bool rightToLeft) => new StringFormat()
        {
            Alignment = StringAlignment.Far,
            LineAlignment = StringAlignment.Near,
            FormatFlags = rightToLeft ? StringFormatFlags.DirectionRightToLeft : 0,
            Trimming = StringTrimming.EllipsisCharacter
        };

        /// <summary>
        /// Measure the size of a string (Islam Image Handling Stuff)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static Size MeasureDrawTextBitmapSize(string text, Font font)
        {
            Bitmap bmp = new Bitmap(1, 1);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                SizeF size = g.MeasureString(text, font);
                return new Size((int)(Math.Ceiling(size.Width)), (int)(Math.Ceiling(size.Height)));
            }
        }

        /// <summary>
        /// Determine the size of the font (Islam Image Handling)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="rectanglef"></param>
        /// <param name="isWarp"></param>
        /// <param name="MinumumFontSize"></param>
        /// <param name="MaximumFontSize"></param>
        /// <returns></returns>
        public static Font GetFontSize(this Font font, string text, RectangleF rectanglef, bool isWarp = false, int MinumumFontSize = 10, int MaximumFontSize = 20)
        {
            Font newFont;
            Rectangle rect = Rectangle.Ceiling(rectanglef);

            for (int newFontSize = MinumumFontSize; ; newFontSize++)
            {
                newFont = new Font(font.FontFamily, newFontSize, font.Style);

                List<string> ls = WarpText(text, newFont, rect.Width);

                StringBuilder sb = new StringBuilder();
                if (isWarp)
                    for (int i = 0; i < ls.Count; ++i)
                        sb.Append(ls[i] + Environment.NewLine);
                else
                    sb.Append(text);

                Size size = MeasureDrawTextBitmapSize(sb.ToString(), newFont);
                if (size.Width > rectanglef.Width || size.Height > rectanglef.Height)
                    return new Font(font.FontFamily, (newFontSize - 1), font.Style);
                if (newFontSize >= MaximumFontSize)
                    return new Font(font.FontFamily, (newFontSize - 1), font.Style);

            }
        }

        /// <summary>
        /// Determine where to wrap the text
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="lineWidthInPixels"></param>
        /// <returns></returns>
        public static List<string> WarpText(string text, Font font, int lineWidthInPixels)
        {
            string[] originalLines = text.Split(new string[] { " " }, StringSplitOptions.None);

            List<string> wrappedLines = new List<string>();

            StringBuilder actualLine = new StringBuilder();
            double actualWidthInPixels = 0;

            foreach (string str in originalLines)
            {
                Size size = MeasureDrawTextBitmapSize(str, font);

                actualLine.Append(str + " ");
                actualWidthInPixels += size.Width;

                if (actualWidthInPixels > lineWidthInPixels)
                {
                    actualLine = actualLine.Remove(actualLine.ToString().Length - str.Length - 1, str.Length);
                    wrappedLines.Add(actualLine.ToString());
                    actualLine.Clear();
                    actualLine.Append(str + " ");
                    actualWidthInPixels = size.Width;
                }
            }

            if (actualLine.Length > 0)
                wrappedLines.Add(actualLine.ToString());

            return wrappedLines;
        }

        public static Size ResizeKeepAspect(this Size src, int maxWidth, int maxHeight, bool enlarge = false)
        {
            maxWidth = enlarge ? maxWidth : Math.Min(maxWidth, src.Width);
            maxHeight = enlarge ? maxHeight : Math.Min(maxHeight, src.Height);

            decimal rnd = Math.Min(maxWidth / (decimal)src.Width, maxHeight / (decimal)src.Height);
            return new Size((int)Math.Round(src.Width * rnd), (int)Math.Round(src.Height * rnd));
        }

        /// <summary>
        /// SLOW
        /// </summary>
        /// <param name="bmp"></param>
        /// <param name="Number"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Bitmap RotateImage(this Bitmap bmp, int Number, float angle = 18.01f)
        {
            angle = (Number * angle);
            float height = bmp.Height;
            float width = bmp.Width;

            Bitmap rotatedImage = new Bitmap((int)width, (int)height);
            using Graphics graphics = Graphics.FromImage(rotatedImage);

            graphics.TranslateTransform((float)rotatedImage.Width / 2, (float)rotatedImage.Height / 2); //set the rotation point as the center into the matrix
            graphics.RotateTransform(angle); //rotate
            graphics.TranslateTransform(-(float)rotatedImage.Width / 2, -(float)rotatedImage.Height / 2); //restore rotation point into the matrix
            graphics.DrawImage(bmp, 0, 0, 2000, 2000);

            return rotatedImage;
        }

        public static Bitmap ResizeImage(this Bitmap image, Size Size)
        {
            return image.ResizeImage(Size.Width, Size.Height);
        }

        public static Bitmap ResizeImage(this Bitmap image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using var graphics = Graphics.FromImage(destImage);

            graphics.SetGraphics();

            using var wrapMode = new ImageAttributes();

            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
            graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);

            return destImage;
        }

        /// <summary>
        /// Set graphics to highest quality, may cause slowness
        /// </summary>
        /// <param name="graphics"></param>
        public static void SetGraphics(this Graphics graphics)
        {
            graphics.InterpolationMode = InterpolationMode.High;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingMode = CompositingMode.SourceOver;
        }

        /// <summary>
        /// Round an image
        /// </summary>
        /// <param name="StartImage"></param>
        /// <param name="CornerRadius"></param>
        /// <returns></returns>
        public static Bitmap RoundCorners(this Bitmap StartImage, int CornerRadius)
        {
            CornerRadius *= 2;
            Bitmap RoundedImage = new Bitmap(StartImage.Width, StartImage.Height);

            using (Graphics g = Graphics.FromImage(RoundedImage))
            {
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                using Brush brush = new TextureBrush(StartImage);
                using GraphicsPath gp = new GraphicsPath();

                gp.AddArc(-1, -1, CornerRadius, CornerRadius, 180, 90);
                gp.AddArc(0 + RoundedImage.Width - CornerRadius, -1, CornerRadius, CornerRadius, 270, 90);
                gp.AddArc(0 + RoundedImage.Width - CornerRadius, 0 + RoundedImage.Height - CornerRadius, CornerRadius, CornerRadius, 0, 90);
                gp.AddArc(-1, 0 + RoundedImage.Height - CornerRadius, CornerRadius, CornerRadius, 90, 90);

                g.FillPath(brush, gp);

            }

            return RoundedImage;
        }
    
        /// <summary>
        /// Convert online state to its respected color
        /// </summary>
        /// <param name="onlineState"></param>
        /// <returns></returns>
        public static Color OnlineStateToColor(this OnlineState onlineState)
        {
            switch (onlineState)
            {
                case OnlineState.Online:
                    return Color.ForestGreen;
                case OnlineState.Idle:
                case OnlineState.Away:
                    return Color.Yellow;
                case OnlineState.Busy:
                    return Color.Red;
                default:
                    return Color.Gray;
            }
        }
    }
}
