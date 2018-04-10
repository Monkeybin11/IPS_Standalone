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
using ThicknessAndComposition_Inspector_IPS_Data;

namespace ThicknessAndComposition_Inspector_IPS
{
	/// <summary>
	/// Interaction logic for UC_DataGrid.xaml
	/// </summary>
	public partial class UC_DataGrid : UserControl
	{
		public UC_DataGrid()
		{
			InitializeComponent();
		}

		public void UpdateGrid( List<IPSResult_ForGrid> datas)
		{
			dgdResult.ItemsSource = null;
			dgdResult.ItemsSource = datas;
		}
	}
}
