using Core.MonteCarlo;
using Core.MonteCarlo.Simulations;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.ObjectModel;
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
using WPFComponentsLibrary;

namespace GUI_Sem1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MonteCarloGUI : Window
    {
        public List<PlotModel> Graphs { get; set; } = [];
        private List<BaseCore> _simulations = [];

        public MonteCarloGUI()
        {
            WindowState = WindowState.Maximized;
            InitializeComponent();
            InitGraphs();
        }

        private void InitGraphs()
        {
            PlotModel graph1 = CreateReplicationTimeGraph("Simulation 1 - Z-S-R-D-Z");
            PlotModel graph2 = CreateReplicationTimeGraph("Simulation 2 - Z-D-R-S-Z");
            PlotModel graph3 = CreateReplicationTimeGraph("Simulation 3 - Z-S-D-R-Z");
            PlotModel graph4 = CreateReplicationTimeGraph("Simulation 4 - Z-D-S-R-Z");
            PlotModel graph5 = CreateReplicationTimeGraph("Simulation 5 - Z-R-D-S-Z");
            PlotModel graph6 = CreateReplicationTimeGraph("Simulation 6 - Z-R-S-D-Z");

            Graphs.AddRange(graph1, graph2, graph3, graph4, graph5, graph6);

            Sim1Graph.Model = graph1;
            Sim2Graph.Model = graph2;
            Sim3Graph.Model = graph3;
            Sim4Graph.Model = graph4;
            Sim5Graph.Model = graph5;
            Sim6Graph.Model = graph6;

            Graphs.ForEach(graph => graph.InvalidatePlot(true));
        }

        private PlotModel CreateReplicationTimeGraph(string title)
        {
            var model = new PlotModel
            {
                Title = title
            };

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Replications",
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Time (min)"
            });

            model.Series.Add(new LineSeries
            {
                Title = "Time (min)"
            });

            return model;
        }

        private async void BestPath_Click(object sender, RoutedEventArgs e)
        {
            TopBar.Stop = false;
            TopBar.ResetStopButton();
            Graphs.Clear();
            InitGraphs();
            _simulations.Clear();
            Graphs.ForEach(graph => graph.InvalidatePlot(true));

            int replications = int.Parse(TopBar.Replications);
            int replicationsCuts = int.Parse(TopBar.ReplicationCut);

            listBoxResults.Items.Clear();

            _simulations = new List<BaseCore>() 
            {
                new Sim1(replications),
                new Sim2(replications),
                new Sim3(replications),
                new Sim4(replications),
                new Sim5(replications),
                new Sim6(replications)
            };

            int i = 0;
            foreach (BaseCore simulation in _simulations)
            {

                if (TopBar.Stop) break;
                int index = i;
                simulation.GraphPercentCut = replicationsCuts;
                listBoxResults.Items.Add($"Starting {Graphs[index].Title}");

                Action stopAction = () => 
                { 
                    simulation.Stopped = true; 
                };
                TopBar.OnStopRequest += stopAction;

                await Task.Run(() =>
                {
                    var series = Graphs[index].Series[0] as LineSeries;

                    simulation.OnUpdate = (rep, avg) =>
                    {
                        Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            DataPoint point = new DataPoint(rep, avg / 60);
                            series.Points.Add(point);
                            Graphs[index].InvalidatePlot(true);
                        });
                    };
                    simulation.Run();
                });

                if (TopBar.Stop) break;
                ListBoxItem timeItem = new ListBoxItem();
                timeItem.Foreground = Brushes.Green;
                timeItem.Content = TimeSpan.FromSeconds(simulation.AverageTime).ToString(@"h\:mm\:ss");

                listBoxResults.Items.Add($"Finished {Graphs[index].Title}");
                listBoxResults.Items.Add(timeItem);
                i++;
            }
        }

        private async void Task2_Click(object sender, RoutedEventArgs e)
        {
            listBoxResults.Items.Clear();
            int replications = int.Parse(TopBar.Replications);
            bool correctTime = false;
            TimeOnly time = TimeOnly.Parse(txtTime.Text);
            
            BestSim1 bestSim1 = new BestSim1(replications, time);
            TopBar.Stop = false;
            TopBar.ResetStopButton();

            while (!correctTime)
            {
                if (TopBar.Stop) break;

                bestSim1 = new BestSim1(replications, time);
                listBoxResults.Items.Add($"Starting BEST ROUTE - Z-S-R-D-Z");

                Action stopAction = () =>
                {
                    bestSim1.Stopped = true; ;
                };
                TopBar.OnStopRequest += stopAction;

                await Task.Run(() =>
                {
                    bestSim1.Run();
                });
                listBoxResults.Items.Add($"Finished BEST ROUTE - Z-S-R-D-Z");
                
                if (bestSim1.NumberOfCorrectArrivals > 0.81)
                {
                    time = time.AddMinutes(1);
                } else if (bestSim1.NumberOfCorrectArrivals > 0.8 && bestSim1.NumberOfCorrectArrivals <= 0.81)
                {
                    time = time.Add(TimeSpan.FromSeconds(1));
                } else if (bestSim1.NumberOfCorrectArrivals < 0.8)
                {
                    time = time.Add(TimeSpan.FromSeconds(-1));
                }
                if (bestSim1.NumberOfCorrectArrivals > 0.8 && bestSim1.NumberOfCorrectArrivals < 0.805)
                {
                    correctTime = true;
                    ListBoxItem timeItem = new ListBoxItem();
                    timeItem.Foreground = Brushes.Green;
                    timeItem.Content = $"Start Time: {bestSim1.StartTime.ToString(@"h\:mm\:ss")}";
                    listBoxResults.Items.Add(timeItem);
                    listBoxResults.Items.Add($"Correct Arrivals: {bestSim1.NumberOfCorrectArrivals}");
                    break;
                }
                listBoxResults.Items.Add($"Start Time: {bestSim1.StartTime.ToString(@"h\:mm\:ss")}");
                listBoxResults.Items.Add($"Correct Arrivals: {bestSim1.NumberOfCorrectArrivals}");
                listBoxResults.ScrollIntoView(listBoxResults.Items[listBoxResults.Items.Count - 1]);
            }
        }

        private async void Task2Path2_Click(object sender, RoutedEventArgs e)
        {
            int replications = int.Parse(TopBar.Replications);
            bool correctTime = false;
            TimeOnly time = TimeOnly.Parse(txtTime.Text);

            BestSim2 bestSim2 = new BestSim2(replications, time);
            TopBar.Stop = false;
            TopBar.ResetStopButton();

            while (!correctTime)
            {
                if (TopBar.Stop) break;
                bestSim2 = new BestSim2(replications, time);
                listBoxResults.Items.Add($"Starting BEST ROUTE 2 - Z-D-R-S-Z");

                Action stopAction = () =>
                {
                    bestSim2.Stopped = true; ;
                };
                TopBar.OnStopRequest += stopAction;

                await Task.Run(() =>
                {
                    bestSim2.Run();
                });
                listBoxResults.Items.Add($"Finished BEST ROUTE 2 - Z-D-R-S-Z");

                if (bestSim2.NumberOfCorrectArrivals > 0.81)
                {
                    time = time.AddMinutes(1);
                }
                else if (bestSim2.NumberOfCorrectArrivals > 0.8 && bestSim2.NumberOfCorrectArrivals <= 0.81)
                {
                    time = time.Add(TimeSpan.FromSeconds(1));
                }
                else if (bestSim2.NumberOfCorrectArrivals < 0.8)
                {
                    time = time.Add(TimeSpan.FromSeconds(-1));
                }
                if (bestSim2.NumberOfCorrectArrivals > 0.8 && bestSim2.NumberOfCorrectArrivals < 0.805)
                {
                    correctTime = true;
                    ListBoxItem timeItem = new ListBoxItem();
                    timeItem.Foreground = Brushes.Green;
                    timeItem.Content = $"Start Time: {bestSim2.StartTime.ToString(@"h\:mm\:ss")}";
                    listBoxResults.Items.Add(timeItem);
                    listBoxResults.Items.Add($"Correct Arrivals: {bestSim2.NumberOfCorrectArrivals}");
                    break;
                }
                listBoxResults.Items.Add($"Start Time: {bestSim2.StartTime.ToString(@"h\:mm\:ss")}");
                listBoxResults.Items.Add($"Correct Arrivals: {bestSim2.NumberOfCorrectArrivals}");
                listBoxResults.ScrollIntoView(listBoxResults.Items[listBoxResults.Items.Count - 1]);
            }
        }
    }
}