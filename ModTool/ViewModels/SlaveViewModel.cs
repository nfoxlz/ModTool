using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NModbus;
using System.Linq;

namespace ModTool.ViewModels
{
    internal sealed partial class SlaveViewModel : DocumentViewModel
    {
        private readonly IModbusSlave slave;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RunCommand))]
        [NotifyCanExecuteChangedFor(nameof(StopCommand))]
        private bool _isRun;

        public SlaveViewModel(IModbusSlave modbusSlave, Models.ReadWriteSetting setting)
        {
            slave = modbusSlave;
            Setting = setting;

            for (int i = 0; i < setting.Quantity; i++)
                Items.Add(new Models.DataItem());

            RefreshTimer.Interval = Setting.ScanRate;
            RefreshTimer.Start();

            IsRun = true;
        }

        [RelayCommand(CanExecute = nameof(CanRun))]
        private void Run()
        {
            RefreshTimer.Start();
            IsRun = true;
        }

        private bool CanRun() => !IsRun;

        [RelayCommand(CanExecute = nameof(CanStop))]
        private void Stop()
        {
            RefreshTimer.Stop();
            IsRun = false;
        }

        private bool CanStop() => IsRun;

        protected override bool Elapsed()
        {
            switch (Setting!.Function)
            {
                case Models.Function.ReadCoils:
                    DocumentSynchronizationContext.Send(ReadCallback<bool>, slave.DataStore.CoilDiscretes.ReadPoints(Setting.Address, Setting.Quantity));
                    break;
                case Models.Function.ReadInputs:
                    DocumentSynchronizationContext.Send(ReadCallback<bool>, slave.DataStore.CoilInputs.ReadPoints(Setting.Address, Setting.Quantity));
                    break;
                case Models.Function.ReadHoldingRegisters:
                    DocumentSynchronizationContext.Send(ReadCallback<ushort>, slave.DataStore.HoldingRegisters.ReadPoints(Setting.Address, Setting.Quantity));
                    break;
                case Models.Function.ReadInputRegisters:
                    DocumentSynchronizationContext.Send(ReadCallback<ushort>, slave.DataStore.InputRegisters.ReadPoints(Setting.Address, Setting.Quantity));
                    break;
                case Models.Function.WriteSingleCoil:
                    slave.DataStore.CoilDiscretes.WritePoints(Setting.Address, [Items[0].Data != 0]);
                    break;
                case Models.Function.WriteSingleRegister:
                    slave.DataStore.HoldingRegisters.WritePoints(Setting.Address, [Items[0].Data]);
                    break;
                case Models.Function.WriteMultipleCoils:
                    bool[] digits = new bool[Setting.Quantity];
                    for (int i = 0; i < Setting.Quantity; i++)
                        digits[i] = Items[i].Data != 0;
                    slave.DataStore.CoilDiscretes.WritePoints(Setting.Address, digits);
                    break;
                case Models.Function.WriteMultipleRegisters:
                    slave.DataStore.HoldingRegisters.WritePoints(Setting.Address, (from item in Items
                                                                                   select item.Data).ToArray());
                    break;
                default:
                    IsRun = false;
                    return false;
            }

            return true;
        }
    }
}
