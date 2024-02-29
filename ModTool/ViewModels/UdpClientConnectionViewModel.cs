using System.Net.Sockets;

namespace ModTool.ViewModels
{
    internal sealed class UdpClientConnectionViewModel : IpConnectionViewModel
    {
        protected override void OK(object sender)
        {
            var host = string.Join('.', Ip);
            var client = new UdpClient(host, Port);

            var ui = new Views.DataDocument
            {
                DataContext = new MasterDocumentViewModel(Global.Factory.CreateMaster(client)),
            };
            Global.AddDocument(ui, $"UDP Client by {host} - {Port}");

            base.OK(sender);
        }
    }
}
