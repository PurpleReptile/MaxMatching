using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GraduateWork_updated
{
    class Errors
    {
        // variables
        public bool matrixIsEmpty { get; set; }
        public bool matrixIsAsymmetrical { get; set; }

        string msgBoxMessage, msgBoxCaption;
        MessageBoxButton msgBoxBtn;
        MessageBoxImage msgBoxIcon;

        // check for errors
        public bool is_error()
        {
            if (matrixIsEmpty == true) return true;
            if (matrixIsAsymmetrical == true) return true;

            return false;
        }

        //  show MessageBox if the matrix is empty
        public void message_matrix_is_empty()
        {
            msgBoxMessage = "Извините, но исходная матрица оказалась пустой!";
            msgBoxCaption = "Пустая матрица";
            msgBoxBtn = MessageBoxButton.OK;
            msgBoxIcon = MessageBoxImage.Error;

            MessageBox.Show(msgBoxMessage, msgBoxCaption, msgBoxBtn, msgBoxIcon);
        }

        // show MessageBox if the matrix is asymmetrical
        public void message_matrix_is_asymmetrical()
        {
            msgBoxMessage = "Извините, но данная матрица асимметрична и не является матрицей смежности";
            msgBoxCaption = "Некорректные данные";
            msgBoxBtn = MessageBoxButton.OK;
            msgBoxIcon = MessageBoxImage.Error;

            MessageBox.Show(msgBoxMessage, msgBoxCaption, msgBoxBtn, msgBoxIcon);
        }


    }
}
