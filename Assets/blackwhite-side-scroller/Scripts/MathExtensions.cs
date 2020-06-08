using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.blackwhite_side_scroller
{
    public static class MathExtensions
    {
        public static float ClosestTo(this IEnumerable<float> collection, float target)
        {
            return collection.OrderBy(x => Math.Abs(target - x)).First();
        }
    }
}
