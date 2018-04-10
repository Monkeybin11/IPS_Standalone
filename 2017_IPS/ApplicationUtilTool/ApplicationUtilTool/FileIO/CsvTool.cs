using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUtilTool.FileIO
{
	public static class CsvTool
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="rowskip"></param>
		/// <param name="colskip"></param>
		/// <param name="delimiter"></param>
		/// <param name="order0Dirction"> ReadLine Diection. same as numpy direction. If data is listed in vertical, use order0Direction = true  </param>
		/// <returns></returns>
		public static string [] [] ReadCsv2String(string path , int rowskip = 0 , int colskip = 0 , Char delimiter = ',' , bool order0Dirction = true )
		{
			if ( !order0Dirction ) return File.ReadLines( path ).Skip(rowskip).Select( lines => lines.Split( delimiter ).Skip(colskip).ToArray() ).ToArray();
			else
			{
				var res = File.ReadLines( path ).Select( lines => lines.Split( delimiter ) ).ToArray();

				List<string[]> output = new List<string[]>();
				for ( int i = colskip ; i < res[0].GetLength(0) ; i++ )
				{
					List<string> column = new List<string>();
					for ( int j = rowskip ; j < res.GetLength(0) ; j++ )
					{
						column.Add( res[j][i] );
					}
					output.Add( column.ToArray() );
				}
				return output.ToArray();
			}
		}

		public static string [ ] [ ] ReadCsv2String( string path ,int rowend , int colend , int rowskip = 0 , int colskip = 0 , Char delimiter = ',' , bool order0Dirction = true )
		{
			if ( !order0Dirction )
				return File.ReadLines( path )
							.Skip( rowskip )
							.Take( rowend - rowskip )
							.Select( lines => lines.Split( delimiter )
													.Skip( colskip )
													.Take(colend - colskip)
													.ToArray() )
							.ToArray();
			else
			{
				var res = File.ReadLines( path ).Select( lines => lines.Split( delimiter ) ).ToArray();

				List<string[]> output = new List<string[]>();
				for ( int i = colskip ; i < colend; i++ )
				{
					List<string> column = new List<string>();
					for ( int j = rowskip ; j < rowend ; j++ )
					{
						column.Add( res [ j ] [ i ] );
					}
					output.Add( column.ToArray() );
				}
				return output.ToArray();
			}
		}

		






	}
}
