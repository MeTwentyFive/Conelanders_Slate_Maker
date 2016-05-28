using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Conelanders_Slate_Maker {

	public class DriverInfo {
		public string Name        { get; set; }
		public string Class       { get; set; }
		public string SteamId     { get; set; }
		public string YouTubeLink { get; set; }
	}

	public class Drivers {

		public ConcurrentDictionary<string, DriverInfo> DriverLookup;

		//Get all the .acd files from the given directory
		private string[] GetFileList( string directoryPath ){
			var          files         = Directory.GetFiles( directoryPath );
			List<string> filteredFiles = new List<string>();

			foreach( string file in files ) {

				if( Path.GetExtension( file ).ToLower() == ".acd" ) {
					filteredFiles.Add( file );
				}

			}

			return filteredFiles.ToArray();

		}

		private async Task ParseDriverFile( string file ) {
			var driver = new DriverInfo();

			//Console.WriteLine( "Parsing file: " + file );
			using( var driverData = new StreamReader( file ) ) {
				string line = string.Empty;

				while( !driverData.EndOfStream ) {
					line = driverData.ReadLine();

					if( line == null ) {
						continue;
					}

					if( line.StartsWith( "DRIVERNAME=" ) ) {
						line = line.Replace( "DRIVERNAME=", "" );
						driver.Name = line;
					}
					else if( line.StartsWith( "CLASS=" ) ) {
						line = line.Replace( "CLASS=", "" );
						driver.Class = line;
					}
					else if( line.StartsWith( "GUID" ) ) {
						line = line.Replace( "GUID=", "" );
						driver.SteamId = line;
					}
					else if( line.StartsWith( "YOUTUBE" ) ) {
						line = line.Replace( "YOUTUBE=", "" );
						driver.YouTubeLink = line;
					}

				}

			}

			DriverLookup[ driver.SteamId ] = driver;

		}

		public Drivers( string directoryPath ) {
			DriverLookup = new ConcurrentDictionary<string, DriverInfo>();

			if( !Directory.Exists( directoryPath ) ) {
				throw new ArgumentException( "Directory not found: " + directoryPath );
			}

			string[]   driverFiles = GetFileList( directoryPath );
			List<Task> tasks       = new List<Task>();

			foreach( string file in driverFiles ) {
				Task task = ParseDriverFile( file );
				tasks.Add( task );
			}



			//Console.WriteLine( "Task.Run" );
			//new Thread( () => {
				Task.WhenAll( tasks );
			//} ).Start();
			//Console.WriteLine( "Task.Finished" );

		}


	}

}
