using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GraduateWork_updated
{
    public partial class WindowRenderingOfResult : Window
    {
        // copy variables from solveGraph
        string typeAlgorithm;
        string typeMatrix;
        int numberThreads, columns;
        int countMaxMatching;
        int[] arrVertices;
        List<int> maxMatchingPos;
        List<List<sbyte>> ListOfVertices;
        int original_columns;

        bool ResizeInProcess;


        // constructor
        public WindowRenderingOfResult(string typeAlgorithm, string typeMatrix, int numberThreads, int columns, List<int> maxMatchingPos, List<List<sbyte>> ListVertices, int countMaxMatching, int[] arrVertices, int original_columns)
        {
            this.typeAlgorithm = typeAlgorithm;
            this.typeMatrix = typeMatrix;
            this.numberThreads = numberThreads;
            this.columns = columns;
            this.countMaxMatching = countMaxMatching;
            this.arrVertices = arrVertices;
            this.maxMatchingPos = maxMatchingPos;
            this.ListOfVertices = ListVertices;
            this.original_columns = original_columns;

            InitializeComponent();
        }

        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            switch (typeAlgorithm)
            {
                case MyResources.lblSingleThreadAlgorithm:
                    lbl_topPanel.Content = "Однопоточный алгоритм";
                    drawing_all_matchings_new();
                    break;

                case MyResources.lblMultiThreadAlgorithm:
                    lbl_topPanel.Content = "Многопоточный алгоритм | " + numberThreads + " потока";
                    drawing_all_matchings_new();
                    break;

                case MyResources.lblFordFulkersonAlgorithm:
                    lbl_topPanel.Content = "Алгоритм Форда-Фалкерсона";
                    drawing_matchings();
                    break;
            }

            this.Title = Convert.ToString(lbl_topPanel.Content);
            Mouse.OverrideCursor = null;
        }


        /*************** BEGIN calculation functions ***************/

        // drawing number vertex in circle 
        void drawing_a_txtBlck(Canvas canvas, int CanvasLeft, int CanvasTop, int numberInCircle)
        {
            // add number vertex in circle
            TextBlock txtBlock = new TextBlock();

            // set options 
            txtBlock.FontSize = 34;
            txtBlock.Text = Convert.ToString(numberInCircle);

            // set Z-index 
            Panel.SetZIndex(txtBlock, 1);

            if (numberInCircle >= 10)
                CanvasLeft -= 10;

            // set left number vertex
            Canvas.SetLeft(txtBlock, CanvasLeft);
            Canvas.SetTop(txtBlock, CanvasTop);
            canvas.Children.Add(txtBlock);
        }

        // drawing an ellipse 
        void drawing_an_ellipse(int CenterLeft, int CenterTop, int radiusX, int radiusY, GeometryGroup geometryGroup)
        {
            Point center;
            EllipseGeometry EllipseGeom;

            center = new Point(CenterLeft, CenterTop);
            EllipseGeom = new EllipseGeometry(center, radiusX, radiusY);

            // add Ellipse to GeometryGroup
            geometryGroup.Children.Add(EllipseGeom);
        }

        // drawing a line
        void drawing_a_line(int startPoint_1, int startPoint_2, int endPoint_1, int endPoint_2, GeometryGroup geomGroupLines)
        {
            LineGeometry line = new LineGeometry();
            Point startPoint = new Point(startPoint_1, startPoint_2);
            Point endPoint = new Point(endPoint_1, endPoint_2);

            line.StartPoint = startPoint;
            line.EndPoint = endPoint;

            geomGroupLines.Children.Add(line);
        }

        // drawing matchings (Ford-Fulkerson)
        void drawing_matchings()
        {
            // options for Ellipse
            int radiusX, radiusY;
            int EllipseLeft_CenterLeft, EllipseLeft_CenterTop;
            int EllipseRight_CenterLeft, EllipseRigth_CenterTop;

            // options for TextBlock
            int CanvasTop_LeftNumber, CanvasLeft_LeftNumber;
            int CanvasTop_RigthNumber, CanvasLeft_RightNumber;

            // options for lines
            int startPoint_1, startPoint_2;
            int endPoint_1, endPoint_2;

            int numberInCircle;

            // elements for insert
            var scrViewer = new ScrollViewer();
            var canvas = new Canvas();
            var txtBlock = new TextBlock();
            var wrapPanel = new WrapPanel();
            
            Path pathMain = new Path();
            Path pathLines = new Path();
            GeometryGroup geometryGroup = new GeometryGroup();
            GeometryGroup geomGroupLines = new GeometryGroup();

            // initialization variables 
            EllipseLeft_CenterLeft = 0;
            EllipseLeft_CenterTop = 0;
            EllipseRight_CenterLeft = 0;
            EllipseRigth_CenterTop = 0;

            CanvasTop_LeftNumber = 0;
            CanvasLeft_LeftNumber = 0;

            CanvasTop_RigthNumber = 0;
            CanvasLeft_LeftNumber = 0;

            radiusX = 30;
            radiusY = 30;

            numberInCircle = 0;

            // set options for ScrollViewer
            scrViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrViewer.Name = "scrViewer";

            // set options for WrapPanel
            wrapPanel.Background = new SolidColorBrush(Colors.White);
            wrapPanel.ItemWidth = 340;
            wrapPanel.ItemHeight = (20 + (columns * 135));

            // set options for TextBlock
            txtBlock.Text = "Максимальное";
            txtBlock.FontSize = 34;
            txtBlock.TextAlignment = TextAlignment.Center;

            // add textBlock in canvas
            Canvas.SetLeft(txtBlock, 65);
            Canvas.SetTop(txtBlock, 10);
            canvas.Children.Add(txtBlock);

            // values a path element
            pathMain.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF034F84"));
            pathMain.StrokeThickness = 2;
            pathMain.Fill = new SolidColorBrush(Colors.White);

            EllipseLeft_CenterLeft = 75;
            EllipseLeft_CenterTop = 100;

            EllipseRight_CenterLeft = 300;
            EllipseRigth_CenterTop = 100;

            CanvasTop_LeftNumber = 76;
            CanvasLeft_LeftNumber = 66;

            CanvasTop_RigthNumber = 76;
            CanvasLeft_RightNumber = 292;

            startPoint_1 = 75;
            startPoint_2 = 100;
            endPoint_1 = 300;

            // set options for lines
            pathLines.Stroke = new SolidColorBrush(Colors.Black);
            pathLines.StrokeThickness = 2;

            for (int column = 0; column < columns; column++)
            {
                // drawing a lines
                if (arrVertices[column] != -1)
                {
                    endPoint_2 = 100;
                    endPoint_2 += (arrVertices[column] * 125);
                    drawing_a_line(startPoint_1, startPoint_2, endPoint_1, endPoint_2, geomGroupLines);
                }

                if (typeMatrix == MyResources.lblAdjacencyMatrix)
                    numberInCircle = (column + original_columns / 2);
                else
                    numberInCircle = column;

                // drawing a TextBlock
                drawing_a_txtBlck(canvas, CanvasLeft_LeftNumber, CanvasTop_LeftNumber, column);
                drawing_a_txtBlck(canvas, CanvasLeft_RightNumber, CanvasTop_RigthNumber, numberInCircle);
                
                // drawing a Ellipse
                drawing_an_ellipse(EllipseLeft_CenterLeft, EllipseLeft_CenterTop, radiusX, radiusY, geometryGroup);
                drawing_an_ellipse(EllipseRight_CenterLeft, EllipseRigth_CenterTop, radiusX, radiusY, geometryGroup);

                // update variables for the next rendering 
                EllipseLeft_CenterTop += 125;
                EllipseRigth_CenterTop += 125;

                CanvasTop_LeftNumber += 125;
                CanvasTop_RigthNumber += 125;

                startPoint_2 += 125;
            }

            // add lines in Canvas
            pathLines.Data = geomGroupLines;
            Panel.SetZIndex(pathLines, -1);
            canvas.Children.Add(pathLines);

            pathMain.Data = geometryGroup;
            canvas.Children.Add(pathMain);

            wrapPanel.Children.Add(canvas);

            // add WrapPanel in ScrollViewer
            scrViewer.Content = wrapPanel;

            // add scrollViewer in Grid
            Grid.SetRow(scrViewer, 1);
            Grid.SetColumn(scrViewer, 1);
            grid_rendering.Children.Add(scrViewer);
        }

        // TEST drawing all matchings (single/multi flow algorithms) 
        void drawing_all_matchings_new()
        {
            // options for Ellipse
            int radiusX, radiusY;
            int EllipseLeft_CenterLeft, EllipseLeft_CenterTop;
            int EllipseRight_CenterLeft, EllipseRigth_CenterTop;

            // options for TextBlock
            int CanvasTop_LeftNumber, CanvasLeft_LeftNumber;
            int CanvasTop_RigthNumber, CanvasLeft_RightNumber;

            // options for lines
            int startPoint_1, startPoint_2;
            int endPoint_1, endPoint_2;

            int numberInCircle;

            // elements for insert
            var scrViewer = new ScrollViewer();
            var wrapPanel = new WrapPanel();

            // set options for WrapPanel
            wrapPanel.Background = new SolidColorBrush(Colors.White);
            wrapPanel.ItemWidth = 340;
            wrapPanel.ItemHeight = (20 + (columns * 135));

            // initialization variables 
            EllipseLeft_CenterLeft = 0;
            EllipseLeft_CenterTop = 0;
            EllipseRight_CenterLeft = 0;
            EllipseRigth_CenterTop = 0;

            CanvasTop_LeftNumber = 0;
            CanvasLeft_LeftNumber = 0;

            CanvasTop_RigthNumber = 0;
            CanvasLeft_LeftNumber = 0;

            radiusX = 30;
            radiusY = 30;

            numberInCircle = 0;

            // set options for ScrollViewer
            scrViewer.Name = "scrViewer";
            scrViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

            // drawing all matchings
            for (int row = 0; row < countMaxMatching; row++)
            {
                var canvas = new Canvas();
                var txtBlock = new TextBlock();
                Path pathEllipse = new Path();
                Path pathLines = new Path();
                GeometryGroup geometryGroup = new GeometryGroup();
                GeometryGroup geomGroupLines = new GeometryGroup();

                // set options for TextBlock
                txtBlock.Text = "Паросочетание №" + (row + 1);
                txtBlock.FontSize = 34;
                txtBlock.TextAlignment = TextAlignment.Center;

                // set options for Lines
                pathLines.Stroke = new SolidColorBrush(Colors.Black);
                pathLines.StrokeThickness = 2;

                // set options for Ellipse
                pathEllipse.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF034F84"));
                pathEllipse.StrokeThickness = 2;
                pathEllipse.Fill = new SolidColorBrush(Colors.White);

                // set values for elements
                EllipseLeft_CenterLeft = 65;
                EllipseLeft_CenterTop = 100;

                EllipseRight_CenterLeft = 290;
                EllipseRigth_CenterTop = 100;

                CanvasTop_LeftNumber = 76;
                CanvasLeft_LeftNumber = 56;

                CanvasTop_RigthNumber = 76;
                CanvasLeft_RightNumber = 282;

                startPoint_1 = 65;
                startPoint_2 = 100;
                endPoint_1 = 285;

                // add TextBlock in Canvas
                Canvas.SetTop(txtBlock, 0);
                Canvas.SetLeft(txtBlock, 25);
                canvas.Children.Add(txtBlock);
                
                for (int column = 0; column < columns; column++)
                {
                    // drawing a lines
                    if (ListOfVertices[maxMatchingPos[row]][column] != -1)
                    {
                        endPoint_2 = 100;
                        endPoint_2 += (ListOfVertices[maxMatchingPos[row]][column] * 125);
                        drawing_a_line(startPoint_1, startPoint_2, endPoint_1, endPoint_2, geomGroupLines);
                    }

                    // set numbering in Ellipse
                    if (typeMatrix == MyResources.lblAdjacencyMatrix)
                        numberInCircle = (column + original_columns / 2);
                    else
                        numberInCircle = column;

                    // drawing a TextBlock 
                    drawing_a_txtBlck(canvas, CanvasLeft_LeftNumber, CanvasTop_LeftNumber, column);
                    drawing_a_txtBlck(canvas, CanvasLeft_RightNumber, CanvasTop_RigthNumber, numberInCircle);

                    // drawing a Ellipse
                    drawing_an_ellipse(EllipseLeft_CenterLeft, EllipseLeft_CenterTop, radiusX, radiusY, geometryGroup);
                    drawing_an_ellipse(EllipseRight_CenterLeft, EllipseRigth_CenterTop, radiusX, radiusY, geometryGroup);

                    // update variables for the next rendering 
                    EllipseLeft_CenterTop += 125;
                    EllipseRigth_CenterTop += 125;

                    CanvasTop_LeftNumber += 125;
                    CanvasTop_RigthNumber += 125;

                    startPoint_2 += 125;
                }

                // add lines in Canvas
                pathLines.Data = geomGroupLines;
                Panel.SetZIndex(pathLines, -1);
                canvas.Children.Add(pathLines);

                // add Ellipse in Canvas
                pathEllipse.Data = geometryGroup;
                canvas.Children.Add(pathEllipse);

                wrapPanel.Children.Add(canvas);
            }

            // add canvas in ScrollViewer
            scrViewer.Content = wrapPanel;

            // add scrollViewer in Grid
            Grid.SetRow(scrViewer, 1);
            Grid.SetColumn(scrViewer, 1);
            grid_rendering.Children.Add(scrViewer);
        }

        /*************** END calculation functions ***************/


        // change application window
        void btnMinimize_window_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        void btnRestore_window_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                //scrViewer.Height = 680;

                // Compensate for the extra space WPF adds by increasing the max width and height here
                this.MaxHeight = SystemParameters.WorkArea.Height + 7;
                this.MaxWidth = SystemParameters.WorkArea.Width + 7;
                grid_mainContainer.Margin = new Thickness(5, 5, 0, 0);
                WindowState = WindowState.Maximized;

            }

            else
                if (WindowState == WindowState.Maximized)
                {
                    grid_mainContainer.Margin = new Thickness(0);
                    WindowState = WindowState.Normal;
                }
        }
        void btnClose_window_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void top_docPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();

            // maximized and normal window state
            if (e.ClickCount == 2)
            {
                if (WindowState == WindowState.Normal)
                {
                    // Compensate for the extra space WPF adds by increasing the max width and height here
                    MaxHeight = SystemParameters.WorkArea.Height + 7;
                    MaxWidth = SystemParameters.WorkArea.Width + 7;

                    grid_mainContainer.Margin = new Thickness(5, 5, 0, 0);
                    WindowState = WindowState.Maximized;
                }

                else
                    if (WindowState == WindowState.Maximized)
                {
                    grid_mainContainer.Margin = new Thickness(0);
                    WindowState = WindowState.Normal;
                }
            }
        }

        // change size window application
        #region ResizeWindows

        private void Resize_Init(object sender, MouseButtonEventArgs e)
        {
            Rectangle senderRect = sender as Rectangle;
            if (senderRect != null)
            {
                ResizeInProcess = true;
                senderRect.CaptureMouse();
            }
        }

        private void Resize_End(object sender, MouseButtonEventArgs e)
        {
            Rectangle senderRect = sender as Rectangle;
            if (senderRect != null)
            {
                ResizeInProcess = false; ;
                senderRect.ReleaseMouseCapture();
            }
        }

        private void Resizeing_Form(object sender, MouseEventArgs e)
        {
            if (ResizeInProcess)
            {
                Rectangle senderRect = sender as Rectangle;
                Window mainWindow = senderRect.Tag as Window;
                if (senderRect != null)
                {
                    double width = e.GetPosition(mainWindow).X;
                    double height = e.GetPosition(mainWindow).Y;
                    senderRect.CaptureMouse();
                    if (senderRect.Name.ToLower().Contains("right"))
                    {
                        width += 5;
                        if (width > 0)
                            mainWindow.Width = width;
                    }
                    if (senderRect.Name.ToLower().Contains("left"))
                    {
                        width -= 5;
                        mainWindow.Left += width;
                        width = mainWindow.Width - width;
                        if (width > 0)
                        {
                            mainWindow.Width = width;
                        }
                    }
                    if (senderRect.Name.ToLower().Contains("bottom"))
                    {
                        height += 5;
                        if (height > 0)
                            mainWindow.Height = height;
                    }
                    if (senderRect.Name.ToLower().Contains("top"))
                    {
                        height -= 5;
                        mainWindow.Top += height;
                        height = mainWindow.Height - height;
                        if (height > 0)
                        {
                            mainWindow.Height = height;
                        }
                    }
                }
            }
        }

        #endregion

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                topLeftSizeGrip.IsEnabled = false;
                topRightSizeGrip.IsEnabled = false;
                topSizeGrip.IsEnabled = false;

                rightSizeGrip.IsEnabled = false;
                leftSizeGrip.IsEnabled = false;

                bottomLeftSizeGrip.IsEnabled = false;
                bottomRightSizeGrip.IsEnabled = false;
                bottomLeftSizeGrip.IsEnabled = false;
            }

            else
            {
                topLeftSizeGrip.IsEnabled = true;
                topRightSizeGrip.IsEnabled = true;
                topSizeGrip.IsEnabled = true;

                rightSizeGrip.IsEnabled = true;
                leftSizeGrip.IsEnabled = true;

                bottomLeftSizeGrip.IsEnabled = true;
                bottomRightSizeGrip.IsEnabled = true;
                bottomLeftSizeGrip.IsEnabled = true;
            }
        }
    }
}
