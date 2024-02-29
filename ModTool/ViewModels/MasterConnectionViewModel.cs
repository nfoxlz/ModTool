using NModbus;
using NModbus.Serial;
using System.IO.Ports;

namespace ModTool.ViewModels
{
    internal sealed partial class MasterConnectionViewModel : SerialPortConnectionViewModel
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

                var ui = new Views.DataDocument
                {
                    DataContext = new MasterDocumentViewModel(IsRtu ? Global.Factory.CreateRtuMaster(port) : Global.Factory.CreateAsciiMaster(port))
                };
                Global.AddDocument(ui, $"Master by {SerialPortName}");

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
