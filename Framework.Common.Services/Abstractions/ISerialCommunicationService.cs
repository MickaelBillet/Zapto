namespace Framework.Common.Services
{
    public interface ISerialCommunicationService : ISendMessageService
    {
        void Dispose();
    }
}