using NModbus;
using System;
using System.Windows;
using Xceed.Wpf.AvalonDock.Layout;

namespace ModTool
{
    internal static class Global
    {
        /// <summary>
        /// 获取或设置主文档面板。
        /// </summary>
        public static LayoutDocumentPane? MainDocumentPane { get; set; }

        public static readonly ModbusFactory Factory = new();

        public static void AddDocument(object ui, string title)
        {
            var document = new LayoutDocument() { Content = ui, Title = title };
            document.Closed += Document_Closed;
            MainDocumentPane!.Children.Add(document);
            MainDocumentPane.SelectedContentIndex = MainDocumentPane.Children.Count - 1;
        }

        private static void Document_Closed(object? sender, EventArgs e)
        {
            if (((FrameworkElement)((LayoutContent)sender!).Content).DataContext is ViewModels.IClosedViewModel closedViewModel)
                closedViewModel.Closed();
        }

        public static object? GetViewModel()
        {
            if (MainDocumentPane?.SelectedContent.Content is FrameworkElement element)
                return element.DataContext;

            return null;
        }
    }
}
