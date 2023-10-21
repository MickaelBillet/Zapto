using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Framework.Core.Base
{
    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }

        public Grouping(K key, ObservableCollection<T> items)
        {
            this.Key = key;

            foreach (var item in items)
            {
                this.Items.Add(item);
            }
        }
    }
}
