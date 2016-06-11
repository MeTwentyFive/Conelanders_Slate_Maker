using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Conelanders_Slate_Maker {

	public class QualifyTimes {

		public string DriverName;
		public string DriverGuid;
		public string CarId;
		public string CarModel;
		public int    BestLap;
		public int    TotalTime;
		public int    BallastKG;

		[ScriptIgnore]
		public bool ValidTime {

			get {

				if( BestLap == 999999999 || BestLap == -1 ) {
					return false;
				}

				return true;

			}

		}

		[ScriptIgnore]
		public string LapTime {

			get {
				TimeSpan lap  = TimeSpan.FromMilliseconds( BestLap );
				string output = "NO TIME";

				if( ValidTime ) {
					output = String.Format("{0}:{1,2}.{2,3}", lap.Minutes, lap.Seconds.ToString("D2"), lap.Milliseconds.ToString("D3") );
				}
				else if( BestLap == -1 ) {
					output = string.Empty;
				}

				return output;

			}

		}

	}

}
