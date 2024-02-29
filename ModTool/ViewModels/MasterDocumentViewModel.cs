using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NModbus;
using System;
using System.Linq;

namespace ModTool.ViewModels
{
    internal sealed partial class MasterDocumentViewModel : DocumentViewModel, IClosedViewModel
    {
        private readonly IModbusMaster master;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RunCommand))]
        [NotifyCanExecuteChangedFor(nameof(StopCommand))]
        private bool _isRun;

        public MasterDocumentViewModel(IModbusMaster modbusMaster)
        {
            master = modbusMaster;
            Setting = new();
        }

        public void Closed()
        {
            RefreshTimer.Stop();
            master.Dispose();
        }

        [RelayCommand(CanExecute = nameof(CanRun))]
        private void Run()
        {
            var dialog = new Views.ReadWriteDefinitionDialog()
            {
                DataContext = new ReadWriteDefinitionViewModel(Setting!),
            };

            if (dialog.ShowDialog() != true)
                return;

            for (int i = 0; i < Setting!.Quantity; i++)
                Items.Add(new Models.DataItem());

            master.Transport.ReadTimeout = Setting.Timeout;
            master.Transport.WriteTimeout = Setting.Timeout;

            RefreshTimer.Interval = Setting.ScanRate;
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
            try
            {
                switch (Setting!.Function)
                {
                    case Models.Function.ReadCoils:
                        DocumentSynchronizationContext.Send(ReadCallback<bool>, master.ReadCoils(Setting.SlaveId, Setting.Address, Setting.Quantity));
                        break;
                    case Models.Function.ReadInputs:
                        DocumentSynchronizationContext.Send(ReadCallback<bool>, master.ReadInputs(Setting.SlaveId, Setting.Address, Setting.Quantity));
                        break;
                    case Models.Function.ReadHoldingRegisters:
                        DocumentSynchronizationContext.Send(ReadCallback<ushort>, master.ReadHoldingRegisters(Setting.SlaveId, Setting.Address, Setting.Quantity));
                        break;
                    case Models.Function.ReadInputRegisters:
                        DocumentSynchronizationContext.Send(ReadCallback<ushort>, master.ReadInputRegisters(Setting.SlaveId, Setting.Address, Setting.Quantity));
                        break;
                    case Models.Function.WriteSingleCoil:
                        master.WriteSingleCoil(Setting.SlaveId, Setting.Address, Items[0].Data != 0);
                        break;
                    case Models.Function.WriteSingleRegister:
                        master.WriteSingleRegister(Setting.SlaveId, Setting.Address, Items[0].Data);
                        break;
                    case Models.Function.WriteMultipleCoils:
                        bool[] digits = new bool[Setting.Quantity];
                        for (int i = 0; i < Setting.Quantity; i++)
                            digits[i] = Items[i].Data != 0;
                        master.WriteMultipleCoils(Setting.SlaveId, Setting.Address, digits);
                        break;
                    case Models.Function.WriteMultipleRegisters:
                        master.WriteMultipleRegisters(Setting.SlaveId, Setting.Address, (from item in Items
                                                                                         select item.Data).ToArray());
                        break;
                    default:
                        IsRun = false;
                        return false;
                }
            }
            catch (TimeoutException)
            {
                // TODO: 记录超时。
            }
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Error");
            //}

            return true;
        }
    }
}
