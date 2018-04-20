using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyCoding
{
    /// <summary>
    /// Extension Method for IO, File Manipulation
    /// </summary>
    public static class IO
    {
        /// <summary>
        /// Get file name with extension. file.exe
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns> string : File name</returns>
        public static string TrimBaseName( this string filepath )
        {
            return Path.GetFileName( filepath );
        }

        /// <summary>
        /// Get only file name without extension 
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns>stirng : filename without extension </returns>
        public static string TrimFileNameOnly( this string filepath )
        {
            return Path.GetFileName( filepath )
                       .Split( '.' )
                       .First();
        }

        /// <summary>
        /// Get full path directory 
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns>string : full directory path</returns>
        public static string TrimDirPath( this string filepath )
        {
            return Path.GetDirectoryName( filepath );
        }

        /// <summary>
        /// Check directory is exist or not 
        /// and if direcotry is not exist create directory.
        /// </summary>
        /// <param name="dirpath"></param>
        /// <returns>string : input </returns>
		public static string CheckAndCreateDir( this string dirpath )
		{
			return Directory.Exists( dirpath ) 
				? dirpath 
				: dirpath.Act( x => Directory.CreateDirectory( x ) );
		}


        /// <summary>
        /// Check file is exist or not 
        /// and if file is not exist create file.
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns>string : input</returns>
        public static string CheckAndCreateFile(this string filepath )
		{
			return File.Exists( filepath ) 
				? filepath 
				: filepath.Act( x => File.Create( x ).Close() );
		}

        #region collection to csv

        /// <summary>
        /// Convert collection to csv string  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToCsv<T>(
        this IEnumerable<T> src) where T : IFormattable
        {
            try
            {
                var sb = new StringBuilder();
                foreach (var item in src)
                {
                    sb.Append(item);
                    sb.Append(',');
                }
                return sb.ToString();            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "";
            }
        }

        /// <summary>
        /// Convert collection of collection to csv string  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToCsv<T>(
    this IEnumerable<IEnumerable<T>> src) where T : IFormattable
        {
            try
            {
                var sb = new StringBuilder();
                foreach (var items in src)
                {
                    foreach (var item in items)
                    {
                        sb.Append(item);
                        sb.Append(',');
                    }
                    sb.Append(Environment.NewLine);

                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "";
            }
        }

        /// <summary>
        /// Convert 2Darray to csv string  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ToCsv<T>(
     this T[,] src) where T : IFormattable
        {
            var sb = new StringBuilder();
            try
            {
                for (int j = 0; j < src.GetLength(0); j++)
                {
                    for (int i = 0; i < src.GetLength(1); i++)
                    {
                        sb.Append(src[j, i]);
                        sb.Append(',');
                    }
                    sb.Append(Environment.NewLine);
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "";
            }
        }




        /// <summary>
        /// Convert collection to csv string and save  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="path"></param>
        /// <returns></returns>
		public static bool ToCsvSave<T>(
            this IEnumerable<T>  src ,
            string path ) where T : IFormattable
        {
            try
            {
                var sb = new StringBuilder();
                foreach ( var item in src )
                {
                    sb.Append( item );
                    sb.Append( ',' );
                }
                File.WriteAllText( path , sb.ToString() );
                return true;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
                return false;
            }
        }

        /// <summary>
        /// Convert collection of collection to csv string and save  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool ToCsvSave<T>(
      this IEnumerable<IEnumerable<T>> src ,
      string path ) where T : IFormattable
		{
            try
            {
                var sb = new StringBuilder();
                foreach ( var items in src )
                {
                    foreach ( var item in items )
                    {
                        sb.Append( item );
                        sb.Append( ',' );
                    }
                    sb.Append( Environment.NewLine );
                   
                }
                File.WriteAllText( path , sb.ToString() );

                return true;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
                return false;
            }
        }


        /// <summary>
        /// Convert 2Darray to csv string and save  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool ToCsvSave<T>(
      this T [,] src ,
      string path ) where T : IFormattable
		{
            var sb = new StringBuilder();
            try
            {
                for ( int j = 0 ; j < src.GetLength(0) ; j++ )
                {
                    for ( int i = 0 ; i < src.GetLength(1) ; i++ )
                    {
                        sb.Append( src[j,i] );
                        sb.Append( ',' );
                    }
                    sb.Append( Environment.NewLine );
                }
                return true;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
                return false;
            }
        }


        #endregion

    }
}
