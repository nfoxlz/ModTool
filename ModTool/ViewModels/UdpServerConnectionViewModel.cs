using System.Net.Sockets;

namespace ModTool.ViewModels
{
    internal sealed class UdpServerConnectionViewModel : IpConnectionViewModel
    {
        protected override void OK(object sender)
        {
            var host = string.Join('.', Ip);
            var client = new UdpClient(host, Port);

            var ui = new Views.SlaveDocument
            {
                DataContext = new SlaveDocumentViewModel(Global.Factory.CreateSlaveNetwork(client)),
            };
            Global.AddDocument(ui, $"UDP Server by {host} - {Port}");

            base.OK(sender);
        }
    }
}
