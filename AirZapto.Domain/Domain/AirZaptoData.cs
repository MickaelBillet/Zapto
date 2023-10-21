using Framework.Core.Domain;

namespace AirZapto.Model
{
	public class AirZaptoData : Item
	{
		public int? CO2 { get; set; }

		public float? Temperature { get; set; }

		public string SensorId { get; set; } = string.Empty;

		public static int GetCoefMoyenneMobile(int minutes, int count)
		{
			int coef = 1;

			if (minutes < 120)
			{
				coef = 1;
			}
			else if (minutes >= 120 && minutes < 360)
			{
				if (count < 120)
				{
					coef = 1;
				}
				else if (count >= 120)
				{
					coef = 2;
				}
			}
			else if (minutes >= 360 && minutes < 1440)
			{
				if (count < 120)
				{
					coef = 1;
				}
				else if (count >= 120 && count < 360)
				{
					coef = 2;
				}
				else if (count >= 360)
				{
					coef = 5;
				}
			}
			else if (minutes >= 1440)
			{
				if (count < 120)
				{
					coef = 1;
				}
				else if (count >= 120 && count < 360)
				{
					coef = 2;
				}
				else if (count >= 360 && count < 1440)
				{
					coef = 5;
				}
				else if (count >= 1440)
				{
					coef = 20;
				}
			}

			return coef;
		}
	}
}
