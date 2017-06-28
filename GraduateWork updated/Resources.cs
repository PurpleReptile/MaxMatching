using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduateWork_updated
{
    static public class MyResources
    {
        // labels types of algorithms 
        public const string lblMaxMatching = "Максимальное паросочетание";
        public const string lblNumberMatching = "Кол-во паросочетаний";

        // labels algorithms 
        public const string lblSingleThreadAlgorithm = "Однопоточный";
        public const string lblMultiThreadAlgorithm = "Многопоточный";

        public const string lblFordFulkersonAlgorithm = "Форд-Фалкерсон";

        // labels types of matrix
        public const string lblReducedMatrix = "приведённая матрица смежности";
        public const string lblAdjacencyMatrix = "матрица смежности";

        // labels size of reduce matrix
        public const int minSizeOfReduceMatrix = 2;
        public const int maxSizeOfReduceMatrix = 10;

        // labels size of adjacency matrix
        public const int minSizeOfAdjacencyMatrix = 6;
        public const int maxSizeOfAdjacencyMatrix = 18;

        public static int reference { get; set; }
    }
}
