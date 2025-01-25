using Framework.Core.Base;
using System;

namespace Framework.Data.Abstractions
{
    public interface IDalSession : IDisposable
    {
        public IDataContext? DataContext { get; }
        public ConnectionType? ConnectionType { get; }
    }
}
