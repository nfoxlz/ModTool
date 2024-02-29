using NModbus;
using NModbus.Serial;
using System.IO.Ports;

namespace ModTool.ViewModels
{
    internal sealed class SlaveConnectionViewModel : SerialPortConnectionViewModel
    {
        protected override void OK(object sender)
        {
            var port = new SerialPort(SerialPortName)
            {
                BaudRate = BaudRate!.Value,
                DataBits = DataBits!.Value,
                Parity = ParityCheck,
                StopBits = StopBits,
            };

            try
            {
                port.Open();

                IModbusSlaveNetwork slaveNetwork = IsRtu ? Global.Factory.CreateRtuSlaveNetwork(port) : Global.Factory.CreateAsciiSlaveNetwork(port);

                var ui = new Views.SlaveDocument
                {
                    DataContext = new SlaveDocumentViewModel(slaveNetwork)
                };
                Global.AddDocument(ui, $"Slave by {SerialPortName}");

                base.OK(sender);
            }
            catch
            {
                if (port.IsOpen)
                    port.Close();
            }
        }
    }
}
