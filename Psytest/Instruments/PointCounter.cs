using System.Collections.Generic;

namespace Psytest.Instruments
{
    public class PointCounter
    {
        //Словарь для учета информации о выбранных ответах студента
        public static Dictionary<int, int> questionAnswerPairs = new Dictionary<int, int>();

        //Словарь для учета информации о количестве баллов по категориям 
        public static Dictionary<int, int> categoryPointCountPairs = new Dictionary<int, int>();
    }
}
