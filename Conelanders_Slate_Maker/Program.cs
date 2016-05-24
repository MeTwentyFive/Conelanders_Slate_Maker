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

	public class Program {

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

			var slate    = new Slate( @"E:\Users\cwinton\Videos\documents-export-2016-05-03\RowPlates_Blank.tif" );
			var template = new TemplateLayout();

			slate.AddText( "LESSCUBES",                      template.LeftSide.Name             );
			slate.AddText( "www.youtube.com/user/lesscubes", template.LeftSide.YouTubeLink      );
			slate.AddText( "Rookie Class",                   template.LeftSide.Class            );
			slate.AddText( "17",                             template.LeftSide.StartingPosition );
			slate.AddText( "2:20.747",                       template.LeftSide.QualifyingTime   );
			slate.AddImage( @"..\..\Skins\25.png",           template.LeftSide.Skin             );

			slate.AddText( "Maestro-Ponchik",           template.RightSide.Name             );
			slate.AddText( "https://goo.gl/z84w5T",     template.RightSide.YouTubeLink      );
			slate.AddText( "\"Rookie\" Class",          template.RightSide.Class            );
			slate.AddText( "13",                        template.RightSide.StartingPosition );
			slate.AddText( "2:18.086",                  template.RightSide.QualifyingTime   );
			slate.AddImage( @"..\..\Skins\roterx2.png", template.RightSide.Skin             );

			////1135, 762, 1827, 1027
			//slate.ClearArea( 1135, 762, 692, 265 );

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
