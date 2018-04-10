using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ApplicationUtilTool.FileIO
{
	public static class PathTool
	{
		public static void CreateFolder( string basepath )
		{
			try
			{
				string outputpath = basepath;
				if ( !Directory.Exists( outputpath ) )
				{
					Directory.CreateDirectory( outputpath );
				}
			}
			catch ( Exception )
			{
				MessageBox.Show( "Access Violation" );
			}
		}
	}
}
