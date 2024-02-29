using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows;

namespace ModTool.ViewModels
{
    internal abstract partial class DialogViewModel : ObservableObject
    {
        [RelayCommand(CanExecute = nameof(CanOK))]
        protected virtual void OK(object sender)
        {
            if (((FrameworkElement)sender).Parent is Window window)
                window.DialogResult = true;
        }

        [RelayCommand]
        protected virtual void Cancel(object sender)
        {
            if (((FrameworkElement)sender).Parent is Window window)
                window.DialogResult = false;
        }

        protected virtual bool CanOK() => true;
    }
}
