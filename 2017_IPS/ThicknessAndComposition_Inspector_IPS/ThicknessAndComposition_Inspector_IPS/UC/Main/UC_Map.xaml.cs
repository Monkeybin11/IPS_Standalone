using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
	/// <summary>
	/// Interaction logic for UC_Map.xaml
	/// </summary>
	public partial class UC_Map : UserControl
	{
		public UC_Map()
		{
			InitializeComponent();
		}

		public void DrawImg(ImageSource src  ) // colormap need to be (255 x 3[rgb]) 
		{
			imgOri.ImageSource = src;
		}

	

	}
}
