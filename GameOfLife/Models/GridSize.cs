using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Models
{
    public delegate void SizeChangedHandler(object sender, SizeChangedArgs e);
    public class SizeChangedArgs : EventArgs 
    {
        public int NewValue { get; set; }

        public int OldValue { get; set; }
    }

    public class GridSize
    {
        private int size;

        public int Size
        {
            get => size;
            set
            {
                SizeChanged?.Invoke(this, new SizeChangedArgs { NewValue = value, OldValue = size });
                size = value;
            }
        }

        public event SizeChangedHandler SizeChanged;

    }
}
