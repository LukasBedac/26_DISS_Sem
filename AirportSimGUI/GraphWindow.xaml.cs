using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AirportSimGUI
{
    public partial class GraphWindow : Window
    {
        public PlotModel PlotModel { get; set; }
        private LineSeries _series;
        public GraphWindow()
        {
            InitializeComponent();
            DataContext = this;
            PlotModel = BuildModel();
        }

        private PlotModel BuildModel()
        {
            var model = new PlotModel { Title = "Airpot Simulation - Capacity Analysis" };

            model.Annotations.Add(new LineAnnotation
            {
                Type = LineAnnotationType.Horizontal,
                Y = 600,
                Color = OxyColors.Red,
                LineStyle = LineStyle.Dash,
                StrokeThickness = 2,
                Text = "Threshold (600s)",
                TextColor = OxyColors.Red
            });

            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Capacity"
            });
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Average Time in System (s)"          
            });

           _series = new LineSeries 
           { 
               Title = "Average Time in System" ,
               MarkerSize = 7,
               MarkerType = MarkerType.Circle,
               Color = OxyColors.Azure
           };
            

            model.Series.Add(_series);
            return model;
        }

        public void AddPoint(double capacity, double avgTime)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _series.Points.Add(new DataPoint(capacity, avgTime));
                var sorted = _series.Points.OrderBy(p => p.X).ToList();
                _series.Points.Clear();
                sorted.ForEach(p => _series.Points.Add(p));
                PlotModel.InvalidatePlot(true);
            });
        }
    }
}
