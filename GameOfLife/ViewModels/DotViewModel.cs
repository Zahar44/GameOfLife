using GameOfLife.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.ViewModels
{
    public delegate void DotStatusChangedHandler(object sender, DotStatusChangedArgs e);

    public class DotStatusChangedArgs : EventArgs
    {
        public bool isAlive { get; set; }
    }

    public class DotViewModel : INotifyPropertyChanged, IDotViewModel
    {
        private readonly int id;
        private bool isAlive = false;
        private Command dotCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public event DotStatusChangedHandler StatusChanged;

        public Command ChangeAliveStatusCommand
        {
            get
            {
                return dotCommand ??
                  (dotCommand = new Command(obj =>
                  {
                      IsAlive = !IsAlive;
                  }));
            }
        }

        public bool IsAlive
        {
            get { return isAlive; }
            set 
            { 
                isAlive = value;
                //OnPropertyChanged();
                StatusChanged?.Invoke(this, new DotStatusChangedArgs { isAlive = value });
            }
        }

        public DotViewModel(ref int id)
        {
            this.id = id;
        }




        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void ChangeDotStatus()
        {
            ChangeAliveStatusCommand.Execute(null);
        }

        public void Kill()
        {
            IsAlive = false;
        }

        public void Revive()
        {
            IsAlive = true;
        }
    }
}
