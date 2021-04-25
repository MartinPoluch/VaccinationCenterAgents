using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GUI.ChartControl;
using LiveCharts;
using LiveCharts.Configurations;

namespace GUI {
	/// <summary>
	/// Interaction logic for MyChart.xaml
	/// </summary>
	public partial class MyChart : UserControl {
		private double _axisMax;
		private double _axisMin;

		public MyChart() {
			InitializeComponent();
			//To handle live data easily, in this case we built a specialized type
			//the MeasureModel class, it only contains 2 properties
			//We need to configure LiveCharts to handle MeasureModel class
			//The next code configures MeasureModel  globally, this means
			//that LiveCharts learns to plot MeasureModel and will use this config every time
			//a IChartValues instance uses this type.
			var mapper = Mappers.Xy<MeasureModel>()
				.X(model => model.Time)
				.Y(model => model.StatValue);

			//lets save the mapper globally.
			Charting.For<MeasureModel>(mapper);

			//the values property will store our values array
			ChartValues = new ChartValues<MeasureModel>();

			SetAxisLimits(1);
			DataContext = this;
		}

		public string Color { get; set; }

		public string NameOfX { get; set; }

		public string NameOfY { get; set; }

		public ChartValues<MeasureModel> ChartValues { get; set; }

		public void AddChartValue(double time, double statValue) {
			MeasureModel model = new MeasureModel {
				Time = time,
				StatValue = statValue
			};
			ChartValues.Add(model);
			SetAxisLimits(time);
		}


		public void Clear() {
			ChartValues.Clear();
			SetAxisLimits(1);
		}

		public double AxisMax {
			get { return _axisMax; }
			set {
				_axisMax = value;
				OnPropertyChanged("AxisMax");
			}
		}

		public double AxisMin {
			get { return _axisMin; }
			set {
				_axisMin = value;
				OnPropertyChanged("AxisMin");
			}
		}

		private void SetAxisLimits(double replication) {
			AxisMax = replication;
			AxisMin = 0;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName = null) {
			if (PropertyChanged != null) {
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
