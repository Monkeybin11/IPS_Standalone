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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;

namespace ThicknessAndComposition_Inspector_IPS
{
	/// <summary>
	/// Interaction logic for UC_LiveLineChart.xaml
	/// </summary>
	public partial class UC_LiveLineChart : UserControl, INotifyPropertyChanged
	{
		//public ChartValues<double[]> ChartDatas { get; set; }

		//public string Title { set { srsMain.Title = value; } }


		public UC_LiveLineChart()
		{
			InitializeComponent();

			var mapper = Mappers.Xy<double[]>()
				.X(model => model[0])            //use DateTime.Ticks as X
                .Y(model => model[1]);           //use the value property as Y

			Charting.For<double [ ]>( mapper );
			DataContext = this;
		}

		public void UpdateSeries( IEnumerable<double> datas , IEnumerable<double> labels )
		{
			//ChartDatas.Clear();
			var dts = datas.ToArray();
			var lbls = labels.ToArray();
			dts [ 0 ] = dts [ 2 ];
			dts [ 1 ] = dts [ 2 ];

			ChartValues<double[]> chartDatas = new ChartValues<double[]>();
			chartDatas.AddRange(
				Enumerable.Range( 0 , datas.Count() )
				.Where( ( _ , i ) => i % 10 == 0 )
				.Select( x => new double [ 2 ] { lbls [ x ] , dts [ x ] } )
				);
			srsMain.Dispatcher.BeginInvoke( ( Action )( () => srsMain.Values = chartDatas ) );
		}
		public event PropertyChangedEventHandler PropertyChanged;
		public void OnPropertyChanged( string propertyName = null )
		{
			if ( PropertyChanged != null )
				PropertyChanged.Invoke( this , new PropertyChangedEventArgs( propertyName ) );
		}
	}
}
