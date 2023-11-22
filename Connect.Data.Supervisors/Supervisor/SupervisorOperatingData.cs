using Connect.Data.Entities;
using Connect.Data.Mappers;
using Connect.Data.Services.Repositories;
using Connect.Model;
using Framework.Core.Base;
using Framework.Data.Abstractions;

namespace Connect.Data.Supervisors
{
    public sealed class SupervisorOperatingData : ISupervisorOperatingData
    {
        private readonly Lazy<IRepository<OperatingDataEntity>> _lazyOperatingDataRepository;
        private readonly Lazy<IRepository<PlugEntity>> _lazyPlugRepository;

        #region Properties
        private IRepository<OperatingDataEntity> OperatingDataRepository => _lazyOperatingDataRepository.Value;
        private IRepository<PlugEntity> PlugRepository => _lazyPlugRepository.Value;
        #endregion

        #region Constructor
        public SupervisorOperatingData(IDalSession session, IRepositoryFactory repositoryFactory)
        {
            _lazyOperatingDataRepository = repositoryFactory.CreateRepository<OperatingDataEntity>(session);
            _lazyPlugRepository = repositoryFactory?.CreateRepository<PlugEntity>(session);
        }
        #endregion

        #region Methods

        public async Task<ResultCode> OperatinDataExists(string id)
        {
            return (await this.OperatingDataRepository.GetAsync(id) != null) ? ResultCode.Ok : ResultCode.ItemNotFound;
        }

        public async Task<ResultCode> DeleteOperationData(DateTime dateTime, string roomId)
        {
            ResultCode result = ResultCode.ItemNotFound;
            OperatingDataEntity entity = (await this.OperatingDataRepository.GetAsync((data) => data.CreationDateTime == dateTime
                                                                                                                && data.RoomId == roomId));
            if (entity != null)
            {
                int res = await this.OperatingDataRepository.DeleteAsync(entity);
                if (res > 0)
                {
                    result = ResultCode.Ok;
                }
                else
                {
                    result = ResultCode.CouldNotDeleteItem;
                }
            }

            return result;
        }

        public async Task<ResultCode> AddOperatingData(OperatingData data)
        {
            ResultCode result = ResultCode.CouldNotCreateItem;
            data.Id = string.IsNullOrEmpty(data.Id) ? Guid.NewGuid().ToString() : data.Id;
            int res = await this.OperatingDataRepository.InsertAsync(OperatingDataMapper.Map(data));
            if (res > 0)
            {
                result = ResultCode.Ok;
            }
            else
            {
                result = ResultCode.CouldNotCreateItem;
            }

            return result;
        }

        public async Task<IEnumerable<OperatingData>> GetRoomOperatingDataOfDay(string roomId, DateTime day)
        {
            IEnumerable<OperatingDataEntity> entities =  await this.OperatingDataRepository.GetCollectionAsync((data) => data.CreationDateTime.Day == day.Day
                                                                                                                    && data.CreationDateTime.Month == day.Month
                                                                                                                    && data.CreationDateTime.Year == day.Year
                                                                                                                    && data.RoomId == roomId
                                                                                                                    && data.ConnectedObjectId == null);
            return entities.Select(item => OperatingDataMapper.Map(item));
        }

        public async Task<string> GetRoomHeatingDurationOfDay(string roomId , DateTime day)
        {
            double duration = await this.GetWorkingDuration(roomId, day, ParameterType.Temperature);
            return (duration/60).ToString("F0");
        }

        public async Task<string> GetRoomVentilationDurationOfDay(string roomId, DateTime day)
        {
            double duration = await this.GetWorkingDuration(roomId, day, ParameterType.Humidity);
            return (duration / 60).ToString("F0");
        }

        private async Task<double> GetWorkingDuration(string roomId, DateTime day, short conditionType)
        {
            double duration = 0;

            IEnumerable<PlugEntity> entities = await this.PlugRepository.GetCollectionAsync((plug) => plug.RoomId == roomId
                                                                                        && plug.ConditionType == conditionType);
            if (entities != null)
            {
                foreach (PlugEntity entity in entities)
                {
                    duration = duration + (await this.OperatingDataRepository.GetCollectionAsync((data) => data.CreationDateTime.Day == day.Day
                                                                                         && data.CreationDateTime.Month == day.Month
                                                                                         && data.CreationDateTime.Year == day.Year
                                                                                         && data.RoomId == roomId
                                                                                         && data.ConnectedObjectId == entity.Id))
                                                                                .Max<OperatingDataEntity>((data) => data.WorkingDuration).Value;
                }
            }

            return duration;
        }

        public async Task<DateTime?> GetRoomMaxDate(string roomId)
        {
            long ticks = (await this.OperatingDataRepository.GetCollectionAsync((data) => data.RoomId == roomId)).Max<OperatingDataEntity>((data) => data.CreationDateTime.Ticks);
            return new DateTime(ticks);
        }

        public async Task<DateTime?> GetRoomMinDate(string roomId)
        {
            long ticks = (await this.OperatingDataRepository.GetCollectionAsync((data) => data.RoomId == roomId)).Min<OperatingDataEntity>((data) => data.CreationDateTime.Ticks);
            return new DateTime(ticks);
        }
        #endregion
    }
}
