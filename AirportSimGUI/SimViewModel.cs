using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GUI
{
    // ── Per-lane live state ────────────────────────────────────────────────────
    public class LaneViewModel : INotifyPropertyChanged
    {
        // Queue lengths
        private int _arrivalQueueCount;
        private int _metalDetectorQueueCount;
        private int _luggagePickupQueueCount;
        private int _inputBeltCount;
        private int _outputBeltCount;
        private int _inputBeltMax;
        private int _outputBeltMax;
        private bool _metalDetectorBusy;
        private bool _rontgenBusy;
        private bool _employeeBusy;
        private string _metalDetectorStatus = "IDLE";
        private string _rontgenStatus = "IDLE";
        private string _personalCheckStatus = "IDLE";

        public int ArrivalQueueCount
        {
            get => _arrivalQueueCount;
            set { _arrivalQueueCount = value; OnPropertyChanged(); }
        }
        public int MetalDetectorQueueCount
        {
            get => _metalDetectorQueueCount;
            set { _metalDetectorQueueCount = value; OnPropertyChanged(); }
        }
        public int LuggagePickupQueueCount
        {
            get => _luggagePickupQueueCount;
            set { _luggagePickupQueueCount = value; OnPropertyChanged(); }
        }
        public int InputBeltCount
        {
            get => _inputBeltCount;
            set { _inputBeltCount = value; OnPropertyChanged(); OnPropertyChanged(nameof(InputBeltLabel)); }
        }
        public int OutputBeltCount
        {
            get => _outputBeltCount;
            set { _outputBeltCount = value; OnPropertyChanged(); OnPropertyChanged(nameof(OutputBeltLabel)); }
        }
        public int InputBeltMax
        {
            get => _inputBeltMax;
            set { _inputBeltMax = value; OnPropertyChanged(); OnPropertyChanged(nameof(InputBeltLabel)); }
        }
        public int OutputBeltMax
        {
            get => _outputBeltMax;
            set { _outputBeltMax = value; OnPropertyChanged(); OnPropertyChanged(nameof(OutputBeltLabel)); }
        }

        public string InputBeltLabel  => $"{InputBeltCount} / {InputBeltMax}";
        public string OutputBeltLabel => $"{OutputBeltCount} / {OutputBeltMax}";

        public bool MetalDetectorBusy
        {
            get => _metalDetectorBusy;
            set { _metalDetectorBusy = value; OnPropertyChanged(); MetalDetectorStatus = value ? "SCANNING" : "IDLE"; }
        }
        public bool RontgenBusy
        {
            get => _rontgenBusy;
            set { _rontgenBusy = value; OnPropertyChanged(); RontgenStatus = value ? "SCANNING" : "IDLE"; }
        }
        public bool EmployeeBusy
        {
            get => _employeeBusy;
            set { _employeeBusy = value; OnPropertyChanged(); PersonalCheckStatus = value ? "IN PROGRESS" : "IDLE"; }
        }

        public string MetalDetectorStatus
        {
            get => _metalDetectorStatus;
            set { _metalDetectorStatus = value; OnPropertyChanged(); }
        }
        public string RontgenStatus
        {
            get => _rontgenStatus;
            set { _rontgenStatus = value; OnPropertyChanged(); }
        }
        public string PersonalCheckStatus
        {
            get => _personalCheckStatus;
            set { _personalCheckStatus = value; OnPropertyChanged(); }
        }

        // DataGrids
        public ObservableCollection<CustomerRow> Customers { get; } = new();
        public ObservableCollection<LuggageRow> InputBeltItems { get; } = new();
        public ObservableCollection<LuggageRow> OutputBeltItems { get; } = new();
        public ObservableCollection<string> EventLog { get; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }

    // ── DataGrid row models ────────────────────────────────────────────────────
    public class CustomerRow
    {
        public int    ID          { get; set; }
        public int    LuggageCount{ get; set; }
        public string State       { get; set; } = "";
        public string TimeInSystem{ get; set; } = "";
    }

    public class LuggageRow
    {
        public int    ID       { get; set; }
        public int    OwnerID  { get; set; }
        public string Location { get; set; } = "";
    }

    // ── Global simulation stats ────────────────────────────────────────────────
    public class GlobalStatsViewModel : INotifyPropertyChanged
    {
        private int    _replication;
        private double _currentTime;
        private int    _totalServed;
        private double _avgTimeInSystem;
        private double _ciLow;
        private double _ciHigh;
        private string _simStatus = "STOPPED";

        public int    Replication     { get => _replication;    set { _replication    = value; OnPropertyChanged(); } }
        public double CurrentTime     { get => _currentTime;    set { _currentTime    = value; OnPropertyChanged(); OnPropertyChanged(nameof(CurrentTimeFormatted)); } }
        public int    TotalServed     { get => _totalServed;    set { _totalServed    = value; OnPropertyChanged(); } }
        public double AvgTimeInSystem { get => _avgTimeInSystem;set { _avgTimeInSystem= value; OnPropertyChanged(); OnPropertyChanged(nameof(AvgTimeFormatted)); } }
        public double CILow           { get => _ciLow;          set { _ciLow          = value; OnPropertyChanged(); OnPropertyChanged(nameof(CIFormatted)); } }
        public double CIHigh          { get => _ciHigh;         set { _ciHigh         = value; OnPropertyChanged(); OnPropertyChanged(nameof(CIFormatted)); } }
        public string SimStatus       { get => _simStatus;      set { _simStatus      = value; OnPropertyChanged(); } }

        public string CurrentTimeFormatted => TimeSpan.FromSeconds(CurrentTime).ToString(@"hh\:mm\:ss");
        public string AvgTimeFormatted     => $"{AvgTimeInSystem:F2} s";
        public string CIFormatted          => $"[{CILow:F2} ; {CIHigh:F2}]";

        // Graph data
        public ObservableCollection<ReplicationPoint> ReplicationHistory { get; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }

    public class ReplicationPoint
    {
        public int    RepNumber    { get; set; }
        public double AvgTime      { get; set; }
        public int    TotalServed  { get; set; }
    }
}
