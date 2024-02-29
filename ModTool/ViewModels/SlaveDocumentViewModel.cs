using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NModbus;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ModTool.ViewModels
{
    internal sealed partial class SlaveDocumentViewModel(IModbusSlaveNetwork slaveNetwork) : ObservableObject, IClosedViewModel
    {
        private readonly IModbusSlaveNetwork modbusSlaveNetwork = slaveNetwork;

        private CancellationTokenSource? cancellationTokenSource;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RunCommand))]
        [NotifyCanExecuteChangedFor(nameof(StopCommand))]
        [NotifyCanExecuteChangedFor(nameof(AddCommand))]
        private bool _isRun;

        public ICollection<TabItem> TabItems { get; private set; } = new ObservableCollection<TabItem>();

        [ObservableProperty]
        public int _tabSelectedIndex;

        public void Closed()
        {
            if (cancellationTokenSource != null)
            {
                if (!cancellationTokenSource.IsCancellationRequested)
                    cancellationTokenSource.Cancel();
                cancellationTokenSource.Dispose();
            }

            modbusSlaveNetwork.Dispose();
        }

        [RelayCommand(CanExecute = nameof(CanRun))]
        private void Run()
        {
            cancellationTokenSource = new();
            Task.Run(() => modbusSlaveNetwork.ListenAsync(cancellationTokenSource.Token));
            IsRun = true;
        }

        private bool CanRun() => !IsRun;

        [RelayCommand(CanExecute = nameof(CanStop))]
        private void Stop()
        {
            cancellationTokenSource!.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
            IsRun = false;
        }

        private bool CanStop() => IsRun;

        [RelayCommand(CanExecute = nameof(CanStop))]
        private void Add()
        {
            Models.ReadWriteSetting setting = new();
            var dialog = new Views.ReadWriteDefinitionDialog()
            {
                DataContext = new ReadWriteDefinitionViewModel(setting),
            };

            if (dialog.ShowDialog() != true)
                return;
            IModbusSlave slave = modbusSlaveNetwork.GetSlave(setting.SlaveId);
            if (slave == null)
            {
                slave = Global.Factory.CreateSlave(setting.SlaveId);
                modbusSlaveNetwork.AddSlave(slave);
            }

            TabItems.Add(new TabItem { Header = $"Slave {setting.SlaveId} {setting.Function}", Content = new Views.DataDocument { DataContext = new SlaveViewModel(slave, setting) }, });
            TabSelectedIndex = TabItems.Count - 1;
        }
    }
}
