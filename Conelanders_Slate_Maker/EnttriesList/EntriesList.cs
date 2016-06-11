using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Conelanders_Slate_Maker {

	public class CarEntry {

		public string Skin { get; set; }
		public string DriverName { get; set; }
		public string Guid { get; set; }
		//public string Ballast { get; set; }

		public CarEntry() {
		}

	}

	public class EntriesList {

		public CarEntry[] Entries;

		public void ParseEntryFile( string file ) {

			if( !File.Exists( file ) ) {
				throw new ArgumentException( "File not found: " + file );
			}

			StreamReader entryList;
			try {
				entryList = File.OpenText( file );
			}
			catch( Exception ex ) {
				Console.WriteLine( "Caught except trying to open entry file: {0}", ex );
				throw ex;
			}


			Regex          carPattern = new Regex( @"\[CAR_\d+]" );
			Regex          infoPatern = new Regex( @"(\S+)=(.+)" );
			List<CarEntry> cars       = new List<CarEntry>();
			CarEntry       entry      = null;

			while( !entryList.EndOfStream ) {
				string line = entryList.ReadLine().Trim();
				line.Trim();

				if( carPattern.IsMatch( line ) ) {

					if( entry != null ) {
						cars.Add( entry );
					}

					entry = new CarEntry();

				}
				else if( infoPatern.IsMatch( line ) ) {
					var    groups = infoPatern.Match( line ).Groups;
					string name   = groups[ 1 ].Value;
					string value  = groups[ 2 ].Value;

					if( name == "SKIN" ) {
						entry.Skin = value;
					}
					else if( name == "DRIVERNAME" ) {
						entry.DriverName = value;
					}
					else if( name == "GUID") {
						entry.Guid = value;
					}

				}

			}

			//Last car needs to be added
			if( entry != null ) {
				cars.Add( entry );
			}

			Entries = cars.ToArray();

		}

		public EntriesList() {
			Entries = new CarEntry[ 0 ];
		}

	}

}
