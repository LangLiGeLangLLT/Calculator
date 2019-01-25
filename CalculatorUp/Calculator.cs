using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CalculatorUp
{
    class Calculator
    {
        string expression = "";
        string number = "0";
        string lastOperator = "#";
        string lastNumber = "";
        Stack<double> OPND = new Stack<double>();
        Stack<char> OPTR = new Stack<char>();
        bool exception = false;
        bool clickOperator = false;
        bool havePoint = false;
        TextBox tbExpression;
        TextBox tbNumber;

        public Calculator(TextBox tbExpression, TextBox tbNumber)
        {
            this.tbExpression = tbExpression;
            this.tbNumber = tbNumber;
        }

        #region 计算器算法

        /// <summary>
        /// 判断字符是否为运算符
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool IsOperator(char c)
        {
            if (c == '+' || c == '-' || c == '*' || c == '/' || c == '%' || c == '#')
                return true;
            return false;
        }

        /// <summary>
        /// 判断两个运算符的优先级
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public char Precede(char a, char b)
        {
            if (a == '+')
            {
                if (b == '+' || b == '-' || b == '#')
                    return '>';
                else if (b == '*' || b == '/' || b == '%')
                    return '<';
            }
            else if (a == '-')
            {
                if (b == '+' || b == '-' || b == '#')
                    return '>';
                else if (b == '*' || b == '/' || b == '%')
                    return '<';
            }
            else if (a == '*')
            {
                if (b == '+' || b == '-' || b == '*' || b == '/' || b == '%'
                        || b == '#')
                    return '>';
            }
            else if (a == '/')
            {
                if (b == '+' || b == '-' || b == '*' || b == '/' || b == '%'
                        || b == '#')
                    return '>';
            }
            else if (a == '%')
            {
                if (b == '+' || b == '-' || b == '*' || b == '/' || b == '%'
                        || b == '#')
                    return '>';
            }
            else if (a == '#')
            {
                if (b == '+' || b == '-' || b == '*' || b == '/' || b == '%')
                    return '<';
                else if (b == '#')
                    return '=';
            }
            return '=';
        }

        /// <summary>
        /// 对两个数进行运算
        /// </summary>
        /// <param name="a"></param>
        /// <param name="theta"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public double Operation(double a, char theta, double b)
        {
            if (theta == '+')
                return a + b;
            else if (theta == '-')
                return a - b;
            else if (theta == '*')
                return a * b;
            else if (theta == '/' && b != 0)
                return a * 1.0 / b;
            else if (theta == '%' && b != 0)
                return ((int)a) % ((int)b);
            else
                exception = true;
            return 0;
        }

        /// <summary>
        /// 计算表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public double EvaluateExpression(string expression)
        {
            char theta;
            double a, b;
            string line = expression + "#";
            string strnum = "";
            OPTR.Push('#');
            int it = 0;
            while (line[it] != '#' || OPTR.Peek() != '#')
            {
                if (!IsOperator(line[it])) // 是操作数
                {
                    // 将两个运算符之间的操作数由 string 转换成 double
                    while (!IsOperator(line[it]))
                        strnum += line[it++];
                    OPND.Push(Double.Parse(strnum));
                    strnum = "";
                }
                else // 是运算符
                {
                    // 比较当前运算符和运算符栈顶运算符
                    switch (Precede(OPTR.Peek(), line[it]))
                    {
                        case '<':
                            OPTR.Push(line[it++]);
                            break;
                        case '>':
                            theta = OPTR.Peek();
                            OPTR.Pop();
                            b = OPND.Peek();
                            OPND.Pop();
                            a = OPND.Peek();
                            OPND.Pop();
                            OPND.Push(Operation(a, theta, b));
                            break;
                        case '=':
                            OPTR.Pop();
                            ++it;
                            break;
                        default:
                            break;
                    }
                }
            }
            return OPND.Peek();
        }

        #endregion

        #region 计算器按钮

        /// <summary>
        /// 将计算器初始化
        /// </summary>
        public void Init()
        {
            expression = "";
            number = "0";
            exception = false;
            clickOperator = false;
            havePoint = false;
            OPTR.Clear();
            OPND.Clear();
            tbExpression.Text = expression;
            tbNumber.Text = number;
        }

        /// <summary>
        /// 点击数字键
        /// </summary>
        /// <param name="btnNumber"></param>
        public void ClickBtnNumber(Button btnNumber)
        {
            btnNumber.Click += BtnNumber_Click;
        }

        void BtnNumber_Click(object sender, RoutedEventArgs e)
        {
            if (!exception)
            {
                Button btnNumber = (Button)sender;
                if (number.Equals("0"))
                    number = "";
                if (clickOperator && !havePoint)
                    number = "";
                clickOperator = false;
                number += btnNumber.Content;
                tbNumber.Text = number;
            }
            else
                tbNumber.Text = "除数不能为 0";
        }

        /// <summary>
        /// 点击运算符键
        /// </summary>
        /// <param name="btnOperator"></param>
        public void ClickBtnOperator(Button btnOperator)
        {
            btnOperator.Click += BtnOperator_Click;
        }

        void BtnOperator_Click(object sender, RoutedEventArgs e)
        {
            if (!exception)
            {
                Button btnOperator = (Button)sender;
                havePoint = false;
                if (clickOperator)
                {
                    // 连续点击运算符则替换当前运算符
                    expression = expression.Substring(0, expression.Length - 1);
                    ClickOperatorEvaluate(btnOperator);
                }
                else
                {
                    clickOperator = true;
                    expression += number;
                    ClickOperatorEvaluate(btnOperator);
                    lastOperator = Convert.ToString(btnOperator.Content);
                }
            }
            else
                tbNumber.Text = "除数不能为 0";
        }

        /// <summary>
        /// 每点击运算符，就计算结果
        /// </summary>
        public void ClickOperatorEvaluate(Button btnOperator)
        {
            if (Precede(Convert.ToChar(btnOperator.Content), Convert.ToChar(lastOperator)) == '>' && !btnOperator.Content.Equals(lastOperator))
            {
                // 当前运算符比上一次运算符优先级高，则先得出结果在加上当前运算符
                lastNumber = Result(EvaluateExpression(expression));
                expression = lastNumber + btnOperator.Content;
                number = lastNumber;
                tbExpression.Text = expression;
                tbNumber.Text = number;
            }
            else
            {
                // 否则，照常计算
                number = Result(EvaluateExpression(expression));
                expression += btnOperator.Content;
                tbExpression.Text = expression;
                tbNumber.Text = number;
            }
        }

        /// <summary>
        /// 将计算结果规格化
        /// </summary>
        /// <param name="answer"></param>
        /// <returns></returns>
        public string Result(double answer)
        {
            return Convert.ToInt64(answer) == answer ? Convert.ToInt64(answer) + "" : answer + "";
        }

        public void ClickBtnC(Button btnC)
        {
            btnC.Click += BtnC_Click;
        }

        void BtnC_Click(object sender, RoutedEventArgs e)
        {
            Init();
        }

        public void ClickBtnCE(Button btnCE)
        {
            btnCE.Click += BtnCE_Click;
        }

        void BtnCE_Click(object sender, RoutedEventArgs e)
        {
            if (!exception)
            {
                number = "0";
                tbNumber.Text = number;
            }
            else
                tbNumber.Text = "除数不能为 0";
        }

        public void ClickBtnBack(Button btnBack)
        {
            btnBack.Click += BtnBack_Click;
        }

        void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (!exception)
            {
                if (number.Length > 1)
                {
                    number = number.Substring(0, number.Length - 1);
                    tbNumber.Text = number;
                }
                else
                {
                    number = "0";
                    tbNumber.Text = number;
                }
            }
            else
                tbNumber.Text = "除数不能为 0";
        }

        public void ClickBtnPoint(Button btnPoint)
        {
            btnPoint.Click += BtnPoint_Click;
        }

        void BtnPoint_Click(object sender, RoutedEventArgs e)
        {
            if (!exception)
            {
                Button btnPoint = (Button)sender;
                if (!number.Contains(Convert.ToString(btnPoint.Content)))
                {
                    havePoint = true;
                    if (number.Length == 0)
                        number += "0";
                    number += btnPoint.Content;
                    tbNumber.Text = number;
                }
            }
            else
                tbNumber.Text = "除数不能为 0";
        }

        public void ClickBtnEqual(Button btnEqual)
        {
            btnEqual.Click += BtnEqual_Click;
        }

        void BtnEqual_Click(object sender, RoutedEventArgs e)
        {
            if (!exception)
            {
                expression += number;
                number = Result(EvaluateExpression(expression));
                if (!exception)
                {
                    tbNumber.Text = number;
                    tbExpression.Text = "";
                    // 清空
                    expression = "";
                    exception = false;
                    clickOperator = false;
                    havePoint = false;
                    OPTR.Clear();
                    OPND.Clear();
                }
                else
                {
                    tbExpression.Text = expression;
                    tbNumber.Text = "除数不能为0";
                }
            }
            else
                tbNumber.Text = "除数不能为0";
        }
    }
}
#endregion