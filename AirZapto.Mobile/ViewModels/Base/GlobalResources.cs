using Framework.Core.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AirZapto.ViewModel
{
	public class GlobalResources : ObservableObject
    {
        // Singleton
        public static GlobalResources Current = new GlobalResources();

        private string _message;

        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private GlobalResources()
        { }
    }
}
