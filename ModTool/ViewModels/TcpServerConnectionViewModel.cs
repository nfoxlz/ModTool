using System.Net;
using System.Net.Sockets;

namespace ModTool.ViewModels
{
    internal sealed class TcpServerConnectionViewModel : IpConnectionViewModel
    {
        protected override void OK(object sender)
        {
            var host = string.Join('.', Ip);
            var listener = new TcpListener(IPAddress.Parse(host), Port);

            var ui = new Views.SlaveDocument
            {
                DataContext = new SlaveDocumentViewModel(Global.Factory.CreateSlaveNetwork(listener)),
            };
            Global.AddDocument(ui, $"TCP Server by {host} - {Port}");

            base.OK(sender);
        }
    }
}
