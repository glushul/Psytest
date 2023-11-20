using Psytest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psytest.Instruments
{
    public class PointCounter
    {
        // Forgot to use Option<int>
        public static Dictionary<int, int> questionAnswerPairs = new Dictionary<int, int>();
        public static Dictionary<int, int> categoryPointCountPairs = new Dictionary<int, int>();
    }
}
