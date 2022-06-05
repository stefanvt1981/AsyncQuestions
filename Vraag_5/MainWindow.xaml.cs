using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vraag_5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Results.Items.Clear();
            Results.Items.Add("Button clicked");
            DoWork();
            Results.Items.Add("Button click done");

        }

        private async Task WaitAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        private void DoWork()
        {
            Results.Items.Add("Do work start");

            Results.Items.Add("Do waitasync start");
            Task task = WaitAsync();
            Results.Items.Add("Do waitasync sop");

            task.GetAwaiter().GetResult();
            Results.Items.Add("Do work done");
        }
    }
}
