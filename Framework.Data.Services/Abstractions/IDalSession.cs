using Framework.Core.Base;
using System;

namespace Framework.Data.Abstractions
{
    public interface IDalSession : IDisposable
    {
        public IDataContextFactory? DataContextFactory { get; }
        public ConnectionType? ConnectionType { get; }
    }
}
