﻿using System;

namespace Framework.Core.Base
{
	public class PlatformCulture
	{
		public PlatformCulture(string platformCultureString)
		{
			if (String.IsNullOrEmpty(platformCultureString))
			{
                throw new ArgumentException("Expected culture identifier", nameof(platformCultureString)); 
			}

			PlatformString = platformCultureString.Replace("_", "-"); // .NET expects dash, not underscore
			int dashIndex = PlatformString.IndexOf("-", StringComparison.Ordinal);

			if (dashIndex > 0)
			{
				string[] parts = PlatformString.Split('-');
				LanguageCode = parts[0];
				LocaleCode = parts[1];
			}
			else
			{
				LanguageCode = PlatformString;
				LocaleCode = "";
			}
		}
		public string PlatformString { get; private set; }
		public string LanguageCode { get; private set; }
		public string LocaleCode { get; private set; }
		public override string ToString()
		{
			return PlatformString;
		}
	}
}
