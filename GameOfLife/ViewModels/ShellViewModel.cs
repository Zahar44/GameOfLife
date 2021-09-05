using GameOfLife.Commands;
using GameOfLife.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameOfLife.ViewModels
{
    public class ShellViewModel : INotifyPropertyChanged, IHoldDots
    {
        private DotsCollection dots;
        private int actualGridSize = 0;
        private int maxSize = 0;
        private GridSize gridSize;
        private Command lowerSizeCommand;
        private Command upperSizeCommand;
        private Command startCommand;
        private Command nextCommand;

        public event SizeChangedHandler GridSizeChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        
        public int RowSize 
        { 
            get => gridSize.Size;
            set
            {
                if (value < 0)
                    return;

                gridSize.Size = value;
                MaxSize = value * value;
                OnPropertyChanged();
            }
        }


        public int MaxSize
        {
            get => maxSize;
            set 
            { 
                maxSize = value; 
                OnPropertyChanged();
            }
        }


        public int ActualGridSize
        {
            get => actualGridSize;
            set
            {
                actualGridSize = value;
                OnPropertyChanged();
            }
        }

        private string playIcon;

        public string PlayIcon
        {
            get => playIcon;
            set 
            { 
                playIcon = value; 
                OnPropertyChanged();
            }
        }

        private bool isPaused;

        public bool IsPaused
        {
            get => isPaused;
            set 
            {
                isPaused = value; 
                OnPropertyChanged();
            }
        }


        public Command LowerSizeCommand => lowerSizeCommand;
        public Command UpperSizeCommand => upperSizeCommand;
        public Command StartCommand => startCommand;
        public Command NextCommand => nextCommand;
        public DotsCollection Dots => dots;

        public ShellViewModel()
        {
            gridSize = new GridSize();
            gridSize.SizeChanged += OnGridSizeChanged;
            PlayIcon = "pack://application:,,,/Resources/Play.png";
            IsPaused = true;

            lowerSizeCommand = new Command((o) => { RowSize--; });
            upperSizeCommand = new Command((o) => { RowSize++; });
            startCommand     = new Command(OnStartCommand);
            nextCommand      = new Command((o) => { SimulationHandler.ProceedAllDots(Dots); });
        }

        public void SetDots(DotsCollection dots)
        {
            if (this.dots is not null)
            {
                this.dots.CollectionLengthChanged -= DotsLengthChanged;
            }

            this.dots = dots;
            this.dots.CollectionLengthChanged += DotsLengthChanged;
        }




        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }




        private void DotsLengthChanged(object sender, CollectionLengthChangedArgs e)
        {
            ActualGridSize = e.NewLength;
        }

        private void OnGridSizeChanged(object sender, SizeChangedArgs e)
        {
            GridSizeChanged?.Invoke(sender, e);
            //SimulationHandler.Start(1000, Dots);
        }

        private void OnStartCommand(object obj)
        {
            if (IsPaused)
            {
                SimulationHandler.Start(50, Dots);
                PlayIcon = "pack://application:,,,/Resources/Pause.png";
            } else
            {
                SimulationHandler.Stop();
                PlayIcon = "pack://application:,,,/Resources/Play.png";
            }

            IsPaused = !IsPaused;
        }
    }
}
