using AirportSimGUI;
using Core.EventSimulation.Airport;
using Core.EventSimulation.Airport.Models;
using Core.EventSimulation.Airport.Models.DTO;
using Core.EventSimulation.Airport.Observer;
using Core.EventSimulation.Models;
using Core.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPFComponentsLibrary;

namespace GUI
{
    public partial class MainWindow : Window, SimDelegate
    {
        private const string SIMFINISHED = "FINISHED";
        private const string SIMRUNNING = "RUNNING";
        private const string SIMPAUSED = "PAUSED";

        private bool _turboMode = true;
        private int _maxInputBelt = 4;
        private int _maxOutputBelt = 5;
        private int _replications = 1;
        private int _sleepInterval = 200;
        private int _sleepMs = 250;
        private bool _needReset = false;
        private TimeOnly _time;
        private GraphWindow _graphWindow;

        private AirportSimulation _sim;
        public ObservableCollection<CustomerDTO> ArrivalCustomers { get; set; } = new();
        public ObservableCollection<CustomerDTO> ArrivalCustomers2 { get; set; } = new();

        public ObservableCollection<LuggageDTO> Luggage { get; set; } = new();
        public ObservableCollection<LuggageDTO> Luggage2 { get; set; } = new();

        public ObservableCollection<LuggageDTO> InputBelt { get; set; } = new();
        public ObservableCollection<LuggageDTO> InputBelt2 { get; set; } = new();

        public ObservableCollection<LuggageDTO> OutputBelt { get; set; } = new();
        public ObservableCollection<LuggageDTO> OutputBelt2 { get; set; } = new();

        public ObservableCollection<CustomerDTO> PickUp { get; set; } = new();
        public ObservableCollection<CustomerDTO> PickUp2 { get; set; } = new();

        public ObservableCollection<string> Logs { get; set; } = new();
        public ObservableCollection<string> Logs2 { get; set; } = new();
        public ObservableCollection<string> Events { get; set; } = new();
        public ObservableCollection<string> Events2 { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            AssignCollections();
        }

        private void AssignCollections()
        {
            Line1CustomerGrid.ItemsSource = ArrivalCustomers;
            Line2CustomerGrid.ItemsSource = ArrivalCustomers2;

            Line1InputBeltGrid.ItemsSource = Luggage;
            Line2InputBeltGrid.ItemsSource = Luggage2;

            Line1OutputBeltGrid.ItemsSource = OutputBelt;
            Line2OutputBeltGrid.ItemsSource = OutputBelt2;

            Line1LuggagePickUpGrid.ItemsSource = PickUp;
            Line2LuggagePickUpGrid.ItemsSource = PickUp2;

            Line1ListBox.ItemsSource = Logs;
            Line2ListBox.ItemsSource = Logs2;

            Line1ListBoxEvents.ItemsSource = Events;
            Line2ListBoxEvents.ItemsSource = Events2;
        }

        

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (_needReset)
            {
                ResetSim();
            }

            _turboMode = chckBoxTurboMode.IsChecked ?? true;
            _maxInputBelt = int.TryParse(txtInputBeltMax.Text, out int maxInput) ? maxInput : 4;
            _maxOutputBelt = int.TryParse(txtOutputBeltMax.Text, out int maxOutput) ? maxOutput : 5;
            _replications = int.TryParse(topBar.Replications, out int replications) ? replications : 1;
            _sleepInterval = int.TryParse(txtSleepInterval.Text, out int sleep) ? sleep : 200;
            _sleepMs = int.TryParse(txtSleepInterval.Text, out int sleepMs) ? sleepMs : 250;

            _sim = new AirportSimulation(_replications, _maxInputBelt, _maxOutputBelt, _sleepMs, _sleepInterval);
            _sim.AddDelegates(this);
            _sim.TurboMode = _turboMode;
            _sim.IsExperiment = chckBoxExperiment.IsChecked ?? false;
            _sim.StartingCustomersNumber = double.TryParse(txtCapacity.Text, out double value) ? value : 5000;

            Action stopAction = () => { _sim.Stopped = true; };
            TopBar.OnStopRequest += stopAction;

            Action pauseAction = () => { _sim.Paused = true; };
            TopBar.OnPauseRequest += pauseAction;

            Action resumeAction = () => { _sim.Paused = false; };
            TopBar.OnResumeRequest += resumeAction;

            Action speedChangeAction = () => { _sim.TimeMultiplier = topBar.TimeMultiplierValue; };
            TopBar.OnSpeedChangeRequest += speedChangeAction;
            
            if (_sim.IsExperiment)
            {
                Task.Run(() =>
                {
                    while (_sim.IsExperiment)
                    {
                        if (_sim.Stopped) 
                        {
                            return;
                        }
                        _sim.Run();
                    }
                });
            } else
            {
                Task.Run(() => { _sim.Run(); });
            }
            _needReset = true;
        }

        private void ResetSim()
        {
            IDGenerator.ResetIDs();
            ArrivalCustomers.Clear();
            ArrivalCustomers2.Clear();
            Luggage.Clear();
            Luggage2.Clear();
            InputBelt.Clear();
            InputBelt2.Clear();
            OutputBelt.Clear();
            OutputBelt2.Clear();
            Logs.Clear();
            Logs2.Clear();
        }

        private void ShowGraph_Click(object sender, RoutedEventArgs e)
        {
            _graphWindow = new GraphWindow();
            _graphWindow.Show();
        }
        private async void StartGraph_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtCapacity.Text, out int targetCapacity)) return;
            if (!int.TryParse(topBar.ReplicationPoints, out int numPoints) || numPoints < 2) return;
            if (!double.TryParse(txtPercentGraph.Text, out double percent)) return;
            if (!int.TryParse(topBar.Replications, out int replications)) return;

            _maxInputBelt = int.TryParse(txtInputBeltMax.Text, out int maxInput) ? maxInput : 4;
            _maxOutputBelt = int.TryParse(txtOutputBeltMax.Text, out int maxOutput) ? maxOutput : 5;

            double factor = percent / 100.0;
            double minCapacity = targetCapacity * (1.0 - factor);
            double maxCapacity = targetCapacity * (1.0 + factor);
            double step = (maxCapacity - minCapacity) / (numPoints - 1);

            if (_graphWindow == null)
            {
                _graphWindow = new GraphWindow();
            }
            _graphWindow.Show();

            btnShowGraph.IsEnabled = false;
            btnStart.IsEnabled = false;

            txtSimStatus.Text = SIMRUNNING;
            await Task.Run(async () =>
            {

                for (int i = 0; i < numPoints; i++)
                {
                    double capacity = minCapacity + i * step;
                    
                    double arrivalRate = capacity / 86400.0;

                    var sim = new AirportSimulation(
                        replications, _maxInputBelt, _maxOutputBelt,
                        sleepMs: 0, sleepInterval: 0
                    );
                    sim.TurboMode = true;
                    sim.ArrivalRate = arrivalRate;
                    sim.IsGraphExperiment = true;

                    await Task.Run(() => sim.Run());

                    double avgTime = sim.TimeInSystemCustomersGlobal.Mean;
                    _graphWindow.AddPoint(capacity, avgTime);
                }
            });

            txtSimStatus.Text = SIMFINISHED;

            btnShowGraph.IsEnabled = true;
            btnStart.IsEnabled = true;
        }

        public void Refresh(AirportSimulation sim)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                txtActualReplication.Text = sim.ActualReplication.ToString();
                txtSimStatus.Text = sim.Stopped ? SIMFINISHED : sim.Paused ? SIMPAUSED : SIMRUNNING;
                if (sim.CurrentTime > sim.WarmUpLength)
                {
                    if (sim.TurboMode)
                    {
                        txtAllPeople.Text = sim.AverageCustomersStatGlobal.Mean.ToString();
                        //txtAllLuggage.Text = sim.AllLuggageNumber.ToString();

                        double arrivalQueue = sim.ArrivalQueueStatGlobal.Mean;
                        (double, double) arrivalQueueCI = sim.ArrivalQueueStatGlobal.ConfidenceInterval90();
                        txtAverageArrivalLength.Text = string.Format($"{arrivalQueue:F2} <{arrivalQueueCI.Item1:F2}, {arrivalQueueCI.Item2:F2}>");

                        double timeInSystemCustomers = sim.TimeInSystemCustomersGlobal.Mean;
                        (double, double) timeInSystemCustomersCI = sim.TimeInSystemCustomersGlobal.ConfidenceInterval90();
                        txtAverageTimePeople.Text = string.Format($"{timeInSystemCustomers:F2} <{timeInSystemCustomersCI.Item1:F2}, {timeInSystemCustomersCI.Item2:F2}>");

                        double inputBeltQueue = sim.InputBeltStatGlobal.Mean;
                        (double, double) inputBeltQueueCI = sim.InputBeltStatGlobal.ConfidenceInterval90();
                        txtAverageInputBeltLength.Text = string.Format($"{inputBeltQueue:F2} <{inputBeltQueueCI.Item1:F2}, {inputBeltQueueCI.Item2:F2}>");

                        double metalDetectorQueue = sim.MetalDetectorQueueStatGlobal.Mean;
                        (double, double) metalDetectorQueueCI = sim.MetalDetectorQueueStatGlobal.ConfidenceInterval90();
                        txtAverageMetalDetectorLength.Text = string.Format($"{metalDetectorQueue:F2} <{metalDetectorQueueCI.Item1:F2}, {metalDetectorQueueCI.Item2:F2}>");

                        double outputBeltQueue = sim.OutputBeltStatGlobal.Mean;
                        (double, double) outputBeltQueueCI = sim.OutputBeltStatGlobal.ConfidenceInterval90();
                        txtAverageOutputBeltLength.Text = string.Format($"{outputBeltQueue:F2} <{outputBeltQueueCI.Item1:F2}, {outputBeltQueueCI.Item2:F2}>");

                        double pickUpQueue = sim.LuggagePickUpQueueStatGlobal.Mean;
                        (double, double) pickUpQueueCI = sim.LuggagePickUpQueueStatGlobal.ConfidenceInterval90();
                        txtAveragePickUpLength.Text = string.Format($"{pickUpQueue:F2} <{pickUpQueueCI.Item1:F2}, {pickUpQueueCI.Item2:F2}>");
                    }
                    else
                    {

                        if (sim.CurrentTime < 86_400)
                        {
                            _time = TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(sim.CurrentTime));
                            txtTime.Text = _time.ToString("HH:mm:ss");
                        } else
                        {
                            _time = TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(sim.CurrentTime % 86_400));
                            txtTime.Text = _time.ToString("HH:mm:ss");
                        }

                            Line1RontgenMachine.Text = sim.RontgenMachine1.IsBusy
                            ? $"Scanning bag #{sim.RontgenMachine1.ActualLuggage?.ID}"
                            : "IDLE";

                        Line1MetalDetector.Text = sim.MetalDetector1.IsBusy
                            ? $"Scanning customer #{sim.MetalDetector1.ActualCustomer?.ID}"
                            : "IDLE";

                        Line1PersonalCheck.Text = sim.Employee1.IsBusy
                            ? $"Checking customer #{sim.MetalDetector1.ActualCustomer?.ID}"
                            : "IDLE";

                        Line2RontgenMachine.Text = sim.RontgenMachine2.IsBusy
                            ? $"Scanning bag #{sim.RontgenMachine2.ActualLuggage?.ID}"
                            : "IDLE";

                        Line2MetalDetector.Text = sim.MetalDetector2.IsBusy
                            ? $"Scanning customer #{sim.MetalDetector2.ActualCustomer?.ID}"
                            : "IDLE";

                        Line2PersonalCheck.Text = sim.Employee2.IsBusy
                            ? $"Checking customer #{sim.MetalDetector2.ActualCustomer?.ID}"
                            : "IDLE";

                        ArrivalCustomers.Clear();
                        List<Customer> arr1 = sim.ArrivalQueue1.GetAll();
                        
                        arr1.ForEach(c => ArrivalCustomers.Add(CustomerDTO.FromCustomer(c)));

                        if (Line1CustomerGrid.Items.Count > 3)
                        {
                            Line1CustomerGrid.ScrollIntoView(ArrivalCustomers.LastOrDefault());
                        }

                        ArrivalCustomers2.Clear();
                        List<Customer> arr2 = sim.ArrivalQueue2.GetAll();
                        arr2.ForEach(c => ArrivalCustomers2.Add(CustomerDTO.FromCustomer(c)));

                        if (Line2CustomerGrid.Items.Count > 3)
                        {
                            Line2CustomerGrid.ScrollIntoView(ArrivalCustomers2.LastOrDefault());
                        }
                        InputBelt.Clear();
                        List<Luggage> lugg1 = sim.InputBelt1.GetAllLuggage();
                        lugg1.ForEach(l => InputBelt.Add(LuggageDTO.FromLuggage(l)));

                        if (Line1InputBeltGrid.Items.Count > 2)
                        {
                            Line1InputBeltGrid.ScrollIntoView(InputBelt.LastOrDefault());
                        }

                        InputBelt2.Clear();
                        List<Luggage> lugg2 = sim.InputBelt2.GetAllLuggage();
                        lugg2.ForEach(l => InputBelt2.Add(LuggageDTO.FromLuggage(l)));

                        PickUp.Clear();
                        List<Customer> pick1 = sim.LuggagePickUpQueue1.GetAll();
                        pick1.ForEach(c => PickUp.Add(CustomerDTO.FromCustomer(c)));

                        PickUp2.Clear();
                        List<Customer> pick2 = sim.LuggagePickUpQueue2.GetAll();
                        pick2.ForEach(c => PickUp2.Add(CustomerDTO.FromCustomer(c)));

                        if (Line2InputBeltGrid.Items.Count > 2)
                        {
                            Line2InputBeltGrid.ScrollIntoView(InputBelt2.LastOrDefault());
                        }

                        while (sim.Logs1.TryDequeue(out string? log))
                        {
                            if (!string.IsNullOrEmpty(log))
                            {
                                Logs.Add(log);
                            }
                        }

                        if (Line1ListBox.Items.Count > 5)
                        {
                            Line1ListBox.ScrollIntoView(Logs.LastOrDefault());
                        }

                        while (sim.Logs2.TryDequeue(out string? log))
                        {
                            if (!string.IsNullOrEmpty(log))
                            {
                                Logs2.Add(log);
                            }
                        }

                        if (Line2ListBox.Items.Count > 5)
                        {
                            Line2ListBox.ScrollIntoView(Logs2.LastOrDefault());
                        }

                        while (sim.Events1.TryDequeue(out string? log))
                        {
                            if (!string.IsNullOrEmpty(log))
                            {
                                Events.Add(log);
                            }
                        }

                        if (Line1ListBoxEvents.Items.Count > 5)
                        {
                            Line1ListBoxEvents.ScrollIntoView(Events.LastOrDefault());
                        }

                        while (sim.Events2.TryDequeue(out string? log))
                        {
                            if (!string.IsNullOrEmpty(log))
                            {
                                Events2.Add(log);
                            }
                        }

                        if (Line2ListBoxEvents.Items.Count > 5)
                        {
                            Line2ListBoxEvents.ScrollIntoView(Events2.LastOrDefault());
                        }

                        OutputBelt.Clear();
                        List<Luggage> out1 = _sim.OutputBelt1.GetAllLuggage();
                        out1.ForEach(l => OutputBelt.Add(LuggageDTO.FromLuggage(l)));

                        OutputBelt2.Clear();
                        List<Luggage> out2 = _sim.OutputBelt2.GetAllLuggage();
                        out2.ForEach(l => OutputBelt2.Add(LuggageDTO.FromLuggage(l)));

                        string arrivalQueue = (sim.ArrivalQueue1.AverageQueueLength + sim.ArrivalQueue2.AverageQueueLength).ToString();
                        //string arrivalQueueCI = (sim.ArrivalQueue1.)
                        txtAverageArrivalLength.Text = arrivalQueue.ToString();

                        double timeInSystemCustomers = sim.TimeInSystemCustomers.Mean;
                        (double, double) timeInSystemCustomersCI = sim.TimeInSystemCustomers.ConfidenceInterval90();
                        txtAverageTimePeople.Text = string.Format($"{timeInSystemCustomers:F2} <{timeInSystemCustomersCI.Item1:F2}, {timeInSystemCustomersCI.Item2:F2}>");

                        txtAllPeople.Text = sim.AllCustomersNumber.ToString();

                        //txtAllLuggage.Text = sim.AllLuggageNumber.ToString();

                        txtAverageInputBeltLength.Text = (sim.InputBelt1.Luggage.AverageQueueLength + sim.InputBelt2.Luggage.AverageQueueLength).ToString();

                        txtAverageMetalDetectorLength.Text = (sim.MetalDetectorQueue1.AverageQueueLength + sim.MetalDetectorQueue2.AverageQueueLength).ToString();

                        txtAverageOutputBeltLength.Text = (sim.OutputBelt1.Luggage.AverageQueueLength + sim.OutputBelt2.Luggage.AverageQueueLength).ToString();

                        txtAveragePickUpLength.Text = (sim.LuggagePickUpQueue1.AverageQueueLength + sim.LuggagePickUpQueue2.AverageQueueLength).ToString();
                    }
                }
            });
        }


    }
}
