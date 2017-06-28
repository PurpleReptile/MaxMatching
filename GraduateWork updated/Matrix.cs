using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Windows.Controls;
using System.Windows;
using System.Xml.Linq;
using System.Collections;
using System.Threading;


namespace GraduateWork_updated
{
    public class Matrix
    {
        /*************** BEGIN declaration of variables ***************/
        public int[,] matrix;
        public int rows { get; set; }
        public int columns { get; set; }
        public int[,] original_matrix;
        public int original_rows { get; set; }
        public int original_columns { get; set; }

        public string typeMatrix { get; set; }

        // information about file
        public string fullNameOpenFile { get; set; }
        public string nameOpenFile { get; set; }
        public string typeOpenFile { get; set; }
        public string pathOpenFile { get; set; }
        XmlDocument FileXML;
        /*************** END declaration of variables ***************/


        /*************** BEGIN initialization constructors ***************/
        public Matrix(int rows, int columns, bool isZeroMatrix, bool isAdjacencyMatrix)
        {
            matrix = new int[rows, columns];
            this.rows = rows;
            this.columns = columns;

            if (isZeroMatrix || isAdjacencyMatrix)
            {
                if (isZeroMatrix)
                    fill_zero_matrix();
                else
                    if (isAdjacencyMatrix)
                    fill_random_matrix_adjacency();
            }
            else
                fill_random_matrix();
        }
        public Matrix(string fileName)
        {
            fullNameOpenFile = Path.GetFileName(fileName);
            nameOpenFile = Path.GetFileNameWithoutExtension(fileName);
            typeOpenFile = Path.GetExtension(fileName);
            pathOpenFile = Path.GetDirectoryName(fileName);

            if (File.Exists(fileName))
            {
                switch (typeOpenFile)
                {
                    // read file format XML 
                    case ".xml":
                        read_format_xml(fileName);
                        break;

                    // read file format TXT
                    case ".txt":
                        StreamReader FileTXT = new StreamReader(pathOpenFile + "\\" + fullNameOpenFile, Encoding.Default);
                        read_format_txt(FileTXT);
                        break;
                }
            }
        }
        /*************** END initialization constructors ***************/


        /*************** BEGIN functions read matrix from file ***************/
        public void read_format_xml(string fileName)
        {
            int tempInt;
            int row, column;

            FileXML = new XmlDocument();
            FileXML.Load(fileName);

            typeMatrix = FileXML.GetElementsByTagName("матрица")[0].Attributes["тип"].Value;
            XmlNode xmlNode = FileXML.GetElementsByTagName("матрица")[0];

            typeMatrix = xmlNode.Attributes["тип"].Value;
            XmlNodeList xmlNodeList = xmlNode.ChildNodes;

            rows = Convert.ToInt32(xmlNode.LastChild.Attributes["строка"].Value) + 1;
            columns = Convert.ToInt32(xmlNode.LastChild.Attributes["столбец"].Value) + 1;

            matrix = new int[rows, columns];

            foreach (XmlNode matrixElement in xmlNodeList)
            {
                tempInt = Convert.ToInt32(matrixElement.InnerText);
                row = Convert.ToInt32(matrixElement.Attributes["строка"].Value);
                column = Convert.ToInt32(matrixElement.Attributes["столбец"].Value);

                matrix[row, column] = tempInt;
            }
        }
        public void read_format_txt(StreamReader FileTXT)
        {
            string currString;
            int row, column;

            typeMatrix = FileTXT.ReadLine();
            currString = FileTXT.ReadLine();

            var currElement = currString.Split(' ');

            rows = Convert.ToInt32(currElement[0]);
            columns = Convert.ToInt32(currElement[1]);
            matrix = new int[rows, columns];

            for (row = 0; row < rows; row++)
            {
                currString = FileTXT.ReadLine();
                currElement = currString.Split(' ');

                for (column = 0; column < columns; column++)
                {
                    matrix[row, column] = Convert.ToInt32(currElement[column]);
                }
            }

            FileTXT.Close();
        }
        /*************** END functions read matrix from file ***************/

        public bool convert_adjacency_to_reduce_matrix()
        {
            int col_matrix;

            col_matrix = 0;

            if ((object)original_matrix != null)
                return true;

            // check matrix is bipartite
            if (is_bipartite())
            {
                // adjacency matrix values
                original_matrix = (int[,])matrix.Clone();
                original_rows = rows;
                original_columns = columns;

                matrix = new int[rows / 2, columns / 2];

                if (rows == columns)
                {
                    for (int row = 0; row < rows / 2; row++)
                        for (int col = columns / 2; col < columns; col++)
                        {
                            matrix[row, col_matrix] = original_matrix[row, col];

                            if (col_matrix == (columns / 2 - 1))
                                col_matrix = 0;
                            else
                                col_matrix++;
                        }

                    // new size matrix 
                    rows /= 2;
                    columns /= 2;
                    return true;
                }
            }

            return false;
        }

        // check correct matrix data
        public bool is_bipartite()
        {
            int u;
            int[] colorArray;
            int vertex;

            vertex = 0;
            colorArray = new int[columns];
            Queue queue = new Queue();

            for (int i = 0; i < columns; ++i)
                colorArray[i] = -1;

            colorArray[vertex] = 1;
            queue.Enqueue(vertex);

            while (queue.Count > 0)
            {
                u = 0;

                queue.Dequeue();
                queue.TrimToSize();

                for (int v = 0; v < columns; ++v)
                {
                    if ((matrix[u, v] > 0) && (colorArray[v] == -1))
                    {
                        colorArray[v] = 1 - colorArray[u];
                        queue.Enqueue(v);
                    }

                    else
                        if ((matrix[u, v] > 0) && (colorArray[v] == colorArray[u]))
                        return false;
                }
            }

            return true;
        }
        public bool is_empty_matrix()
        {
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    if (matrix[row, column] == 1)
                        return false;
                }
            }

            return true;
        }
        public bool is_asymmetrical()
        {
            for (int row = 0; row < rows / 2; row++)
            {
                for (int column = columns / 2; column < columns; column++)
                {
                    if (matrix[row, column] != matrix[column, row])
                        return true;
                }
            }

            return false;
        }


        /*************** BEGIN get/set element matrix ***************/
        public void get_matrix_view(Grid gridMatrixContent, Main myWindow)
        {
            RowDefinition rowd;
            ColumnDefinition cold;
            CheckBox checkB;
            Label lblCountList;
            Border borderGridTotal = new Border();

            gridMatrixContent.Children.Clear();

            Grid.SetRow(borderGridTotal, 1);
            Grid.SetColumn(borderGridTotal, 1);
            Grid.SetColumnSpan(borderGridTotal, (int)columns);
            Grid.SetRowSpan(borderGridTotal, (int)rows);

            borderGridTotal.BorderBrush = System.Windows.Media.Brushes.Black;
            borderGridTotal.BorderThickness = new Thickness(1, 1, 0, 0);

            gridMatrixContent.Children.Add(borderGridTotal);

            for (int row = 0; row <= rows; row++)
            {
                rowd = new RowDefinition();
                rowd.Height = GridLength.Auto;
                gridMatrixContent.RowDefinitions.Add(rowd);

                for (int column = 0; column <= columns; column++)
                {
                    if ((column == 0) && (row != 0))
                    {
                        lblCountList = new Label();
                        lblCountList.Content = (row - 1).ToString();
                        lblCountList.Style = myWindow.FindResource("lblCountList") as Style;

                        Grid.SetColumn(lblCountList, 0);
                        Grid.SetRow(lblCountList, row);
                        gridMatrixContent.Children.Add(lblCountList);
                    }

                    if (row == 0)
                    {
                        if (column != 0)
                        {
                            lblCountList = new Label();
                            lblCountList.Content = (column - 1).ToString();
                            lblCountList.Style = myWindow.FindResource("lblCountList") as Style;

                            Grid.SetColumn(lblCountList, column);
                            Grid.SetRow(lblCountList, 0);
                            gridMatrixContent.Children.Add(lblCountList);
                        }

                        cold = new ColumnDefinition();
                        cold.Width = GridLength.Auto;

                        gridMatrixContent.ColumnDefinitions.Add(cold);
                    }

                    else if (column != 0)
                    {
                        checkB = new CheckBox();
                        checkB.Style = myWindow.FindResource("CheckBoxStyle") as Style;

                        Grid.SetRow(checkB, row);
                        Grid.SetColumn(checkB, column);
                        checkB.Checked += new RoutedEventHandler(myWindow.CheckBox_Checked);
                        checkB.Unchecked += new RoutedEventHandler(myWindow.CheckBox_Unchecked);

                        checkB.IsChecked = Convert.ToBoolean(matrix[row - 1, column - 1]);

                        gridMatrixContent.Children.Add(checkB);
                    }
                }
            }
        }
        public void set_matrix_element(int row, int column, int value)
        {
            matrix[row, column] = value;
        }
        /*************** END get/set element matrix ***************/

        public int[,] fill_random_matrix()
        {
            Random randomValue = new Random();

            for (int row = 0; row < rows; row++)
                for (int column = 0; column < columns; column++)
                    matrix[row, column] = randomValue.Next(0, 2);
            return matrix;
        }
        public int[,] fill_random_matrix_adjacency()
        {
            Random randomValue = new Random();

            for (int row = 0; row < rows / 2; row++)
            {
                for (int column = columns / 2; column < columns; column++)
                {
                    matrix[row, column] = randomValue.Next(0, 2);
                    matrix[column, row] = matrix[row, column];
                }
            }

            //for (int row = 0; row < rows; row++)
            //    for (int column = 0; column < columns; column++)
            //        if (row == column)
            //            matrix[row, column] = 0;
            //        else
            //            if (row < column)
            //            {
            //                matrix[row, column] = randomValue.Next(0, 2);
            //                matrix[column, row] = matrix[row, column];
            //            }

            return matrix;
        }
        public int[,] fill_zero_matrix()
        {
            for (int row = 0; row < rows; row++)
                for (int column = 0; column < columns; column++)
                    matrix[row, column] = 0;
            return matrix;
        }
    }

    class solveGraph
    {
        /*************** BEGIN initialization variables ***************/

        public Matrix class_Matrix;
        public Errors class_Errors;

        public int rows, columns;
        protected int max;

        protected int[,] helpMatrix;
        protected Object lck = new Object();

        public List<int> maxMatchingPos;
        public List<List<sbyte>> ListOfVertices;
        protected List<sbyte> allColumns;

        public string TypeMatrix { get; set; }

        // result calculation labels
        string strCurrentAlgorithm { get; set; }
        string strNumberThreads { get; set; }
        string strComplexityAlgorithm { get; set; }
        string strCountOperations { get; set; }
        string strCountMatching { get; set; }
        string strCountMaxMatching { get; set; }
        string strCountPerfectMatching { get; set; }
        string strTimeSpent { get; set; }
        public string currTypeMatrix { get; set; }

        // result calculation value
        public string currTypeAlgorithm { get; set; }
        public string currAlgorithm { get; set; }
        public int countMaxMatching { get; set; }
        public int countPerfectMatching { get; set; }
        public int numberThreads { get; set; }
        public string strResult { get; set; }
        public string strTimeAlg { get; set; }
        public ulong countOperations { get; set; }
        public string complexityAlgorithm { get; set; }

        //public bool matrixIsEmpty { get; set; }
        public int[] arrVertices;

        /*************** END initialization variables ***************/


        /*************** BEGIN initialization constructors ***************/

        // constructor random and keyboard input
        public solveGraph(int rows, int columns, bool isZeroMatrix, bool isAdjacencyMatrix)
        {
            class_Matrix = new Matrix(rows, columns, isZeroMatrix, isAdjacencyMatrix);
            this.rows = class_Matrix.rows;
            this.columns = class_Matrix.columns;
            ListOfVertices = new List<List<sbyte>>();
        }

        // constructor read file
        public solveGraph(string fileName)
        {
            class_Matrix = new Matrix(fileName);
            rows = class_Matrix.rows;
            columns = class_Matrix.columns;
            ListOfVertices = new List<List<sbyte>>();
        }

        /*************** END initialization constructors ***************/


        public void solver(int currentNumberThreads, string currTypeAlgorithm, string currAlgorithm, string currTypeMatrix)
        {
            class_Errors = new Errors();

            this.currAlgorithm = currAlgorithm;
            this.currTypeAlgorithm = currTypeAlgorithm;
            this.currTypeMatrix = currTypeMatrix;

            numberThreads = currentNumberThreads;

            // check if matrix zero
            if (class_Matrix.is_empty_matrix())
            {
                class_Errors.matrixIsEmpty = true;
                return;
            }

            // check that the selected type of matrix is the adjacency matrix
            if (MyResources.lblAdjacencyMatrix == currTypeMatrix)
            {
                // checking data from a file
                if (class_Matrix.is_asymmetrical() || class_Matrix.is_bipartite() == false)
                {
                    class_Errors.matrixIsAsymmetrical = true;
                    return;
                }

                // convert adjacency matrix to reduce matrix
                if (class_Matrix.convert_adjacency_to_reduce_matrix())
                {
                    // update new size matrix
                    rows = class_Matrix.rows;
                    columns = class_Matrix.columns;
                }
            }

            switch (currTypeAlgorithm)
            {
                // if selected algorithm find max matching
                case MyResources.lblMaxMatching:
                    complexityAlgorithm = "O((M+N)*(M+N))";
                    counter_maxMatching();
                    break;

                // if selected algorithm count matchings
                case MyResources.lblNumberMatching:
                    complexityAlgorithm = "O(n!)";
                    counter_numberMatching(currentNumberThreads);
                    all_matching_finder();
                    break;
            }

        }


        /*************** BEGIN find all matching using algorithms ***************/

        // main function for singleThread/multiThread algorithms
        void counter_numberMatching(int currentNumberThreads)
        {
            List<Thread> listThreads;
            List<sbyte>[] allColumnsToProccess;
            List<sbyte>[] unUsedColumns;
            List<sbyte> resultPath;

            bool flag;
            sbyte j;

            // initialization variables
            helpMatrix = (int[,])class_Matrix.matrix.Clone();
            resultPath = new List<sbyte>();
            listThreads = new List<Thread>();

            if (columns < currentNumberThreads)
                currentNumberThreads = columns;

            allColumnsToProccess = new List<sbyte>[currentNumberThreads];
            unUsedColumns = new List<sbyte>[currentNumberThreads];
            allColumns = new List<sbyte>();
            j = 0;

            for (sbyte i = 0; i < columns; i++)
            {
                allColumns.Add(i);

                if (allColumnsToProccess[j] == null)
                    allColumnsToProccess[j] = new List<sbyte>();

                allColumnsToProccess[j].Add(i);

                if (j++ >= currentNumberThreads - 1)
                    j = 0;
            }

            for (sbyte j1 = 0; j1 < currentNumberThreads; j1++)
            {
                unUsedColumns[j1] = new List<sbyte>();

                for (sbyte i = 0; i < columns; i++)
                    unUsedColumns[j1].Add(i);
            }

            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            countOperations = 0;

            ListOfVertices.Clear();
            stopWatch.Start();      // start timer

            for (sbyte ind = 0; ind < currentNumberThreads; ind++)
            {
                List<sbyte> ColumnsToProccess = new List<sbyte>(allColumnsToProccess[ind]);

                Thread thread = new Thread((object tempInd) =>
                {
                    recursive_matching_finder(unUsedColumns[(sbyte)tempInd], 0, resultPath, ColumnsToProccess);
                });

                thread.Start(ind);
                listThreads.Add(thread);
            }

            do
            {
                flag = false;
                foreach (Thread thread in listThreads)
                {
                    flag |= thread.IsAlive;
                }

            } while (flag);

            stopWatch.Stop();       // stop timer
            strTimeAlg = string.Format("{0:N6} с", stopWatch.Elapsed.TotalSeconds);
        }
        void all_matching_finder()
        {
            int sizeList;
            int row;
            sbyte[] helpArray;

            // initialization variables
            countPerfectMatching = 0;
            countMaxMatching = 0;
            max = 0;

            sizeList = ListOfVertices.Count;

            helpArray = new sbyte[sizeList];
            maxMatchingPos = new List<int>();

            for (row = 0; row < sizeList; row++)
            {
                helpArray[row] = count_one_in_list(ListOfVertices[row]);
                if (helpArray[row] > max)
                    max = helpArray[row];
            }

            for (row = 0; row < sizeList; row++)
                if ((helpArray[row] == max) && (max != 0))
                {
                    maxMatchingPos.Add((int)row);
                    countMaxMatching++;
                }

            if ((max == rows) && (rows == columns))
                countPerfectMatching = countMaxMatching;

            strResult = countMaxMatching + " (" + max + " из " + rows + ")";
        }
        void recursive_matching_finder(List<sbyte> unUsedColumns, sbyte row, List<sbyte> resultPath, List<sbyte> columnsToProccess)
        {
            List<sbyte> nextUnUsedColumns;

            foreach (sbyte column in unUsedColumns)
            {
                if (!columnsToProccess.Contains(column))
                {
                    continue;
                }
                countOperations++;

                if (0 == row)
                {
                    resultPath = new List<sbyte>();
                    resultPath.Clear();
                    countOperations++;
                }

                if (helpMatrix[row, column] == 1)
                {
                    countOperations++;
                    resultPath.Add(column);
                }
                else
                {
                    countOperations++;
                    resultPath.Add(-1);
                }

                if ((resultPath.Count == rows) || (resultPath.Count == columns))
                {
                    countOperations++;

                    lock (lck)
                    {
                        ListOfVertices.Add(new List<sbyte>(resultPath));
                        resultPath.RemoveAt(row);
                    }
                    return;
                }

                else
                {
                    countOperations++;

                    sbyte[] temp = new sbyte[unUsedColumns.Count];
                    unUsedColumns.CopyTo(temp);

                    nextUnUsedColumns = temp.ToList<sbyte>();
                    nextUnUsedColumns.Remove(column);

                    recursive_matching_finder(nextUnUsedColumns, (sbyte)(row + 1), resultPath, allColumns);
                    resultPath.RemoveAt(row);

                    nextUnUsedColumns.TrimExcess();
                    nextUnUsedColumns = null;
                    temp = null;
                }

            }
        }
        sbyte count_one_in_list(List<sbyte> currentMatching)
        {
            sbyte countOne = 0;

            for (sbyte row = 0; row < rows; row++)
            {
                if (currentMatching[row] != -1)
                    countOne++;
            }

            return countOne;
        }
        void add_new_lines()
        {
            for (uint row = 0; row < rows; row++)
            {
                for (uint column = 0; column < columns; column++)
                    helpMatrix[row, column] = class_Matrix.matrix[row, column];
            }
        }

        /*************** END find all matching using algorithms ***************/


        /*************** BEGIN find maximum matching using algorithms ***************/

        // main function for algorithm Ford-Fulkerson
        void counter_maxMatching()
        {
            int[] arrMarker;
            System.Diagnostics.Stopwatch stopWatch;

            arrMarker = new int[columns];
            arrVertices = new int[rows];
            helpMatrix = (int[,])class_Matrix.matrix.Clone();
            stopWatch = new System.Diagnostics.Stopwatch();

            stopWatch.Start();
            alg_Ford_Fulkerson(helpMatrix, arrMarker, arrVertices);
            stopWatch.Stop();

            strResult = countMaxMatching + " из " + rows; //+ " (" + max + " из " + rows + ")";

            if (countMaxMatching == rows)
                countPerfectMatching = 1;

            strTimeAlg = string.Format("{0:N6} с", stopWatch.Elapsed.TotalSeconds);
        }

        // realization depth-first search for Ford-Fulkerson 
        bool DFS(int[] arrMarker, bool[] arrVisited, int[] arrVertices, int[,] matrix, int row)
        {
            for (int column = 0; column < columns; column++)
            {
                if ((matrix[row, column] == 1) && (!arrVisited[column]))
                {
                    countOperations++;
                    arrVisited[column] = true;

                    if ((arrMarker[column] < 0) || (DFS(arrMarker, arrVisited, arrVertices, matrix, arrMarker[column])))
                    {
                        countOperations++;
                        arrMarker[column] = row;
                        arrVertices[row] = column;
                        return true;
                    }
                }
            }
            return false;
        }

        // realization algorithm Ford-Fulkerson
        void alg_Ford_Fulkerson(int[,] matrix, int[] arrMarker, int[] arrVertices)
        {
            for (int i = 0; i < columns; i++)
            {
                countOperations++;
                arrMarker[i] = -1;
                arrVertices[i] = -1;
            }

            countMaxMatching = 0;
            for (int row = 0; row < rows; row++)
            {
                bool[] arrVisited = new bool[columns];     // initialization array "arrVisited"
                for (int i = 0; i < columns; i++)
                {
                    countOperations++;
                    arrVisited[i] = false;
                }

                if (DFS(arrMarker, arrVisited, arrVertices, matrix, row))
                {
                    countOperations++;
                    countMaxMatching++;
                }
            }
        }

        /*************** END find maximum matching using algorithms ***************/


        /*************** BEGIN save result to file ***************/

        public void result_to_file(string saveFileName)
        {
            string typeMatching = "";

            filling_content_for_result(saveFileName);

            switch (class_Matrix.typeOpenFile)
            {
                /***** save file to format XML */
                case ".xml":
                    save_result_to_xml_format(typeMatching, saveFileName);
                    break;

                /***** save file to format TXT */
                case ".txt":
                    save_result_to_txt_format(typeMatching, saveFileName);
                    break;

                /***** save file to format HTML */
                case ".html":
                    save_result_to_html_format(saveFileName);
                    break;

                default:
                    break;
            }
        }

        void filling_content_for_result(string saveFileName)
        {
            class_Matrix.typeOpenFile = System.IO.Path.GetExtension(saveFileName);

            // filling content xml format
            if (".xml" == class_Matrix.typeOpenFile)
            {
                strCurrentAlgorithm = "алгоритм";
                strNumberThreads = "кол-во_потоков";
                strComplexityAlgorithm = "оценка_сложности";
                strCountOperations = "кол-во_операций";
                strCountMatching = "кол-во_паросочетаний";
                strCountMaxMatching = "максимальных";
                strCountPerfectMatching = "совершенных";
                strTimeSpent = "временные_затраты";
            }

            // filling content txt format
            if (".txt" == class_Matrix.typeOpenFile)
            {
                strCurrentAlgorithm = "Алгоритм:\t\t";
                strNumberThreads = "Кол-во потоков:\t\t";
                strComplexityAlgorithm = "Оценка сложности:\t";
                strCountOperations = "Кол-во операций:\t";
                strCountMatching = "Паросочетаний";
                strCountMaxMatching = "Максимальных:\t\t";
                strCountPerfectMatching = "Совершенных:\t\t";
                strTimeSpent = "Временные затраты:\t";
            }
        }

        /*************** BEGIN save result to file format XML ***************/

        // save result XML
        void save_result_to_xml_format(string typeMatch, string saveFileName)
        {
            if (currAlgorithm == MyResources.lblFordFulkersonAlgorithm)
                strResult = countMaxMatching + " из " + rows;
            else
                strResult = Convert.ToString(countMaxMatching);

            XDocument doc = new XDocument(
                        new XDeclaration("1.0", "utf-8", "true"),
                        new XElement("Результат",
                            new XElement(strCurrentAlgorithm, currAlgorithm),
                            new XElement(strNumberThreads, numberThreads),
                            new XElement(strComplexityAlgorithm, complexityAlgorithm),
                            new XElement(strCountOperations, countOperations),
                            new XElement(strCountMatching,
                                new XElement(strCountMaxMatching, strResult),
                                new XElement(strCountPerfectMatching, countPerfectMatching)),
                            new XElement(strTimeSpent, strTimeAlg)));


            if (currTypeMatrix == MyResources.lblAdjacencyMatrix)
                drawing_an_adjacency_matrix(doc);

            drawing_a_reduce_matrix(doc);

            switch (currTypeAlgorithm)
            {
                case MyResources.lblNumberMatching:
                    drawing_all_matching_XML(doc, typeMatch);
                    break;

                case MyResources.lblMaxMatching:
                    drawing_matchings_XML(doc, typeMatch);
                    break;
            }

            doc.Save(saveFileName);
        }

        // drawing new adjacency matrix for file format XML
        void drawing_an_adjacency_matrix(XDocument doc)
        {
            // add element matrix
            var elemMatrix = new XElement("матрица", new XAttribute("тип", MyResources.lblAdjacencyMatrix));

            for (int row = 0; row < class_Matrix.original_rows; row++)
            {
                var elemRow = new XElement("строка");

                for (int column = 0; column < class_Matrix.original_columns; column++)
                {
                    var elemCol = new XElement("столбец", class_Matrix.original_matrix[row, column]);
                    elemRow.Add(elemCol);
                }
                elemMatrix.Add(elemRow);
            }

            // add new element in original document
            doc.Element("Результат").Add(elemMatrix);
        }

        // drawing new reduce matrix for file format XML
        void drawing_a_reduce_matrix(XDocument doc)
        {
            // add element matrix
            var elemMatrix = new XElement("матрица", new XAttribute("тип", MyResources.lblReducedMatrix));

            for (int row = 0; row < rows; row++)
            {
                var elemRow = new XElement("строка");

                for (int column = 0; column < columns; column++)
                {
                    var elemCol = new XElement("столбец", class_Matrix.matrix[row, column]);
                    elemRow.Add(elemCol);
                }

                elemMatrix.Add(elemRow);
            }

            // add new element in original document
            doc.Element("Результат").Add(elemMatrix);
        }

        // drawing all matching edges for file format XML
        void drawing_all_matching_XML(XDocument doc, string typeMatch)
        {
            var matching_edges = new XElement("рёбра_паросочетаний"); ;
            int numberColumn;

            if (rows == max)
                typeMatch = "максимальное/совершенное";
            else
                typeMatch = "максимальное";

            for (int row = 0; row < countMaxMatching; row++)
            {
                var element = new XElement("паросочетание", new XAttribute("тип", typeMatch));
                matching_edges.Add(element);

                for (int column = 0; column < columns; column++)
                {
                    if (currTypeMatrix == MyResources.lblAdjacencyMatrix)
                        numberColumn = (ListOfVertices[maxMatchingPos[row]][column] + class_Matrix.original_columns / 2);
                    else
                        numberColumn = ListOfVertices[maxMatchingPos[row]][column];

                    if (ListOfVertices[maxMatchingPos[row]][column] != -1)
                    {
                        var edge = new XElement("ребро", numberColumn,
                            new XAttribute("строка", column));
                        element.Add(edge);
                    }
                }
            }

            doc.Element("Результат").Add(matching_edges);
        }

        // drawing a match edges for file format XML
        void drawing_matchings_XML(XDocument doc, string typeMatch)
        {
            int numberColumn;

            numberColumn = 0;

            if (rows == countMaxMatching)
                typeMatch = "максимальное/совершенное";
            else
                typeMatch = "максимальное";

            var elemMatch = new XElement("паросочетание", new XAttribute("тип", typeMatch));

            for (int column = 0; column < countMaxMatching; column++)
            {
                if (currTypeMatrix == MyResources.lblAdjacencyMatrix)
                    numberColumn = (arrVertices[column] + class_Matrix.original_columns / 2);
                else
                    numberColumn = arrVertices[column];

                var edge = new XElement("ребро", numberColumn,
                            new XAttribute("строка", column));
                elemMatch.Add(edge);
            }

            doc.Element("Результат").Add(elemMatch);
        }

        /*************** END save result to file format XML ***************/


        /*************** BEGIN save result to file format TXT ***************/

        // save result TXT
        void save_result_to_txt_format(string typeMatch, string saveFileName)
        {
            using (StreamWriter writer = File.CreateText(saveFileName))
            {
                string strMatrixToFile = "";

                if (currAlgorithm == MyResources.lblFordFulkersonAlgorithm)
                    strResult = countMaxMatching + " из " + rows;
                else
                    strResult = Convert.ToString(countMaxMatching);

                writer.WriteLine(strCurrentAlgorithm + currAlgorithm);
                writer.WriteLine(strNumberThreads + numberThreads);
                writer.WriteLine(strComplexityAlgorithm + complexityAlgorithm);
                writer.WriteLine(strCountOperations + countOperations);
                writer.WriteLine(strTimeSpent + strTimeAlg);
                writer.WriteLine();
                writer.WriteLine(strCountMatching);
                writer.WriteLine(strCountMaxMatching + strResult);
                writer.WriteLine(strCountPerfectMatching + countPerfectMatching);
                writer.WriteLine();
                writer.WriteLine("Тип матрицы:\t\t" + currTypeMatrix);

                switch (currTypeMatrix)
                {
                    case MyResources.lblAdjacencyMatrix:
                        writer.WriteLine("Столбцов:\t\t" + class_Matrix.original_columns);
                        writer.WriteLine("Строк:\t\t\t" + class_Matrix.original_rows);
                        writer.WriteLine();

                        writer.WriteLine("***** Исходная матрица *****");
                        drawing_an_adjacency_matrix(writer, strMatrixToFile);
                        writer.WriteLine("***** Приведённая матрица смежности *****");
                        drawing_a_reduce_matrix(writer, strMatrixToFile);
                        break;

                    case MyResources.lblReducedMatrix:
                        writer.WriteLine("Столбцов:\t\t" + columns);
                        writer.WriteLine("Строк:\t\t\t" + rows);
                        writer.WriteLine();

                        writer.WriteLine("***** Матрица *****");
                        drawing_a_reduce_matrix(writer, strMatrixToFile);
                        break;
                }


                writer.WriteLine("***** Паросочетания *****");

                switch (currTypeAlgorithm)
                {
                    case MyResources.lblMaxMatching:
                        drawing_matchings_TXT(writer);
                        break;

                    case MyResources.lblNumberMatching:
                        drawing_all_matching_TXT(writer);
                        break;
                }

                writer.Close();
            }
        }

        // drawing new adjacency matrix for file format TXT
        void drawing_an_adjacency_matrix(StreamWriter writer, string strMatrixToFile)
        {
            writer.WriteLine();

            for (int row = 0; row < class_Matrix.original_rows; row++)
            {
                strMatrixToFile = "";

                for (int column = 0; column < class_Matrix.original_columns; column++)
                {
                    strMatrixToFile += class_Matrix.original_matrix[row, column] + "\t";
                }
                writer.WriteLine(strMatrixToFile);
            }

            writer.WriteLine();
        }

        // drawing new reduce matrix for file format TXT
        void drawing_a_reduce_matrix(StreamWriter writer, string strMatrixToFile)
        {
            writer.WriteLine();

            for (int row = 0; row < rows; row++)
            {
                strMatrixToFile = "";

                for (int column = 0; column < columns; column++)
                {
                    strMatrixToFile += class_Matrix.matrix[row, column] + "\t";
                }
                writer.WriteLine(strMatrixToFile);
            }

            writer.WriteLine();
        }

        // drawing all matching edges for file format TXT
        void drawing_all_matching_TXT(StreamWriter writer)
        {
            int numberColumn = 0;
            string typeMatch = "";

            if (rows == max)
                typeMatch = "максимальное/совершенное";
            else
                typeMatch = "максимальное";

            for (int row = 0; row < countMaxMatching; row++)
            {
                writer.WriteLine();
                writer.WriteLine(typeMatch);

                for (int column = 0; column < columns; column++)
                {
                    if (currTypeMatrix == MyResources.lblAdjacencyMatrix)
                        numberColumn = (ListOfVertices[maxMatchingPos[row]][column] + class_Matrix.original_columns / 2);
                    else
                        numberColumn = ListOfVertices[maxMatchingPos[row]][column];

                    if (ListOfVertices[maxMatchingPos[row]][column] != -1)
                        writer.WriteLine("строка " + column + " -> ребро " + numberColumn);
                }
            }
        }

        // drawing a match edges for file format TXT
        void drawing_matchings_TXT(StreamWriter writer)
        {
            int numberColumn = 0;
            string typeMatch = "";

            if (rows == countMaxMatching)
                typeMatch = "максимальное/совершенное";
            else
                typeMatch = "максимальное";

            writer.WriteLine();
            writer.WriteLine(typeMatch);

            for (int column = 0; column < countMaxMatching; column++)
            {
                if (currTypeMatrix == MyResources.lblAdjacencyMatrix)
                    numberColumn = (arrVertices[column] + class_Matrix.original_columns / 2);
                else
                    numberColumn = arrVertices[column];

                writer.WriteLine("строка " + (column) + " -> ребро " + numberColumn);
            }
        }

        /*************** END save result to file format TXT ***************/


        /*************** BEGIN save result to file format HTML ***************/

        // save result HTML 
        void save_result_to_html_format(string saveFileName)
        {
            int[,] matrix;
            string strTemplateHTML;

            matrix = new int[0, 0];
            strTemplateHTML = "";

            switch (currTypeMatrix)
            {
                case MyResources.lblAdjacencyMatrix:
                    strTemplateHTML = File.ReadAllText("..\\..\\Template\\template_for_adjacency.html");
                    break;

                case MyResources.lblReducedMatrix:
                    strTemplateHTML = File.ReadAllText("..\\..\\Template\\template_for_reduce.html");
                    break;
            }

            if (currAlgorithm == MyResources.lblFordFulkersonAlgorithm)
                strResult = countMaxMatching + " из " + rows;
            else
                strResult = Convert.ToString(countMaxMatching);

            // update data about work algorithm
            strTemplateHTML = strTemplateHTML.Replace("{алгоритм}", currAlgorithm);
            strTemplateHTML = strTemplateHTML.Replace("{потоки}", Convert.ToString(numberThreads));
            strTemplateHTML = strTemplateHTML.Replace("{сложность}", complexityAlgorithm);
            strTemplateHTML = strTemplateHTML.Replace("{операции}", Convert.ToString(countOperations));
            strTemplateHTML = strTemplateHTML.Replace("{время}", strTimeAlg);
            strTemplateHTML = strTemplateHTML.Replace("{максимальных}", strResult);
            strTemplateHTML = strTemplateHTML.Replace("{совершенных}", Convert.ToString(countPerfectMatching));

            if (strTemplateHTML != null)
            {
                using (StreamWriter writer = File.CreateText(saveFileName))
                {
                    writer.Write(strTemplateHTML);

                    // ***************** BEGIN drawing a matrix ***************** //
                    switch (currTypeMatrix)
                    {
                        case MyResources.lblAdjacencyMatrix:
                            matrix = new int[class_Matrix.original_rows, class_Matrix.original_columns];
                            matrix = (int[,])class_Matrix.original_matrix.Clone();

                            writer.WriteLine("<h3><a name=\"original-matrix\">Исходная матрица</a></h3>");
                            drawing_an_adjacency_matrix(writer, matrix);

                            matrix = new int[class_Matrix.rows, class_Matrix.columns];
                            matrix = (int[,])class_Matrix.matrix.Clone();

                            writer.WriteLine("<h3><a name=\"reduce-matrix\">Приведённая матрица</a></h3>");
                            drawing_a_reduce_matrix(writer, matrix, true);
                            break;

                        case MyResources.lblReducedMatrix:
                            matrix = new int[class_Matrix.rows, class_Matrix.columns];
                            matrix = (int[,])class_Matrix.matrix.Clone();

                            writer.WriteLine("<h3><a name=\"reduce-matrix\">Матрица</a></h3>");
                            drawing_a_reduce_matrix(writer, matrix, false);
                            break;
                    }

                    // choice of algorithm for drawing
                    if (countMaxMatching != 0)
                    {
                        // Ford-Fulkerson
                        if (currAlgorithm == MyResources.lblFordFulkersonAlgorithm)
                            drawing_matchings_HTML(writer);
                        else
                            if (currTypeAlgorithm == MyResources.lblNumberMatching)
                            drawing_all_matchings_HTML(writer);
                    }
                    // ***************** END drawing a matrix ***************** //

                    writer.Write("</body></html>");
                    writer.Close();
                }
            }
        }

        // drawing original matrix for file format HTML
        void drawing_a_matrix(StreamWriter writer, int[,] matrix, int columns, int rows, bool adjacencyToReduce)
        {
            writer.WriteLine("<table class=\"table tbl-matrix\">");
            writer.WriteLine("<tr class=\"bold-td\">");
            writer.WriteLine("<td style=\"font-weight: normal;\">-</td>");

            // drawing a count rows
            if (adjacencyToReduce)
            {
                for (int col = class_Matrix.original_columns / 2; col < class_Matrix.original_columns; col++)
                    writer.WriteLine("<td>" + col + "</td>");
            }

            else
            {
                for (int col = 0; col < columns; col++)
                    writer.WriteLine("<td>" + col + "</td>");
            }

            writer.WriteLine("</tr>");


            // draw a body matrix
            for (int row = 0; row < rows; row++)
            {
                writer.WriteLine("<tr>");
                writer.WriteLine("<td style=\"font-weight: bold;\">" + row + "</td>");
                for (int col = 0; col < columns; col++)
                {
                    if (matrix[row, col] == 1)
                        writer.WriteLine("<td class=\"check-cell\">" + matrix[row, col] + "</td>");
                    else
                        writer.WriteLine("<td>" + matrix[row, col] + "</td>");
                }
                writer.WriteLine("</tr>");
            }

            writer.WriteLine("</table>");
        }

        // drawing new adjacency matrix for file format HTML
        void drawing_an_adjacency_matrix(StreamWriter writer, int[,] matrix)
        {
            writer.WriteLine("<div class=\"flex-container\">");
            writer.WriteLine("<div class=\"flex-item flex-item-matrix\">");
            drawing_a_matrix(writer, matrix, class_Matrix.original_columns, class_Matrix.original_rows, false);
            writer.WriteLine("</div>");
            writer.WriteLine("<div class=\"flex-item flex-item-matrix\">");
            writer.WriteLine("<table class=\"table tbl-result tbl-matrix-origin\">");
            writer.WriteLine("<tr><th>Параметр</th><th>Значение</th></tr><tr><td>Тип матрицы</td>");
            writer.WriteLine("<td>" + MyResources.lblAdjacencyMatrix + "</td>");
            writer.WriteLine("</tr><tr><td>Кол-во столбцов</td>");
            writer.WriteLine("<td>" + class_Matrix.original_columns + "</td>");
            writer.WriteLine("</tr><tr><td>Кол-во строк</td>");
            writer.WriteLine("<td>" + class_Matrix.original_rows + "</td>");
            writer.WriteLine("</tr></table></div></div>");
        }

        // drawing new reduce matrix for file format HTML
        void drawing_a_reduce_matrix(StreamWriter writer, int[,] matrix, bool adjacencyToReduce)
        {
            writer.WriteLine("<div class=\"flex-container\">");
            writer.WriteLine("<div class=\"flex-item flex-item-matrix\">");

            drawing_a_matrix(writer, matrix, class_Matrix.columns, class_Matrix.rows, adjacencyToReduce);
            writer.WriteLine("</div>");
            writer.WriteLine("<div class=\"flex-item flex-item-matrix\">");
            writer.WriteLine("<table class=\"table tbl-result tbl-matrix-origin\">");
            writer.WriteLine("<tr><th>Параметр</th><th>Значение</th></tr><tr>");
            writer.WriteLine("<td>Тип матрицы</td>");
            writer.WriteLine("<td>" + MyResources.lblReducedMatrix + "</td>");
            writer.WriteLine("</tr><tr><td>Кол-во столбцов</td>");
            writer.WriteLine("<td>" + class_Matrix.columns + "</td>");
            writer.WriteLine("</tr><tr><td>Кол-во строк</td>");
            writer.WriteLine("<td>" + class_Matrix.rows + "</td>");
            writer.WriteLine("</tr></table></div></div>");
        }

        // drawing all matching edges for file format HTML
        void drawing_all_matchings_HTML(StreamWriter writer)
        {
            int numberInCycle;
            int cx, cy;             // SVG circle coordinates
            int x, y;               // SVG text coordinates
            int x1, x2, y2;         // SVG line coordinates

            numberInCycle = 0;

            writer.WriteLine("<h3><a name=\"matching\">Паросочетания</a></h3>");
            writer.WriteLine("<div class=\"matching\">");
            writer.WriteLine("<div class=\"flex-container\">");

            for (int row = 0; row < countMaxMatching; row++)
            {
                cx = 21;
                cy = 21;
                x = 17;
                y = 25;
                x1 = 21;
                x2 = 131;

                writer.WriteLine("<div class=\"flex-item\">");
                writer.WriteLine("<p>Паросочетание №" + (row + 1) + "</p>");
                writer.WriteLine("<svg width=\"152\" height=\"" + rows * 60 + "\">");

                // drawing a lines
                for (int column = 0; column < columns; column++)
                {
                    if (ListOfVertices[maxMatchingPos[row]][column] != -1)
                    {
                        y2 = 21;
                        y2 += (ListOfVertices[maxMatchingPos[row]][column] * 60);

                        writer.WriteLine("<line x1=\"" + x1 + "\" y1=\"" + cy + "\" x2=\"" + x2 + "\" y2=\"" + y2 + "\" class=\"line-svg\" />");

                        // update variables for next SVG draw
                        y += 60;
                    }
                    cy += 60;
                }

                cx = 21;
                cy = 21;
                x = 17;
                y = 25;
                x1 = 21;
                x2 = 131;


                // drawing a circle and text in the circle
                for (int column = 0; column < columns; column++)
                {
                    if (currTypeMatrix == MyResources.lblAdjacencyMatrix)
                        numberInCycle = (column + class_Matrix.original_columns / 2);
                    else
                        numberInCycle = column;

                    writer.WriteLine("<circle cx=\"" + cx + "\" cy=\"" + cy + "\" r=\"20\" class=\"circle-svg\" />");
                    writer.WriteLine("<text x=\"" + x + "\" y=\"" + y + "\" fill=\"black\">" + column + "</text>");

                    writer.WriteLine("<circle cx=\"" + (cx + 110) + "\" cy=\"" + cy + "\" r=\"20\" class=\"circle-svg\" />");
                    writer.WriteLine("<text x=\"" + (x + 110) + "\" y=\"" + y + "\" fill=\"black\">" + numberInCycle + "</text>");

                    // update variables for next SVG drawing
                    cy += 60;
                    y += 60;
                }

                writer.WriteLine("</svg>");
                writer.WriteLine("</div>");
            }
        }

        // drawing a match edges for file format HTML
        void drawing_matchings_HTML(StreamWriter writer)
        {
            int numberInCircle;
            int cx, cy;             // SVG circle coordinates
            int x, y;               // SVG text coordinates
            int x1, x2, y2;         // SVG line coordinates

            numberInCircle = 0;

            writer.WriteLine("<h3><a name=\"matching\">Паросочетания</a></h3>");
            writer.WriteLine("<div class=\"matching\">");
            writer.WriteLine("<div class=\"flex-container\">");

            cx = 21;
            cy = 21;
            x = 17;
            y = 25;
            x1 = 21;
            x2 = 131;

            writer.WriteLine("<div class=\"flex-item\">");
            writer.WriteLine("<p>Максимальное</p>");
            writer.WriteLine("<svg width=\"152\" height=\"" + rows * 60 + "\">");

            // drawing a lines
            for (int column = 0; column < columns; column++)
                if (arrVertices[column] != -1)
                {
                    y2 = 21;
                    y2 += (arrVertices[column] * 60);

                    writer.WriteLine("<line x1=\"" + x1 + "\" y1=\"" + cy + "\" x2=\"" + x2 + "\" y2=\"" + y2 + "\" class=\"line-svg\" />");

                    // update variables for next SVG draw
                    cy += 60;
                    y += 60;
                }

            cx = 21;
            cy = 21;
            x = 17;
            y = 25;
            x1 = 21;
            x2 = 131;

            // drawing a circle and text in the circle
            for (int column = 0; column < columns; column++)
            {
                if (currTypeMatrix == MyResources.lblAdjacencyMatrix)
                    numberInCircle = (column + class_Matrix.original_columns / 2);
                else
                    numberInCircle = column;

                writer.WriteLine("<circle cx=\"" + cx + "\" cy=\"" + cy + "\" r=\"20\" class=\"circle-svg\" />");
                writer.WriteLine("<text x=\"" + x + "\" y=\"" + y + "\" fill=\"black\">" + column + "</text>");

                writer.WriteLine("<circle cx=\"" + (cx + 110) + "\" cy=\"" + cy + "\" r=\"20\" class=\"circle-svg\" />");
                writer.WriteLine("<text x=\"" + (x + 110) + "\" y=\"" + y + "\" fill=\"black\">" + numberInCircle + "</text>");

                // update variables for next SVG drawing
                cy += 60;
                y += 60;
            }

            writer.WriteLine("</svg>");
            writer.WriteLine("</div>");
        }

        /*************** END save result to file format HTML ***************/

        /*************** END save result to file ***************/


        //void counter_maxMatching(int[,] matrix)
        //{
        //    //int[,] _matrix;
        //    System.Diagnostics.Stopwatch stopWatch;
        //    int[] resultPath;
        //    List<int> helpList;


        //    // initialization variables
        //    stopWatch = new System.Diagnostics.Stopwatch();


        //    //_matrix = (int[,])inputMatrix.matrix.Clone();
        //    //_matrix = { { 17, 16, 32, 52 }, { 54, 6, 57, 98 }, { 52, 3, 2, 15 }, { 354, 54, 98, 59 }};

        //    //int[,] _matrix = { { 0, 5, 3 }, { 1, 4, 7 }, { 2, 1, 4 } };

        //    rows = 3;
        //    columns = 3;

        //    stopWatch.Start();

        //    //int res1 = solveAssignmentProblem(_matrix);
        //    //int res2 = solveAssignmentProblemSlow(_matrix);
        //    //if (res1 != res2)
        //    //{
        //    //    MessageBox.Show(res1 + " " + res2);
        //    //    //System.err.println(res1 + " " + res2);
        //    //}

        //    //resultPath = Hungarian_algorithm(_matrix);

        //    int n2 = 3;

        //    List<int>[] g = new List<int>[3];
        //    for (int i = 0; i < 3; i++)
        //        g[i] = new List<int>();

        //    g[0].Add(0);
        //    g[0].Add(5);
        //    g[0].Add(3);

        //    g[1].Add(1);
        //    g[1].Add(4);
        //    g[1].Add(7);

        //    g[2].Add(2);
        //    g[2].Add(1);
        //    g[2].Add(4);

        //    //int res1 = maxMatching(g, n2);

        //    stopWatch.Stop();

        //    // save result path 
        //    //resultList = new List<List<int>>();

        //    //for (int row = 0; row < rows; row++)
        //    //{
        //    //    helpList = new List<int>();

        //    //    helpList.Add(row);
        //    //    helpList.Add(resultPath[row]);
        //    //    resultList.Add(helpList);
        //    //}


        //    strTimeAlg = string.Format("{0:N6} с", stopWatch.Elapsed.TotalSeconds);
        //}

        /*************** BEGIN Hungarian algorithm  ***************/
        /* int[] hungarianMethod(int[,] w) 
       {
           int n = rows;
           int m = columns;
           int PHI = -1, NOL = -2;
           bool[,]x = new bool[n,m];
           bool[] ss = new bool[n], st = new bool[m];
        
           int[] u = new int[n], v = new int[m], p = new int[m], ls = new int[n], lt = new int[m], a = new int[n];
           int f = 0;

           for (int i = 0; i < n; i++)
               for (int j = 0; j < m; j++)
                   f = Math.Max(f, w[i,j]);

            //fill(u, f);
            //fill(p, INF);
            //fill(lt, NOL);
            //fill(ls, PHI);
            //fill(a, -1);
           
           while (true) 
           {
               f = -1;

               for (int i = 0; i < n && f == -1; i++)
                   if (ls[i] != NOL && !ss[i])
                       f = i;

            if (f != -1) {
                ss[f] = true;
                for (int j = 0; j < m; j++)
                    if (!x[f,j] && u[f] + v[j] - w[f,j] < p[j]) {
                        lt[j] = f;
                        p[j] = u[f] + v[j] - w[f,j];
                    }
            } else {
                for (int i = 0; i < m && f == -1; i++)
                    if (lt[i] != NOL && !st[i] && p[i] == 0)
                        f = i;

                if (f == -1) {
                    //int d1 = INF, d2 = INF, d;
                    
                    for (int i = 0; i < u.Length; i++)
                        d1 = Math.Min(d1, i);

                    for (int i = 0; i < p.Length; i++)
                        if (i > 0)
                            d2 = Math.Min(d2, i);

                    d = Math.Min(d1, d2);

                    for (int i = 0; i < n; i++)
                        if (ls[i] != NOL)
                            u[i] -= d;

                    for (int i = 0; i < m; i++) {
                        if (p[i] == 0)
                            v[i] += d;
                        if (p[i] > 0 && lt[i] != NOL)
                            p[i] -= d;
                    }

                    if (d2 >= d1)
                        break;
                } 
                
                else 
                {
                    st[f] = true;
                    int s = -1;

                    for (int i = 0; i < n && s == -1; i++)
                        if (x[i,f])
                            s = i;

                    if (s == -1) {
                        for (int l, r; ; f = r) {
                            r = f;
                            l = lt[r];

                            if (r >= 0 && l >= 0)
                                x[l,r] = !x[l,r];
                            else
                                break;

                            r = ls[l];
                            if (r >= 0 && l >= 0)
                                x[l,r] = !x[l,r];
                            else
                                break;
                        }

                        //fill(p, INF);
                        //fill(lt, NOL);
                        //fill(ls, NOL);
                        //fill(ss, false);
                        //fill(st, false);

                        for (int i = 0; i < n; i++) {
                            bool ex = true;
                            for (int j = 0; j < m && ex; j++)
                                ex = !x[i,j];
                            if (ex)
                                ls[i] = PHI;
                        }
                    } 
                    else
                        ls[s] = f;
                }
            }
        }

        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                if (x[i,j])
                    a[j] = i;
        return a;
    } */

        /*public static int maxMatching(List<int>[] graph, int n2)
        {
            int n1 = graph.Length;
            int[] matching = new int[n2];

            for (int row = 0; row < n2; row++)
            {
                matching[row] = -1;
            }
            //Arrays.fill(matching, -1);
            
            int matches = 0;
            for (int u = 0; u < n1; u++)
            {
                if (findPath(graph, u, matching, new bool[n1]))
                    ++matches;
            }
            return matches;
        }

        static bool findPath(List<int>[] graph, int u1, int[] matching, bool[] vis) {
            vis[u1] = true;
            
            //for (int v : graph[u1]) 
            for (int v = 0; v < graph.Length; v++)
            {
                int u2 = matching[v];
                if (u2 == -1 || !vis[u2] && findPath(graph, u2, matching, vis))
                {
                    matching[v] = u1;
                    return true;
                }
            }
            return false;
          }*/


        //static int slowMinVertexCover(List<int>[] g, int n2) {
        //    int n1 = g.Length;
        //    int[] mask = new int[n1];

        //    for (int i = 0; i < n1; i++)
        //        for (int j = 0; j < g.Length; j++) //for (int j : g[i])
        //            mask[i] |= 1 << j;

        //    int res = n2;

        //    for (int m = 0; m < 1 << n2; m++) 
        //    {
        //        int cur = m; // int. //Integer.bitCount(m);
        //      for (int i = 0; i < n1; i++)
        //        if ((mask[i] & m) != mask[i])
        //          ++cur;

        //        res = Math.Min(res, cur);
        //    }

        //    return res;
        //  }


        /*// a[1..n][1..m] >= 0, n <= m
        public static int solveAssignmentProblem(int[,] a) 
        {
            int n = 3 - 1;
            int m = 3 - 1;
            int[] u = new int[n + 1];
            int[] v = new int[m + 1];
            int[] p = new int[m + 1];
            int[] way = new int[m + 1];
            for (int i = 1; i <= n; ++i) 
            {
              p[0] = i;
              int j0 = 0;
              int[] minv = new int[m + 1];


              for (int row = 0; row < m + 1; row++)
              {
                  minv[row] = int.MaxValue;
              }
                //Arrays.fill(minv, int.MaxValue);
              
                bool[] used = new bool[m + 1];
              
                do {
                
                    used[j0] = true;
                    int i0 = p[j0];
                    int delta = int.MaxValue;
                    int j1 = 0;
                    for (int j = 1; j <= m; ++j)
                  if (!used[j]) {
                    int cur = a[i0,j] - u[i0] - v[j];
                    if (cur < minv[j]) {
                      minv[j] = cur;
                      way[j] = j0;
                    }
                    if (minv[j] < delta) {
                      delta = minv[j];
                      j1 = j;
                    }
                  }
                for (int j = 0; j <= m; ++j)
                  if (used[j]) {
                    u[p[j]] += delta;
                    v[j] -= delta;
                  } else
                    minv[j] -= delta;
                j0 = j1;
              } while (p[j0] != 0);
              do {
                int j1 = way[j0];
                p[j0] = p[j1];
                j0 = j1;
              } while (j0 != 0);
            }
            return -v[0];
          }

          // random test
          //public static void main(String[] args) {
          //  Random rnd = new Random(1);
          //  for (int step = 0; step < 1000; step++) {
          //    int n = rnd.nextInt(8) + 1;
          //    int m = n + rnd.nextInt(9 - n);
          //    int[][] a = new int[n + 1][m + 1];
          //    for (int i = 1; i <= n; i++) {
          //      for (int j = 1; j <= m; j++) {
          //        a[i][j] = rnd.nextInt(100000);
          //      }
          //    }
          //    int res1 = solveAssignmentProblem(a);
          //    int res2 = solveAssignmentProblemSlow(a);
          //    if (res1 != res2) {
          //      System.err.println(res1 + " " + res2);
          //    }
          //  }
          //}

          static int solveAssignmentProblemSlow(int[,] a) {
            int n = 3 - 1;
            int m = 3 - 1;
            int res = int.MaxValue;
            int[] p = new int[n];
            for (int i = 0; i < n; i++)
              p[i] = i;
            do {
              int cur = 0;
              for (int i = 0; i < p.Length; i++)
                cur += a[i + 1, p[i] + 1];
              res = Math.Min(res, cur);
            } while (nextArrangement(p, m));
            return res;
          }

          static bool nextArrangement(int[] p, int n) {
            bool[] used = new bool[n];
            for (int x = 0; x < p.Length; x++)
            {
                used[x] = true;
            }
            int m = p.Length;
            for (int i = m - 1; i >= 0; i--) 
            {
              used[p[i]] = false;

              for (int j = p[i] + 1; j < n; j++) 
              {
                if (!used[j]) {
                  p[i++] = j;
                  used[j] = true;
                  for (int k = 0; k < n && i < m; k++) 
                  {
                    if (!used[k]) 
                    {
                      p[i++] = k;
                    }
                  }
                  return true;
                }
              }
            }
            return false;
          }*/

        /*int[] Hungarian_algorithm(int[,] matrix)
        //{
        //    int number;
        //    int[,] modif_matrix;

        //    number = 0;
        //    modif_matrix = new int[0,0];

        //    ListOfVertices = new List<List<int>>();
        //    ListOfZero = new List<List<int>>();

        //    Hungarian_algorithm_step_1(matrix);

        //    // find zero elements
        //    for (int row = 0; row < rows; row++)
        //    {
        //        currLine = new List<int>();
        //        for (int col = 0; col < columns; col++)
        //        {
        //            if (matrix[row, col] == 0)
        //                currLine.Add(col);
        //        }

        //        if (currLine.Count == 0)    // check if line don't have zero element
        //            break;
        //        else 
        //            ListOfVertices.Add(currLine);
        //    }

        //    // find path max flow
        //    if (ListOfVertices.Count != 0)
        //    {
        //        while (number != ListOfVertices[0].Count)
        //        {
        //            currLine = new List<int>(); // current path 

        //            find_optimal_path(number);

        //            if (currLine.Count == rows)
        //                return currLine.ToArray();

        //            number++;
        //        }
        //    }


        //    build_modif_matrix(matrix, modif_matrix);
            
        //    return null;
            
        //}


        //void Hungarian_algorithm_step_1(int [,] matrix)
        //{
        //    int maxValue;
        //    int[] deductArray;

        //    maxValue = int.MinValue;
        //    deductArray = new int[rows];

        //    // find max element
        //    for (int row = 0; row < rows; row++)
        //        for (int col = 0; col < columns; col++)
        //        {
        //            if (matrix[row, col] > maxValue)
        //                maxValue = matrix[row, col];
        //        }

        //    // mul all elements -1 and add max value for each element
        //    for (int row = 0; row < rows; row++)
        //        for (int col = 0; col < columns; col++)
        //        {
        //            matrix[row, col] = (matrix[row, col] * (-1)) + maxValue;
        //        }

        //    deductArray = filling_array_elements(deductArray);

        //    // find min element in row
        //    for (int row = 0; row < rows; row++)
        //        for (int col = 0; col < columns; col++)
        //        {
        //            if (matrix[row, col] < deductArray[row])
        //                deductArray[row] = matrix[row, col];
        //        }

        //    // deduct min element for each element in row
        //    for (int row = 0; row < rows; row++)
        //        for (int col = 0; col < columns; col++)
        //        {
        //            matrix[row, col] = matrix[row, col] - deductArray[row];
        //        }

        //    deductArray = filling_array_elements(deductArray);

        //    // find min element in column
        //    for (int row = 0; row < rows; row++)
        //        for (int col = 0; col < columns; col++)
        //        {
        //            if (matrix[row, col] < deductArray[col])
        //                deductArray[col] = matrix[row, col];
        //        }

        //    // deduct min element for each element
        //    for (int row = 0; row < rows; row++)
        //        for (int col = 0; col < columns; col++)
        //        {
        //            matrix[row, col] = matrix[row, col] - deductArray[col];
        //        }
        //}

        //int[] filling_array_elements(int[] arr)
        //{
        //    for (int col = 0; col < columns; col++)
        //        arr[col] = int.MaxValue;

        //    return arr;
        //}

        //bool find_the_same_item(List <int> currLine, int element)
        //{
        //    for (int col = 0; col < currLine.Count; col++)
        //    {
        //        if (currLine[col] == element)
        //            return true;
        //    }
        //    return false;
        //}
        
        //void find_optimal_path(int startFirstElement)
        //{
        //    currLine.Add(ListOfVertices[0][startFirstElement]);

        //    for (int row = 1; row < rows; row++)
        //    {
        //        for (int col = 0; col < ListOfVertices[row].Count; col++)
        //        {
        //            if (find_the_same_item(currLine, ListOfVertices[row][col]))
        //            {
        //                continue;
        //            }
        //            else
        //                currLine.Add(ListOfVertices[row][col]);
        //        }
        //    }
        //}

        //int[,] analiz_delete_lines(int [,] arrLines)
        //{
        //    return arrLines;
        //}

        //int [,] build_modif_matrix(int [,] srcMatrix, int [,] modif_matrix)
        //{

        //    return modif_matrix;
        //} */



        //int maxFlow(int[,] matrix, int source, int slink)
        //{
        //    for (int flow = 0; ; )
        //    {
        //        int df = findPath(matrix, new bool[3], source, slink, int.MaxValue);
        //        if (df == 0)
        //            return flow;
        //        flow += df;
        //    }
        //}

        //int findPath(int[,] matrix, bool[] vis, int u, int slink, int f)
        //{
        //    if (u == slink)
        //        return f;

        //    vis[u] = true;

        //    for (int v = 0; v < vis.Length; v++)
        //        if (!vis[v] && matrix[u, v] > 0)
        //        {
        //            int df = findPath(matrix, vis, v, slink, Math.Min(f, matrix[u, v]));
        //            if (df > 0)
        //            {
        //                matrix[u, v] -= df;
        //                matrix[v, u] += df;
        //                return df;
        //            }
        //        }

        //    return 0;
        //}


        //public static int maxFlowSimple(int[,] cap, int s, int t)
        //{
        //    for (int flow = 0; ; ++flow)
        //        if (!augmentPath(cap, new bool[3], s, t))
        //            return flow;
        //}

        //static bool augmentPath(int[,] cap, bool[] vis, int i, int t)
        //{
        //    if (i == t)
        //        return true;

        //    vis[i] = true;
        //    for (int j = 0; j < vis.Length; j++)
        //        if (!vis[j] && cap[i,j] > 0 && augmentPath(cap, vis, j, t))
        //        {
        //            --cap[i,j];
        //            ++cap[j,i];
        //            return true;
        //        }
        //    return false;
        //}



        //bool DFS(int[] arrMarker, bool[] arrVisited, int[] arrVertices, int[,] matrix, int row)
        //{
        //    for (int column = 0; column < columns; column++)
        //    {
        //        if ((matrix[row, column] == 1) && (!arrVisited[column]))
        //        {
        //            //countOperations++;
        //            arrVisited[column] = true;

        //            if ((arrMarker[column] < 0) || (DFS(arrMarker, arrVisited, arrVertices, matrix, arrMarker[column])))
        //            {
        //                //countOperations++;
        //                arrMarker[column] = row;
        //                arrVertices[row] = column;
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        //void alg_Ford_Fulkerson(int[,] matrix, int[] arrMarker, int[] arrVertices)
        //{
        //    int maxMatching;

        //    for (int i = 0; i < columns; i++)
        //    {
        //        //countOperations++;
        //        arrMarker[i] = -1;
        //        arrVertices[i] = -1;
        //    }

        //    maxMatching = 0;
        //    for (int row = 0; row < rows; row++)
        //    {
        //        bool[] arrVisited = new bool[columns];     // initialization array "arrVisited"
        //        for (int i = 0; i < columns; i++)
        //        {
        //            //countOperations++;
        //            arrVisited[i] = false;
        //        }

        //        if (DFS(arrMarker, arrVisited, arrVertices, matrix, row))
        //        {
        //            //countOperations++;
        //            maxMatching++;
        //        }
        //    }
        //}
        /*************** END algorithm  ***************/





    }

}

//bool count_all_true_in_array(int startIndex, bool[] checkArray)
//{
//    int countTrue;
//    int range;

//    countTrue = 0;
//    range = checkArray.Length - startIndex;

//    for (int column = startIndex; column < checkArray.Length; column++)
//        if (checkArray[column] == true)
//            countTrue++;

//    if (countTrue == range)
//        return true;

//    return false;
//}

/*  */
// realization 1
//void counter(int[,] matrix)
//{
//    int counterOne = 0;
//    int column, column;
//    int currentNumElements;
//    int countVertex;
//    int countElements;

//    // arrays
//    int[] arrMarker = new int[columns];             // initialization array "arrInput"
//    int[,] helpMatrix = new int[rows, columns];     // initialization array "helpMatrix"
//    int[] arrVertices = new int[rows];
//    int[] helpArray;

//    List<int> ListVertices = new List<int>();

//    System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();        // initialization stopwatch
//    countOperations = 0;
//    countMaxMatching = 0;
//    countPerfectMatching = 0;

//    for (column = 0; column < rows; column++)
//    {
//        for (column = 0; column < columns; column++)
//        {
//            helpMatrix[column, column] = matrix[column, column];
//            countOperations++;
//        }
//        countOperations++;
//    }

//    stopWatch.Start();
//    using (StreamWriter writer = File.CreateText("check algorithm.txt"))
//    {
//        writer.WriteLine("\t\t\t****** Проверка корректности матрицы *****");

//        alg_Ford_Fulkerson(helpMatrix, arrMarker, arrVertices);

//        // if first step have not found perfect matching
//        if (false == check_array(arrVertices))
//        {
//            countVertex = 0;

//            // count numbers in arrVertices
//            for (column = 0; column < columns; column++)
//                if (arrVertices[column] != -1)
//                    countVertex++;

//            //countMaxMatching++;
//            currentNumElements = countVertex;

//            switch (currentNumElements)
//            {
//                // if zero vertices
//                case 0:
//                    break;

//                default:

//                    for (column = Convert.ToInt32(rows - 1); column > -1; column--)
//                    {
//                        if (arrVertices[column] != -1)
//                        {
//                            counterOne = 0;

//                            // count vertices in current line
//                            for (column = 0; column < columns; column++)
//                                if (helpMatrix[column, column] == 1)
//                                    counterOne++;

//                            if (counterOne > 0)
//                            {
//                                // write current line
//                                helpArray = new int[columns];
//                                for (column = 0; column < columns; column++)
//                                    helpArray[column] = helpMatrix[column, column];

//                                // delete first input vertex
//                                for (column = 0; column < columns; column++)
//                                {
//                                    if ((helpMatrix[column, column] == 1) && (counterOne > 0))
//                                    {
//                                        alg_Ford_Fulkerson(helpMatrix, arrMarker, arrVertices);
//                                        helpMatrix[column, column] = 0;

//                                        countVertex = 0;
//                                        for (int col = 0; col < columns; col++)
//                                            if (arrVertices[col] != -1)
//                                                countVertex++;

//                                        // check identical vertices
//                                        countElements = ListVertices.Count;

//                                        if ((countElements > 0) && (countVertex == currentNumElements))
//                                        {
//                                            if (check_identical_vertices(ListVertices, arrVertices))
//                                            {
//                                                countElements = 0;
//                                                counterOne--;
//                                            }

//                                            if (countElements != 0)
//                                            {
//                                                countMaxMatching++;
//                                                ListVertices.AddRange(arrVertices);
//                                                counterOne--;
//                                            }
//                                        }

//                                        else
//                                        {
//                                            if (countVertex == currentNumElements)
//                                            {
//                                                countMaxMatching++;
//                                                ListVertices.AddRange(arrVertices);
//                                                counterOne--;
//                                            }

//                                        }
//                                    }
//                                }

//                                // return line values in matrix
//                                for (column = 0; column < columns; column++)
//                                    helpMatrix[column, column] = helpArray[column];
//                            }
//                        }
//                    }
//                    break;

//            }
//        }

//        //for (column = 0; column < rows; column++)
//        //{
//        //    counterOne = 0;
//        //    for (column = 0; column < columns; column++)
//        //    {
//        //        if (helpMatrix[column, column] == 1)
//        //        {
//        //            counterOne++;
//        //            countOperations++;
//        //        }
//        //        countOperations++;
//        //    }

//        //    if (counterOne > 1)
//        //    {
//        //        helpMatrix[column, arrMarker[column]] = 0;
//        //        countOperations++;
//        //        break;
//        //    }
//        //}

//        //} while (check_matrix(helpMatrix));

//        //                  check algorithm
//        //currStep++;
//        //if (currStep != 1)
//        //{
//        //    writer.WriteLine();
//        //    writer.WriteLine();
//        //    writer.WriteLine();
//        //}

//        //writer.WriteLine("Шаг " + currStep + ":");
//        //writer.WriteLine("--------------------------");

//        //for (column = 0; column < rows; column++)
//        //{
//        //    for (column = 0; column < columns; column++)
//        //    {
//        //        writer.Write(helpMatrix[column, column] + ((column < (columns - 1)) ? " " : ""));
//        //    }
//        //    writer.WriteLine();
//        //}

//        //writer.WriteLine();
//        //writer.Write("arrVertices = ");

//        //for (column = 0; column < rows; column++)
//        //    writer.Write(arrVertices[column] + " ");



//        stopWatch.Stop();
//        timeAlg = Convert.ToDouble(stopWatch.Elapsed.TotalSeconds);
//        //writer.WriteLine();
//    }
//}

//bool DFS(int[] arrMarker, bool[] arrVisited, int[] arrVertices, int[,] matrix, int row)
//{
//    for (int column = 0; column < columns; column++)
//    {
//        if ((matrix[row, column] == 1) && (!arrVisited[column]))
//        {
//            countOperations++;
//            arrVisited[column] = true;

//            if ((arrMarker[column] < 0) || (DFS(arrMarker, arrVisited, arrVertices, matrix, arrMarker[column])))
//            {
//                countOperations++;
//                arrMarker[column] = row;
//                arrVertices[row] = column;
//                return true;
//            }
//        }
//    }
//    return false;
//}

//void alg_Ford_Fulkerson(int[,] matrix, int[] arrMarker, int[] arrVertices)
//{
//    //int maxMatching;

//    for (int i = 0; i < columns; i++)
//    {
//        countOperations++;
//        arrMarker[i] = -1;
//        arrVertices[i] = -1;
//    }

//    maxMatching = 0;
//    for (int row = 0; row < rows; row++)
//    {
//        bool[] arrVisited = new bool[columns];     // initialization array "arrVisited"
//        for (int i = 0; i < columns; i++)
//        {
//            countOperations++;
//            arrVisited[i] = false;
//        }

//        if (DFS(arrMarker, arrVisited, arrVertices, matrix, row))
//        {
//            countOperations++;
//            maxMatching++;
//        }
//    }
//}


/*void counter(int[,] matrix)
//{

//    helpMatrix = (int[,])inputMatrix.matrix.Clone();

//    resultPath = new List<sbyte>();
//    List<sbyte> allColumnsToProccess = new List<sbyte>();

//    for (sbyte i = 0; i < columns; i++)
//    {
//        allColumnsToProccess.Add(i);
//    }

//    System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
//    countOperations = 0;

//    ListVertices.Clear();
//    stopWatch.Start();      // start timer

//    recursive_matching_finder(allColumnsToProccess, 0);
//    all_matching_finder();

//    stopWatch.Stop();       // stop timer

//    strTimeAlg = string.Format("{0:N6}", stopWatch.Elapsed.TotalSeconds) + " с";
//} */








