using System;
using System.Collections.Generic;

namespace Framework.Core.Base
{
    public static class Mediator
    {
        private static readonly IDictionary<string, List<Action<object>>> _dict = new Dictionary<string, List<Action<object>>>();

        public static void Subscribe(string token, Action<object> callback)
        {
            if (!_dict.ContainsKey(token))
            {
                List<Action<object>> list = new List<Action<object>>
                {
                    callback
                };
                _dict.Add(token, list);
            }
            else
            {
                bool found = false;

                foreach (var item in _dict[token])
                    if (item.Method.ToString() == callback.Method.ToString())
                        found = true;

                if (!found)
                    _dict[token].Add(callback);
            }
        }

        public static void Unsubscribe(string token, Action<object> callback)
        {
            if (_dict.ContainsKey(token))
                _dict[token].Remove(callback);
        }

        public static void Notify(string token, object args = null)
        {
            if (_dict.ContainsKey(token))
                foreach (var callback in _dict[token])
                    callback(args);
        }
    }
}
