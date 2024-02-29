using System.Windows;

namespace ModTool.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Global.MainDocumentPane = MainDocumentPane;
        }
    }
}