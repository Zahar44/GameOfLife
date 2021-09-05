using GameOfLife.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.ViewModels
{
    interface IHoldDots
    {
        public DotsCollection Dots { get; }
    }
}
