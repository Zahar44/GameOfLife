using GameOfLife.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.ViewModels
{
    public interface IDotViewModel
    {
        bool IsAlive { get; }
        event DotStatusChangedHandler StatusChanged;

        void ChangeDotStatus();
        void Kill();
        void Revive();
    }
}
