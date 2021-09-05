using GameOfLife.Models;
using GameOfLife.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace GameOfLife.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : UserControl
    {
        private ShellViewModel viewModel;
        private int dotSize = 25;
        public ShellView()
        {
            InitializeComponent();

            viewModel = new ShellViewModel();
            DataContext = viewModel;
            viewModel.GridSizeChanged += GridSizeChanged;
            viewModel.RowSize = 10;
        }

        private void GridSizeChanged(object sender, SizeChangedArgs e)
        {
            int size = e.NewValue;
            if(size > 0)
            {
                RedrowGrid(size);
            }
        }


        private void RedrowGrid(int currentSize)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += ProceedRedrow;
            worker.ProgressChanged += worker_ProgressChanged;

            worker.RunWorkerAsync(currentSize);
        }

        private void ProceedRedrow(object sender, DoWorkEventArgs e)
        {
            DotsCollection dots = new DotsCollection(new ObservableCollection<ObservableCollection<IDotViewModel>>());
            int currentSize = (int)e.Argument;
            var worker = sender as BackgroundWorker;

            viewModel.SetDots(dots);

            Dispatcher.Invoke(() =>
            {
                ContentGrid.Height = currentSize * dotSize;
                ContentGrid.Width = currentSize * dotSize;
                ContentGrid.Children.Clear();
            });

            for (int i = 0; i < currentSize; i++)
            {
                dots.Data.Add(new ObservableCollection<IDotViewModel>());
                for (int j = 0; j < currentSize; j++)
                {
                    int dotId = (i * 10) + j;
                    var dotViewModel = new DotViewModel(ref dotId);
                    dots.Data[i].Add(dotViewModel);

                    Dispatcher.Invoke(() =>
                    {
                        var dot = new Dot(ref dotId, ref dotViewModel) { Width = dotSize, Height = dotSize };
                        ContentGrid.Children.Add(dot);
                        Canvas.SetLeft(dot, i * (dot.Width - 1));
                        Canvas.SetTop(dot, j * (dot.Height - 1));
                    });
                    
                    worker.ReportProgress(dots.Length);
                }
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            DotsProgressBar.Value = e.ProgressPercentage;
        }
    }
}
