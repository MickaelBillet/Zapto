using Framework.Core.Domain;

namespace Connect.Model
{
    public class ClientApp : Item
    {
        private string? _description = null;
        private string? _token = null;

        #region Property

        public string LocationId { get; set; } = string.Empty;

        public string? Description
        {
            get { return _description; }

            set { SetProperty<string?>(ref _description, value); }
        }

        public string? Token
        {
            get { return _token; }

            set { SetProperty<string?>(ref _token, value); }
        }

        #endregion

        #region Constructor

        public ClientApp() : base()
        {
        }

        #endregion

        #region Method

        #endregion
    }
}