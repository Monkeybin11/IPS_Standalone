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
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using SpeedyCoding;
using static System.Linq.Enumerable;

namespace ThicknessAndComposition_Inspector_IPS
{
	/// <summary>
	/// Interaction logic for TestRemoveplss.xaml
	/// </summary>
	public partial class TestRemoveplss : Window
	{

		SeriesCollection SeriesColl = new SeriesCollection();
		BrushList[] ColorList;
		public TestRemoveplss()
		{
			InitializeComponent();
			DataContext = this;
			var mapper = Mappers.Xy<double[]>()
				.X(model => model[0])            //use DateTime.Ticks as X
                .Y(model => model[1]);           //use the value property as Y

			Charting.For<double [ ]>( mapper );



			for ( int i = 0 ; i < 49 ; i++ )
			{
				SeriesColl.Add( new LineSeries() );
			}


			ColorList = typeof( Brushes ).GetProperties()
						.Select( ( x , i ) => new BrushList { Index = i , Color = x.GetValue( null ) as Brush } )
						.ToArray();


		}

		private void btn1_Click( object sender , RoutedEventArgs e )
		{
			Random rnd = new Random();
			var list = Range(0,100).Select( x => (double)x + rnd.NextDouble()*3 ).ToArray();
			var labels = Range(0,100).Select( x => (double)x).ToArray();

			AddSeries( list , labels , 10 );

		}

		private void btn2_Click( object sender , RoutedEventArgs e )
		{
			PopSeries( 10 );
		}

		public void AddSeries( IEnumerable<double> src , IEnumerable<double> labels , int indexer )
		{
			var chartDatas = new ChartValues<double[]>();
			var fixeddata = src.ToArray();
			var lbls = labels.ToArray();

			chartDatas.AddRange( Range( 0 , fixeddata.Length )
								  .Where( ( _ , i ) => i % 10 == 0 )
								  .Select( i => new double [ 2 ] { lbls [ i ] , fixeddata [ i ] } )
								  .ToArray() );

			SeriesColl.Insert(indexer, CreateSeries( chartDatas , indexer ) );
			chtLiveLine.Series = SeriesColl;
			//SeriesColl [ 99 ] = new LineSeries();
			//SeriesColl [ 99 ] = ( CreateSeries( chartDatas , indexer.ToString() ) );
		}

		public void PopSeries( int indexer )
		{
			SeriesColl [ indexer ] = new LineSeries();
		}

		LineSeries CreateSeries( ChartValues<double[]> chartDatas , int indexer )
		{
			var newseries = new LineSeries();
			newseries.Values = chartDatas;
			newseries.DataLabels = false;
			newseries.PointGeometrySize = 0;
			newseries.Fill = Brushes.Transparent;
			newseries.Stroke = ColorList [ indexer ].Color;
			return newseries;
		}

		private void btn3_Click( object sender , RoutedEventArgs e )
		{
			Random rnd = new Random();
			var list = Range(0,100).Select( x => (double)x + 10 + rnd.NextDouble()*3 ).ToArray();
			var labels = Range(0,100).Select( x => (double)x).ToArray();
			AddSeries( list , labels , 40 );
		}

		private void btn4_Click( object sender , RoutedEventArgs e )
		{
			PopSeries( 40 );
		}

		public BrushList[] Brushlist()
			=> typeof(Brushes).GetProperties()
						.Select( (x,i) => new BrushList{ Index = i , Color = x.GetValue(null) as Brush} )
						.ToArray();
	}

	public class BrushList
	{
		public int Index;
		public Brush Color;
	}


}
