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
