using CommunityToolkit.Mvvm.ComponentModel;
using NModbus;
using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace ModTool.ViewModels
{
    internal abstract partial class SerialPortConnectionViewModel : DialogViewModel
    {
        public static IEnumerable<string> PortNames { get; private set; }

        public static IEnumerable<Parity> ParityChecks { get; private set; }

        public static IEnumerable<StopBits> StopBitses { get; private set; }

        static SerialPortConnectionViewModel()
        {
            var names = new List<string>();
            for (short i = 1; i <= byte.MaxValue; i++)
                names.Add("COM" + i.ToString());
            PortNames = names;

            ParityChecks = Enum.GetValues<Parity>();
            StopBitses = Enum.GetValues<StopBits>();
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OKCommand))]
        private string? _serialPortName = "COM1";

        [ObservableProperty]
        private int? _baudRate = 9600;

        [ObservableProperty]
        private int? _dataBits = 8;

        [ObservableProperty]
        private Parity _parityCheck;

        [ObservableProperty]
        private StopBits _stopBits = StopBits.One;

        [ObservableProperty]
        private bool _isRtu = true;

        protected override bool CanOK()
        {
            if (string.IsNullOrWhiteSpace(SerialPortName) || BaudRate == null)
                return false;

            return base.CanOK();
        }

        //protected ModbusFactory CreateFactory()
        //{
        //    var port = new SerialPort(SerialPortName)
        //    {
        //        BaudRate = BaudRate!.Value,
        //        DataBits = DataBits!.Value,
        //        Parity = ParityCheck,
        //        StopBits = StopBits,
        //    };

        //    try
        //    {
        //        port.Open();

        //        //var adapter = new SerialPortAdapter(port);

        //        //var factory = new ModbusFactory();
        //        //IModbusMaster master = factory.CreateRtuMaster(adapter);

        //        //return new ModbusFactory();

        //    }
        //    catch
        //    {
        //        if (port.IsOpen)
        //            port.Close();
        //        throw;
        //    }
        //}
    }
}
