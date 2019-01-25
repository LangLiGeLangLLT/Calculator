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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalculatorUp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Calculator calculator = new Calculator(tbExpression, tbNumber);

            calculator.Init();

            calculator.ClickBtnNumber(btnZero);
            calculator.ClickBtnNumber(btnOne);
            calculator.ClickBtnNumber(btnTwo);
            calculator.ClickBtnNumber(btnThree);
            calculator.ClickBtnNumber(btnFour);
            calculator.ClickBtnNumber(btnFive);
            calculator.ClickBtnNumber(btnSix);
            calculator.ClickBtnNumber(btnSeven);
            calculator.ClickBtnNumber(btnEight);
            calculator.ClickBtnNumber(btnNine);

            calculator.ClickBtnOperator(btnAdd);
            calculator.ClickBtnOperator(btnMinus);
            calculator.ClickBtnOperator(btnMultiply);
            calculator.ClickBtnOperator(btnDivide);
            calculator.ClickBtnOperator(btnRemainder);

            calculator.ClickBtnC(btnC);
            calculator.ClickBtnCE(btnCE);
            calculator.ClickBtnBack(btnBack);
            calculator.ClickBtnPoint(btnPoint);
            calculator.ClickBtnEqual(btnEqual);
        }
    }
}
