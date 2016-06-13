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

		const string UNKNOWN_SKIN = "unknown";

		static string SkinPath;

		//Make sure all the fonts used are actually installed.
		static void CheckFonts( string[] usedFonts ) {
			FontFamily[]            fontFamilies;
			InstalledFontCollection installedFontCollection = new InstalledFontCollection();
			HashSet<string>         fontLookup              = new HashSet<string>();

			// Get the array of FontFamily objects.
			fontFamilies = installedFontCollection.Families;

			foreach( System.Drawing.FontFamily family in fontFamilies ) {
				fontLookup.Add( family.Name.ToLower() );
			}

			//foreach( System.Drawing.FontFamily font in fontFamilies ) {
			foreach( string font in usedFonts ) {
				//string temp = font.Name;

				if( !fontLookup.Contains( font.ToLower() ) ) {
					Console.WriteLine( "Missing font: {0}", font );
				}

			}

		}

		//Read in the json data and deserialize it.  You'll want to try catch this if I don't do it later.
		static QualifyResults ReadQualifyData( string filename ) {
			string               jsonInfo   = File.ReadAllText( filename );
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			QualifyResults       results    = serializer.Deserialize<QualifyResults>( jsonInfo.ToString() );

			for( int driverIndex = 0; driverIndex < results.Cars.Length; driverIndex++ ) {

				if( String.IsNullOrWhiteSpace( results.Cars[ driverIndex ].Driver.Guid ) ) {
					List<CarInfoResult> cars = results.Cars.ToList();
					cars.RemoveAt( driverIndex-- );
					results.Cars = cars.ToArray();
				}

			}

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
			string           skinPath    = Path.Combine( SkinPath, carInfo[ 0 ].Skin + ".png" );
			string           lapTime     = qualTime[ 0 ].LapTime;

			if( !File.Exists( skinPath ) ) {
				Console.WriteLine( "Couldn't find skin: {0}", skinPath );
				skinPath    = Path.Combine( SkinPath, UNKNOWN_SKIN + ".png" );
				Console.WriteLine( "\tReplacing with alternative: {0}", skinPath );
			}

			//Do the left side
			slate.AddText(  driver.Name,                  template.LeftSide.Name             );
			slate.AddText(  driverExtra.Class + " Class", template.LeftSide.Class            );
			slate.AddText(  startingPosition.ToString(),  template.LeftSide.StartingPosition );
			slate.AddText(  lapTime,                      template.LeftSide.QualifyingTime   );
			slate.AddImage( skinPath,                     template.LeftSide.Skin             );

			if( carInfo.Length > 1 ) {

				driver      = carInfo[ 1 ].Driver;
				driverExtra = drivers.DriverLookup[ driver.Guid ];
				skinPath    = Path.Combine( SkinPath, carInfo[ 1 ].Skin + ".png" );
				lapTime     = qualTime[ 1 ].LapTime;
				startingPosition++;

				if( !File.Exists( skinPath ) ) {
					Console.WriteLine( "Couldn't find skin: {0}", skinPath );
					skinPath    = Path.Combine( SkinPath, UNKNOWN_SKIN + ".png" );
					Console.WriteLine( "\tReplacing with alternative: {0}", skinPath );
				}

				slate.AddText(  driver.Name,                  template.RightSide.Name             );
				slate.AddText(  driverExtra.Class + " Class", template.RightSide.Class            );
				slate.AddText(  startingPosition.ToString(),  template.RightSide.StartingPosition );
				slate.AddText(  lapTime,                      template.RightSide.QualifyingTime   );
				slate.AddImage( skinPath,                     template.RightSide.Skin             );

			}
			else {
				//Needs to be moved into the TemplateLayout
				slate.ClearArea( 1077, 0, 1920, 1080 );
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
				Console.WriteLine( "\tFile: '{0}'", args[ 0 ] );
				Console.ReadKey();
				Environment.Exit( 26 );
			}

			if( !Directory.Exists( args[ 1 ] ) ) {

				try {
					Directory.CreateDirectory( args[ 1 ] );
				}
				catch( Exception ex ) {
					Console.WriteLine( "Failed to create output directory, can't write output(we out)... Press Any Key to Continue\n" );
					Console.WriteLine( "Directory: '{0}' Reason: {1}", args[ 1 ], ex );
					Console.ReadKey();
					Environment.Exit( 26 );
				}

			}

			SkinPath = FindSkinsPath();

			//I'm lazy and don't want to deal with proper locations
			string DriverFiles   = "DriverFiles";
			if( !Directory.Exists( DriverFiles ) ) {
				DriverFiles = Path.Combine( @"..\..", DriverFiles );
			}

			string         slateTemplate = @"RowPlates_Blank.png";
			var            drivers       = new Drivers( DriverFiles );
			var            template      = new TemplateLayout();
			List<Task>     tasks         = new List<Task>();
			string         slateOutput   = null;
			QualifyResults qualifyData   = null;
			int            numDrivers    = 0;

			CheckFonts( template.UsedFonts.ToArray() );
			Console.ReadKey();
			System.Environment.Exit(0);

			//2016_6_5_15_18_QUALIFY.json
			try {

				if( Path.GetExtension( args[ 0 ] ) == ".json" ) {
					qualifyData = ReadQualifyData( args[ 0 ] );
				}
				else if( Path.GetExtension( args[ 0 ] ) == ".ini" ) {
					var driverList = new EntriesList();
					driverList.ParseEntryFile( args[ 0 ] );

					qualifyData = new QualifyResults();

					if( driverList.Entries != null && driverList.Entries.Length > 0 ) {
						qualifyData.GenerateFromEntries( driverList.Entries );
					}
					else {
						throw new Exception("No drivers parsed from input file: " + args[ 0 ] );
					}

				}

			}
			catch( Exception ex ) {
				Console.WriteLine( "There was an error trying to parse the inputs: " +  ex );
				Console.ReadLine();
				Environment.Exit( 25 );
			}

			slateOutput   = qualifyData.TrackName;
			numDrivers    = qualifyData.Result.Count();

			if( numDrivers >= qualifyData.Cars.Length ) {
				numDrivers = qualifyData.Cars.Length;
			}

			//NOT_FOR_COMMIT - debugging
			//numDrivers = 2;

			//Leave me alone, I'm being lazy and I didn't think of this until I had the other stuff done already.
			int slateNum = 0;
			for( int driverIndex = 0; driverIndex < numDrivers; driverIndex++ ) {
				List<CarInfoResult> carInfo   = new List<CarInfoResult>();
				List<QualifyTimes>  qualTimes = new List<QualifyTimes>();

				if( !drivers.DriverLookup.ContainsKey( qualifyData.Cars[ driverIndex ].Driver.Guid ) ) {
					Console.WriteLine( "Exiting due to not being able to find: '{0}', {1}", qualifyData.Cars[ driverIndex ].Driver.Guid, qualifyData.Cars[ driverIndex ].Driver.Name );
					Console.ReadKey();
					Environment.Exit( 25 );
				}

				//Driver for left side of slate
				carInfo.Add( qualifyData.Cars.First( m => m.Driver.Guid == qualifyData.Result[ driverIndex ].DriverGuid ) );
				qualTimes.Add( qualifyData.Result[ driverIndex ] );

				//Increment driver for the right side of slate
				driverIndex++;

				//If we are less than numDrivers we aren't at the end and will have a right side.
				if( driverIndex < numDrivers && !String.IsNullOrWhiteSpace( qualifyData.Result[ driverIndex ].DriverGuid ) ) {
					carInfo.Add( qualifyData.Cars.First( m => m.Driver.Guid == qualifyData.Result[ driverIndex ].DriverGuid ) );
					qualTimes.Add( qualifyData.Result[ driverIndex ] );
				}

				string slateName = Path.Combine( args[ 1 ], String.Format( "{0}_{1}.png", slateOutput, slateNum++ ) );

				CreateSlate( carInfo.ToArray(), qualTimes.ToArray(), driverIndex, drivers, template, slateTemplate, slateName );
				//Task task = CreateSlate( carInfo.ToArray(), qualTimes.ToArray(), driverIndex, drivers, template, slateTemplate, slateName );
				//tasks.Add( task );

			}

			////Task.WhenAll( tasks );
			//Task.Run( async () => {
			//	await Task.WhenAll( tasks );
			//} );

			Console.WriteLine( "Press any key to continue..." );
			Console.ReadKey();

		}

	}

}
