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
			font      = new FontProperties( "Bulletproof BB", 72 );
			leftBbox  = new BoundingBox(  132, 776, 545, 90 );
			rightBbox = new BoundingBox( 1259, 776, 545, 90 );
			LeftSide.Name  = new LayoutTextBounded( font, leftBbox );
			RightSide.Name = new LayoutTextBounded( font, rightBbox );

			//YouTubeLink
			font      = new FontProperties( "DisposableDigi BB", 48 );
			leftBbox  = new BoundingBox(  162, 903, 545, 44 );
			rightBbox = new BoundingBox( 1262, 903, 545, 44 );
			LeftSide.YouTubeLink  = new LayoutTextBounded( font, leftBbox );
			RightSide.YouTubeLink = new LayoutTextBounded( font, rightBbox );

			//Class
			font       = new FontProperties( "DisposableDigi BB", 40 );
			leftCoord  = new TemplatePoint(  291, 974 );
			rightCoord = new TemplatePoint( 1391, 974 );
			LeftSide.Class  = new LayoutTextFixedSize( font, leftCoord );
			RightSide.Class = new LayoutTextFixedSize( font, rightCoord );

			//StartingPosition
			font       = new FontProperties( "DisposableDigi BB", 99 );
			leftCoord  = new TemplatePoint(  697, 836 );
			rightCoord = new TemplatePoint( 1107, 836 );
			LeftSide.StartingPosition  = new LayoutTextFixedOutlined( font, leftCoord );
			RightSide.StartingPosition = new LayoutTextFixedOutlined( font, rightCoord );

			//QualifyingTime
			font       = new FontProperties( "DisposableDigi BB", 48 );
			leftCoord  = new TemplatePoint(  306, 1035 );
			rightCoord = new TemplatePoint( 1406, 1035 );
			LeftSide.QualifyingTime  = new LayoutTextFixedOutlined( font, leftCoord );
			RightSide.QualifyingTime = new LayoutTextFixedOutlined( font, rightCoord );

			//Skin(These are negative due to me being to lazy to scale the skins down from being 1920x1080 images
			leftCoord  = new TemplatePoint( -341, -230 );
			rightCoord = new TemplatePoint(  659, -230 );
			LeftSide.Skin  = leftCoord;
			RightSide.Skin = rightCoord;

		}

	}

	public class LayoutPart {

		public LayoutText    Name             { get; set; }
		public LayoutText    YouTubeLink      { get; set; }
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

			Coordinate = coords;

		}

	}

	/// <summary>
	/// This is stupid right now but later add settings for the outlinging and it will be less dumb.
	/// </summary>
	public class LayoutTextFixedOutlined : LayoutText {

		public TemplatePoint Coordinate { get; set; }

		public LayoutTextFixedOutlined( FontProperties font, TemplatePoint coords )
			: base( font ) {

			Coordinate = coords;

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
