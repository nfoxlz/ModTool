using System.Net.Sockets;

namespace ModTool.ViewModels
{
    internal sealed class TcpClientConnectionViewModel : IpConnectionViewModel
    {
        protected override void OK(object sender)
        {
            var host = string.Join('.', Ip);
            var client = new TcpClient(host, Port);

            var ui = new Views.DataDocument
            {
                DataContext = new MasterDocumentViewModel(Global.Factory.CreateMaster(client)),
            };
            Global.AddDocument(ui, $"TCP Client by {host} - {Port}");

            base.OK(sender);
        }
    }
}
