﻿namespace Zapto.Component.Common.IServices
{
    public interface IStorageService
    {
        Task<T> GetItemAsync<T>(string key);
        Task SetItemAsync<T>(string key, T value);
    }
}