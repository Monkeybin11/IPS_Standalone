using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SpeedyCoding
{
    /// <summary>
    /// Extension Method of higher order function ( Functor ).
    /// Not only including ordinary higher order function, but also including apecific propose higher order function.
    /// </summary>
    public static class HigherOrderFunction
    {
        /// <summary>
        /// Signature : [(A -> B -> A)] -> A -> A
        /// This is Special version of foldl. 
        /// like applicative functor, elements of function collection are unwrapped and 
        /// results of each function are folded  
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <param name="funList"></param>
        /// <param name="first"></param>
        /// <returns></returns>
        public static A FoldL<A>(
            this IEnumerable<Func<A, A>> funList, A first)
        {
            A output = default(A);
            foreach (var item in funList)
            {
                output = item(first);
            }
            return output;
        }


        /// <summary>
        /// Signature : (A -> B -> A) -> A -> [B]-> A
        /// Foldl extension method
        /// </summary>
        /// <param name="fn">(A -> B -> A)</param>
        /// <param name="src">A</param>
        /// <param name="xs">[B]</param>
        /// <returns></returns>
        public static A Foldl<A, B>
            (this Func<A, B, A> fn,
            A src,
            IEnumerable<B> xs)
        {
            if (xs.Count() == 0) return src;
            return fn.Foldl(fn(src, xs.First()), xs.Skip(1));
        }

        /// <summary>
        /// Signature : (B -> A -> A) -> A  -> [B] -> A
        /// Foldr extension method  
        /// </summary>
        /// <param name="fn">(B -> A -> A)</param>
        /// <param name="src">A</param>
        /// <param name="xs">[B]</param>
        /// <returns></returns>
        public static A Foldr<A, B>
            (this Func<B ,A, A> fn,
            A src,
            IEnumerable<B> xs)
        {
            if (xs.Count() == 0) return src;
            return fn(xs.First(), fn.Foldr(src, xs.Skip(1) ));
        }

        /// <summary>
        /// Signature : (A -> B) -> [A] -> [B]
        /// Common synonyms : Lift , fMap 
        /// </summary>
        /// <param name="src"> [A]</param>
        /// <param name="fn"> (A -> B) </param>
        /// <returns> Type of [B] </returns>
        public static B Map<A, B>(
          this A src,
          Func<A, B> fn)
          => fn(src);

        /// <summary>
        /// Signature : (A -> B) -> [[A]] -> [[B]] 
        /// Special purpose H.O.F 
        /// Apply function to element that Array of Array 
        /// </summary>
        /// <param name="src"> [[A]] </param>
        /// <param name="fn"> (A -> B) </param>
        /// <returns>[[B]]</returns>
        public static IEnumerable<B[]> MapLoop<A, B>(
           this IEnumerable<IEnumerable<A>> src
           , Func<A, B> fn)
        {
            return src.Select(row =>
                               row.Select(col =>
                                          fn(col))
                                  .ToArray())
                      .ToArray();
        }

        public static B[][][] MapLoop<A, B>(
           this A[][][] src
           , Func<A, B> fn)
        {
            return src.Select(first =>
                               first.Select(second =>
                                              second.Select(thrid =>
                                                     fn(thrid))
                                                    .ToArray())

                                  .ToArray())
                      .ToArray();
        }

        /// <summary>
        /// Signature : (A -> B) -> 2DArray<A> -> 2DArray<B> 
        /// Special purpose H.O.F 
        /// </summary>
        /// <param name="src"> 2DArray<A> </param>
        /// <param name="fn"> (A -> B) </param>
        /// <returns>2DArray<B></returns>
        public static B[,] MapLoop<A, B>(
           this A[,] src
           , Func<A, B> fn)
        {
            int row = src.GetLength(0);
            int col = src.GetLength(1);
            B[,] result = new B[row, col];

            for (int j = 0; j < row; j++)
            {
                for (int i = 0; i < col; i++)
                {
                    result[j, i] = fn(src[j, i]);
                }
            }
            return result;
        }
        /// <summary>
        /// Signature : (A -> B) -> 3DArray<A> -> 3DArray<B> 
        /// Special purpose H.O.F 
        /// </summary>
        /// <param name="src">3DArray<A></param>
        /// <param name="fn"> (A -> B) </param>
        /// <returns>3DArray<B></returns>
        public static B[,,] MapLoop<A, B>(
           this A[,,] src
           , Func<A, B> fn)
        {
            int first = src.GetLength(0);
            int second = src.GetLength(1);
            int third = src.GetLength(2);
            B[,,] result = new B[first, second, third];

            for (int j = 0; j < first; j++)
            {
                for (int i = 0; i < second; i++)
                {
                    for (int k = 0; k < third; k++)
                    {
                        result[j, i, k] = fn(src[j, i, k]);
                    }
                }
            }
            return result;
        }



        /// <summary>
        ///  Signature : ( A -> () ) -> A -> A
        ///  Special purpose H.O.F for Action
        ///  Func can't take void type like 'Func<A,void>' because of Poor supporting type system
        ///  This Extension method execute 'Action' delegate and return input value itself 
        /// </summary>
        /// <param name="src"> A </param>
        /// <param name="action"> ( A => () )  </param>
        /// <returns></returns>
        public static A Act<A>(
            this A src,
            Action<A> action)
        {
            action(src);
            return src;
        }

        /// <summary>
        ///  Signature : ( A -> () ) -> [A] -> [A]
        ///  Special purpose H.O.F for Action
        ///  Func can't take void type like 'Func<A,void>' because of Poor supporting type system
        ///  This Extension method execute 'Action' delegate on each element of [a] and return input value itself 
        /// </summary>
        /// <param name="src"> [A] </param>
        /// <param name="action"> ( A => () )</param>
        /// <returns>A</returns>
        public static IEnumerable<A> ActLoop<A>(
            this IEnumerable<A> src
            , Action<A> action)
        {
            foreach (var item in src)
            {
                action(item);
            }
            return src;
        }


        /// <summary>
        ///  Signature : ( (A, int) -> () ) -> [A] -> [A]
        ///  Special purpose H.O.F for Action
        ///  Func can't take void type like 'Func<A,void>' because of Poor supporting type system
        ///  This Extension method execute 'Action' delegate on each element of [a] with counter 'i' and return input value itself 
        /// </summary>
        /// <param name="src"> [A] </param>
        /// <param name="action">( A => () )</param>
        /// <returns>[A]</returns>
        public static IEnumerable<A> ActLoop<A>(
            this IEnumerable<A> src
            , Action<A, int> action)
        {
            for (int i = 0; i < src.Count(); i++)
            {
                action(src.ElementAt(i), i);
            }
            return src;
        }


        /// <summary>
        ///  Signature : ( (A, int) -> () ) -> [[A]] -> [[A]]
        ///  Special purpose H.O.F for Action
        ///  Func can't take void type like 'Func<A,void>' because of Poor supporting type system
        ///  This Extension method execute 'Action' delegate on each element of [a] and return input value itself 
        /// </summary>
        /// <param name="src">[[A]]</param>
        /// <param name="action">( A => () )</param>
        /// <returns> [[A]] </returns>
        public static IEnumerable<IEnumerable<A>> ActLoop<A>(
            this IEnumerable<IEnumerable<A>> src,
            Action<A> action)
        {
            foreach (var items in src)
            {
                foreach (var item in items)
                {
                    action(item);
                }
            }
            return src;
        }




        /// <summary>
        ///  Signature : ( (A, int) -> () ) -> [[[A]]] -> [[[A]]]
        ///  Special purpose H.O.F for Action
        ///  Func can't take void type like 'Func<A,void>' because of Poor supporting type system
        ///  This Extension method execute 'Action' delegate on each element of [a] and return input value itself 
        /// </summary>
        /// <param name="src">[[[A]]]</param>
        /// <param name="action">( A => () )</param>
        /// <returns> [[[A]]] </returns>
        public static IEnumerable<IEnumerable<IEnumerable<A>>> ActLoop<A>(
              this IEnumerable<IEnumerable<IEnumerable<A>>> src,
              Action<A> action)
        {
            foreach (var items in src)
            {
                foreach (var item in items)
                {
                    foreach (var element in item)
                    {
                        action(element);
                    }
                }
            }
            return src;
        }

        /// <summary>
        /// Signature : (A -> ()) -> 2DArray<A> -> 2DArray<A>
        /// Special purpose H.O.F 
        /// </summary>
        /// <param name="src">2DArray<A>]</param>
        /// <param name="action">( A => () )</param>
        /// <returns>2DArray<A> </returns>
        public static T[,] ActLoop<T>(
            this T[,] src
            , Action<T> action)
            where T : struct
        {
            for (int j = 0; j < src.GetLength(0); j++)
            {
                for (int i = 0; i < src.GetLength(1); i++)
                {
                    action(src[j, i]);
                }
            }
            return src;
        }
        /// <summary>
        /// Signature : (A -> ()) -> 3DArray<A> -> 3DArray<A>
        /// Special purpose H.O.F 
        /// </summary>
        /// <param name="src">3DArray<A>]</param>
        /// <param name="action">( A => () )</param>
        /// <returns>3DArray<A> </returns>
        public static T[,,] ActLoop<T>(
            this T[,,] src
            , Action<T> action)
            where T : struct
        {
            for (int j = 0; j < src.GetLength(0); j++)
            {
                for (int i = 0; i < src.GetLength(1); i++)
                {
                    for (int k = 0; k < src.GetLength(0); k++)
                    {
                        action(src[j, i, k]);
                    }
                }
            }
            return src;
        }


        /// <summary>
        /// Signature : (A -> ()) -> A -> string -> int -> A
        /// Special purpose H.O.F 
        /// This H.O.F is just for measuring running time of action 
        /// </summary>
        /// <param name="src">input of action</param>
        /// <param name="msg">header of string of measurement result  </param>
        /// <param name="iter"> count of repeating action  </param>
        /// <param name="fn">action</param>
        /// <returns>A</returns>
        public static A Measure_Act<A>(
                this A src,
                string msg,
                int iter,
                Action<A> fn)
        {
            Stopwatch stw = new Stopwatch();
            stw.Start();
            for (int i = 0; i < iter; i++)
            {
                fn(src);
            }
            stw.Stop();
            Console.WriteLine(msg + $"{stw.ElapsedMilliseconds / 1.0}");
            return src;
        }

        /// <summary>
        /// Signature : (A -> B) -> A -> string -> int -> B
        /// Special purpose H.O.F 
        /// This H.O.F is just for measuring running time of action 
        /// </summary>
        /// <param name="src">input of func</param>
        /// <param name="msg">header of string of measurement result  </param>
        /// <param name="iter"> count of repeating func  </param>
        /// <param name="fn">func</param>
        /// <returns>B</returns>
        public static B Measure_Map<A, B>(
                this A src,
                string msg,
                int iter,
                Func<A, B> fn)
        {
            Stopwatch stw = new Stopwatch();
            stw.Start();
            for (int i = 0; i < iter; i++)
            {
                fn(src);
            }
            stw.Stop();
            Console.WriteLine(msg + $"{stw.ElapsedMilliseconds / 1.0}");
            return fn(src);
        }

        /// <summary>
        /// Signature : (A -> B -> C) -> A -> B -> string -> int -> C
        /// Special purpose H.O.F 
        /// This H.O.F is just for measuring running time of action  
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <typeparam name="C"></typeparam>
        /// <param name="src">type of A input of func</param>
        /// <param name="src2">type of B input of func</param>
        /// <param name="msg">header of string of measurement result  </param>
        /// <param name="iter"> count of repeating func  </param>
        /// <param name="fn">func</param>
        /// <returns></returns>
        public static C Measure<A, B, C>(
            this A src,
             B src2,
            string msg,
            int iter,
            Func<A, B, C> fn)
        {
            Stopwatch stw = new Stopwatch();
            stw.Start();
            for (int i = 0; i < iter; i++)
            {
                fn(src, src2);
            }
            stw.Stop();
            Console.WriteLine($"{stw.ElapsedMilliseconds / 1.0}" + msg);
            return fn(src, src2);
        }
    }
}


