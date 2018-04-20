using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeedyCoding
{
    public static class Matrix
    {


        #region Shape Convertion
        /// <summary>
        /// Reshape 1D to 2D array
        /// Data shape will be -> [ [second ] [second] [second] ... ] , 
        /// </summary>
        /// <typeparam name="Tsrc"></typeparam>
        /// <param name="src"></param>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Tsrc [ , ] Reshape<Tsrc>(
            this Tsrc [ ] src
            , int first
            , int second
            )
        {
            var result = new Tsrc[first,second];
            int idx = 0;
            for ( int f = 0 ; f < first ; f++ )
            {
                for ( int s = 0 ; s < second ; s++ )
                {
                    result [ f , s ] = src [ idx++ ];
                }
            }
            return result;
        }

        // ( [ third ] x second ) x first order 
        /// <summary>
        /// result [ f , s , t ] = src [ idx++ ]. data is setted third order first
        /// Data shape will be -> [ [ [third] ] ] [ [third] ] [ [third] ] ... ] , 
        /// </summary>
        /// <typeparam name="Tsrc"></typeparam>
        /// <param name="src"></param>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="third"></param>
        /// <returns></returns>
        public static Tsrc [ , , ] Reshape<Tsrc>(
            this Tsrc [ ] src
            , int first
            , int second
            , int third
            )
        {
            var result = new Tsrc[first,second,third];
            int idx = 0;
            for ( int f = 0 ; f < first ; f++ )
            {
                for ( int s = 0 ; s < second ; s++ )
                {
                    for ( int t = 0 ; t < third ; t++ )
                    {
						result [ f , s , t ] = src [ idx++ ];
                    }
                }
            }
            return result;
        }

        // ( [ second ] x first )  order 
        /// <summary>
        /// result [ f , s  ] = src [ s + (s-1)*f ]. data is setted second order first
        /// Assign row and col index to each element of data
        /// </summary>
        /// <typeparam name="TSrc"></typeparam>
        /// <param name="this"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns> [ Tuple< srcData , row Index , col Index > ] </returns>
        public static IEnumerable< Tuple<TSrc , int , int> > ZipFlattenReshape<TSrc>(
          this TSrc [ ] src
          , int row
          , int col )
        {
            if ( src.GetLength( 0 ) != row * col ) return null;
            return Enumerable.Range(0, row)
                                    .SelectMany(
                                        j => Enumerable.Range(0, col),
                                        (j, i) => Tuple.Create(src[j * col + i], j, i));
        }

        // ( [ third ] x second ) x first order 
        /// <summary>
        /// result [ f , s , t ] = src [ t + (s+(t-1)) * f ]. data is setted thrid order first
        /// Assign 1st , 2nd , 3rd index to each element of data 
        /// </summary>
        /// <typeparam name="TSrc"></typeparam>
        /// <param name="src"></param>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="third"></param>
        /// <returns></returns>
        public static Tuple<TSrc , int , int , int> [ ] ZipFlattenReshape<TSrc>(
          this TSrc [ ] src
          , int first
          , int second
          , int third)
        {
            if ( src.GetLength( 0 ) != first * second * third ) return null;
            return Enumerable.Range( 0 , first )
                    .SelectMany( ( x , f ) => Enumerable.Range( 0 , second )
                                        .SelectMany( s => Enumerable.Range( 0 , third )
                                                     , ( s , t ) => Tuple.Create( src [ f * first + s * second + t ] , f , s , t ) ))
                    .ToArray();
        }

        //( [ col ] x rows )
        /// <summary>
        /// Flatten data. 
        /// </summary>
        /// <typeparam name="TSrc"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static TSrc [ ] Flatten<TSrc>(
            this TSrc [ ] [ ] src )
        {
            int pagingSize = src[0].GetLength(0), pagingNum = src.GetLength(0);
            return src.SelectMany(
                            rows => rows ,
                            ( rows , col ) => col ).ToArray();
        }

        /// <summary>
        /// Flatten data
        /// </summary>
        /// <typeparam name="TSrc"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static TSrc [ ] Flatten<TSrc>(
           this TSrc [ ] [ ] [ ] src )
        {
            int fsize = src[0].GetLength(0);
            int ssize = src[1].GetLength(0);
            int tsize = src[2].GetLength(0);


            int totalSize =  fsize + ssize + tsize;
            List<TSrc> result = new List<TSrc>();
            for ( int f = 0 ; f < fsize ; f++ )
            {
                for ( int s = 0 ; s < ssize ; s++ )
                {
                    for ( int t = 0 ; t < tsize ; t++ )
                    {
                        result.Add( src [f][s][t] );
                    }
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// Flatten data
        /// </summary>
        /// <typeparam name="TSrc"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
		public static TSrc [ ] Flatten<TSrc>(
		  this TSrc [ , ] src )
		{
			int fsize = src.GetLength(0);
			int ssize = src.GetLength(1);


			int totalSize =  fsize + ssize;
			List<TSrc> result = new List<TSrc>();
			for ( int f = 0 ; f < fsize ; f++ )
			{
				for ( int s = 0 ; s < ssize ; s++ )
				{
					result.Add( src [ f , s ] );
				}
			}
			return result.ToArray();
		}


        /// <summary>
        /// Flatten data
        /// </summary>
        /// <typeparam name="TSrc"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
		public static TSrc [ ] Flatten<TSrc>(
		  this TSrc [ ,, ] src )
		{
			int fsize = src.GetLength(0);
			int ssize = src.GetLength(1);
			int tsize = src.GetLength(2);


			int totalSize =  fsize + ssize + tsize;
			List<TSrc> result = new List<TSrc>();
			for ( int f = 0 ; f < fsize ; f++ )
			{
				for ( int s = 0 ; s < ssize ; s++ )
				{
					for ( int t = 0 ; t < tsize ; t++ )
					{
						result.Add( src [ f , s , t ] );
					}
				}
			}
			return result.ToArray();
		}


        /// <summary>
        /// Padding 0 value to 2d jagged array
        /// Total size of row and col become [ rowLength + paddingSize*2 , colLength + paddingSize*2 ]
        /// </summary>
        /// <typeparam name="TSrc"></typeparam>
        /// <param name="src"></param>
        /// <param name="padSize">Size of padding expanding each side</param>
        /// <returns></returns>
		public static TSrc [ ] [ ] Padding<TSrc>(
    this TSrc [ ] [ ] src ,
    int padSize )
        {
            if ( padSize == 0 ) return src;
            int rownum = src.GetLength( 0 );
            int colnum = src[0].GetLength( 0 );


            var basedata
                = new TSrc[rownum + padSize*2]
                     .Select( x
                         => new TSrc[colnum + padSize*2] )
                     .ToArray();
            for ( int j = padSize ; j < basedata.Len() - padSize ; j++ )
            {
                for ( int i = padSize ; i < basedata [ j ].Len() - padSize ; i++ )
                {
                    basedata [ j ] [ i ] = src [ j - padSize ] [ i - padSize ];
                }
            }
            return basedata;
        }


        /// <summary>
        /// Padding 0 value to 2d jagged array
        /// Total size of row and col become [ rowLength + paddingSize*2 , colLength + paddingSize*2 ]
        /// </summary>
        /// <typeparam name="TSrc"></typeparam>
        /// <param name="src"></param>
        /// <param name="padSize">Size of padding exapanding each side</param>
        /// <returns></returns>
        public static TSrc [ , ] Padding<TSrc>(
           this TSrc [ , ] src ,
           int padSize )
        {
            if ( padSize == 0 ) return src;

            var basedata
                = new TSrc[src.GetLength( 0 ) + padSize*2,src.GetLength(1) + padSize*2];

            for ( int j = padSize ; j < basedata.GetLength( 0 ) - padSize ; j++ )
            {
                for ( int i = padSize ; i < basedata.GetLength( 1 ) - padSize ; i++ )
                {
                    basedata [ j , i ] = src [ j - padSize , i - padSize ];
                }
            }
            return basedata;
        }

        /// <summary>
        /// Array Deep Copy
        /// </summary>
        /// <typeparam name="TSrc"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static Array [ ] DCopy<TSrc>(
          this Array [ ] src )
        {
            Array[] output = new Array[src.GetLength(0)];
            for ( int i = 0 ; i < src.GetLength( 0 ) ; i++ )
            {
                output [ i ] = src [ i ];
            }
            return output;
        }

        /// <summary>
        /// Slice 1D array. 
        /// Range = [start : end]
        /// </summary>
        /// <typeparam name="TSrc"></typeparam>
        /// <param name="src"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static TSrc [ ] Slice1D<TSrc>(
            this TSrc [ ] src ,
            int start ,
            int end )
        {
            TSrc[] output = new TSrc[end - start + 1];
            int count = 0;
            for ( int i = start ; i <= end ; i++ )
            {
                output [ count ] = src [ i ];
                count++;
            }
            return output;
        }

        /// <summary>
        /// Slice 2DArray.
        /// Range = [ startY : endY , startX : endX ]
        /// </summary>
        /// <typeparam name="TSrc"></typeparam>
        /// <param name="src"></param>
        /// <param name="yxStart">int[y,x]</param>
        /// <param name="yxEnd">int[y,x]</param>
        /// <returns></returns>
        public static TSrc [ ] [ ] Slice2D<TSrc>(
            this TSrc [ ] [ ] src ,
            int [ ] yxStart ,
            int [ ] yxEnd )
        {
            TSrc[][] output = new TSrc[yxEnd[0] - yxStart[0] + 1][];
            for (int i = 0; i < output.GetLength(0); i++)
            {
                output[i] = new TSrc[yxEnd[1] - yxStart[1] + 1];
            }

            int countY = 0;
            int countX = 0;
            for ( int j  = yxStart [ 0 ] ; j <= yxEnd [ 0 ] ; j++ )
            {
                countX = 0;
                for ( int i  = yxStart [ 1 ] ; i <= yxEnd [ 1 ] ; i++ )
                {
                    output [ countY ] [ countX ] = src [ j ] [ i ];
                    countX++;
                }
                countY++;
            }
            return output;
        }

		[Obsolete]
        public static T [ ] Transpose<T>(
            this T [ ] src ,
            int currentRowNum,
            int currentColumnNum)
        {
            List<T> output = new List<T>();
            for ( int i = 0 ; i < currentRowNum ; i++ )
            {
                for ( int j = 0 ; j < currentColumnNum ; j++ )
                {
                    output.Add( src [ currentRowNum * j + i ] );
                }
            }
            return output.ToArray();
        }

		public static T [ ] [ ] Transpose<T>(
			this T [ ] [ ] src )
		{
			int rownum = src.Length;
			int colnum = src[0].Length;

			T[][] res = new T[colnum][];
			for ( int i = 0 ; i < colnum ; i++ )
			{
				T[] rowline = new T[rownum];

				for ( int j = 0 ; j < rownum ; j++ )
				{
					rowline [ j ] = src [j] [i];
				}
				res [ i ] = rowline;
			}
			return res;
		}


        #endregion

        #region Concatenate
        /// <summary>
        /// Concatenate 1D collection horizontally
        /// Same as IEnumerable.Concat
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="targetRight"></param>
        /// <returns></returns>
        public static IEnumerable<T> Concate_H<T>(
            this IEnumerable<T> src,
            IEnumerable<T> targetRight)
            => src.Concat(targetRight);



        /// <summary>
        /// Concatenate 2D array horizontally
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="targetRight"></param>
        /// <returns></returns>
        public static T[,] Concate_H<T>(
            this T[,] src,
            T[,] targetRight)
        {
            if (src.GetLength(0) == targetRight.GetLength(0)
                && src.GetLength(1) == targetRight.GetLength(1))
            {
                int order0Len = src.GetLength(0);
                int order1Len = src.GetLength(1) + targetRight.GetLength(1);
                T[,] output = new T[order0Len, order1Len];

                Action<int> act = new Action<int>(j =>
                {
                    for (int i = 0; i < src.GetLength(1); i++)
                    {
                        output[j, i] = src[j, i];

                    }

                    for (int i = src.GetLength(1); i < order1Len; i++)
                    {
                        output[j, i] = targetRight[j, i - src.GetLength(1) ];
                    }
                });

                if (order0Len * order1Len > Int32.MaxValue)
                {
                    Parallel.For(0, order0Len, new Action<int>(j => {
                        act(j);
                    }));
                }
                else
                {
                    for (int j = 0; j < order0Len; j++)
                    {
                        act(j);
                    }
                }
                return output;
            }
            else
            {
                throw new IndexOutOfRangeException("Source and Target Length are not same");
            }
        }

        /// <summary>
        /// Concatenate 3D array following second order. 
        /// Result shape become [ src.GetLength(0), src.GetLength(1) + targetRight.GetLength(1) , src.GetLength(2)  ]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="targetRight"></param>
        /// <returns></returns>
        public static T [ , , ] Concate_H<T>(
           this T [ , , ] src ,
           T [ , , ] targetRight )
        {
            if (src.GetLength(0) == targetRight.GetLength(0)
                && src.GetLength(2) == targetRight.GetLength(2))
            {
                int order0Len = src.GetLength(0);
                int order1Len = src.GetLength(1) + targetRight.GetLength(1);
                int order2Len = src.GetLength(2);

                T[,,] output = new T[order0Len, order1Len, order2Len];

                Action<int> act = new Action<int>(j =>
               {
                   for (int i = 0; i < src.GetLength(1); i++)
                   {
                       for (int k = 0; k < order2Len; k++)
                       {
                           try
                           {
                               output[j, i, k] = src[j, i, k];
                           }
                           catch (Exception e)
                           {
                               e.ToString().Print();
                           }
                       }
                   }

                   for (int i = src.GetLength(1); i < order1Len; i++)
                   {
                       for (int k = 0; k < order2Len; k++)
                       {
                           try
                           {
                               output[j, i, k] = targetRight[j, i - src.GetLength(1), k];
                           }
                           catch (Exception e)
                           {
                               e.ToString().Print();
                           }
                       }
                   }
               });

                if (order0Len * order1Len * order2Len > Int32.MaxValue)
                {
                    Parallel.For(0, order0Len, new Action<int>(j =>
                    {
                        act(j);
                    }));
                }
                else
                {
                    for (int j = 0; j < order0Len; j++)
                    {
                        act(j);
                    }
                }
                return output;
            }
            else
            {
                throw new IndexOutOfRangeException("Source and Target Length are not same");
            }
          
        }

        /// <summary>
        /// Concatenate 3D array following second order. 
        /// Result shape become [ src.GetLength(0), (src.GetLength(1)  - clipping) + (targetRight.GetLength(1) - clipping) , src.GetLength(2)  ]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="targetRight"></param>
        /// <param name="clipping"></param>
        /// <returns></returns>
        public static T [ , , ] Concate_H<T>(
          this T [ , , ] src
           , T [ , , ] targetRight
           , int clipping )
           where T : new()
        {
            if (src.GetLength(0) == targetRight.GetLength(0)
                && src.GetLength(2) == targetRight.GetLength(2))
            {
                int order0Len = src.GetLength(0);
                int order1Len = src.GetLength(1) + targetRight.GetLength(1) - 2 * clipping;
                int order2Len = src.GetLength(2);

                T[,,] output = new T[order0Len, order1Len, order2Len];

                Action<int> act = new Action<int>(j =>
               {
                   for (int i = 0; i < src.GetLength(1) - clipping; i++)
                   {
                       for (int k = 0; k < order2Len; k++)
                       {
                           output[j, i, k] = src[j, i, k];
                       }
                   }

                   for (int i = src.GetLength(1) - clipping; i < order1Len; i++)
                   {
                       for (int k = 0; k < order2Len; k++)
                       {

                           output[j, i, k] = targetRight[j, i - src.GetLength(1) + clipping, k];
                       }
                   }
               });

                if (order0Len * order1Len * order2Len > double.MaxValue)
                {
                    Parallel.For(0, order0Len, new Action<int>(j =>
                    {
                        act(j);
                    }));
                }
                else
                {
                    for (int j = 0; j < order0Len; j++)
                    {
                        act(j);
                    }
                }
                return output;
            }
            else
            {
                throw new IndexOutOfRangeException("Source and Target Length are not same");
            }
        }


        /// <summary>
        /// Concatenate 3D array following first order. 
        /// Result shape become [ src.GetLength(0) targetBottom.GetLength(0) , src.GetLength(1) , src.GetLength(2)  ]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="targetBottom"></param>
        /// <returns></returns>
        public static T [ , , ] Concate_V<T>(
           this T [ , , ] src ,
           T [ , , ] targetBottom )
            where T : new()
        {
            if (src.GetLength(1) == targetBottom.GetLength(1)
                && src.GetLength(2) == targetBottom.GetLength(2))
            {
                int order0Len = src.GetLength(0) + targetBottom.GetLength(0);
                int order1Len = src.GetLength(1);
                int order2Len = src.GetLength(2);

                T[,,] output = new T[order0Len, order1Len, order2Len];

                Action<int> act = new Action<int>(i =>
                {
                    for (int j = 0; j < src.GetLength(0); j++)
                    {
                        for (int k = 0; k < order2Len; k++)
                        {
                            output[j, i, k] = src[j, i, k];
                        }
                    }

                    for (int j = src.GetLength(0); j < order0Len; j++)
                    {
                        for (int k = 0; k < order2Len; k++)
                        {
                            output[j, i, k] = targetBottom[j - src.GetLength(0), i, k];
                        }
                    }
                });

                if (order0Len * order1Len * order2Len > double.MaxValue)
                {
                    Parallel.For(0, order1Len, new Action<int>(i =>
                    {
                        act(i);
                    }));
                }
                else
                {
                    for (int i = 0; i < order1Len; i++)
                    {
                        act(i);
                    }
                }
                return output;
            }
            else
            {
                throw new ArgumentOutOfRangeException("Source and Target Length are not same");
            }
        }


        #endregion

        #region MinMax


        /// <summary>
        /// Get max-value of 2D array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T MaxArray<T>(
            this T [ , ] src )
            where T : struct
        {
            return Enumerable.Range( 0 , src.GetLength( 0 ) )
                    .Select( j =>
                         Enumerable.Range( 0 , src.GetLength( 1 ) )
                             .Select( i => src [ j , i ] )
                             .Max() )
                    .Max();
        }

        /// <summary>
        /// Get max-value of 3D array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T MaxArray<T>(
            this T [ , , ] src )
            where T : struct
        {
            return Enumerable.Range( 0 , src.GetLength( 0 ) )
                    .Select( j =>
                         Enumerable.Range( 0 , src.GetLength( 1 ) )
                             .Select( i =>
                                  Enumerable.Range( 0 , src.GetLength( 2 ) )
                                      .Select( k => src [ j , i , k ] )
                                      .Max() )
                             .Max() )
                    .Max();
        }


        /// <summary>
        /// Get min-value of 2D array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T MinArray<T>(
            this T [ , ] src )
            where T : struct
        {
            return Enumerable.Range( 0 , src.GetLength( 0 ) )
                    .Select( j =>
                         Enumerable.Range( 0 , src.GetLength( 1 ) )
                             .Select( i => src [ j , i ] )
                             .Min() )
                    .Min();
        }

        /// <summary>
        /// Get min-value of 3D array 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T MinArray<T>(
          this T [ , , ] src )
          where T : struct
        {
            return Enumerable.Range( 0 , src.GetLength( 0 ) )
                    .Select( j =>
                         Enumerable.Range( 0 , src.GetLength( 1 ) )
                             .Select( i =>
                                  Enumerable.Range( 0 , src.GetLength( 2 ) )
                                      .Select( k => src [ j , i , k ] )
                                      .Min() )
                             .Min() )
                    .Min();
        }
        #endregion

        #region Conversion

        /// <summary>
        /// 2D jagged array to 2D array (matrix)
        /// </summary>
        /// <typeparam name="TSrc"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static TSrc [ , ] ToMat<TSrc>(
            this TSrc [ ] [ ] src )
        {
            int rowL = src.Len(0), colL = src.Len(1);

            TSrc[,] output = new TSrc[rowL, colL];
            for ( int j = 0 ; j < rowL ; j++ )
            {
                for ( int i = 0 ; i < colL ; i++ )
                {
                    output [ j , i ] = src [ j ] [ i ];
                }
            }
            return output;
        }

        /// <summary>
        /// 3D jagged array to 3D array (matrix)
        /// </summary>
        /// <typeparam name="TSrc"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static TSrc [ , , ] ToMat<TSrc>(
            this TSrc [ ] [ ] [ ] src )
        {
            int rowL = src.Len(0), fcolL = src.Len(1), scolL = src.Len(2);

            TSrc[,,] output = new TSrc[rowL, fcolL, scolL];
            for ( int j = 0 ; j < rowL ; j++ )
            {
                for ( int i = 0 ; i < fcolL ; i++ )
                {
                    for ( int k = 0 ; k < scolL ; k++ )
                    {
                        output [ j , i , k ] = src [ j ] [ i ] [ k ];
                    }
                }
            }
            return output;
        }

        /// <summary>
        /// 2D array (matrix) to 2D jagged array 
        /// </summary>
        /// <typeparam name="TSrc"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
		public static TSrc [ ] [ ] ToJagged<TSrc>(
		 this TSrc [ , ] src )
		{
			int rowL = src.Len(0), fcolL = src.Len(1);

			TSrc[][] output = new TSrc[rowL][];
			for ( int j = 0 ; j < rowL ; j++ )
			{
				TSrc[] second = new TSrc[fcolL];
				for ( int i = 0 ; i < fcolL ; i++ )
				{
					second [ i ] = src[j,i];
				}
				output [ j ] = second;
			}
			return output;
		}

        /// <summary>
        /// 3D array (matrix) to 3D jagged array 
        /// </summary>
        /// <typeparam name="TSrc"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
		public static TSrc [ ] [ ] [ ] ToJagged<TSrc>(
           this TSrc [ , , ] src )
        {
            int rowL = src.Len(0), fcolL = src.Len(1), scolL = src.Len(2);

            TSrc[][][] output = new TSrc[rowL][][];
            for ( int j = 0 ; j < rowL ; j++ )
            {
                TSrc[][] second = new TSrc[fcolL][];
                for ( int i = 0 ; i < fcolL ; i++ )
                {
                    TSrc[] third = new TSrc[scolL];
                    for ( int k = 0 ; k < scolL ; k++ )
                    {
                        third [ k ] = src [ j , i , k ];
                    }
                    second [ i ] = third;
                }
                output [ j ] = second;
            }
            return output;
        }


        /// <summary>
        /// Create 2D jagged array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static T [ ] [ ] JArray<T>( this int src , int col )
        {
            return Enumerable.Range( 0 , src ).Select( x => new T [ col ] ).ToArray();
        }

        /// <summary>
        /// Create 3D jagged array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="fcol"></param>
        /// <param name="scol"></param>
        /// <returns></returns>
        public static T [ ] [ ] [ ] JArray<T>( this int src , int fcol , int scol )
        {
            return Enumerable.Range( 0 , src )
                    .Select( fcols => Enumerable.Range( 0 , fcol )
                                        .Select( scols => new T [ scol ] )
                                        .ToArray() )
                    .ToArray();
        }
        #endregion

        #region Operation
        /// <summary>
        /// Dot operation. 
        /// (Elementwise product. Hadamard product 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int [ ] Dot(
           this IEnumerable<int> src ,
           IEnumerable<int> target )
        {
            return src.Count() != target.Count() ? null
                : src.Zip( target , ( f , s ) => f * s ).ToArray();
        }

        /// <summary>
        /// Dot operation. 
        /// (Elementwise product. Hadamard product 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double [ ] Dot(
            this IEnumerable<double> src ,
            IEnumerable<double> target )
        {
            return src.Count() != target.Count() ? null
                : src.Zip( target , ( f , s ) => f * s ).ToArray();
        }

        /// <summary>
        /// Dot operation. 
        /// (Elementwise product. Hadamard product 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float [ ] Dot(
           this IEnumerable<float> src ,
           IEnumerable<float> target )
        {
            return src.Count() != target.Count() ? null
                : src.Zip( target , ( f , s ) => f * s ).ToArray();
        }

        /// <summary>
        /// Dot operation. 
        /// (Elementwise product. Hadamard product 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int [ ] [ ] Dot(
            this IEnumerable<IEnumerable<int>> src ,
            IEnumerable<IEnumerable<int>> target )
        {
            return src.Zip( target
                , ( flist , slist ) =>
                                    flist.Dot( slist ) ).ToArray();
        }

        /// <summary>
        /// Dot operation. 
        /// (Elementwise product. Hadamard product 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static double [ ] [ ] Dot(
            this IEnumerable<IEnumerable<double>> src ,
            IEnumerable<IEnumerable<double>> target )
        {
            return src.Zip( target
                , ( flist , slist ) =>
                                    flist.Dot( slist ) ).ToArray();
        }

        /// <summary>
        /// Dot operation. 
        /// (Elementwise product. Hadamard product 
        /// </summary>
        /// <param name="src"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float [ ] [ ] Dot(
            this IEnumerable<IEnumerable<float>> src ,
            IEnumerable<IEnumerable<float>> target )
        {
            return src.Zip( target
                , ( flist , slist ) =>
                                    flist.Dot( slist ) ).ToArray();
        }

		/// <summary>
		/// Elementwise dot for particular region. output is same size with target mask
		/// </summary>
		/// <typeparam name="Tsrc"></typeparam>
		/// <param name="this"></param>
		/// <param name="target"></param>
		/// <param name="startYX">Index of [Y,X] </param>
		/// <param name="hw">Size of [Width,Height]</param>
		/// <returns></returns>
        public static Tsrc [ ] [ ] PartialDot<Tsrc>(
             this Tsrc [ ] [ ] src ,
             dynamic [ ] [ ] target ,
             int [ ] startYX ,
             int [ ] hw )
        {
            Tsrc[][] output = new Tsrc[target.GetLength(0)]
                                .Select(x => new Tsrc[target[0].GetLength(0)])
                                .ToArray();

            int county = 0 , countx = 0;

            for ( int j = startYX [ 0 ] ; j < startYX [ 0 ] + hw [ 0 ] ; j++ )
            {
                countx = 0;
                for ( int i = startYX [ 1 ] ; i < startYX [ 1 ] + hw [ 1 ] ; i++ )
                {
                    output [ county ] [ countx ] = target [ county ] [ countx ] * src [ j ] [ i ];
                    countx++;
                }
                county++;
            }
            return output;
        }

        /// <summary>
        ///  Elementwise dot for particular region. output is same size with target mask
        /// </summary>
        /// <param name="this"></param>
        /// <param name="target"></param>
        /// <param name="startYX">Index of [Y,X] </param>
        /// <param name="hw">Size of [Width,Height]</param>
        /// <returns></returns>
        public static double [ ] [ ] PartialDot(
           this double [ ] [ ] src ,
           double [ ] [ ] target ,
           int [ ] startYX ,
           int [ ] hw )
        {
            double[][] output = new double[target.GetLength(0)]
                                .Select(x => new double[target[0].GetLength(0)])
                                .ToArray();

            int county = 0 , countx = 0;

            for ( int j = startYX [ 0 ] ; j < startYX [ 0 ] + hw [ 0 ] ; j++ )
            {
                countx = 0;
                for ( int i = startYX [ 1 ] ; i < startYX [ 1 ] + hw [ 1 ] ; i++ )
                {
                    output [ county ] [ countx ] = target [ county ] [ countx ] * src [ j ] [ i ];
                    countx++;
                }
                county++;
            }
            return output;
        }

        /// <summary>
        ///  Elementwise dot for particular region. output is same size with target mask
        /// </summary>
        /// <param name="this"></param>
        /// <param name="target"></param>
        /// <param name="startYX">Index of [Y,X] </param>
        /// <param name="hw">Size of [Width,Height]</param>
        /// <returns></returns>
        public static int[][] PartialDot(
          this int[][] src,
          int[][] target,
          int[] startYX,
          int[] hw)
        {
            int[][] output = new int[target.GetLength(0)]
                                .Select(x => new int[target[0].GetLength(0)])
                                .ToArray();

            int county = 0, countx = 0;

            for (int j = startYX[0]; j < startYX[0] + hw[0]; j++)
            {
                countx = 0;
                for (int i = startYX[1]; i < startYX[1] + hw[1]; i++)
                {
                    output[county][countx] = target[county][countx] * src[j][i];
                    countx++;
                }
                county++;
            }
            return output;
        }

        /// <summary>
        ///  Elementwise dot for particular region. output is same size with target mask
        /// </summary>
        /// <param name="this"></param>
        /// <param name="target"></param>
        /// <param name="startYX">Index of [Y,X] </param>
        /// <param name="hw">Size of [Width,Height]</param>
        /// <returns></returns>
        public static double[,] PartialDot(
          this double[,] src,
          double[,] target,
          int[] startYX,
          int[] hw)
        {
            double[,] output = new double[target.GetLength(0), target.GetLength(1)];

            int county = 0, countx = 0;

            for (int j = startYX[0]; j < startYX[0] + hw[0]; j++)
            {
                countx = 0;
                for (int i = startYX[1]; i < startYX[1] + hw[1]; i++)
                {
                    output[county,countx] = target[county,countx] * src[j,i];
                    countx++;
                }
                county++;
            }
            return output;
        }


        /// <summary>
        ///  Elementwise dot for particular region. output is same size with target mask
        /// </summary>
        /// <param name="this"></param>
        /// <param name="target"></param>
        /// <param name="startYX">Index of [Y,X] </param>
        /// <param name="hw">Size of [Width,Height]</param>
        /// <returns></returns>
        public static int[,] PartialDot(
          this int[,] src,
          int[,] target,
          int[] startYX,
          int[] hw)
        {
            int[,] output = new int[target.GetLength(0), target.GetLength(1)];

            int county = 0, countx = 0;

            for (int j = startYX[0]; j < startYX[0] + hw[0]; j++)
            {
                countx = 0;
                for (int i = startYX[1]; i < startYX[1] + hw[1]; i++)
                {
                    output[county, countx] = target[county, countx] * src[j, i];
                    countx++;
                }
                county++;
            }
            return output;
        }

        #endregion  

    }
}
