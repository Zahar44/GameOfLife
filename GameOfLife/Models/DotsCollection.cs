using GameOfLife.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Models
{
    public delegate void CollectionLengthChangedHandler(object sender, CollectionLengthChangedArgs e);

    public class CollectionLengthChangedArgs : EventArgs
    {
        public int OldLength { get; set; }
        public int NewLength { get; set; }
    }

    public class DotsCollection
    {
        ObservableCollection<ObservableCollection<IDotViewModel>> dots;
        protected int internalLength;
        protected int InternalLength 
        {
            get => internalLength;
            set
            {
                CollectionLengthChanged?.Invoke(this, new CollectionLengthChangedArgs { OldLength = internalLength, NewLength = value });
                internalLength = value;
            }
        }
        public int Length => InternalLength;

        public ObservableCollection<ObservableCollection<IDotViewModel>> Data => dots;

        public DotsCollection(ObservableCollection<ObservableCollection<IDotViewModel>> dots)
        {
            this.dots = dots;
            this.dots.CollectionChanged += OnCollectionChanged;
            InternalLength = GetTotalLength();

            SetEventToAllCollections();
        }

        public event CollectionLengthChangedHandler CollectionLengthChanged;

        private void SetEventToAllCollections()
        {
            foreach (var item in dots)
            {
                item.CollectionChanged -= OnCollectionLengthChanged;
            }

            foreach (var item in dots)
            {
                item.CollectionChanged += OnCollectionLengthChanged;
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetEventToAllCollections();
        }

        private void OnCollectionLengthChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            InternalLength = GetTotalLength();
        }

        private int GetTotalLength()
        {
            int res = 0;
            foreach (var item in dots)
            {
                res += item.Count;
            }
            return res;
        }
    }
}
