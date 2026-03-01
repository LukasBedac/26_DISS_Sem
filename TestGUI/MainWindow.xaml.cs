using OxyPlot;
using OxyPlot.Series;
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

namespace TestGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<DataPoint>? Data { get; set; }
        public PlotModel? PlotModel { get; set; }
        public LineSeries LineSeries { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Data = new List<DataPoint>();
            InitGraph();
        }

        private void InitGraph()
        {
            PlotModel = new PlotModel();
            PlotModel.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Title = "Replications",
                MajorGridlineStyle = OxyPlot.LineStyle.Solid,
                MinorGridlineStyle = OxyPlot.LineStyle.Dot,
                StringFormat = "N0"
            });
            PlotModel.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                Title = "Value",
                MajorGridlineStyle = OxyPlot.LineStyle.Solid,
                MinorGridlineStyle = OxyPlot.LineStyle.Dot,
                StringFormat = "N0"
            });

            LineSeries = new LineSeries();
            PlotModel.Series.Add(LineSeries);
        }

        private void Uniform_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Empiric_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Exponential_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Triangular_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}