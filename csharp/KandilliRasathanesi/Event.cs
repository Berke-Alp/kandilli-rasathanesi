using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KandilliRasathanesi
{
	class Event
	{
		public string ID { get; set; }
		public string MapImage { get; set; }
		public string Time { get; set; }
		public float Magnitude { get; set; }
		public string MagnitudeType { get; set; }
		public string Latitude { get; set; }
		public string Longitude { get; set; }
		public float Depth { get; set; }
		public string Region { get; set; }
		public string AM { get; set; }
		public string LastUpdate { get; set; }
	}
}
