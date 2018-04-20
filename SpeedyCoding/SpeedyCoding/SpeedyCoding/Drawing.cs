using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SpeedyCoding
{

    /// <summary>
    /// Extension Method for system.Drawing
    /// </summary>
    public static class Drawing
    {

        /// <summary>
        /// Expend rectangle size. 
        /// top   = top   + margin
        /// bot   = bot   + margin
        /// left  = left  + margin
        /// right = right + margin
        /// </summary>
        /// <param name="this"></param>
        /// <param name="margin"></param>
        /// <returns></returns>
        public static System.Drawing.Rectangle ExpendRect(
          this System.Drawing.Rectangle @this ,
          int margin
          )
        {
            return new System.Drawing.Rectangle(
                @this.X - margin
                , @this.Y - margin
                , @this.Width + margin * 2
                , @this.Height + margin * 2 );
        }




        /// <summary>
        /// Shurink rectangle size. 
        /// top   = top   - margin
        /// bot   = bot   - margin
        /// left  = left  - margin
        /// right = right - margin 
        /// </summary>
        /// <param name="this"></param>
        /// <param name="margin"></param>
        /// <returns></returns>
        public static System.Drawing.Rectangle ShurinkRect(
            this System.Drawing.Rectangle @this ,
            int margin
            )
        {
            return new System.Drawing.Rectangle(
                @this.X + margin
                , @this.Y + margin
                , @this.Width - margin * 2
                , @this.Height - margin * 2 );
        }
    }
}
