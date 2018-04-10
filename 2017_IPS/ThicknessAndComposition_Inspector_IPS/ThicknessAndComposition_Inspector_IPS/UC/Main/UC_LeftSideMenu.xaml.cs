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

namespace ThicknessAndComposition_Inspector_IPS
{
	public delegate void BtnEvt( string name );
	/// <summary>
	/// Interaction logic for UC_LeftSideMenu.xaml
	/// </summary>
	public partial class UC_LeftSideMenu : UserControl
	{
		public event BtnEvt evtBtn;
		public UC_LeftSideMenu()
		{
			InitializeComponent();
		}

		public void LeftSideBtnClick( object sender , RoutedEventArgs e )
		=> evtBtn( ( sender as Button ).Name );



	}
}
