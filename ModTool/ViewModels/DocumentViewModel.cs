using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Timers;

namespace ModTool.ViewModels
{
    internal abstract class DocumentViewModel : ObservableObject
    {
        protected System.Timers.Timer RefreshTimer = new();

        protected Models.ReadWriteSetting? Setting { get; set; }

        protected SynchronizationContext DocumentSynchronizationContext { get; private set; } = SynchronizationContext.Current!;

        public IList<Models.DataItem> Items { get; private set; } = new ObservableCollection<Models.DataItem>();

        public DocumentViewModel() => RefreshTimer.Elapsed += Timer_Elapsed;

        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            RefreshTimer.Stop();
            if (Elapsed())
                RefreshTimer.Start();
        }

        protected abstract bool Elapsed();

        protected void ReadCallback<T>(object? state)
        {
            T[] data = (T[])state!;
            var length = Setting!.Address + Math.Min(data.Length, Setting.Quantity);

            Items.Clear();
            for (int i = Setting.Address; i < length; i++)
                Items.Add(new Models.DataItem()
                {
                    No = i,
                    Data = Convert.ToUInt16(data[i]),
                });
        }

    }
}
