﻿namespace Zapto.Component.Common.Models
{
    public sealed record PositionModel : BaseModel
    {
        public string? CurrentLongitude { get; set; }
        public string? CurrentLatitude { get; set; }
		public string? HomeLongitude { get; set; }
		public string? HomeLatitude { get; set; }
	}
}
