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

		static string SkinPath;

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

		//Creates a single complete slate, pretty rough but I want to get something out there for feedback.
		static async Task CreateSlate( CarInfoResult[] carInfo, QualifyTimes[] qualTime, int startingPosition, Drivers drivers, TemplateLayout template, string slateTemplate, string outputName ) {
			Console.WriteLine( "Processing slate: {0}", outputName );
			var              slate       = new Slate( slateTemplate );
			DriverResultInfo driver      = carInfo[0].Driver;
			DriverInfo       driverExtra = drivers.DriverLookup[ driver.Guid ];
			string           skinPath    = Path.Combine( SkinPath, carInfo[0].Skin + ".png" );
			string           lapTime     = qualTime[0].LapTime;

			if( !File.Exists( skinPath ) ) {
				Console.WriteLine( "Couldn't find skin: {0}", skinPath );
				Console.WriteLine( "\tReplacing with alternative: {0}", skinPath );
				skinPath = Path.Combine( SkinPath, "25.png" );
			}

			//Do the left side
			slate.AddText(  driver.Name,                  template.LeftSide.Name             );
			slate.AddText(  driverExtra.YouTubeLink,      template.LeftSide.YouTubeLink      );
			slate.AddText(  driverExtra.Class + " Class", template.LeftSide.Class            );
			slate.AddText(  startingPosition.ToString(),  template.LeftSide.StartingPosition );
			slate.AddText(  lapTime,                      template.LeftSide.QualifyingTime   );
			slate.AddImage( skinPath,                     template.LeftSide.Skin             );

			if( carInfo.Length > 1 ) {

				driver      = carInfo[1].Driver;
				driverExtra = drivers.DriverLookup[ driver.Guid ];
				skinPath    = Path.Combine( SkinPath, carInfo[1].Skin + ".png" );
				lapTime     = qualTime[1].LapTime;
				startingPosition++;

				if( !File.Exists( skinPath ) ) {
					Console.WriteLine( "Couldn't find skin: {0}", skinPath );
					Console.WriteLine( "\tReplacing with alternative: {0}", skinPath );
					skinPath = Path.Combine( SkinPath, "25.png" );
				}

				slate.AddText(  driver.Name,                  template.RightSide.Name             );
				slate.AddText(  driverExtra.YouTubeLink,      template.RightSide.YouTubeLink      );
				slate.AddText(  driverExtra.Class + " Class", template.RightSide.Class            );
				slate.AddText(  startingPosition.ToString(),  template.RightSide.StartingPosition );
				slate.AddText(  lapTime,                      template.RightSide.QualifyingTime   );
				slate.AddImage( skinPath,                     template.RightSide.Skin             );

			}
			else {
				//Needs to be moved into the TemplateLayout
				slate.ClearArea( 1135, 762, 692, 265 );
			}

			Console.WriteLine( "Writing slate: {0}", outputName );

			slate.Save( outputName );

		}

		//Arial Heavy
		//DisposableDigi BB
		static void Main( string[] args ) {

			if( args.Count() < 2 ) {
				Console.WriteLine( "Please specify input file and output directory...  Press Any Key to Continue" );
				Console.ReadKey();
				Environment.Exit( 25 );
			}

			if( !File.Exists( args[ 0 ] ) ) {
				Console.WriteLine( "Couldn't find qualify results file...  Press Any Key to Continue" );
				Console.WriteLine( "\tFile: '{0}'", args[0] );
				Console.ReadKey();
				Environment.Exit( 26 );
			}

			if( !Directory.Exists( args[ 1 ] ) ) {

				try {
					Directory.CreateDirectory( args[ 1 ] );
				}
				catch( Exception ex ) {
					Console.WriteLine( "Failed to create output directory, can't write output(we out)... Press Any Key to Continue\n" );
					Console.WriteLine( "Directory: '{0}' Reason: {1}", args[1], ex );
					Console.ReadKey();
					Environment.Exit( 26 );
				}

			}

			SkinPath = FindSkinsPath();

			string slateTemplate = @"RowPlates_Blank.tif";
			var    qualifyData   = ReadQualifyData( @"2016_5_15_15_32_QUALIFY.json" );
			string slateOutput   = qualifyData.TrackName;
			var    drivers       = new Drivers( @"..\..\TestDriverFiles" );
			int    numDrivers    = qualifyData.Result.Count();
			var    template      = new TemplateLayout();
			List<Task> tasks     = new List<Task>();

			//Leave me alone, I'm being lazy and I didn't think of this until I had the other stuff done already.
			int slateNum = 0;
			for( int driverIndex = 0; driverIndex < numDrivers; driverIndex++ ) {
				List<CarInfoResult> carInfo   = new List<CarInfoResult>();
				List<QualifyTimes>  qualTimes = new List<QualifyTimes>();

				if( !drivers.DriverLookup.ContainsKey( qualifyData.Cars[ driverIndex ].Driver.Guid ) ) {
					Console.WriteLine( "Exiting due to not being able to find: {0}", qualifyData.Cars[ driverIndex ].Driver.Name );
					Console.ReadKey();
					Environment.Exit( 25 );
				}

				//Driver for left side of slate
				carInfo.Add( qualifyData.Cars.First( m => m.Driver.Guid == qualifyData.Result[ driverIndex ].DriverGuid ) );
				qualTimes.Add( qualifyData.Result[ driverIndex ] );

				//Increment driver for the right side of slate
				driverIndex++;

				//If we are less than numDrivers we aren't at the end and will have a right side.
				if( driverIndex < numDrivers ) {
					carInfo.Add( qualifyData.Cars.First( m => m.Driver.Guid == qualifyData.Result[ driverIndex ].DriverGuid ) );
					qualTimes.Add( qualifyData.Result[ driverIndex ] );
				}

				string slateName = Path.Combine( args[ 1 ], String.Format( "{0}_{1}.png", slateOutput, slateNum++ ) );

				//CreateSlate( carInfo.ToArray(), qualTimes.ToArray(), (driverIndex - 1), drivers, template, slateTemplate, slateName );
				Task task = CreateSlate( carInfo.ToArray(), qualTimes.ToArray(), (driverIndex - 1), drivers, template, slateTemplate, slateName );
				tasks.Add( task );

			}

			//Task.WhenAll( tasks );
			Task.Run( async () => {
				await Task.WhenAll( tasks );
			} );

			Console.WriteLine( "Press any key to continue..." );
			Console.ReadKey();

		}

	}

	//Don't want to get rid of this just yet.  I plan on using it later for something in here. (ie checking to make sure people have the right shit).
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

}
