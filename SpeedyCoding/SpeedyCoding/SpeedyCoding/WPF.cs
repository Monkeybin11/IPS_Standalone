using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SpeedyCoding
{
    /// <summary>
    /// Extension Method for WPF 
    /// </summary>
    public static class WPF
    {
        /// <summary>
        /// Extension method of (Grid.SetRow , Grid.SetColumn)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="row">row position</param>
        /// <param name="col">column position</param>
        /// <returns>src</returns>
        public static T SetGridPos<T>(
            this T src
            , int row
            , int col )
            where T : Control
        {
            Grid.SetRow( src , row );
            Grid.SetColumn( src , col );
            return src;
        }


        /// <summary>
        /// Extension method of (Canvas.SetTop, Canvas.SetLeft)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <param name="top">Top length</param>
        /// <param name="left">Left length</param>
        /// <returns>src</returns>
        public static T SetCanvasPos<T>(
            this T src
            , int top
            , int left)
            where T : Control
        {
            Canvas.SetTop(src, top);
            Canvas.SetLeft(src, left);
            return src;
        }

    }
}
