using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conelanders_Slate_Maker {

	/// <summary>
	/// This is a temporary solution to make it easier to do as a configurable option later.
	/// Yes this is all hard coded for now.  Need to get something out the door sooner rather than later.
	/// Also yes, this class layout and inheritence is atrocious.
	/// </summary>
	public class TemplateLayout {

		public LayoutPart LeftSide  { get; set; }
		public LayoutPart RightSide { get; set; }

		public TemplateLayout() {
			FontProperties font       = null;
			BoundingBox    leftBbox   = null;
			BoundingBox    rightBbox  = null;
			TemplatePoint  leftCoord  = null;
			TemplatePoint  rightCoord = null;

			LeftSide  = new LayoutPart();
			RightSide = new LayoutPart();

			//Name Area
			font      = new FontProperties( "Bulletproof BB", 60 );
			leftBbox  = new BoundingBox(  236, 829, 344, 72 );
			rightBbox = new BoundingBox( 1330, 829, 344, 72 );
			LeftSide.Name  = new LayoutTextBounded( font, leftBbox );
			RightSide.Name = new LayoutTextBounded( font, rightBbox );

			//Class
			font       = new FontProperties( "DisposableDigi BB", 35 );
			leftCoord  = new TemplatePoint(  298, 934 );
			rightCoord = new TemplatePoint( 1388, 934 );
			LeftSide.Class  = new LayoutTextFixedOutlined( font, leftCoord,  4 );
			RightSide.Class = new LayoutTextFixedOutlined( font, rightCoord, 4 );

			//StartingPosition
			font       = new FontProperties( "DisposableDigi BB", 149 );
			leftCoord  = new TemplatePoint(  51,  807 );
			rightCoord = new TemplatePoint( 1759, 807 );

			var temp = new LayoutTextFixedOutlined( font, leftCoord,  5, System.Drawing.Color.Red );
			temp.MultiCharOffset = -25;
			LeftSide.StartingPosition  = temp;

			temp = new LayoutTextFixedOutlined( font, rightCoord, 5, System.Drawing.Color.Red );
			temp.MultiCharOffset = -25;
			RightSide.StartingPosition = temp;


			//QualifyingTime
			font       = new FontProperties( "DisposableDigi BB", 46 );
			leftCoord  = new TemplatePoint(  326, 777 );
			rightCoord = new TemplatePoint( 1414, 777 );
			LeftSide.QualifyingTime  = new LayoutTextFixedOutlined( font, leftCoord,  5 );
			RightSide.QualifyingTime = new LayoutTextFixedOutlined( font, rightCoord, 5 );

			//457, 779 - 23, 637, 1110
			//Skin(These are negative due to me being to lazy to scale the skins down from being 1920x1080 images
			leftCoord  = new TemplatePoint( -434, -142 );
			rightCoord = new TemplatePoint(  650, -142 );
			LeftSide.Skin  = leftCoord;
			RightSide.Skin = rightCoord;

		}

	}

	public class LayoutPart {

		public LayoutText    Name             { get; set; }
		public LayoutText    Class            { get; set; }
		public LayoutText    StartingPosition { get; set; }
		public LayoutText    QualifyingTime   { get; set; }
		public TemplatePoint Skin             { get; set; }

	}

	/// <summary>
	/// Used for text that will attempt to fit the text in the bounding box.
	/// </summary>
	public class LayoutTextBounded : LayoutText {

		public BoundingBox BoundingBox { get; set; }

		public LayoutTextBounded( FontProperties font, BoundingBox bbox )
			: base( font ) {

			BoundingBox = bbox;

		}

	}

	/// <summary>
	/// Used for fixed text that will always be the size as specified
	/// </summary>
	public class LayoutTextFixedSize : LayoutText {

		public TemplatePoint Coordinate { get; set; }

		public LayoutTextFixedSize( FontProperties font, TemplatePoint coords )
			: base( font ) {

			Coordinate      = coords;

		}

	}

	/// <summary>
	/// This is stupid right now but later add settings for the outlinging and it will be less dumb.
	/// </summary>
	public class LayoutTextFixedOutlined : LayoutText {

		public TemplatePoint        Coordinate { get; set; }

		public int                  Thickness { get; set; }

		public System.Drawing.Color InnerColor { get; set; }

		//Dirty hack
		public int                  MultiCharOffset { get; set; }

		public LayoutTextFixedOutlined( FontProperties font, TemplatePoint coords, int outlineThickness )
			: base( font ) {

			Coordinate      = coords;
			Thickness       = outlineThickness;
			InnerColor      = System.Drawing.Color.Black;
			MultiCharOffset = 0;

		}

		public LayoutTextFixedOutlined( FontProperties font, TemplatePoint coords, int outlineThickness, System.Drawing.Color innerColor )
			: this( font, coords, outlineThickness ) {

			InnerColor = innerColor;

		}


	}

	/// <summary>
	/// Base layout item class
	/// </summary>
	public class LayoutText {

		public FontProperties Font { get; set; }

		public LayoutText( FontProperties font ) {
			Font = font;
		}

	}

	public class FontProperties {

		/// <summary>
		/// Name of the font family (Since we are using GDI+ must be a True Type Font)
		/// </summary>
		public string FontFamily { get; set; }

		/// <summary>
		/// For bounded text, this is the max/starting size
		/// </summary>
		public int    Size       { get; set; }

		public FontProperties( string font, int size ) {
			FontFamily = font;
			Size       = size;
		}

	}

	public class BoundingBox {

		/// <summary>
		/// Top Left corner
		/// </summary>
		public int X { get; set; }

		/// <summary>
		/// Top Left corner
		/// </summary>
		public int Y { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public BoundingBox( int x, int y, int width, int height ) {

			X      = x;
			Y      = y;

			Width  = width;
			Height = height;

		}

		//public BoundingBox( int x1, int y1, int x2, int y2 ) {

		//	X      = x1;
		//	Y      = y1;

		//	Width  = x2 - x1;
		//	Height = y2 - y1;

		//}

	}

	public class TemplatePoint {

		public int X;
		public int Y;

		public TemplatePoint( int x, int y ) {
			X = x;
			Y = y;
		}

	}

}
