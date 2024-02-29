using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ModTool.ViewModels
{
    internal sealed partial class MainViewModel : ObservableObject
    {
        [RelayCommand]
        private static void MasterConnect() => (new Views.SerialPortConnectionDialog() { DataContext = new MasterConnectionViewModel(), }).ShowDialog();

        [RelayCommand]
        private static void SlaveConnect() => (new Views.SerialPortConnectionDialog() { DataContext = new SlaveConnectionViewModel(), }).ShowDialog();

        [RelayCommand]
        private static void TcpClientConnect()
        {
            var dialog = new Views.IpConnectionDialog()
            {
                DataContext = new TcpClientConnectionViewModel(),
            };
            dialog.ShowDialog();
        }

        [RelayCommand]
        private static void UdpClientConnect()
        {
            var dialog = new Views.IpConnectionDialog()
            {
                DataContext = new UdpClientConnectionViewModel(),
            };
            dialog.ShowDialog();
        }

        [RelayCommand]
        private static void TcpServerConnect()
        {
            var dialog = new Views.IpConnectionDialog()
            {
                DataContext = new TcpServerConnectionViewModel(),
            };
            dialog.ShowDialog();
        }

        [RelayCommand]
        private static void UdpServerConnect()
        {
            var dialog = new Views.IpConnectionDialog()
            {
                DataContext = new UdpServerConnectionViewModel(),
            };
            dialog.ShowDialog();
        }
    }
}
