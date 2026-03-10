using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFComponentsLibrary.DropDowns;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace WPFComponentsLibrary
{
    /// <summary>
    /// Interaction logic for Topbar.xaml
    /// </summary>
    public partial class TopBar : UserControl
    {
        public string Replications { get; set; } = "100_000";
        public string ReplicationPoints { get; set; }
        public string ReplicationCut { get; set; } = string.Empty;
        public string TimeMultiplier { get; set; }
        public bool Stop { get; set; } = false;
        public static event Action OnStopRequest;

        private readonly Dictionary<Time, double> _timeMultipliers = new()
        {
            { Time.x025, 0.25 },
            { Time.x05, 0.5 },
            { Time.x075, 0.75 },
            { Time.x1, 1.0 },
            { Time.x2, 2.0 },
            { Time.x5, 5.0 },
            { Time.x10, 10.0 },
            { Time.x25, 25.0 },
            { Time.x50, 50.0 },
            { Time.x100, 100.0 }
        };

        public TopBar()
        {
            InitializeComponent();
            InitializeReplicationsDropdown();
            InitiliazeTimeDropdown();
        }

        private void InitiliazeTimeDropdown()
        {
            cmbTime.Items.Clear();

            foreach (Time t in Enum.GetValues(typeof(Time)))
            {
                double multiplier = _timeMultipliers[t];
                var item = new ComboBoxItem
                {
                    Content = multiplier.ToString(CultureInfo.InvariantCulture),
                    Tag = multiplier
                };
                cmbTime.Items.Add(item);
            }

            cmbTime.SelectionChanged += CmbTime_SelectionChanged;

            int defaultIndex = Array.IndexOf(Enum.GetValues(typeof(Time)), Time.x1);
            if (defaultIndex >= 0 && defaultIndex < cmbTime.Items.Count)
            {
                cmbTime.SelectedIndex = defaultIndex;
            }
        }

        private void InitializeReplicationsDropdown()
        {
            cmbReplications.Items.Clear();

            foreach (Replications r in Enum.GetValues(typeof(Replications)))
            {
                int value = (int)r;
                var item = new ComboBoxItem
                {
                    Content = value.ToString("N0", CultureInfo.InvariantCulture),
                    Tag = value
                };
                cmbReplications.Items.Add(item);
            }

            cmbReplications.SelectionChanged += CmbReplications_SelectionChanged;

            int defaultIndex = Array.IndexOf(Enum.GetValues(typeof(Replications)), DropDowns.Replications.Thousand);
            if (defaultIndex >= 0 && defaultIndex < cmbReplications.Items.Count)
            {
                cmbReplications.SelectedIndex = defaultIndex;
            }
        }

        private void CmbReplications_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbReplications.SelectedItem is ComboBoxItem item && item.Tag != null)
            {
                txtReplicationsCount.Text = item.Tag.ToString();
                Replications = txtReplicationsCount.Text;
            }
        }

        private void CmbTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTime.SelectedItem is ComboBoxItem item && item.Tag != null)
            {
                cmbTime.ToolTip = $"Multiplier: {item.Tag.ToString()}";
            }
        }

        private void ReplicationsPercent_TextChanged(object sender, TextChangedEventArgs e)
        {
            ReplicationCut = txtReplicationsPercent.Text;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Stop = true;
            //btnStop.IsEnabled = false;
            OnStopRequest?.Invoke();
        }

        public void ResetStopButton()
        {
            btnStop.IsEnabled = true;
        }
    }
}
