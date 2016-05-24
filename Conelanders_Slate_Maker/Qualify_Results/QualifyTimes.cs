using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conelanders_Slate_Maker {

	public class QualifyTimes {

		public string DriverName;
		public string DriverId;
		public string CarId;
		public string CarModel;
		public int    BestLap;
		public int    TotalTime;
		public int    BallastKG;

		public TimeSpan LapTime {

			get {
				return TimeSpan.FromMilliseconds( BestLap );
			}

		}

	}

}
