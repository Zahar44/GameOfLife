using GameOfLife.ViewModels;
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

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for Rectangle.xaml
    /// </summary>
    public partial class Dot : UserControl
    {
        public Dot(ref int id, ref DotViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
            viewModel.StatusChanged += OnDotChanged;
            //DebugText(id.ToString());
        }

        private void DebugText(string text)
        {
            ContentGrid.Children.Add(new TextBlock { Width = 10, FontSize = 8, Text = text });
        }

        private void OnDotChanged(object sender, DotStatusChangedArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ContentRectangle.Fill = e.isAlive
                    ? Brushes.Black
                    : Brushes.White;
            });
        }
    }
}
