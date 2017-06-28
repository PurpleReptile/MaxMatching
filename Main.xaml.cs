using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace GraduateWork_updated
{
    public partial class Main : Window
    {
        /*************** BEGIN initialization const variables ***************/

        // labels the number of available threads
        int numberThreads;
        string typeInput;

        bool ResizeInProcess;


        // initialization classes
        solveGraph class_Graph;
        /*************** END initialization const variables ***************/

        public Main()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //System.Diagnostics.Process.Start("chrome.exe", "https://www.youtube.com/watch?v=XrTZT49u0kM&index=24&list=PLD15A95D4A6CD2457");

            initialization_variables();

            if (numberThreads <= 1)
                MessageBox.Show("Извините, но на данной ЭВМ не доступен многопоточный алгоритм",
                    "Технические ограничения", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        bool close_window()
        {
            string message, caption;
            MessageBoxButton button;
            MessageBoxImage icon;
            MessageBoxResult result;

            message = "Вы уверены, что хотите закрыть приложение?";
            caption = "Выход";

            button = MessageBoxButton.YesNo;
            icon = MessageBoxImage.Question;
            result = MessageBox.Show(message, caption, button, icon);

            if (result == MessageBoxResult.Yes)
                return true;

            return false;
        }

        void initialization_variables()
        {
            numberThreads = Environment.ProcessorCount;

            //btnSaveInFile.IsEnabled = false;
            typeInput = "keyboard";
            default_values();
            radBtn_keyboard.IsChecked = true;

            fill_cmbBox_typeAlgorithms();
        }

        void btnReset_Click(object sender, RoutedEventArgs e)
        {
            if (("file" == typeInput) || ("random" == typeInput) || ("keyboard" == typeInput))
                reset_values();
        }
        void btnReadFile_Click(object sender, RoutedEventArgs e)
        {
            string errMessage;
            string errCaption;
            MessageBoxButton errButton;
            MessageBoxImage errIcon;

            try
            {
                if (read_matrix_from_file())
                {
                    fill_information_about_file();

                    cmbBoxTypeAlgorithms.Items.Clear();         // clear old information in combobox
                    
                    if (cmbBoxTypeAlgorithms.IsEnabled == true)
                        fill_cmbBox_typeAlgorithms();           // filling combobox new information
                    else
                        cmbBoxTypeAlgorithms.IsEnabled = true;

                    isEnable_buttons(false, true, true, false);
                    gridMatrixContent.IsEnabled = false;

                    txtBlck_matrixNotLoaded.Visibility = Visibility.Hidden;
                }
            }

            catch
            {
                errMessage = "Извините, но структура данных текущего файла некорректна!";
                errCaption = "Некорректные данные";
                errButton = MessageBoxButton.OK;
                errIcon = MessageBoxImage.Exclamation;

                MessageBox.Show(errMessage, errCaption, errButton, errIcon);
            }
        }
        void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            string message, caption;
            MessageBoxButton button;
            MessageBoxImage icon;
            MessageBoxResult result;
            string typeMatrix;

            caption = "Внимание";
            message = "Вы потеряете текущие результаты вычислений. Хотите продолжить?";
            button = MessageBoxButton.YesNo;
            icon = MessageBoxImage.Exclamation;
            typeMatrix = "";

            typeMatrix = get_current_type_input_matrix();

            switch (btnSaveInFile.IsEnabled)
            {
                case true:
                    result = MessageBox.Show(message, caption, button, icon);

                    if (result == MessageBoxResult.Yes)
                    {
                        save_result(true);

                        if ("keyboard" == typeInput)
                            update_generatedMatrix(true, false);
                        else
                            switch (typeMatrix)
                            {
                                case MyResources.lblReducedMatrix:
                                    update_generatedMatrix(false, false);
                                    break;

                                case MyResources.lblAdjacencyMatrix:
                                    update_generatedMatrix(false, true);
                                    break;
                            }

                        default_field_results();
                        isEnable_buttons(false, true, false, false);
                    }
                    break;

                case false:
                    if ("keyboard" == typeInput)
                        update_generatedMatrix(true, false);
                    else
                        switch (typeMatrix)
                        {
                            case MyResources.lblReducedMatrix:
                                update_generatedMatrix(false, false);
                                break;

                            case MyResources.lblAdjacencyMatrix:
                                update_generatedMatrix(false, true);
                                break;
                        }


                    isEnable_buttons(false, true, false, false);
                    break;
            }
        }
        void btnCompute_Click(object sender, RoutedEventArgs e)
        {
            string message, caption;
            MessageBoxButton button;
            MessageBoxImage icon;
            MessageBoxResult result;

            caption = "Внимание";
            message = "Вы потеряете текущие результаты вычислений. Хотите продолжить?";
            button = MessageBoxButton.YesNo;
            icon = MessageBoxImage.Exclamation;

            if (true == btnSaveInFile.IsEnabled)
            {
                result = MessageBox.Show(message, caption, button, icon);

                if (result == MessageBoxResult.Yes)
                {
                    save_result(true);
                    default_field_results();
                    btnResultOfRendering.IsEnabled = false;
                    btnSaveInFile.IsEnabled = false;
                }
            }

            else
                compute();
        }
        void btnSaveInFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                save_result(false);
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void compute()
        {
            string currAlg, currTypeMatrix, currTypeAlg;
            int currentNumberThreads;
            
            // initialization variables
            currAlg = "";
            currTypeMatrix = "";
            currTypeAlg = "";
            currentNumberThreads = 1;

            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                // get value data about algorithm
                if (cmbBoxTypeAlgorithms.HasItems)
                    currTypeAlg = Convert.ToString(cmbBoxTypeAlgorithms.SelectedItem);

                if (cmbBoxAlgorithms_maxMatching.HasItems)
                    currAlg = Convert.ToString(cmbBoxAlgorithms_maxMatching.SelectedItem);

                if (cmbBoxAlgorithms_numberMatching.HasItems)
                    currAlg = Convert.ToString(cmbBoxAlgorithms_numberMatching.SelectedItem);

                if (cmbBoxAlgorithms_numberThreads.HasItems)
                    currentNumberThreads = Convert.ToInt32(cmbBoxAlgorithms_numberThreads.SelectedItem);

                switch (btnReadFile.IsEnabled)
                {
                    case true:
                        if (txtBoxTypeMatrix.Text != null)
                            currTypeMatrix = txtBoxTypeMatrix.Text.ToString();
                        break;

                    case false:
                        if (true == cmbBoxTypeMatrix.IsEnabled && cmbBoxTypeMatrix.HasItems)
                            currTypeMatrix = cmbBoxTypeMatrix.SelectedValue.ToString();
                        break;
                }

                class_Graph.solver(currentNumberThreads, currTypeAlg, currAlg, currTypeMatrix);

                // error checking 
                if (class_Graph.class_Errors.is_error())
                {
                    // matrix is empty
                    if (class_Graph.class_Errors.matrixIsEmpty)
                        class_Graph.class_Errors.message_matrix_is_empty();

                    // matrix is asymmetrical
                    if (class_Graph.class_Errors.matrixIsAsymmetrical)
                        class_Graph.class_Errors.message_matrix_is_asymmetrical();

                    return; // exit from method
                }

                else
                {
                    fill_result_to_textbox(currAlg);
                    btnSaveInFile.IsEnabled = true;
                    btnResultOfRendering.IsEnabled = true;

                    if ("random" == typeInput || "keyboard" == typeInput)
                        btnReset.IsEnabled = true;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        void changeSizeInputMatrix_Click(object sender, RoutedEventArgs e)
        {
            int contentRows, contentColumns;
            string typeInputMatrix, btnName;
            bool isAdjacency;
            Button btnClick;

            typeInputMatrix = "";
            isAdjacency = false;

            contentRows = Int32.Parse(txtBoxNumRows.Text);
            contentColumns = Int32.Parse(txtBoxNumColumns.Text);

            btnName = (sender as Button).Name.ToString();
            btnClick = (sender as Button);

            // get the type of the original matrix
            typeInputMatrix = get_current_type_input_matrix();

            if (typeInputMatrix != null)
            {
                // the choice of method for data processing
                btn_control_sizeMatrix_square_matrix(btnName, contentColumns, contentRows, isAdjacency);
            }
        }

        void btn_control_sizeMatrix_adjacency_matrix(string btnName, int contentColumns, int contentRows, bool isAdjacency)
        {
            int minValue, maxValue;
            string typeMatrix;
            int stepInMinus, stepInPlus;

            minValue = 0;
            maxValue = 0;
            stepInMinus = 0;
            stepInPlus = 0;
            typeMatrix = "";

            minValue = get_current_min_size_of_matrix();
            maxValue = get_current_max_size_of_matrix();
            typeMatrix = get_current_type_input_matrix();

            if (typeMatrix == MyResources.lblAdjacencyMatrix)
            {
                stepInMinus = 2;
                stepInPlus = 2;
            }
            else
                if (typeMatrix == MyResources.lblReducedMatrix)
            {
                stepInMinus = 1;
                stepInPlus = 1;
            }

            switch (btnName)
            {
                case "btnArrowMinusColumn":
                    if (contentColumns - 1 == minValue)
                        btnArrowMinusColumn.IsEnabled = false;
                    else
                    {
                        btnArrowMinusColumn.IsEnabled = true;
                        btnArrowPlusColumn.IsEnabled = true;
                    }
                    contentColumns -= stepInMinus;
                    break;

                case "btnArrowPlusColumn":

                    if (contentColumns + 1 == maxValue)
                        btnArrowPlusColumn.IsEnabled = false;
                    else
                    {
                        btnArrowPlusColumn.IsEnabled = true;
                        btnArrowMinusColumn.IsEnabled = true;
                    }
                    contentColumns += stepInPlus;
                    break;

                case "btnArrowMinusRow":

                    if (contentRows - 1 == minValue)
                        btnArrowMinusRow.IsEnabled = false;
                    else
                    {
                        btnArrowMinusRow.IsEnabled = true;
                        btnArrowPlusRow.IsEnabled = true;
                    }
                    contentRows -= stepInMinus;
                    break;

                case "btnArrowPlusRow":

                    if (contentRows + 1 == maxValue)
                        btnArrowPlusRow.IsEnabled = false;
                    else
                    {
                        btnArrowPlusRow.IsEnabled = true;
                        btnArrowMinusRow.IsEnabled = true;
                    }
                    contentRows += stepInPlus;
                    break;
            }

            txtBoxNumRows.Text = contentRows.ToString();
            txtBoxNumColumns.Text = contentColumns.ToString();

            if ("keyboard" == typeInput)
                update_generatedMatrix(true, false);
            else
                switch (typeMatrix)
                {
                    case MyResources.lblReducedMatrix:
                        update_generatedMatrix(false, false);
                        break;

                    case MyResources.lblAdjacencyMatrix:
                        update_generatedMatrix(false, true);
                        break;
                }
        }
        void btn_control_sizeMatrix_square_matrix(string btnName, int contentColumns, int contentRows, bool isAdjacency)
        {
            int minValue, maxValue;
            string typeMatrix;
            int stepInMinus, stepInPlus;

            minValue = 0;
            maxValue = 0;
            stepInMinus = 0;
            stepInPlus = 0;
            typeMatrix = "";

            minValue = get_current_min_size_of_matrix();
            maxValue = get_current_max_size_of_matrix();
            typeMatrix = get_current_type_input_matrix();

            // set range minus/plus step on buttons
            if (typeMatrix == MyResources.lblAdjacencyMatrix)
            {
                stepInMinus = 2;
                stepInPlus = 2;
            }
            else
                if (typeMatrix == MyResources.lblReducedMatrix)
            {
                stepInMinus = 1;
                stepInPlus = 1;
            }

            if (("btnArrowMinusColumn" == btnName) || ("btnArrowMinusRow" == btnName))
            {
                if (((contentColumns - stepInMinus) == minValue) || ((contentRows - stepInMinus) == minValue))
                {
                    btnArrowMinusColumn.IsEnabled = false;
                    btnArrowMinusRow.IsEnabled = false;
                }
                else
                {
                    btnArrowMinusColumn.IsEnabled = true;
                    btnArrowPlusColumn.IsEnabled = true;
                    btnArrowMinusRow.IsEnabled = true;
                    btnArrowPlusRow.IsEnabled = true;
                }

                contentRows -= stepInMinus;
                contentColumns -= stepInMinus;
            }

            if (("btnArrowPlusColumn" == btnName) || ("btnArrowPlusRow" == btnName))
            {
                if (((contentColumns + stepInPlus) == maxValue) || ((contentRows + stepInPlus) == maxValue))
                {
                    btnArrowPlusColumn.IsEnabled = false;
                    btnArrowPlusRow.IsEnabled = false;
                }
                else
                {
                    btnArrowMinusColumn.IsEnabled = true;
                    btnArrowPlusColumn.IsEnabled = true;
                    btnArrowMinusRow.IsEnabled = true;
                    btnArrowPlusRow.IsEnabled = true;
                }

                contentRows += stepInPlus;
                contentColumns += stepInPlus;
            }

            txtBoxNumRows.Text = contentRows.ToString();
            txtBoxNumColumns.Text = contentColumns.ToString();

            if ("keyboard" == typeInput)
                update_generatedMatrix(true, false);
            else
                switch (typeMatrix)
                {
                    case MyResources.lblReducedMatrix:
                        update_generatedMatrix(false, false);
                        break;

                    case MyResources.lblAdjacencyMatrix:
                        update_generatedMatrix(false, true);
                        break;
                }
        }

        void dataRadBtnChanged(object sender, RoutedEventArgs e)
        {
            string nameRadBtn;

            nameRadBtn = (sender as RadioButton).Name.ToString();

            if ((false == btnSaveInFile.IsEnabled) &&
                ((gridMatrixContent.Children.Count == 0) || ("random" == typeInput) || ("keyboard" == typeInput)))
            {
                statusBarInfo.Content = (sender as RadioButton).Content.ToString() + " > ";
                switch (nameRadBtn)
                {
                    // if activated mode input "from file" 
                    case "radBtn_file":
                        default_setting_for_radBtn_file();
                        break;

                    // if activated mode input "random"
                    case "radBtn_random":
                        default_value_sizeMatrix();
                        default_setting_for_radBtn_random();
                        break;

                    // if activated mode input "with key"
                    case "radBtn_keyboard":
                        default_value_sizeMatrix();
                        default_setting_for_radBtn_keyboard();
                        break;

                    default:
                        break;
                }
            }
        }

        void radBtn_Unchecked(object sender, RoutedEventArgs e)
        {
            string message, caption;
            MessageBoxButton button;
            MessageBoxImage icon;
            MessageBoxResult result;
            string currTypeInput;

            caption = "Внимание";
            message = "Вы потеряете все текущие настройки. Хотите продолжить?";
            button = MessageBoxButton.YesNo;
            icon = MessageBoxImage.Exclamation;

            currTypeInput = (sender as RadioButton).Name.ToString();

            switch (currTypeInput)
            {
                case "radBtn_file":
                    currTypeInput = "file";
                    break;

                case "radBtn_random":
                    currTypeInput = "random";
                    break;

                case "radBtn_keyboard":
                    currTypeInput = "keyboard";
                    break;
            }

            if ((gridMatrixContent.Children.Count != 0) && (typeInput == currTypeInput))
            {
                if (("file" == typeInput) || ((("random" == typeInput) || ("keyboard" == typeInput)) && (btnSaveInFile.IsEnabled == true)))
                {
                    statusBarInfo.Content = (sender as RadioButton).Content.ToString() + " > ";
                    result = MessageBox.Show(message, caption, button, icon);
                    radBtn_change_Unchecked(result);
                }
            }

        }
        void radBtn_change_Unchecked(MessageBoxResult result)
        {
            if (MessageBoxResult.Yes == result)
            {
                if (true == btnSaveInFile.IsEnabled)
                {
                    save_result(true);
                    switch (typeInput)
                    {
                        case "keyboard":
                            cmbBoxTypeAlgorithms.Items.Clear();
                            fill_cmbBox_typeAlgorithms();
                            default_setting_for_radBtn_keyboard();
                            break;
                        case "random":
                            cmbBoxTypeAlgorithms.Items.Clear();
                            fill_cmbBox_typeAlgorithms();
                            default_setting_for_radBtn_random();
                            break;
                        case "file":
                            default_setting_for_radBtn_file();
                            break;
                    }
                }

                else
                {
                    switch (typeInput)
                    {
                        case "keyboard":
                            fill_cmbBox_typeAlgorithms();
                            default_setting_for_radBtn_keyboard();
                            break;
                        case "random":
                            fill_cmbBox_typeAlgorithms();
                            default_setting_for_radBtn_random();
                            break;
                        case "file":
                            default_setting_for_radBtn_file();
                            break;
                    }
                }
                default_values();
            }

            else
            {
                switch (typeInput)
                {
                    case "file":
                        radBtn_file.IsChecked = true;
                        break;
                    case "random":
                        //radBtn_random.IsChecked = true;
                        break;
                    case "keyboard":
                        radBtn_keyboard.IsChecked = true;
                        break;
                }
            }
        }

        void default_setting_for_radBtn_file()
        {
            string contentBtnReadFile;

            typeInput = "file";
            default_values();

            visible_buttons(false, false, true);
            isEnable_buttons(false, false, false, false);

            visible_panel_data_matrix(true, false);

            contentBtnReadFile = "Открыть файл";

            cmbBoxTypeAlgorithms.IsEnabled = false;

            gridMatrixContent.Children.Clear();
            gridMatrixContent.IsEnabled = false;

            if (btnReadFile.Content.ToString() != contentBtnReadFile)
                btnReadFile.Content = contentBtnReadFile;

            txtBlck_matrixNotLoaded.Visibility = Visibility.Visible;
            statusBarInfo_2.Content = "";
        }
        void default_setting_for_radBtn_random()
        {
            string typeInputMatrix;
            int minValue, maxValue;

            typeInputMatrix = "";
            minValue = 0;
            maxValue = 0;

            typeInput = "random";
            isEnable_buttons(false, true, false, false);
            visible_panel_data_matrix(false, true);

            cmbBoxTypeMatrix.Items.Clear();
            fill_cmbBox_typeMatrix();
            cmbBoxTypeAlgorithms.IsEnabled = true;

            // get current data for matrix setting
            typeInputMatrix = get_current_type_input_matrix();
            minValue = get_current_min_size_of_matrix();
            maxValue = get_current_max_size_of_matrix();

            lblRangeColumns.Content = "диапазон | " + minValue + " «---» " + maxValue;
            lblRangeRows.Content = "диапазон | " + minValue + " «---» " + maxValue;

            visible_buttons(false, true, false);
            gridMatrixContent.IsEnabled = false;

            // update current matrix
            update_generatedMatrix(false, false);

            txtBlck_matrixNotLoaded.Visibility = Visibility.Hidden;
        }
        void default_setting_for_radBtn_keyboard()
        {
            string typeInputMatrix;
            int minValue, maxValue;

            typeInputMatrix = "";
            minValue = 0;
            maxValue = 0;

            typeInput = "keyboard";
            isEnable_buttons(false, true, false, false);
            visible_panel_data_matrix(false, true);

            gridMatrixContent.IsEnabled = true;

            cmbBoxTypeMatrix.Items.Clear();
            fill_cmbBox_typeMatrix();
            cmbBoxTypeAlgorithms.IsEnabled = true;

            // get current data for matrix setting
            typeInputMatrix = get_current_type_input_matrix();
            minValue = get_current_min_size_of_matrix();
            maxValue = get_current_max_size_of_matrix();

            lblRangeColumns.Content = "диапазон | " + minValue + " «---» " + maxValue;
            lblRangeRows.Content = "диапазон | " + minValue + " «---» " + maxValue;

            visible_buttons(true, false, false);
            update_generatedMatrix(true, false);

            txtBlck_matrixNotLoaded.Visibility = Visibility.Hidden;
        }
        void default_value_sizeMatrix()
        {
            txtBoxNumColumns.Text = (MyResources.minSizeOfReduceMatrix + 2).ToString();
            txtBoxNumRows.Text = (MyResources.minSizeOfReduceMatrix + 2).ToString();

            btnArrowPlusRow.IsEnabled = true;
            btnArrowPlusColumn.IsEnabled = true;
            btnArrowMinusRow.IsEnabled = true;
            btnArrowMinusColumn.IsEnabled = true;
        }
        void default_values()
        {
            string fileIsNotSelected = "файл не выбран";
            string notRead = "не считано из файла";
            string notCalculated = "не вычислено";
            string strReadFile = "Открыть файл";

            txtBoxFileName.Text = fileIsNotSelected;
            txtBoxTypeFile.Text = fileIsNotSelected;
            txtBoxPathFile.Text = fileIsNotSelected;

            switch (typeInput)
            {
                case "file":
                    txtBoxTypeMatrix.Text = notRead;
                    txtBoxNumRows_file.Text = notRead;
                    txtBoxNumColumns_file.Text = notRead;
                    gridMatrixContent.IsEnabled = false;
                    break;

                case "random":
                    cmbBoxTypeMatrix.SelectedItem = 0;
                    default_value_sizeMatrix();
                    gridMatrixContent.IsEnabled = false;
                    break;
            }

            //txtBoxMatrixContent.Text = "";

            txtBoxCurrentAlgorithm.Text = notCalculated;
            txtBoxNumberThreads.Text = notCalculated;
            txtBoxComplexityAlgorithm.Text = notCalculated;
            txtBoxCountOperations.Text = notCalculated;
            txtBoxMaxMatching.Text = notCalculated;
            txtBoxPerfectMatching.Text = notCalculated;
            txtBoxTimeSpent.Text = notCalculated;

            btnReadFile.Content = strReadFile;

            isEnable_buttons(false, false, false, false);
        }
        void default_field_results()
        {
            string notCalculated = "не вычислено";

            txtBoxCurrentAlgorithm.Text = notCalculated;
            txtBoxNumberThreads.Text = notCalculated;
            txtBoxComplexityAlgorithm.Text = notCalculated;
            txtBoxCountOperations.Text = notCalculated;
            txtBoxMaxMatching.Text = notCalculated;
            txtBoxPerfectMatching.Text = notCalculated;
            txtBoxTimeSpent.Text = notCalculated;
        }

        bool save_result(bool askQuestion)
        {
            // to ask question 
            string message, caption;
            MessageBoxButton button;
            MessageBoxImage icon;
            MessageBoxResult resultQuestion;

            // to save
            string nameSaveFile;
            string currentTypeMatrix;
            string fullNameOpenFile;
            string pathOpenFile;
            Nullable<bool> resultSave;
            SaveFileDialog dlgSave;

            if (true == askQuestion)
            {
                message = "Хотите сохранить результат вычислений?";
                caption = "Сохранение результатов";
                button = MessageBoxButton.YesNo;
                icon = MessageBoxImage.Question;
                resultQuestion = MessageBox.Show(message, caption, button, icon);

                if (resultQuestion == MessageBoxResult.No)
                    return false;
            }

            fullNameOpenFile = "";
            pathOpenFile = "";

            dlgSave = new SaveFileDialog();
            dlgSave.Title = "Сохранение файла";
            dlgSave.FileName = "Документ";
            dlgSave.Filter = "Текстовый документ (.txt)|*.txt|Язык разметки (.xml)|*.xml|Веб-страница (.html)|*.html";
            dlgSave.FilterIndex = 2;
            dlgSave.RestoreDirectory = true;

            resultSave = dlgSave.ShowDialog();

            if (resultSave == true)
            {
                nameSaveFile = dlgSave.FileName;

                switch (typeInput)
                {
                    case "keyboard":
                        currentTypeMatrix = cmbBoxTypeMatrix.SelectedValue.ToString();
                        class_Graph.currTypeMatrix = currentTypeMatrix;

                        fullNameOpenFile = System.IO.Path.GetFileName(nameSaveFile);
                        pathOpenFile = System.IO.Path.GetDirectoryName(nameSaveFile);
                        break;

                    case "random":
                        currentTypeMatrix = cmbBoxTypeMatrix.SelectedValue.ToString();
                        class_Graph.currTypeMatrix = currentTypeMatrix;

                        fullNameOpenFile = System.IO.Path.GetFileName(nameSaveFile);
                        pathOpenFile = System.IO.Path.GetDirectoryName(nameSaveFile);
                        break;

                    case "file":
                        class_Graph.currTypeMatrix = txtBoxTypeMatrix.Text;
                        fullNameOpenFile = class_Graph.class_Matrix.fullNameOpenFile;
                        pathOpenFile = class_Graph.class_Matrix.pathOpenFile;
                        break;
                }

                class_Graph.result_to_file(nameSaveFile);
                message = "Файл " + fullNameOpenFile + "\n" + "успешно сохранён в директорию\n" + pathOpenFile;
                caption = "Сохранение";
                button = MessageBoxButton.OK;
                icon = MessageBoxImage.Information;
                MessageBox.Show(message, caption, button, icon);

                return true;
            }
            return false;
        }

        void visible_panel_data_matrix(bool forDataFromFile, bool forDataInputRandomOrKeyboard)
        {
            switch (forDataFromFile)
            {
                case true:
                    gridPanelDataFromFileInputMatrix.Visibility = Visibility.Visible;
                    gridPanelDataFromFileInputMatrix.IsEnabled = true;
                    break;

                case false:
                    gridPanelDataFromFileInputMatrix.Visibility = Visibility.Hidden;
                    gridPanelDataFromFileInputMatrix.IsEnabled = false;
                    break;

                default:
                    break;
            }

            switch (forDataInputRandomOrKeyboard)
            {
                case true:
                    gridPanelRandomDataInputMatrix.Visibility = Visibility.Visible;
                    gridPanelRandomDataInputMatrix.IsEnabled = true;
                    break;

                case false:
                    gridPanelRandomDataInputMatrix.Visibility = Visibility.Hidden;
                    gridPanelRandomDataInputMatrix.IsEnabled = false;
                    break;

                default:
                    break;
            }

        }
        void visible_buttons(bool modeKeyboard, bool modeGeneration, bool modeReadFile)
        {
            switch (modeKeyboard)
            {
                case true:
                    btnKeyboardHelp.Visibility = Visibility.Visible;
                    break;

                case false:
                    btnKeyboardHelp.Visibility = Visibility.Hidden;
                    break;
            }

            switch (modeGeneration)
            {
                case true:
                    btnGenerate.Visibility = Visibility.Visible;
                    btnGenerateHelp.Visibility = Visibility.Visible;
                    btnGenerate.IsEnabled = true;
                    break;

                case false:
                    btnGenerate.Visibility = Visibility.Hidden;
                    btnGenerateHelp.Visibility = Visibility.Hidden;
                    btnGenerate.IsEnabled = false;
                    break;

                default:
                    break;
            }

            switch (modeReadFile)
            {
                case true:
                    btnReadFile.Visibility = Visibility.Visible;
                    btnReadFileHelp.Visibility = Visibility.Visible;
                    btnReadFile.IsEnabled = true;
                    break;

                case false:
                    btnReadFile.Visibility = Visibility.Hidden;
                    btnReadFileHelp.Visibility = Visibility.Hidden;
                    btnReadFile.IsEnabled = false;
                    break;

                default:
                    break;
            }
        }

        void reset_values()
        {
            string message;
            string caption;
            MessageBoxButton button;
            MessageBoxImage icon;
            MessageBoxResult result;

            caption = "Сброс параметров";
            message = "Вы уверены, что хотите сбросить текущие настройки?";
            button = MessageBoxButton.YesNo;
            icon = MessageBoxImage.Question;
            result = MessageBox.Show(message, caption, button, icon);

            if (result == MessageBoxResult.Yes)
            {
                if (true == btnSaveInFile.IsEnabled)
                    save_result(true);

                switch (typeInput)
                {
                    case "file":
                        default_setting_for_radBtn_file();
                        break;

                    case "random":
                        default_setting_for_radBtn_random();
                        break;

                    case "keyboard":
                        default_setting_for_radBtn_keyboard();
                        break;
                }

                // clear field results, if it's need
                if ("file" != typeInput && (false == btnSaveInFile.IsEnabled))
                    default_field_results();
            }
        }
        void update_generatedMatrix(bool isZeroMatrix, bool isAdjacencyMatrix)
        {
            int rows, columns;

            rows = int.Parse(txtBoxNumRows.Text);
            columns = int.Parse(txtBoxNumColumns.Text);
            class_Graph = new solveGraph(rows, columns, isZeroMatrix, isAdjacencyMatrix);
            class_Graph.class_Matrix.get_matrix_view(gridMatrixContent, this);

            //txtBoxMatrixContent.Text = class_Graph.inputMatrix.matrix_to_string();
        }

        void isEnable_buttons(bool valSaveInFile, bool valCompute, bool valReset, bool valRenderingResult)
        {
            btnSaveInFile.IsEnabled = valSaveInFile;
            btnCompute.IsEnabled = valCompute;
            btnReset.IsEnabled = valReset;
            btnResultOfRendering.IsEnabled = valRenderingResult;
        }

        bool read_matrix_from_file()
        {
            Nullable<bool> result;
            OpenFileDialog dlgOpen;

            dlgOpen = new OpenFileDialog();
            dlgOpen.Title = "Выберите файл";
            dlgOpen.FileName = "Документ";
            dlgOpen.Filter = "Текстовый документ (.txt)|*.txt|Язык разметки (.xml)|*.xml";
            dlgOpen.FilterIndex = 2;

            result = dlgOpen.ShowDialog();

            if (true == result)
            {
                class_Graph = new solveGraph(dlgOpen.FileName);
                return true;
            }

            return false;
        }

        void fill_information_about_file()
        {
            string changeReadBtn;

            changeReadBtn = "Изменить";

            btnReadFile.Content = changeReadBtn;

            // information about input file
            txtBoxFileName.Text = class_Graph.class_Matrix.nameOpenFile;
            txtBoxTypeFile.Text = class_Graph.class_Matrix.typeOpenFile;
            txtBoxPathFile.Text = class_Graph.class_Matrix.pathOpenFile;

            // tool tip for information about input file
            txtBoxFileName.ToolTip = class_Graph.class_Matrix.nameOpenFile;
            txtBoxTypeFile.ToolTip = class_Graph.class_Matrix.typeOpenFile;
            txtBoxPathFile.ToolTip = class_Graph.class_Matrix.pathOpenFile;

            // information about input matrix
            txtBoxTypeMatrix.Text = class_Graph.class_Matrix.typeMatrix;
            txtBoxNumColumns_file.Text = class_Graph.class_Matrix.columns.ToString();
            txtBoxNumRows_file.Text = class_Graph.class_Matrix.rows.ToString();

            class_Graph.class_Matrix.get_matrix_view(gridMatrixContent, this);

            //update_generatedMatrix(false);
            //txtBoxMatrixContent.Text = class_Graph.inputMatrix.matrix_to_string();
        }
        void fill_result_to_textbox(string currAlg)
        {
            txtBoxCurrentAlgorithm.Text = currAlg;
            txtBoxNumberThreads.Text = class_Graph.numberThreads.ToString();
            txtBoxComplexityAlgorithm.Text = class_Graph.complexityAlgorithm.ToString();
            txtBoxCountOperations.Text = class_Graph.countOperations.ToString();
            txtBoxMaxMatching.Text = class_Graph.strResult;
            txtBoxPerfectMatching.Text = class_Graph.countPerfectMatching.ToString();
            txtBoxTimeSpent.Text = class_Graph.strTimeAlg;
        }

        /*************** BEGIN filling comboboxes information ***************/
        void fill_cmbBox_typeAlgorithms()
        {
            List<string> typeAlgorithms = new List<string>();

            typeAlgorithms.Add(MyResources.lblNumberMatching);
            typeAlgorithms.Add(MyResources.lblMaxMatching);

            // add items in combobox with type algorithm
            foreach (var algorithm in typeAlgorithms)
            {
                cmbBoxTypeAlgorithms.Items.Add(algorithm);
            }

            cmbBoxTypeAlgorithms.SelectedIndex = 0;
        }
        void fill_cmbBox_algorithms_for_maxMatching()
        {
            List<string> maxAlgorithms = new List<string>();

            // clear combobox, if not empty
            if (cmbBoxAlgorithms_maxMatching.HasItems)
                cmbBoxAlgorithms_maxMatching.Items.Clear();

            // filling combobox
            maxAlgorithms.Add(MyResources.lblFordFulkersonAlgorithm);

            // add items in combobox with algorithms
            foreach (var algorithm in maxAlgorithms)
            {
                cmbBoxAlgorithms_maxMatching.Items.Add(algorithm);
            }

            cmbBoxAlgorithms_maxMatching.SelectedIndex = 0;
        }
        void fill_cmbBox_algorithms_for_numberMatching()
        {
            List<string> numberAlgorithms = new List<string>();

            // clear combobox
            if (cmbBoxAlgorithms_numberMatching.HasItems)
                cmbBoxAlgorithms_numberMatching.Items.Clear();

            // filling combobox
            numberAlgorithms.Add(MyResources.lblSingleThreadAlgorithm);

            if (numberThreads != 0)
                numberAlgorithms.Add(MyResources.lblMultiThreadAlgorithm);

            // add items in combobox with algorithms
            foreach (var algorithm in numberAlgorithms)
            {
                cmbBoxAlgorithms_numberMatching.Items.Add(algorithm);
            }

            cmbBoxAlgorithms_numberMatching.SelectedIndex = 0;
        }
        void fill_cmbBox_typeMatrix()
        {
            List<string> typesMatrix = new List<string>();

            // clear combobox, if not empty
            if (cmbBoxTypeMatrix.HasItems)
                cmbBoxTypeMatrix.Items.Clear();

            // filling combobox types of matrix
            typesMatrix.Add(MyResources.lblReducedMatrix);
            typesMatrix.Add(MyResources.lblAdjacencyMatrix);

            // add items in combobox with algorithms
            foreach (var type in typesMatrix)
            {
                cmbBoxTypeMatrix.Items.Add(type);
            }

            cmbBoxTypeMatrix.SelectedIndex = 0;
        }
        void fill_cmbBoxAlgorithms_numberThreads()
        {
            for (int thread = 2; thread < numberThreads + 1; thread++)
            {
                cmbBoxAlgorithms_numberThreads.Items.Add(thread);
            }

            cmbBoxAlgorithms_numberThreads.SelectedIndex = 0;
        }
        /*************** END filling comboboxes information ***************/

        void inputText_txtBox_sizeMatrix(object sender, TextCompositionEventArgs e)
        {
            e.Handled = ("1234567890".IndexOf(e.Text) < 0);
        }
        void inputText_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if ((e.Command == ApplicationCommands.Cut) ||
                (e.Command == ApplicationCommands.Copy) ||
                (e.Command == ApplicationCommands.Paste))
            {
                e.Handled = true;
                e.CanExecute = false;
            }
        }

        // control limits for text box size matrix
        void changeText_txtBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int currentValue, minValue, maxValue;
            string typeMatrix;
            int errorMessage;

            string msgBoxMessage, msgBoxMessageAdjMatrix, msgBoxCaption;
            string txtBoxContent, txtBoxName;
            bool isSuccess;

            MessageBoxButton msgBoxBtn;
            MessageBoxImage msgBoxIcon;

            // initialization variables
            errorMessage = 0;
            currentValue = 0;
            typeMatrix = "";
            isSuccess = false;

            msgBoxMessage = " лимит был преодолён!";
            msgBoxMessageAdjMatrix = "Извините, но для матрицы смежности нужны размеры кратные 2";
            msgBoxCaption = "Внимание";
            msgBoxBtn = MessageBoxButton.OK;
            msgBoxIcon = MessageBoxImage.Exclamation;

            txtBoxContent = (sender as TextBox).Text;
            txtBoxName = (sender as TextBox).Name;

            if (cmbBoxTypeMatrix.HasItems == true)
                isSuccess = int.TryParse(txtBoxContent, out currentValue);

            // get current type matrix and her size
            typeMatrix = get_current_type_input_matrix();
            minValue = get_current_min_size_of_matrix();
            maxValue = get_current_max_size_of_matrix();

            if (typeMatrix != null)
            {
                // control for the other matrix
                if (isSuccess)
                {
                    currentValue = int.Parse(txtBoxContent);

                    // change size for adjacency matrix
                    if ((typeMatrix == MyResources.lblAdjacencyMatrix) && (currentValue % 2 != 0))
                    {
                        if ((currentValue + 1) == (maxValue + 1))
                        {
                            ((TextBox)sender).Text = Convert.ToString(currentValue - 1);
                        }
                        else
                            if ((currentValue > minValue) && (currentValue < maxValue))
                        {
                            ((TextBox)sender).Text = Convert.ToString(currentValue + 1);
                            errorMessage = 1;
                        }
                    }

                    if (currentValue > maxValue)
                    {
                        ((TextBox)sender).Text = Convert.ToString(maxValue);
                        errorMessage = 2;
                    }
                    else
                        if (currentValue < minValue)
                    {
                        ((TextBox)sender).Text = Convert.ToString(minValue);
                        errorMessage = 3;
                    }

                    switch (txtBoxName)
                    {
                        case "txtBoxNumColumns":
                            btnArrowPlusColumn.IsEnabled = true;
                            btnArrowMinusColumn.IsEnabled = true;

                            btnArrowPlusRow.IsEnabled = true;
                            btnArrowMinusRow.IsEnabled = true;

                            if (currentValue >= maxValue)
                            {
                                btnArrowPlusColumn.IsEnabled = false;
                                btnArrowMinusColumn.IsEnabled = true;

                                btnArrowPlusRow.IsEnabled = false;
                                btnArrowMinusRow.IsEnabled = true;
                            }
                            else
                                if (currentValue <= minValue)
                            {
                                btnArrowPlusColumn.IsEnabled = true;
                                btnArrowMinusColumn.IsEnabled = false;

                                btnArrowPlusRow.IsEnabled = true;
                                btnArrowMinusRow.IsEnabled = false;
                            }
                            break;

                        case "txtBoxNumRows":
                            btnArrowMinusRow.IsEnabled = true;
                            btnArrowPlusRow.IsEnabled = true;

                            btnArrowMinusColumn.IsEnabled = true;
                            btnArrowPlusColumn.IsEnabled = true;

                            if (currentValue >= maxValue)
                            {
                                btnArrowPlusRow.IsEnabled = false;
                                btnArrowMinusRow.IsEnabled = true;

                                btnArrowPlusColumn.IsEnabled = false;
                                btnArrowMinusColumn.IsEnabled = true;
                            }
                            else
                                if (currentValue <= minValue)
                            {
                                btnArrowPlusRow.IsEnabled = true;
                                btnArrowMinusRow.IsEnabled = false;

                                btnArrowPlusColumn.IsEnabled = true;
                                btnArrowMinusColumn.IsEnabled = false;
                            }
                            break;
                    }

                    if (txtBoxName == "txtBoxNumRows")
                        txtBoxNumColumns.Text = txtBoxNumRows.Text;

                    else
                        if (txtBoxName == "txtBoxNumColumns")
                        txtBoxNumRows.Text = txtBoxNumColumns.Text;

                }

                if ("keyboard" == typeInput)
                    update_generatedMatrix(true, false);
                else
                    switch (typeMatrix)
                    {
                        case MyResources.lblReducedMatrix:
                            update_generatedMatrix(false, false);
                            break;

                        case MyResources.lblAdjacencyMatrix:
                            update_generatedMatrix(false, true);
                            break;
                    }

                // show message box errors
                switch (errorMessage)
                {
                    // the size of the adjacency matrix is incorrect
                    case 1:
                        MessageBox.Show(msgBoxMessageAdjMatrix, msgBoxCaption, msgBoxBtn, msgBoxIcon);
                        break;

                    // exceeding the upper limit
                    case 2:
                        MessageBox.Show("Верхний" + msgBoxMessage, msgBoxCaption, msgBoxBtn, msgBoxIcon);
                        break;

                    // exceeding the lower limit
                    case 3:
                        MessageBox.Show("Нижний" + msgBoxMessage, msgBoxCaption, msgBoxBtn, msgBoxIcon);
                        break;

                    default:
                        break;
                }
            }
        }


        /*************** BEGIN get type input matrix and size ***************/
        string get_current_type_input_matrix()
        {
            string typeInputMatrix;

            typeInputMatrix = "";

            if (cmbBoxTypeMatrix.HasItems == true)
            {
                return typeInputMatrix = cmbBoxTypeMatrix.SelectedValue.ToString();
            }
            else
                if (txtBoxTypeMatrix.Text.ToString() != null)
                return typeInputMatrix = txtBoxTypeMatrix.Text.ToString();

            return null;
        }
        int get_current_min_size_of_matrix()
        {
            string typeInputMatrix;

            typeInputMatrix = get_current_type_input_matrix();

            if (typeInputMatrix != null)
            {
                if (MyResources.lblAdjacencyMatrix == typeInputMatrix)
                    return MyResources.minSizeOfAdjacencyMatrix;

                else
                    return MyResources.minSizeOfReduceMatrix;
            }

            return 0;
        }
        int get_current_max_size_of_matrix()
        {
            string typeInputMatrix;

            typeInputMatrix = get_current_type_input_matrix();

            if (typeInputMatrix != null)
            {
                if (MyResources.lblAdjacencyMatrix == typeInputMatrix)
                    return MyResources.maxSizeOfAdjacencyMatrix;

                else
                    return MyResources.maxSizeOfReduceMatrix;
            }

            return 0;
        }
        /*************** END get type input matrix and size ***************/

        void menuItem_close_click(object sender, RoutedEventArgs e)
        {
            if (true == btnSaveInFile.IsEnabled)
                save_result(true);

            if (close_window())
                Application.Current.Shutdown();
        }

        void btnClose_application_Click(object sender, RoutedEventArgs e)
        {
            if (true == btnSaveInFile.IsEnabled)
                save_result(true);

            if (close_window())
                Application.Current.Shutdown();
        }

        void top_menu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
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

        void btnMinimize_window_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void cmbBoxTypeAlgorithms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmbBox = (sender as ComboBox);

            if (cmbBox.HasItems)
            {
                switch (cmbBox.SelectedValue.ToString())
                {
                    // selected number matching 
                    case MyResources.lblNumberMatching:
                        gridCurrentAlgorithm_numberMatching.Visibility = Visibility.Visible;
                        gridCurrentAlgorithm_numberMatching.IsEnabled = true;

                        fill_cmbBox_algorithms_for_numberMatching();

                        gridCurrentAlgorithm_maxMatching.Visibility = Visibility.Hidden;
                        gridCurrentAlgorithm_maxMatching.IsEnabled = false;

                        cmbBoxAlgorithms_maxMatching.Items.Clear();
                        break;

                    // selected maximal matching
                    case MyResources.lblMaxMatching:
                        gridCurrentAlgorithm_maxMatching.Visibility = Visibility.Visible;
                        gridCurrentAlgorithm_maxMatching.IsEnabled = true;

                        fill_cmbBox_algorithms_for_maxMatching();

                        gridCurrentAlgorithm_numberMatching.Visibility = System.Windows.Visibility.Hidden;
                        gridCurrentAlgorithm_numberMatching.IsEnabled = false;

                        cmbBoxAlgorithms_numberMatching.Items.Clear();

                        if (cmbBoxAlgorithms_maxMatching.HasItems)
                            statusBarInfo_2.Content = Convert.ToString(cmbBoxAlgorithms_maxMatching.SelectedValue) + " > ";

                        break;
                }
            }
        }
        private void cmbBox_algorithms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cmbBox = (sender as ComboBox);

            if (cmbBox.HasItems)
            {
                statusBarInfo_2.Content = (sender as ComboBox).SelectedValue.ToString() + " > ";

                switch (cmbBox.SelectedValue.ToString())
                {
                    case MyResources.lblSingleThreadAlgorithm:
                        cmbBoxAlgorithms_numberThreads.IsEnabled = false;
                        cmbBoxAlgorithms_numberThreads.Items.Clear();
                        break;

                    case MyResources.lblMultiThreadAlgorithm:
                        cmbBoxAlgorithms_numberThreads.IsEnabled = true;
                        fill_cmbBoxAlgorithms_numberThreads();
                        break;
                }
            }
        }

        private void cmbBoxTypeAlgorithms_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            switch ((sender as ComboBox).IsEnabled)
            {
                case true:
                    gridCurrentAlgorithm_maxMatching.IsEnabled = true;
                    gridCurrentAlgorithm_maxMatching.Visibility = Visibility.Visible;

                    gridCurrentAlgorithm_numberMatching.IsEnabled = false;
                    gridCurrentAlgorithm_numberMatching.Visibility = Visibility.Hidden;

                    fill_cmbBox_typeAlgorithms();
                    fill_cmbBox_algorithms_for_maxMatching();
                    fill_cmbBox_algorithms_for_numberMatching();
                    break;

                case false:
                    gridCurrentAlgorithm_maxMatching.IsEnabled = false;
                    gridCurrentAlgorithm_maxMatching.Visibility = Visibility.Visible;

                    gridCurrentAlgorithm_numberMatching.IsEnabled = false;
                    gridCurrentAlgorithm_numberMatching.Visibility = Visibility.Hidden;

                    // clear combobox with algorithms
                    cmbBoxTypeAlgorithms.Items.Clear();
                    cmbBoxAlgorithms_maxMatching.Items.Clear();
                    cmbBoxAlgorithms_numberMatching.Items.Clear();
                    break;
            }
        }
        //private void cmbBox_algorithms_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        //{
        //    var cmbBox = (sender as ComboBox);

        //    if (false == cmbBox.IsEnabled)
        //    {
        //        statusBarInfo_2.Content = "";
        //        cmbBoxAlgorithms_numberThreads.IsEnabled = false;
        //        cmbBoxAlgorithms_numberThreads.Items.Clear();
        //    }
        //}

        private void txtBoxSizeMatrix_file_TextChanged(object sender, TextChangedEventArgs e)
        {
            var fieldSize = (sender as TextBox);

            if ((fieldSize.Text != "") && (fieldSize.Text != "не считано из файла"))
            {
                switch (fieldSize.Name.ToString())
                {
                    case "txtBoxNumColumns_file":
                        statusBarInfo_3.Content = "Матрица " + fieldSize.Text.ToString() + " | ";
                        break;

                    case "txtBoxNumRows_file":
                        statusBarInfo_3.Content += fieldSize.Text.ToString();
                        break;
                }
            }

            else
            {
                statusBarInfo_3.Content = "";
            }
        }
        private void txtBoxSizeMatrix_TextChanged(object sender, TextChangedEventArgs e)
        {
            var fieldSize = (sender as TextBox);

            if (fieldSize.Text != "")
            {
                statusBarInfo_3.Content = "Матрица ";

                switch (fieldSize.Name.ToString())
                {
                    case "txtBoxNumColumns":
                        statusBarInfo_3.Content += fieldSize.Text.ToString() + " | " + txtBoxNumRows.Text.ToString();
                        break;

                    case "txtBoxNumRows":
                        statusBarInfo_3.Content += txtBoxNumColumns.Text.ToString() + " | " + fieldSize.Text.ToString();
                        break;
                }
            }

            else
            {
                statusBarInfo_3.Content = "";
            }
        }
        private void txtBoxSizeMatrix_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            switch ((sender as TextBox).IsEnabled)
            {
                case true:
                    statusBarInfo_3.Content = "Матрица " + txtBoxNumColumns.Text.ToString() + " | " + txtBoxNumRows.Text.ToString();
                    break;

                case false:
                    statusBarInfo_3.Content = "";
                    break;
            }
        }

        private void cmbBoxTypeMatrix_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int minValue, maxValue;
            string typeMatrix;
            var cmbBox = (sender as ComboBox);

            if (cmbBox.HasItems)
            {
                minValue = 0;
                maxValue = 0;
                typeMatrix = "";

                minValue = get_current_min_size_of_matrix();
                maxValue = get_current_max_size_of_matrix();
                typeMatrix = get_current_type_input_matrix();

                txtBoxNumColumns.Text = (minValue + 2).ToString();
                txtBoxNumRows.Text = (minValue + 2).ToString();

                lblRangeColumns.Content = "диапазон | " + minValue + " «---» " + maxValue;
                lblRangeRows.Content = "диапазон | " + minValue + " «---» " + maxValue;

                // update enabled buttons control size matrix
                btnArrowMinusColumn.IsEnabled = true;
                btnArrowMinusRow.IsEnabled = true;
                btnArrowPlusColumn.IsEnabled = true;
                btnArrowPlusRow.IsEnabled = true;

                // set tooltip on the buttons
                if (typeMatrix == MyResources.lblAdjacencyMatrix)
                    set_tooltip_buttons(2, 2, 2, 2);
                else
                    if (typeMatrix == MyResources.lblReducedMatrix)
                    set_tooltip_buttons(1, 1, 1, 1);

                if ("random" == typeInput)
                {
                    switch (typeMatrix)
                    {
                        case MyResources.lblReducedMatrix:
                            update_generatedMatrix(false, false);
                            break;

                        case MyResources.lblAdjacencyMatrix:
                            update_generatedMatrix(false, true);
                            break;
                    }
                }
                else
                    update_generatedMatrix(true, false);
            }

        }
        private void cmbBoxAlgorithms_numberThreads_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbBoxAlgorithms_numberThreads.HasItems)
                statusBarInfo_2.Content = cmbBoxAlgorithms_numberMatching.SelectedValue.ToString() +
                    " | потоков - " + cmbBoxAlgorithms_numberThreads.SelectedValue.ToString() + " > ";
        }

        // set tooltip on the button size of matrix
        void set_tooltip_buttons(int numberMinusColumn, int numberMinusRow, int numberPlusColumn, int numberPlusRow)
        {
            btnArrowMinusColumn.ToolTip = "Уменьшить на " + numberMinusColumn;
            btnArrowMinusRow.ToolTip = "Уменьшить на " + numberMinusRow;
            btnArrowPlusColumn.ToolTip = "Увеличить на " + numberPlusColumn;
            btnArrowPlusRow.ToolTip = "Увеличить на " + numberPlusRow;
        }

        // open reference
        private void btnReference_Click(object sender, RoutedEventArgs e)
        {
            var btnName = ((sender as Button).Name);

            if (("btnAlgorithmsHelp" == btnName) || ("btnTypeAlgorithmsHelp" == btnName))
                MyResources.reference = 1;

            else
                MyResources.reference = 0;

            WindowReference wndReference = new WindowReference();
            wndReference.Show();
        }

        private void top_menu_reference_Click(object sender, RoutedEventArgs e)
        {
            WindowReference wndReference = new WindowReference();
            wndReference.Show();
        }

        // change input matrix elements
        public void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            int row, column;
            row = Grid.GetRow((CheckBox)sender) - 1;
            column = Grid.GetColumn((CheckBox)sender) - 1;

            class_Graph.class_Matrix.set_matrix_element(row, column, 1);
        }
        public void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            int row, column;
            row = Grid.GetRow((CheckBox)sender) - 1;
            column = Grid.GetColumn((CheckBox)sender) - 1;

            class_Graph.class_Matrix.set_matrix_element(row, column, 0);
        }

        private void btnResultOfRendering_Click(object sender, RoutedEventArgs e)
        {
            WindowRenderingOfResult wnd;

            wnd = new WindowRenderingOfResult(class_Graph.currAlgorithm, class_Graph.currTypeMatrix, class_Graph.numberThreads, class_Graph.columns, class_Graph.maxMatchingPos, class_Graph.ListOfVertices, class_Graph.countMaxMatching, class_Graph.arrVertices, class_Graph.class_Matrix.original_columns);
            wnd.Show();
        }

        private void btnRestore_window_Click(object sender, RoutedEventArgs e)
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
            if (mainWindow.WindowState == WindowState.Maximized)
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
