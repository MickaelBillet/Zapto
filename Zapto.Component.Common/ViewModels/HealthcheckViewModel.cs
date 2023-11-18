﻿using AirZapto.Application;
using AirZapto.Model.Healthcheck;
using Connect.Application;
using Connect.Model.Healthcheck;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using WeatherZapto.Application;
using WeatherZapto.Model.Healthcheck;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface IHealthCheckViewModel : IBaseViewModel
	{
        Task<List<HealthCheckModel>?> GetModelList();
    }

	public sealed class HealthCheckViewModel : BaseViewModel, IHealthCheckViewModel
    {
        #region Properties
        private IApplicationHealthCheckConnectServices ApplicationHealthCheckConnectServices { get; }
        private IApplicationHealthCheckAirZaptoServices ApplicationHealthCheckAirZaptoServices { get; }
        private IApplicationHealthCheckWeatherZaptoServices ApplicationHealthCheckWeatherZaptoServices { get; }
        #endregion

        #region Constructor
        public HealthCheckViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
            this.ApplicationHealthCheckConnectServices = serviceProvider.GetRequiredService<IApplicationHealthCheckConnectServices>();
            this.ApplicationHealthCheckAirZaptoServices = serviceProvider.GetRequiredService<IApplicationHealthCheckAirZaptoServices>();
            this.ApplicationHealthCheckWeatherZaptoServices = serviceProvider.GetRequiredService<IApplicationHealthCheckWeatherZaptoServices>();
        }
        #endregion

        #region Methods
        public override async Task InitializeAsync(string? parameter)
		{
			await base.InitializeAsync(parameter);
		}

        public async Task<List<HealthCheckModel>?> GetModelList()
        {
            List<HealthCheckModel> items = new List<HealthCheckModel>();

                HealthCheckConnect? healthcheckConnect = await this.ApplicationHealthCheckConnectServices.GetHealthCheckConnect();
                if (healthcheckConnect != null)
                {                    
                    items.Add(this.GetModel(healthcheckConnect));
                }

                HealthCheckAirZapto? healthcheckAirZapto = await this.ApplicationHealthCheckAirZaptoServices.GetHealthCheckAirZapto();
                if (healthcheckAirZapto != null)
                {                   
                    items.Add(this.GetModel(healthcheckAirZapto));
                }

                HealthCheckWeatherZapto? healthCheckWeatherZapto = await this.ApplicationHealthCheckWeatherZaptoServices.GetHealthCheckWeatherZapto();
                if (healthCheckWeatherZapto != null)
                {
                    items.Add(this.GetModel(healthCheckWeatherZapto));
                }

            return items;
        }

        private HealthCheckModel GetModel(HealthCheckWeatherZapto healthCheckWeatherZapto)
        {
            HealthCheckModel weatherZapto = new HealthCheckModel();
            weatherZapto.Name = "WeatherZapto WebServer";
            weatherZapto.Status = healthCheckWeatherZapto.Status;
            weatherZapto.Items = new List<HealthcheckItemModel>();
            weatherZapto.Items?.Add(new HealthcheckItemModel
            {
                Name = healthCheckWeatherZapto?.Entries?.Memory?.GetType().Name,
                Description = healthCheckWeatherZapto?.Entries?.Memory?.Description,
                Status = healthCheckWeatherZapto?.Entries?.Memory?.Status,
                Exception = healthCheckWeatherZapto?.Entries?.Memory?.Exception?.ToString(),
                Tags = healthCheckWeatherZapto?.Entries?.Memory?.Tags?.Aggregate((workingSentence, next) => workingSentence + " " + next),
            });
            weatherZapto.Items?.Add(new HealthcheckItemModel
            {
                Name = healthCheckWeatherZapto?.Entries?.ErrorSystem?.GetType().Name,
                Description = healthCheckWeatherZapto?.Entries?.ErrorSystem?.Description,
                Status = healthCheckWeatherZapto?.Entries?.ErrorSystem?.Status,
                Exception = healthCheckWeatherZapto?.Entries?.ErrorSystem?.Exception?.ToString(),
                Tags = healthCheckWeatherZapto?.Entries?.ErrorSystem?.Tags?.Aggregate((workingSentence, next) => workingSentence + " " + next),
            });
            weatherZapto.Items?.Add(new HealthcheckItemModel
            {
                Name = healthCheckWeatherZapto?.Entries?.PostGreSql?.GetType().Name,
                Description = healthCheckWeatherZapto?.Entries?.PostGreSql?.Description?.ToString(),
                Status = healthCheckWeatherZapto?.Entries?.PostGreSql?.Status,
                Exception = healthCheckWeatherZapto?.Entries?.PostGreSql?.Exception?.ToString(),
                Tags = healthCheckWeatherZapto?.Entries?.PostGreSql?.Tags?.Aggregate((workingSentence, next) => workingSentence + " " + next),
            });
            return weatherZapto;
        }
        private HealthCheckModel GetModel(HealthCheckConnect healthCheckConnect)
        {
            HealthCheckModel connect = new HealthCheckModel();
            connect.Name = "Connect WebServer";
            connect.Status = healthCheckConnect.Status;
            connect.Items = new List<HealthcheckItemModel>();
            connect.Items?.Add(new HealthcheckItemModel
            {
                Name = healthCheckConnect?.Entries?.Memory?.GetType().Name,
                Description = healthCheckConnect?.Entries?.Memory?.Description,
                Status = healthCheckConnect?.Entries?.Memory?.Status,
                Exception = healthCheckConnect?.Entries?.Memory?.Exception?.ToString(),
                Tags = healthCheckConnect?.Entries?.Memory?.Tags?.Aggregate((workingSentence, next) => workingSentence + " " + next),
            });
            connect.Items?.Add(new HealthcheckItemModel
            {
                Name = healthCheckConnect?.Entries?.SensorsStatus?.GetType().Name,
                Description = healthCheckConnect?.Entries?.SensorsStatus?.Description,
                Status = healthCheckConnect?.Entries?.SensorsStatus?.Status,
                Exception = healthCheckConnect?.Entries?.SensorsStatus?.Exception?.ToString(),
                Tags = healthCheckConnect?.Entries?.SensorsStatus?.Tags?.Aggregate((workingSentence, next) => workingSentence + " " + next),
            });
            connect.Items?.Add(new HealthcheckItemModel
            {
                Name = healthCheckConnect?.Entries?.ServerIotConnection?.GetType().Name,
                Description = healthCheckConnect?.Entries?.ServerIotConnection?.Description,
                Status = healthCheckConnect?.Entries?.ServerIotConnection?.Status,
                Exception = healthCheckConnect?.Entries?.ServerIotConnection?.Exception?.ToString(),
                Tags = healthCheckConnect?.Entries?.ServerIotConnection?.Tags?.Aggregate((workingSentence, next) => workingSentence + " " + next),
            });
            connect.Items?.Add(new HealthcheckItemModel
            {
                Name = healthCheckConnect?.Entries?.SignalR?.GetType().Name,
                Description = healthCheckConnect?.Entries?.SignalR?.Description?.ToString(),
                Status = healthCheckConnect?.Entries?.SignalR?.Status,
                Exception = healthCheckConnect?.Entries?.SignalR?.Exception?.ToString(),
                Tags = healthCheckConnect?.Entries?.SignalR?.Tags?.Aggregate((workingSentence, next) => workingSentence + " " + next),
            });
            connect.Items?.Add(new HealthcheckItemModel
            {
                Name = healthCheckConnect?.Entries?.ErrorSystem?.GetType().Name,
                Description = healthCheckConnect?.Entries?.ErrorSystem?.Description,
                Status = healthCheckConnect?.Entries?.ErrorSystem?.Status,
                Exception = healthCheckConnect?.Entries?.ErrorSystem?.Exception?.ToString(),
                Tags = healthCheckConnect?.Entries?.ErrorSystem?.Tags?.Aggregate((workingSentence, next) => workingSentence + " " + next),
            });
            connect.Items?.Add(new HealthcheckItemModel
            {
                Name = healthCheckConnect?.Entries?.Sqlite?.GetType().Name,
                Description = healthCheckConnect?.Entries?.Sqlite?.Description?.ToString(),
                Status = healthCheckConnect?.Entries?.Sqlite?.Status,
                Exception = healthCheckConnect?.Entries?.Sqlite?.Exception?.ToString(),
                Tags = healthCheckConnect?.Entries?.Sqlite?.Tags?.Aggregate((workingSentence, next) => workingSentence + " " + next),
            });
            return connect;
        }
        private HealthCheckModel GetModel(HealthCheckAirZapto healthcheckAirZapto)
        {
            HealthCheckModel airzapto = new HealthCheckModel();
            airzapto.Name = "Air Zapto WebServer";
            airzapto.Status = healthcheckAirZapto.Status;
            airzapto.Items = new List<HealthcheckItemModel>();
            airzapto.Items?.Add(new HealthcheckItemModel
            {
                Name = healthcheckAirZapto?.Entries?.Memory?.GetType().Name,
                Description = healthcheckAirZapto?.Entries?.Memory?.Description,
                Status = healthcheckAirZapto?.Entries?.Memory?.Status,
                Exception = healthcheckAirZapto?.Entries?.Memory?.Exception?.ToString(),
                Tags = healthcheckAirZapto?.Entries?.Memory?.Tags?.Aggregate((workingSentence, next) => workingSentence + " " + next),
            });
            airzapto.Items?.Add(new HealthcheckItemModel
            {
                Name = healthcheckAirZapto?.Entries?.SensorsStatus?.GetType().Name,
                Description = healthcheckAirZapto?.Entries?.SensorsStatus?.Description,
                Status = healthcheckAirZapto?.Entries?.SensorsStatus?.Status,
                Exception = healthcheckAirZapto?.Entries?.SensorsStatus?.Exception?.ToString(),
                Tags = healthcheckAirZapto?.Entries?.SensorsStatus?.Tags?.Aggregate((workingSentence, next) => workingSentence + " " + next),
            });
            airzapto.Items?.Add(new HealthcheckItemModel
            {
                Name = healthcheckAirZapto?.Entries?.ErrorSystem?.GetType().Name,
                Description = healthcheckAirZapto?.Entries?.ErrorSystem?.Description,
                Status = healthcheckAirZapto?.Entries?.ErrorSystem?.Status,
                Exception = healthcheckAirZapto?.Entries?.ErrorSystem?.Exception?.ToString(),
                Tags = healthcheckAirZapto?.Entries?.ErrorSystem?.Tags?.Aggregate((workingSentence, next) => workingSentence + " " + next),
            });
            airzapto.Items?.Add(new HealthcheckItemModel
            {
                Name = healthcheckAirZapto?.Entries?.Sqlite?.GetType().Name,
                Description = healthcheckAirZapto?.Entries?.Sqlite?.Description?.ToString(),
                Status = healthcheckAirZapto?.Entries?.Sqlite?.Status,
                Exception = healthcheckAirZapto?.Entries?.Sqlite?.Exception?.ToString(),
                Tags = healthcheckAirZapto?.Entries?.Sqlite?.Tags?.Aggregate((workingSentence, next) => workingSentence + " " + next),
            });

            return airzapto;
        }
        #endregion
    }
}
