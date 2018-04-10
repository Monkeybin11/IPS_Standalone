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
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using SpeedyCoding;
using static System.Linq.Enumerable;
using System.ComponentModel;
using IPSAnalysis;
using static IPSAnalysis.AnalysisFunc;

namespace ThicknessAndComposition_Inspector_IPS
{
	/// <summary>
	/// Interaction logic for UC_AnalysisChart.xaml
	/// </summary>
	public partial class UC_AnalysisChart : UserControl , INotifyPropertyChanged
	{
		SeriesCollection SeriesColl;
		Brush[] ColorList;

		
		Func<AnalysisState,IEnumerable<double[]>> Extractor;

		// This datas is initilized only first time to load state. 
		double [][] LibDatas;
		double[] Labels;
		public void SetExtractor ( Func<AnalysisState , IEnumerable<double [ ]>> extractor )
			=> Extractor = extractor;

		public UC_AnalysisChart( )
		{
			InitializeComponent();
			
			ColorList = typeof( Brushes ).GetProperties()
						.Select( x => x.GetValue( null ) as Brush )
						.ToArray();


			var mapper = Mappers.Xy<double[]>()
				.X(model => model[0])            //use DateTime.Ticks as X
                .Y(model => model[1]);           //use the value property as Y

			Charting.For<double [ ]>( mapper );
			DataContext = this;

			axisX.Title = "WaveLength";

			axisX.MaxValue = 1200;
			axisX.MinValue = 200;
		



			ClearSeries();

		}

		public void UpdateSeries( AnalysisState state )
		{
			if ( LibDatas == null ) LibDatas = Extractor( state ).ToArray();
			if ( Labels == null ) Labels = ExtractLabel( state ).ToArray();
			if ( state.WaveMinMax != null )
			{
				axisX.MinValue = state.WaveMinMax [ 0 ];
				axisX.MaxValue = state.WaveMinMax [ 1 ];
			}


			var target = state.TargetIdx;

			if (state.IsAdd)
			{
				var chartDatas = new ChartValues<double[]>();
				var fixeddata = ClearNoise( LibDatas[target].ToArray());
				chartDatas.AddRange( Range( 0 , fixeddata.Length )
									  .Where( ( _ , i ) => i % 15 == 0 )
									  .Select( i => new double [ 2 ] { Labels [ i ] , fixeddata [ i ] } )
									  .ToArray() );

				SeriesColl[target] = CreateSeries( chartDatas , target );
			}
			else
			{
				PopSeries( target );
			}
		}

		public void PopSeries( int indexer )
		{
			SeriesColl [ indexer ] = new LineSeries();
		}


		public void ClearSeries()
		{
			SeriesColl = new SeriesCollection();
			for ( int i = 0 ; i < 49 ; i++ )
			{
				SeriesColl.Add( new LineSeries() );
			}
			this.Dispatcher.BeginInvoke( ( Action )( () => chtLiveLine.Series = SeriesColl ) );
		}

		public void Reset()
		{
			ClearSeries();
			LibDatas = null;
			Labels = null;
		}


		#region sub Func

		LineSeries CreateSeries( ChartValues<double [ ]> chartDatas , int indexer )
		{
			var newseries = new LineSeries();
			newseries.Values = chartDatas;
			newseries.DataLabels = false;
			newseries.PointGeometrySize = 0;
			newseries.Fill = Brushes.Transparent;
			newseries.Stroke = ColorList [ indexer +11 ];
			return newseries;
		}

		double [ ] ClearNoise( double [ ] self )
		{
			self [ 0 ] = self [ 2 ];
			self [ 1 ] = self [ 2 ];
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
