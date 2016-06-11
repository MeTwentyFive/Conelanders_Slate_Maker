using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conelanders_Slate_Maker {

	public class QualifyResults {

		public string          TrackName;
		public string          TrackConfig;
		public string          Type;
		public int             Duration;
		public int             RaceLaps;
		public CarInfoResult[] Cars;
		public QualifyTimes[]  Result;

		/// <summary>
		/// So this is the super hacky make everything the same format so it works.
		///  What really should happen is a object that baiscally compiles all the info and sends that to 
		///  the slate maker instead of what we are currently doing.
		/// </summary>
		/// <param name="carsList"></param>
		public void GenerateFromEntries( CarEntry[] carsList ) {
			List<CarInfoResult> cars    = new List<CarInfoResult>();
			List<QualifyTimes>  results = new List<QualifyTimes>();

			TrackName = "Unknown_Track";

			for( int i = 0; i < carsList.Length; i++ ) {
				var result  = new QualifyTimes();
				var carInfo = new CarInfoResult() {
					Driver = new DriverResultInfo()
				};

				carInfo.Driver.Name = carsList[ i ].DriverName;
				carInfo.Driver.Guid = carsList[ i ].Guid;
				carInfo.Skin        = carsList[ i ].Skin;
				carInfo.CarId       = i;

				cars.Add( carInfo );

				result.DriverName = carsList[ i ].DriverName;
				result.DriverGuid = carsList[ i ].Guid;
				result.CarId      = i.ToString();
				result.BestLap    = -1;

				results	.Add( result );

			}

			Cars   = cars.ToArray();
			Result = results.ToArray();

		}

		public QualifyResults() {

			TrackName   = "";
			TrackConfig = "";
			Type        = "";
			Duration    = 0;
			RaceLaps    = 0;
			Cars        = null;
			Result      = null;

		}

	}

}
