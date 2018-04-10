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
using SpeedyCoding;
using static System.Linq.Enumerable;

namespace ThicknessAndComposition_Inspector_IPS
{
	/// <summary>
	/// Interaction logic for UC_LiveLineChartMulti.xaml
	/// </summary>
	public partial class UC_LiveLineChartMulti : UserControl , INotifyPropertyChanged
	{
		SeriesCollection SeriesColl = new SeriesCollection();
		Brush[] ColorList;
		//List<Brush> SeriesColors = new List<Brush>();

		public UC_LiveLineChartMulti()
		{
			InitializeComponent();
			ColorList = typeof( Brushes ).GetProperties()
						.Select( x  => x.GetValue( null ) as Brush )
						.ToArray();


			var mapper = Mappers.Xy<double[]>()
				.X(model => model[0])            //use DateTime.Ticks as X
                .Y(model => model[1]);           //use the value property as Y

			Charting.For<double [ ]>( mapper );
			DataContext = this;
		}

		public void AddNewSeries( IEnumerable<double> datas , IEnumerable<double> labels )
		{
			chtLiveLine.LegendLocation = LegendLocation.None;
			//ChartDatas.Clear();
			var dts = datas.ToArray();
			var lbls = labels.ToArray();
			dts [ 0 ] = dts [ 2 ];
			dts [ 1 ] = dts [ 2 ];

			ChartValues<double[]> chartDatas = new ChartValues<double[]>();
			chartDatas.AddRange(
				Range( 0 , datas.Count() )
				.Where( ( _ , i ) => i % 10 == 0 )
				.Select( x => new double [ 2 ] { lbls [ x ] , dts [ x ] } )
				);

			this.Dispatcher.BeginInvoke( ( Action )( () =>
			{
				SeriesColl.Add( CreateSeries( chartDatas ) );
				foreach ( var item in SeriesColl )
				{
					SeriesColl.Add( item );
				}
				this.Dispatcher.BeginInvoke( ( Action )( () => chtLiveLine.Series = SeriesColl ) );

			} ) );
		}

			public void AddNewSeries( IEnumerable<double> datas , IEnumerable<double> labels , string title)
		{
			ChartValues<double[]> chartDatas = new ChartValues<double[]>();
			this.Dispatcher.BeginInvoke((Action)(()=> chtLiveLine.LegendLocation = LegendLocation.Right ));
			var dts = datas.ToArray();
			var lbls = labels.ToArray();

			//dts [ 0 ] = dts [ 2 ];
			//dts [ 1 ] = dts [ 2 ];
			//
			//ChartValues<double[]> chartDatas = new ChartValues<double[]>();
			//chartDatas.AddRange(
			//	Enumerable.Range( 0 , datas.Count() )
			//	.Where( ( _ , i ) => i % 10 == 0 )
			//	.Select( x => new double [ 2 ] { lbls [ x ] , dts [ x ] } )
			//	);

			var fixeddata = ClearNoise( datas.ToArray() );
			chartDatas.AddRange(
								  Range(0, fixeddata.Length)
								  .Where( i => i % 10 == 0 )
								  .Select( i  => new double [ 2 ] { lbls [ i ] , fixeddata[i] } ) );

			this.Dispatcher.BeginInvoke( ( Action )( () => {
				SeriesColl.Add( CreateSeries( chartDatas ) );
				this.Dispatcher.BeginInvoke( ( Action )( () => chtLiveLine.Series = SeriesColl ) );
			} ) );
		}

		public void BatchDrawSeries( IEnumerable<double [ ]> datasList , IEnumerable<double> labels )
		{
			chtLiveLine.LegendLocation = LegendLocation.None;
			ChartValues<double[]> chartDatas = new ChartValues<double[]>();
			var lbls = labels.ToArray();
			//ChartDatas.Clear();
			var fixeddatasList2 = datasList.ToList();

			var fixeddatasList = datasList.Select( ClearNoise )
					 .Select( singledata => Range(0,singledata.Length)
								.Where( i => i % 10 == 0 )
								.Select( i  => new double [ 2 ] { lbls [ i ] , singledata [ i ] } ) ).ToList();

			this.Dispatcher.BeginInvoke( ( Action )( () =>
			{
				var src = new SeriesCollection();

				fixeddatasList.ActLoop( x => SeriesColl.Add( 
												CreateSeries( 
													new ChartValues<double [ ]>(x) ) ) );

				foreach ( var item in SeriesColl )
				{
					SeriesColl.Add( item );
				}
				this.Dispatcher.BeginInvoke( ( Action )( () => chtLiveLine.Series = SeriesColl ) );
			} ) );
		}

		public void ClearSeries()
		{
			SeriesColl = new SeriesCollection();
			this.Dispatcher.BeginInvoke( ( Action )( () => chtLiveLine.Series = SeriesColl ) );
		}

		#region sub Func
		public int Counter = 0;
		LineSeries CreateSeries( ChartValues<double [ ]> chartDatas ,int indexer )
		{
			var newseries = new LineSeries();
			newseries.Values = chartDatas;
			newseries.DataLabels = false;
			newseries.PointGeometrySize = 0;
			newseries.Fill = Brushes.Transparent;
			newseries.Stroke = ColorList [ indexer ];
			newseries.Title = Counter.ToString();
			return newseries;
		}

		LineSeries CreateSeries( ChartValues<double [ ]> chartDatas  )
		{
			var newseries = new LineSeries();
			newseries.Values = chartDatas;
			newseries.DataLabels = false;
			newseries.PointGeometrySize = 0;
			newseries.Fill = Brushes.Transparent;
			newseries.Title = Counter.ToString();
			return newseries;
		}


		double [ ] ClearNoise( double [ ] self )
		{
			self [ 0 ] = self [ 2 ];
			self [ 1 ] = self[ 2 ];
			return self;
		} 




		#endregion


		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged( string propertyName = null )
		{
			if ( PropertyChanged != null )
				PropertyChanged.Invoke( this , new PropertyChangedEventArgs( propertyName ) );
		}

	}
}
