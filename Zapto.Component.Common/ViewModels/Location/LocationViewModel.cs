using Connect.Application;
using Framework.Core.Base;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Zapto.Component.Common.Models;

namespace Zapto.Component.Common.ViewModels
{
    public interface ILocationViewModel : IBaseViewModel
    {
        Task TestNotification(string? locationId);
        public string GetLocation(LocationModel model);
    }

    public class LocationViewModel : BaseViewModel, ILocationViewModel
	{
		#region Properties
        private IApplicationConnectLocationServices ApplicationConnectLocationServices { get; }
        #endregion

        #region Constructor
        public LocationViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
		{
            this.ApplicationConnectLocationServices = serviceProvider.GetRequiredService<IApplicationConnectLocationServices>();
        }
        #endregion

        #region Methods
        public async Task TestNotification(string? locationId)
        {
            try
            {
                if (locationId != null)
                {
                    await this.ApplicationConnectLocationServices.TestNotication(locationId);
                }
            }
            catch (Exception ex)
            {
                Log.Debug($"{ClassHelper.GetCallerClassAndMethodName()} - {ex.ToString()}");
            }
        }

        public string GetLocation(LocationModel model)
        {
            string res = string .Empty;
            if (model.LocalizationIsAvailable == ProgressStaus.InProgress)
            {
                res = this.Localizer["Location in progress"];
            }
            else if (model.LocationIsAvailable == ProgressStaus.NoAvailable)
            {
                res = this.Localizer["Unable to locate"];
            }
            else if (model.LocationIsAvailable == ProgressStaus.Available)
            {
                res = model.Location!;
            }
            return res;
        }        
        #endregion
    }
}
