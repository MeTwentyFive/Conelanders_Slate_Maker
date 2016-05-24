using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Conelanders_Slate_Maker {

	class Program {

		//Read in the json data and deserialize it.  You'll want to try catch this if I don't do it later.
		static QualifyResults ReadQualifyData( string filename ) {
			string               jsonInfo   = File.ReadAllText( filename );
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			QualifyResults       results    = serializer.Deserialize<QualifyResults>( jsonInfo.ToString() );

			return results;

		}

		//This is me being lazy/quick.  You know how to do this correctly and should.
		//This just checks the directory I'll leave it in and the directoy it will be in during debugging
		static string FindSkinsPath() {

			if( Directory.Exists( "Skins" ) ) {
				return "Skins";
			}

			if( Directory.Exists( @"..\..\Skins" ) ) {
				return @"..\..\Skins";
			}

			throw new FileNotFoundException( "Cloudn't find the skins directory" );

		}

		//Arial Heavy
		//DisposableDigi BB
		static void Main( string[] args ) {
			//FontFamily[]            fontFamilies;
			//InstalledFontCollection installedFontCollection = new InstalledFontCollection();

			// Get the array of FontFamily objects.
			//fontFamilies = installedFontCollection.Families;

			//foreach( System.Drawing.FontFamily font in fontFamilies ) {
			//	string temp = font.Name;
			//	if( temp.ToLower().Contains( "dig" ) ) {
			//		Console.WriteLine( "{0}", temp );
			//	}
			//}
			//

			var slate = new Slate( @"E:\Users\cwinton\Videos\documents-export-2016-05-03\RowPlates_Blank.tif" );
			slate.AddText( "LESSCUBES", "Bulletproof BB", 72, 132, 776, 665, 866 );
			slate.AddText( "www.youtube.com/user/lesscubes", "DisposableDigi BB", 48, 162, 903, 695, 947 );
			slate.AddText( "Rookie Class", "DisposableDigi BB", 40, 291, 974 );
			slate.AddOutLinedText( "17", "DisposableDigi BB", 99, 697, 836 );
			slate.AddOutLinedText( "2:20.747", "DisposableDigi BB", 48, 306, 1035 );
			slate.AddImage( @"..\..\Skins\25.png", -341, -230 );

			//slate.AddText( "Maestro-Ponchik", "Bulletproof BB", 72, 1279, 792, 1756, 856 );
			slate.AddText( "Maestro-Ponchik", "Bulletproof BB", 72, 1259, 776, 1804, 866 );
			slate.AddText( "https://goo.gl/z84w5T", "DisposableDigi BB", 48, 1262, 903, 1795, 947 );
			slate.AddText( "\"Rookie\" Class", "DisposableDigi BB", 40, 1391, 974 );
			slate.AddOutLinedText( "13", "DisposableDigi BB", 99, 1107, 836 );
			slate.AddOutLinedText( "2:18.086", "DisposableDigi BB", 48, 1406, 1035 );
			slate.AddImage( @"..\..\Skins\roterx2.png", 659, -230 );

			slate.Save( @"E:\Users\cwinton\Videos\documents-export-2016-05-03\RowPlates_test1.png" );
			//714, 843


			////NEED A CATCH
			//var    temp = ReadQualifyData( @"2016_5_15_15_32_QUALIFY.json" );
			//string skins = FindSkinsPath();

			//foreach( CarInfoResult car in temp.Cars ) {
			//	string skinPath = Path.Combine( skins, car.Skin + ".png" );

			//	//Console.WriteLine( "found skin for: {0}", car.Driver );

			//	if( !File.Exists( skinPath ) ) {
			//		Console.WriteLine( "Skin not found for: {0} - '{1}'", car.Driver.Name, skinPath );
			//	}

			//}

			//Console.WriteLine( "Press any key to continue..." );
			//Console.ReadKey();

		}

	}

}
