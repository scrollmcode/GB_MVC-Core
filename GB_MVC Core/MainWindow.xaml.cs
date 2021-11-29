using System;
using System.Collections.Generic;
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

namespace GB_MVC_Core
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            Fib rowFib = new Fib();
            rowFib.TimeWait = int.Parse(tbTimeWait.Text) * 1000;

            rowFib.row.change += () =>
                {
                    tbResult.Text = rowFib.ToString();
                };

            //TimerCallback callBack = new TimerCallback((o) =>
            //{
            //    tbResult.Text = rowFib.ToString();
            //});

            //Timer t = new Timer(callBack, null, 0, 500);

            bStart.Click += (s, e) =>
            {
                if (bStart.Content.Equals("Start"))
                {
                    bStart.Content = "Stop";
                    tbStatus.Text = "Расчёт";
                    rowFib.Generate();
                    tbResult.Text = rowFib.ToString();
                }
                else
                {
                    tbStatus.Text = "Расчёт остановлен";
                    bStart.Content = "Start";
                    rowFib.StopGenerate();
                }
            };

            tbTimeWait.TextChanged += (s, e) =>
            {
                rowFib.TimeWait = int.Parse(tbTimeWait.Text) * 1000;
            };
        }

        private class Fib
        {
            public accessList row = new accessList(new List<int>() { 1, 1 });
            public int TimeWait { get; set; }
            Thread tGenerateFib;
            object locker = new object();

            public Fib()
            {
                tGenerateFib = new Thread((locker) =>
                {
                    lock (row)
                    {
                        try
                        {
                            while (true)
                            {
                                Thread.Sleep(TimeWait);
                                GenerateNext();
                            }
                        }
                        catch (ThreadInterruptedException e)
                        {

                        }
                    }
                });
            }

            public void Generate()
            {
                tGenerateFib.Start();
            }

            public void StopGenerate()
            {
                tGenerateFib.Interrupt();
            }

            public void GenerateNext()
            {
                lock (this.row)
                {
                    int nextItem =
                        this.row.get(this.row.Count - 2) + this.row.get(this.row.Count - 1);
                    this.row.Add(nextItem);
                }
            }

            public override string ToString()
            {
                return row.ToString();
            }
        }

        private class accessList
        {
            public accessList(List<int> innerList)
            {
                list = innerList;
            }

            public int Count { get { return list.Count; } }

            private List<int> list;

            public void Add(int item)
            {
                list.Add(item);
                change?.Invoke();
            }

            public int get(int index)
            {
                return list[index];
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();

                foreach (int i in this.list)
                {
                    sb.Append(i + " ");
                }

                return sb.ToString();
            }

            public event Action change;
        }
    }
}
