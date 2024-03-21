using AirZapto.Data.Entities;
using AirZapto.Data.Mappers;
using AirZapto.Data.Services;
using AirZapto.Model;
using Framework.Core.Base;

namespace AirZapto.Data.Supervisors
{
    public sealed class SupervisorSensorData : Supervisor, ISupervisorSensorData
	{
        #region Constructor
        public SupervisorSensorData(IServiceProvider serviceProvider) : base(serviceProvider)
        { }
        #endregion

        #region Methods
        public async Task<ResultCode> DeleteSensorDataAsync(TimeSpan span)
		{
			ResultCode result = ResultCode.CouldNotDeleteItem;
			if (this.Repository != null)
			{
				result = (await this.Repository.DeleteSensorDataAsync(span) == true) ? ResultCode.Ok : ResultCode.CouldNotDeleteItem;
			}

			return result;
		}

		public async Task<ResultCode> AddSensorDataAsync(AirZaptoData data)
		{
			ResultCode result = ResultCode.Ok;
			if (this.Repository != null)
			{
				data.Id = string.IsNullOrEmpty(data.Id) ? Guid.NewGuid().ToString() : data.Id;
				data.Date = Clock.Now;
                SensorDataEntity entity = SensorDataMapper.Map(data);
				result = (await this.Repository.AddSensorDataAsync(entity) == true) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
			}

			return result;
		}

        public async Task<DateTime?> GetTimeLastSensorDataAsync(string sensorId)
        {
            DateTime? dateTime= null;
			if (this.Repository != null)
			{
				dateTime = await this.Repository.GetTimeLastSensorData(sensorId);
			}

            return dateTime;
        }

        public async Task<(ResultCode, IEnumerable<AirZaptoData>?)>GetSensorDataAsync(string sensorId, int minutes)
		{
			ResultCode result = ResultCode.ItemNotFound;
			IEnumerable<AirZaptoData>? data = null;
			if (this.Repository != null)
			{
				List<SensorDataEntity>? entities = (await this.Repository.GetSensorDataAsync(sensorId, minutes))?.ToList();
				IEnumerable<SensorDataEntity>? output = null;
				if (entities != null)
				{
					int coef = AirZaptoData.GetCoefMoyenneMobile(minutes, entities.Count);
					output = (coef > 1) ? this.AverageMoving(entities, coef) : entities;
					data = (output != null) ? output.Select((item) => SensorDataMapper.Map(item)) : null;
				}
				result = ((data != null) && (data.Count() > 0)) ? ResultCode.Ok : ResultCode.ItemNotFound;
			}

			return (result, data);
		}

		private IEnumerable<SensorDataEntity> AverageMoving(List<SensorDataEntity> input, int coef)
		{
			List<SensorDataEntity> output = new List<SensorDataEntity>();

			int count = input.Count();

			//Moyenne mobile
			for (int i = 0; i < count; i = i + coef)
			{
				int? sumCO2 = 0;
				float? sumTemp = 0;
				int nbElt = 0;

				for (int j = i; j < i + coef; j++)
				{
					if (j < count)
					{
						sumCO2 = sumCO2 + input[j].CO2;
						sumTemp = sumTemp + input[j].Temperature;
						nbElt++;
					}
					else
					{
						break;
					}
				}

				output.Add(new SensorDataEntity()
				{
					Id = Guid.NewGuid().ToString(),
					SensorId = input[i].SensorId,
					CreationDateTime = input[i].CreationDateTime,
					CO2 = sumCO2 / nbElt,
					Temperature = sumTemp / nbElt,
				});
			}

			return output;
		}
        #endregion
    }
}
