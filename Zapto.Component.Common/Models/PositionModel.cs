using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zapto.Component.Common.Models
{
    public class PositionModel : BaseModel
    {
        public string? CurrentLongitude { get; set; }
        public string? CurrentLatitude { get; set; }
		public string? HomeLongitude { get; set; }
		public string? HomeLatitude { get; set; }
	}
}
