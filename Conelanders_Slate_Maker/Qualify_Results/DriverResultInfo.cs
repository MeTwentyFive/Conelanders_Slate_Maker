using System;
using System.Web.Script.Serialization;

namespace Conelanders_Slate_Maker {

	public class DriverResultInfo {

		public string Name { get; set; }
		public string Team { get; set; }
		public int    Id   { get; set; }

		[ScriptIgnore]
		public string YouTubeLink { get; set; }

	}

}
