using AirZapto.Data.Entities;
using AirZapto.Data.Mappers;
using AirZapto.Data.Services;
using AirZapto.Data.Services.Repositories;
using AirZapto.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;

namespace AirZapto.Data.Supervisors
{
    public sealed class SupervisorSensorData : Supervisor, ISupervisorSensorData
	{
        private readonly Lazy<ISensorDataRepository>? _lazySensorDataRepository;

        #region Properties
        private ISensorDataRepository SensorDataRepository => _lazySensorDataRepository!.Value;
        #endregion

        #region Constructor
        public SupervisorSensorData(IDalSession session, IRepositoryFactory repositoryFactory) : base()
        {
            _lazySensorDataRepository = repositoryFactory.CreateSensorDataRepository(session);
        }
        #endregion

        #region Methods
        public async Task<ResultCode> DeleteSensorDataAsync(TimeSpan span)
		{
			ResultCode result = ResultCode.CouldNotDeleteItem;
			if (this.SensorDataRepository != null)
			{
				result = (await this.SensorDataRepository.DeleteSensorDataAsync(span) == true) ? ResultCode.Ok : ResultCode.CouldNotDeleteItem;
			}

			return result;
		}

		public async Task<ResultCode> AddSensorDataAsync(AirZaptoData data)
		{
			ResultCode result = ResultCode.Ok;
			if (this.SensorDataRepository != null)
			{
				data.Id = string.IsNullOrEmpty(data.Id) ? Guid.NewGuid().ToString() : data.Id;
				data.Date = Clock.Now;
                SensorDataEntity entity = SensorDataMapper.Map(data);
				result = (await this.SensorDataRepository.AddSensorDataAsync(entity) == true) ? ResultCode.Ok : ResultCode.CouldNotCreateItem;
			}

			return result;
		}

        public async Task<DateTime?> GetTimeLastSensorDataAsync(string sensorId)
        {
            DateTime? dateTime= null;
			if (this.SensorDataRepository != null)
			{
				dateTime = await this.SensorDataRepository.GetTimeLastSensorData(sensorId);
			}

            return dateTime;
        }

        public async Task<(ResultCode, IEnumerable<AirZaptoData>?)>GetSensorDataAsync(string sensorId, int minutes)
		{
			ResultCode result = ResultCode.ItemNotFound;
			IEnumerable<AirZaptoData>? data = null;
			if (this.SensorDataRepository != null)
			{
				List<SensorDataEntity>? entities = (await this.SensorDataRepository.GetSensorDataAsync(sensorId, minutes))?.ToList();
				IEnumerable<SensorDataEntity>? output = null;
				if (entities != null)
				{
					int coef = AirZaptoData.GetCoefMovingAverage(minutes, entities.Count);
					output = (coef > 1) ? this.MovingAverage(entities, coef) : entities;
					data = (output != null) ? output.Select((item) => SensorDataMapper.Map(item)) : null;
				}
				result = ((data != null) && (data.Count() > 0)) ? ResultCode.Ok : ResultCode.ItemNotFound;
			}

			return (result, data);
		}

		private IEnumerable<SensorDataEntity> MovingAverage(List<SensorDataEntity> input, int coef)
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
