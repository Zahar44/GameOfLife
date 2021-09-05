using GameOfLife.Commands;
using GameOfLife.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameOfLife.Models
{
    public static class SimulationHandler
    {
        private static DotsCollection _dots;
        private static int delay = 1000;
        private static Thread thread;
        private static bool threadCreated = false;
        private static bool running = false;

        static SimulationHandler()
        {
            thread = new Thread(new ThreadStart(StartInternal));
            thread.IsBackground = true;
        }

        public static void Start(int _delay, DotsCollection dots)
        {
            _dots = dots;
            delay = _delay;
            running = true;

            if (!threadCreated)
            {
                threadCreated = true;
                thread.Start();
            }
        }

        public static void Stop()
        {
            running = false;
        }

        public static void ProceedAllDots(DotsCollection dots)
        {
            if (dots is null)
                throw new NullReferenceException();

            List<Action> actions = new List<Action>();
            for (int i = 0; i < dots.Data.Count; i++)
            {
                for (int j = 0; j < dots.Data[i].Count; j++)
                {
                    var action = ProceedDotStatus(dots.Data, i, j);
                    if (action is not null) actions.Add(action);
                }
            }

            foreach (var action in actions)
            {
                action();
            }
        }

        private static void StartInternal()
        {
            while (true)
            {
                Thread.Sleep(delay);

                if (running)
                {
                    ProceedAllDots(_dots);
                }
            }
        }

        private static Action ProceedDotStatus(ObservableCollection<ObservableCollection<IDotViewModel>> dots, int i, int j)
        {
            int response = GetNearbyAliveDots(dots, i, j);
            if(response <= 1 || response > 3)
            {
                if (!dots[i][j].IsAlive)
                    return null;
                return new Action(dots[i][j].Kill);
            }
            if(response == 3)
            {
                return new Action(dots[i][j].Revive);
            }
            if (response == 2)
            {
                // Do nothing
                return null;
            }
            throw new InvalidOperationException();
        }

        private static int GetNearbyAliveDots(ObservableCollection<ObservableCollection<IDotViewModel>> dots, int _i, int _j)
        {
            int aliveAmount = 0;

            for (int i = _i - 1; i <= _i + 1; i++)
            {
                for (int j = _j - 1; j <= _j + 1; j++)
                {
                    bool alive = TryToGetStatus(TryToGetDot(dots, i, j));
                    if (alive && !(i == _i && j == _j))
                    {
                        aliveAmount++;
                    }
                }
            }

            return aliveAmount;
        }

        private static IDotViewModel TryToGetDot(ObservableCollection<ObservableCollection<IDotViewModel>> dots, int i, int j)
        {
            if (i >= dots.Count || i < 0)
                return null;
            if (j >= dots[i].Count || j < 0)
                return null;
            return dots[i][j];
        }

        private static bool TryToGetStatus(IDotViewModel dot)
        {
            if (dot is null)
                return false;
            return dot.IsAlive;
        }
    }
}
