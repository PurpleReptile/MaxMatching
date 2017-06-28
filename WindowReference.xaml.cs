using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace GraduateWork_updated
{
    public partial class WindowReference : Window
    {
        bool ResizeInProcess;

        public WindowReference()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            filling_content();

            if (MyResources.reference == 0)
                tab_aboutInput.IsSelected = true;
            else
                if (MyResources.reference == 1)
                tab_aboutAlgorithm.IsSelected = true;
        }

        void filling_content()
        {
            string strInfoAboutUserInput, strInfoAboutFileInput, strInfoAboutGenerationInput;
            string strInfoAboutAlgorithms;
            string strSingleThreadedAlgorithm, strMultiThreadedAlgorithm, strFordFulkersonAlgorithm;
            string strInfoAboutProgram;

            strInfoAboutUserInput = "";
            strInfoAboutFileInput = "";
            strInfoAboutGenerationInput = "";
            strInfoAboutAlgorithms = "";
            strSingleThreadedAlgorithm = "";
            strMultiThreadedAlgorithm = "";
            strFordFulkersonAlgorithm = "";
            strInfoAboutProgram = "";

            strInfoAboutUserInput = File.ReadAllText("..\\..\\Information\\ContentAboutUserInput.txt");
            strInfoAboutFileInput = File.ReadAllText("..\\..\\Information\\ContentAboutFileInput.txt");
            strInfoAboutGenerationInput = File.ReadAllText("..\\..\\Information\\ContentAboutGenerationInput.txt");
            strInfoAboutAlgorithms = File.ReadAllText("..\\..\\Information\\aboutAlgorithm.txt");
            strSingleThreadedAlgorithm = File.ReadAllText("..\\..\\Information\\singleThreadedAlgorithm.txt");
            strMultiThreadedAlgorithm = File.ReadAllText("..\\..\\Information\\multiThreadedAlgorithm.txt");
            strFordFulkersonAlgorithm = File.ReadAllText("..\\..\\Information\\fordFulkersonAlgorithm.txt");
            strInfoAboutProgram = File.ReadAllText("..\\..\\Information\\aboutProgram.txt");

            // Filling tab about input data
            prghAboutUserInput.Text = Convert.ToString(strInfoAboutUserInput);
            prghAboutFileInput.Text = Convert.ToString(strInfoAboutFileInput);
            prghAboutGenerationInput.Text = Convert.ToString(strInfoAboutGenerationInput);

            // Filling tab about algorithms
            prghAboutAlgorithms.Text = Convert.ToString(strInfoAboutAlgorithms);
            prghSingleThreadedAlgorithm.Text = Convert.ToString(strSingleThreadedAlgorithm);
            prghMultiThreadedAlgorithm.Text = Convert.ToString(strMultiThreadedAlgorithm);
            prghFordFulkersonAlgorithm.Text = Convert.ToString(strFordFulkersonAlgorithm);

            // Filling tab about program
            prghAboutProgram.Text = Convert.ToString(strInfoAboutProgram);
        }

        private void btnClose_window_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void stackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();

            if (e.ClickCount == 2)
            {
                if (WindowState == WindowState.Normal)
                {
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
        }

        private void btnMinimizeWindow_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnRestore_window_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
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

        private void mainWindow_StateChanged(object sender, EventArgs e)
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
