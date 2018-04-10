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
using ThicknessAndComposition_Inspector_IPS_Core;
using ThicknessAndComposition_Inspector_IPS_Data;
using Emgu.CV;
using Emgu.CV.Structure;
using IPSAnalysis;
using static IPSAnalysis.Handler;
using SpeedyCoding;

namespace ThicknessAndComposition_Inspector_IPS
{
	using static ThicknessAndComposition_Inspector_IPS_Core.Core_Helper;
	using static ModelLib.AmplifiedType.Handler;
	using static IPSDataHandler.Handler;
	using ModelLib.AmplifiedType;
	using ModelLib.Data;

	public enum MsgType { Add , Remove , ChangeWav }

	/// <summary>
	/// Interaction logic for UC_AnalysisMap.xaml
	/// Draw Button Automatically Create Button Tag and Event.
	/// When Button is Clicked. Clicked event with index number go to Parent Window.
	/// If parent Window recieve event, All state updated automatically 
	/// and Ui update automatically ooccur
	/// </summary>
	public partial class UC_AnalysisMap : UserControl
	{
		public event Action<MsgType,string,double[]> evtClickedIndex;

		public UC_AnalysisMap()
		{
			InitializeComponent();

		}

		public void SetImage( BitmapSource src ) // done 
		{
			imgMap.ImageSource = src;
		}

		public void SetBtnTag( IPSResult result ) // done
		{

			Just( result )
				.Lift( CalcTagPos )
				.ForEach( DrawBtnTag );
			var poslist = CalcTagPos(result);

		}


		private void DrawBtnTag( List<ValPosCrt> tagPos ) // done
		{
			cvsMap.Children.Clear();
			int posNum = tagPos.Count;

			StackPanel[] temp = new StackPanel[ posNum ];
			Button[] btn = new Button[posNum];

			for ( int i = 0 ; i < posNum ; i++ )
			{
				var newbtn = CheckButton(i);
				Canvas.SetLeft( newbtn , tagPos [ i ].X - newbtn.Width / 2 );
				Canvas.SetTop( newbtn , tagPos [ i ].Y - newbtn.Height / 2 );
				cvsMap.Children.Add( newbtn );
				btn [ i ] = newbtn;

				var newlbl = TagLabel(tagPos[i].Value);
				Canvas.SetLeft( newlbl , tagPos [ i ].X - newbtn.Width / 2 - 10 );
				Canvas.SetTop( newlbl , tagPos [ i ].Y - newbtn.Height / 2 - 15 );
				cvsMap.Children.Add( newlbl );
			}
		}

		private Button CheckButton( int i ) // done 
		{
			var btn = new Button();
			btn.Name = "btn" + i.ToString();
			btn.Width = 10;
			btn.Height = 10;
			btn.Opacity = 0.9;
			btn.Background = Brushes.LawnGreen;
			btn.Click += ClickIdx;
			return btn;
		}

		private Label TagLabel(double name)
		{
			var lbl = new Label();
			lbl.Content = name.ToString("###.#");       
			lbl.FontSize = 8;
			lbl.Background = Brushes.Transparent;
			lbl.Foreground = Brushes.AntiqueWhite;
			lbl.HorizontalAlignment = HorizontalAlignment.Left;
			lbl.VerticalAlignment = VerticalAlignment.Top;
			return lbl;
		}

		public void ClickIdx( object sender , RoutedEventArgs e ) // done
		{
			try
			{
				var self = sender as Button;
				if ( Keyboard.IsKeyDown( Key.LeftCtrl ) ) // Remove with ctrl
				{
					( sender as Button ).Name.Print( "Remove " );
					self.Background = Brushes.LawnGreen;
					evtClickedIndex( MsgType.Remove ,( sender as Button ).Name.Replace( "btn" , "" ) , null );
				}
				else
				{
					( sender as Button ).Name.Print( "Add " );
					self.Background = Brushes.OrangeRed;
					evtClickedIndex( MsgType.Add , ( sender as Button ).Name.Replace( "btn" , "" ) , null);
				}

			}
			catch ( Exception ex )
			{ ex.Print( "Map Click Error Msg " ); }

		}


		private List<ValPosCrt> CalcTagPos( IPSResult result ) // done
		{
			var w0 = 300;
			var h0 = 300;
			var w1 = this.ActualWidth - 60;
			var h1 = this.ActualHeight - 60;

			var w2 = this.Width;
			var h2 = this.Height;



			var RealToCanvas = FnReScale( w0 , h0 , w1 , h1, w1/2+10 , h1/2+10);
			Func< Tuple< CrtnCrd ,double> , ValPosCrt> toValPos
				= posval => RealToCanvas( ValPosCrt(posval.Item1.X, posval.Item1.Y , posval.Item2 ) );

			var scaledPosList = result.SpotDataList.Lift(x => Tuple.Create( x.CrtPos , x.Thickness))
												   .Lift(toValPos)
												   .ToList();
			return scaledPosList;
		}

	
	}
}
