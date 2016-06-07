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
				string error_message = String.Format( "Base image could not be found: '{0}'", baseImage );

				Console.WriteLine( error_message );

				throw new ArgumentException( error_message );

			}

			_Image   = Image.FromFile( baseImage );
			_Drawing = Graphics.FromImage( _Image );
			_Drawing.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

		}

		public void AddText( string text, LayoutText textInfo ) {
			Type textType = textInfo.GetType();

			if( textType == typeof( LayoutTextFixedSize ) ) {
				AddText( text, (LayoutTextFixedSize)textInfo );
			}
			else if( textType == typeof( LayoutTextFixedOutlined ) ) {
				AddText( text, (LayoutTextFixedOutlined)textInfo );
			}
			else if( textType == typeof( LayoutTextBounded ) ) {
				AddText( text, (LayoutTextBounded)textInfo );
			}
			else {
				throw new ArgumentException( "Unrecognized type passed to AddText" );
			}

		}

		//public void AddText( string text, string font, int size, int x, int y ) {
		public void AddText( string text, LayoutTextFixedSize textInfo ) {
			Font  textFont = new Font( textInfo.Font.FontFamily, textInfo.Font.Size );
			Point point    = new Point( textInfo.Coordinate.X, textInfo.Coordinate.Y );

			_Drawing.DrawString( text, textFont, Brushes.White, point );

		}

		public void AddText( string text, LayoutTextBounded textInfo ) {
			Font         textFont = new Font( textInfo.Font.FontFamily, textInfo.Font.Size );
			var          rect     = new Rectangle( textInfo.BoundingBox.X, textInfo.BoundingBox.Y, textInfo.BoundingBox.Width, textInfo.BoundingBox.Height );
			StringFormat format   = new StringFormat();

			textFont = GetAdjustedFont( _Drawing, text, textFont, rect.Width, rect.Height, textInfo.Font.Size, 10, false );

			//_Drawing.DrawRectangle( Pens.White, rect );
			_Drawing.DrawString( text, textFont, Brushes.White, GetCenteredPoint( text, textFont, rect ) );

		}

		public void AddText( string text, LayoutTextFixedOutlined textInfo ) {
			Font         textFont = new Font( textInfo.Font.FontFamily, textInfo.Font.Size );
			GraphicsPath path     = new GraphicsPath();
			Point        point    = new Point( textInfo.Coordinate.X, textInfo.Coordinate.Y );
			Pen          fatPen   = new Pen( Color.White );

			//Dirty hack for now
			if( text.Length > 1 ) {
				point.X += textInfo.MultiCharOffset;
			}

			fatPen.Alignment = PenAlignment.Outset;
			fatPen.Width     = textInfo.Thickness;
			//fatPen.StartCap  = LineCap.Round;
			//fatPen.EndCap    = LineCap.Round;
			fatPen.LineJoin  = LineJoin.Round;

			path.AddString( text, textFont.FontFamily, (int) FontStyle.Regular, ( _Drawing.DpiY * textInfo.Font.Size / 72 ), point, new StringFormat() );

			_Drawing.InterpolationMode  = InterpolationMode.High;
			_Drawing.SmoothingMode      = SmoothingMode.HighQuality;
			_Drawing.PixelOffsetMode    = PixelOffsetMode.HighQuality;
			//This only makes it bad apparently(from the few options I tried
			//_Drawing.TextRenderingHint  = TextRenderingHint.AntiAlias;
			_Drawing.CompositingQuality = CompositingQuality.HighQuality;

			//_Drawing.DrawPath( Pens.White, path );
			_Drawing.DrawPath( fatPen, path );
			_Drawing.FillPath( new SolidBrush( textInfo.InnerColor ), path );

		}

		public void AddImage( string path, TemplatePoint coord ) {

			if( !File.Exists( path ) ) {
				throw new ArgumentException( String.Format( "Base image could not be found: '{0}'", path ) );
			}

			var point   = new Point( coord.X, coord.Y );
			var image   = Image.FromFile( path );
			var graphic = Graphics.FromImage( image );
			graphic.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

			_Drawing.DrawImage( image, point );

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
			int y = rect.Y + ( rect.Height - (int)font.Height / 2 ) / 2;

			//_Drawing.DrawRectangle( new Pen( Color.Red, 1 ), x, y, size.Width, size.Height );

			return new Point( x, y );

		}

		public void ClearArea( int x, int y, int width, int height ) {
			GraphicsPath path = new GraphicsPath();
			Rectangle    rect = new Rectangle( x, y, width, height );

			path.AddRectangle( rect );

			_Drawing.SetClip( path );
			_Drawing.Clear( Color.Transparent );
			_Drawing.ResetClip();

		}

		public void Save( string filename ) {
			//_Drawing.Dispose();
			_Image.Save( filename );
		}

	}

}
