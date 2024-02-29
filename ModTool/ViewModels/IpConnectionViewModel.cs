using CommunityToolkit.Mvvm.ComponentModel;

namespace ModTool.ViewModels
{
    internal abstract partial class IpConnectionViewModel : DialogViewModel
    {
        [ObservableProperty]
        private byte[] _ip = [127, 0, 0, 1];

        [ObservableProperty]
        private int _port = 502;
    }
}
