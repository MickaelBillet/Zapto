using Microsoft.Extensions.Configuration;

namespace Zapto.Component.Common.ViewModels
{
    public interface IAuthenticationViewModel : IBaseViewModel
    {
        public void SignOut();
        public void SignIn();
    }

    public sealed class AuthenticationViewModel : BaseViewModel, IAuthenticationViewModel
    {
		#region Properties
        private string? Authority { get; }
        private string? ClientId { get; }
		#endregion

		#region Constructor
		public AuthenticationViewModel(IServiceProvider serviceProvider, IConfiguration configuration) : base(serviceProvider)
        {
            this.Authority = (string?)configuration["Keycloak:Authority"];
            this.ClientId = (string?)configuration["Keycloak:ClientId"];
        }
		#endregion

		#region Methods
        public void SignOut()
        {
            this.NavigationService.NavigateTo($"{this.Authority}/protocol/openid-connect/logout?client_id={this.ClientId}");
        }

        public void SignIn() 
        {
        }
		#endregion
	}
}
