using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Threading;

namespace SuperReminder
{
    public class ThreadSafeDispatchedObservableCollection<T> : INotifyCollectionChanged, IEnumerable<T>
    {
        private readonly List<T> _data = new List<T>();
        private readonly object _sync = new object();

        #region Implementation of INotifyCollectionChanged

        private readonly Dictionary<Dispatcher, List<NotifyCollectionChangedEventHandler>> _listEventDelegates =
            new Dictionary<Dispatcher, List<NotifyCollectionChangedEventHandler>>();

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                if (!_listEventDelegates.ContainsKey(Dispatcher.CurrentDispatcher))
                    _listEventDelegates[Dispatcher.CurrentDispatcher] = new List<NotifyCollectionChangedEventHandler>();
                _listEventDelegates[Dispatcher.CurrentDispatcher].Add(value);
            }
            remove
            {
                if (_listEventDelegates[Dispatcher.CurrentDispatcher] == null)
                    return;
                _listEventDelegates[Dispatcher.CurrentDispatcher].Remove(value);
            }
        }


        private void OnCollectionChange(NotifyCollectionChangedEventArgs args)
        {
            foreach (var listEventDelegate in _listEventDelegates)
            {
                listEventDelegate.Key.Invoke(
                    DispatcherPriority.Normal,
                    new Action(() =>
                                   {
                                       foreach (var del in listEventDelegate.Value)
                                       {
                                           del(this, args);
                                       }
                                   }));
            }
        }

        #endregion

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator to a copy of the list. It is a copy in case the original changes during enumnaration. 
        /// </summary>
        /// <returns>Enumerator for the list</returns>
        public IEnumerator<T> GetEnumerator()
        {
            var clone = new List<T>(_data);
            return clone.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public void Add(T item)
        {
            var newIndex = 0;
            lock (_sync)
            {
                _data.Add(item);
                newIndex = _data.IndexOf(item);
            }
            OnCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, newIndex));
        }

        public void Remove(T item)
        {
            var oldIndex = 0;
            lock (_sync)
            {
                oldIndex = _data.IndexOf(item);
                _data.Remove(item);
            }
            OnCollectionChange(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, oldIndex));
        }


    }
}