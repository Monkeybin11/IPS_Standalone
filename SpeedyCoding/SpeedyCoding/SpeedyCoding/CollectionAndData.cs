using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyCoding
{
    /// <summary>
    /// Extension Method for Collection and Data
    /// </summary>
    public static class CollectionAndData
    {
        #region Length

        /// <summary>
        /// Generalized Length Counter 
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <param name="src">[A]</param>
        /// <returns></returns>
        public static int Len<A>(
          this IEnumerable<A> src)
        {
            return src.Count();
        }


        /// <summary>
        /// Generalized Length Counter 
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <param name="src">[A]</param>
        /// <returns></returns>
        public static int Len<A>(
            this IList<A> src)
        {
            return src.Count();
        }

        /// <summary>
        /// Generalized Length Counter 
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <param name="src">[A]</param>
        /// <returns></returns>
        public static int Len<A>(
            this ICollection<A> src)
        {
            return src.Count();
        }



        /// <summary>
        /// Generalized Length Counter 
        /// </summary>
        /// <typeparam name="A">[A]</typeparam>
        /// <param name="src"></param>
        /// <param name="order"> Target of length dimentsion </param>
        /// <returns></returns>
        public static int Len<A>(
           this A[] src,
           int order = 0)
        {
            return src.GetLength(0);
        }

        /// <summary>
        /// Generalized Length Counter 
        /// </summary>
        /// <typeparam name="A">[[A]]</typeparam>
        /// <param name="src"></param>
        /// <param name="order"> Target of length dimentsion </param>
        /// <returns></returns>
        public static int Len<A>(
          this A[][] src,
          int order = 0)
        {
            if (order == 0) return src.GetLength(0);
            if (order == 1) return src[0].GetLength(0);
            else return src[0].GetLength(0);
        }

        /// <summary>
        /// Generalized Length Counter 
        /// </summary>
        /// <typeparam name="A">[[[A]]]</typeparam>
        /// <param name="src"></param>
        /// <param name="order"> Target of length dimentsion </param>
        /// <returns></returns>
        public static int Len<A>(
          this A[][][] src,
          int order = 0)
        {
            if (order == 0) return src.GetLength(0);
            if (order == 1) return src[0].GetLength(0);
            if (order == 2) return src[0][0].GetLength(0);
            else return src[0][0].GetLength(0);
        }

        /// <summary>
        /// Generalized Length Counter 
        /// </summary>
        /// <typeparam name="A">2DArray</typeparam>
        /// <param name="src"></param>
        /// <param name="order"> Target of length dimentsion </param>
        /// <returns></returns>
        public static int Len<A>(
        this A[,] src,
        int order = 0)
        {
            if (order == 0) return src.GetLength(0);
            if (order == 1) return src.GetLength(1);
            else return src.GetLength(0);
        }

        /// <summary>
        /// Generalized Length Counter 
        /// </summary>
        /// <typeparam name="A">3DArray</typeparam>
        /// <param name="src"></param>
        /// <param name="order"> Target of length dimentsion </param>
        /// <returns></returns>
        public static int Len<A>(
          this A[,,] src,
          int order = 0)
        {
            if (order == 0) return src.GetLength(0);
            if (order == 1) return src.GetLength(1);
            if (order == 2) return src.GetLength(2);
            else return src.GetLength(0);
        }
        #endregion

        #region Collection And Data Manipulation


        /// <summary>
        /// Adding data to dictionary 
        /// Use like this
        /// dictionary.append(key,value)
        ///           .append(key,value)
        /// </summary>
        /// <typeparam name="Tk">Key</typeparam>
        /// <typeparam name="Tv">Value</typeparam>
        /// <param name="src"></param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public static Dictionary<Tk, Tv> Append<Tk, Tv>(
        this Dictionary<Tk, Tv> src,
        Tk key,
        Tv value)
        {
            src.Add(key, value);
            return src;
        }

        /// <summary>
        /// Adding data,key collection to dictionary 
        /// Use like this
        /// dictionary.append([key],[value])
        ///           
        /// </summary>
        /// <typeparam name="Tk"></typeparam>
        /// <typeparam name="Tv"></typeparam>
        /// <param name="src"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<Tk, Tv> Append<Tk, Tv>(
       this Dictionary<Tk, Tv> src,
       IEnumerable<Tk> key,
       IEnumerable<Tv> value)
        {
            var keycount = key.Count();
            if (keycount != value.Count()) return null;
            for (int i = 0; i < keycount; i++)
            {
                src.Add(key.ElementAt(i), value.ElementAt(i));
            }
            return src;
        }



        /// <summary>
        /// Adding data to List
        /// Use like this
        /// list.append(value)
        ///     .append(value)
        /// </summary>
        /// <typeparam name="Tv">Value</typeparam>
        /// <param name="src"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<Tv> Append<Tv>(
            this List<Tv> src,
            Tv value)
        {
            src.Add(value);
            return src;
        }

        /// <summary>
		/// Index of element that satisfy condition
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="src"></param>
		/// <param name="cond"></param>
		/// <returns>'List<int> Index' that satisfy condition</returns>
        public static List<int> IndicesOf<T>(
           this IEnumerable<T> src,
           Func<T, bool> cond)
        {
            var reslist = src.Select(x => cond(x) ? 0 : 1);
            var output = new List<int>();

            if (reslist.First() == 0) output.Add(0);
            reslist.Aggregate((f, s) => s != 0
                                           ? f + s
                                           : f + 1.Act(x => output.Add(f)));
            return output;
        }


        /// <summary>
        /// Select element from last 
        /// last index is 0 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="idxfromlast"> Index </param>
        /// <returns>T</returns>
        public static T FromLast<T>
            (this IEnumerable<T> src, int idxfromlast)
        {
            var count = src.Count();
            return src.ElementAt(count - 1 - idxfromlast);
        }

        /// <summary>
        /// This extension method is safer version of selecting element from collection 
        /// Pick only element that in range for prevent outofindex exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static T Pick<T>(
            this T[][] self,
            int row,
            int col)
        {
            var rowlimit = self.Length;
            var collimit = self[0].Length;

            if (row >= rowlimit || col >= collimit)
            {
                return default(T);
            }
            return self[row][col];
        }




        /// <summary>
        /// This extension method is safer version of selecting element from collection 
        /// Pick only element that in range for prevent outofindex exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static T Pick<T>(
            this T[,] self,
            int row,
            int col)
        {
            var rowlimit = self.GetLength(0);
            var collimit = self.GetLength(1);

            if (row >= rowlimit || col >= collimit)
            {
                return default(T);
            }
            return self[row, col];
        }




        /// <summary>
        /// This extension method is safer version of selecting element from collection 
        /// Pick only element that in range for prevent outofindex exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="self"> Index array int[row,col]</param>
        /// <returns></returns>
        public static T Pick<T>(
            this T[][] self,
            int[] rowcol)
        {
            var rowlimit = self.Length;
            var collimit = self[0].Length;

            if (rowcol[0] >= rowlimit
                || rowcol[1] >= collimit
                || rowcol[0] < 0
                || rowcol[1] < 0)
            {
                return default(T);
            }
            var temp = self[rowcol[0]][rowcol[1]];
            return self[rowcol[0]][rowcol[1]];
        }



        /// <summary>
        /// This extension method is safer version of selecting element from collection 
        /// Pick only element that in range for prevent outofindex exception
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"> Index array int[row,col]</param>
        /// <param name="rowcol"></param>
        /// <returns></returns>
        public static T Pick<T>(
            this T[,] self,
            int[] rowcol)
        {
            var rowlimit = self.GetLength(0);
            var collimit = self.GetLength(1);

            if (rowcol[0] >= rowlimit || rowcol[1] >= collimit)
            {
                return default(T);
            }
            return self[rowcol[0], rowcol[1]];
        }



        #endregion

        #region Sequence Generator

        /// <summary>
        /// Generate [int] ( IEnuemrable<int> )
        /// </summary>
        /// <param name="start">Start Number</param>
        /// <param name="count">Element Count</param>
        /// <returns>[int]</returns>
        public static IEnumerable<int> xRange(
        this int start,
        int count)
        {
            return Enumerable.Range(start, count);
        }



        /// <summary>
        /// Generate [double] ( IEnuemrable<double> ) with selected interval
        /// </summary>
        /// <param name="start">Start Number</param>
        /// <param name="count">Element Count</param>
        /// <param name="step">Interval</param>
        /// <returns>[double]</returns>
        public static IEnumerable<double> xRange(
            this double start,
            int count,
            double step)
        {
            List<double> output = new List<double>();
            for (int i = 0; i < count; i++)
            {
                output.Add(start + i * step);
            }
            return output;
        }

        /// <summary>
        /// Generate [int] ( IEnuemrable<int> ) with selected interval
        /// </summary>
        /// <param name="start">Start Number</param>
        /// <param name="count">Element Count</param>
        /// <param name="step">Interval</param>
        /// <returns>[int]</returns>
        public static IEnumerable<int> xRange(
            this int start,
            int count,
            int step)
        {
            return Enumerable.Range(start, count).Select(x => x * step);
        }
        #endregion

        #region Data
        /// <summary>
        /// Extension method for creating tuple
        /// Ex) a.ToTuple(b)
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="a">Item1</param>
        /// <param name="b">Item2</param>
        /// <returns></returns>
        public static Tuple<A, B> ToTuple<A, B>(this A a, B b)
        { return Tuple.Create(a, b); }


        #endregion
    }
}
