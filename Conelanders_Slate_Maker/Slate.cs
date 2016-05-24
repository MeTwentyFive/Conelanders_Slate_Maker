using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Conelanders_Slate_Maker {

	public class Slate {

		private Image    _Image;
		private Graphics _Drawing;

		//public Slate() {
		//}

		public Slate( string baseImage ) {

			if( !File.Exists( baseImage ) ) {
				throw new ArgumentException( String.Format( "Base image could not be found: '{0}'", baseImage ) );
			}

			_Image   = Image.FromFile( baseImage );
			_Drawing = Graphics.FromImage( _Image );
			_Drawing.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

		}

		public void AddText( string text, string font, int size, int x, int y ) {
			Font  textFont = new Font( font, size );
			Point point    = new Point(x, y);

			_Drawing.DrawString( text, textFont, Brushes.White, point );

		}

		//This should Autoscale text to fit in a bounding box
		public Font GetAdjustedFont( Graphics GraphicRef, string GraphicString, Font OriginalFont, int ContainerWidth, int ContainerHeight, int MaxFontSize, int MinFontSize, bool SmallestOnFail ) {
			Font testFont = null;

			// We utilize MeasureString which we get via a control instance           
			for( int AdjustedSize = MaxFontSize; AdjustedSize >= MinFontSize; AdjustedSize-- ) {
				testFont = new Font( OriginalFont.Name, AdjustedSize, OriginalFont.Style );

				// Test the string with the new size
				SizeF AdjustedSizeNew = GraphicRef.MeasureString( GraphicString, testFont );

				if( ContainerWidth > Convert.ToInt32( AdjustedSizeNew.Width ) && 
					ContainerHeight > Convert.ToInt32( AdjustedSizeNew.Height ) ) {
					// Good font, return it
					return testFont;
				}

			}

			// If you get here there was no fontsize that worked
			// return MinimumSize or Original?
			if( SmallestOnFail ) {
				return testFont;
			}
			else {
				return OriginalFont;
			}

		}

		public Point GetCenteredPoint( string text, Font font, Rectangle rect )  {
			SizeF size = _Drawing.MeasureString( text, font );

			int x = rect.X + ( rect.Width - (int)size.Width ) / 2;
			int y = rect.Y + ( rect.Height - (int)font.Height / 2 ) / 2 - 5;

			//_Drawing.DrawRectangle( new Pen( Color.Red, 1 ), rect.X, rect.Y, size.Width, size.Height );

			return new Point( x, y );

		}

		public void AddText( string text, string font, int size, int x1, int y1, int x2, int y2 ) {
			Font         textFont = new Font( font, size );
			var          rect     = new Rectangle( x1, y1, ( x2 - x1 ), ( y2 - y1 ) );
			StringFormat format   = new StringFormat();

			textFont = GetAdjustedFont( _Drawing, text, textFont, (x2 - x1), (y2 - y1), size, 10, false );

			//_Drawing.DrawRectangle( Pens.White, rect );
			_Drawing.DrawString( text, textFont, Brushes.White, GetCenteredPoint( text, textFont, rect ) );

		}

		public void AddOutLinedText( string text, string font, int size, int x, int y ) {
			Font         textFont = new Font( font, size );
			GraphicsPath path     = new GraphicsPath();
			Point        point    = new Point( x, y );
			Pen          fatPen   = new Pen( Color.White );

			fatPen.Alignment = PenAlignment.Outset;
			fatPen.Width     = 5;

			path.AddString( text, textFont.FontFamily, (int) FontStyle.Regular, ( _Drawing.DpiY * size / 72 ), point, new StringFormat() );

			_Drawing.InterpolationMode  = InterpolationMode.High;
			_Drawing.SmoothingMode      = SmoothingMode.HighQuality;
			_Drawing.PixelOffsetMode    = PixelOffsetMode.HighQuality;
			//This only makes it bad apparently(from the few options I tried
			//_Drawing.TextRenderingHint  = TextRenderingHint.AntiAlias;
			_Drawing.CompositingQuality = CompositingQuality.HighQuality;

			//_Drawing.DrawPath( Pens.White, path );
			_Drawing.DrawPath( fatPen, path );
			_Drawing.FillPath( new SolidBrush(Color.Black), path );

		}

		public void AddImage( string newImage, int x, int y ) {

			if( !File.Exists( newImage ) ) {
				throw new ArgumentException( String.Format( "Base image could not be found: '{0}'", newImage ) );
			}

			var point   = new Point(x, y);
			var image   = Image.FromFile( newImage );
			var graphic = Graphics.FromImage( image );
			graphic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			_Drawing.DrawImage( image, point );

		}

		public void Save( string filename ) {
			//_Drawing.Dispose();
			_Image.Save( filename );
		}

	}

}
