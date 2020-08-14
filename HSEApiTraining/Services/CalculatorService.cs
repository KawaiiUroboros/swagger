using System;
using System.Collections.Generic;
using System.Globalization;
namespace HSEApiTraining
{
    public interface ICalculatorService
    {
        double CalculateExpression(string expression);
        IEnumerable<double> CalculateBatchExpressions(IEnumerable<string> expressions);
    }

    delegate double MathOperation(double a, double b);

    public class CalculatorService : ICalculatorService
    {
        static Dictionary<string, MathOperation> operations = new Dictionary<string, MathOperation>
        {
            ["+"] = (a, b) => a + b,
            ["-"] = (a, b) => a - b,
            ["*"] = (a, b) => a * b,
            ["/"] = (a, b) => (double)a / b,
            ["%"] = (a, b) => a % b
        };

        /// <summary>
        /// Вычисляет значения ряда выражений.
        /// </summary>
        /// <param name="expressions"> Выражения. </param>
        /// <returns> Результаты вычислений для каждого выражения. </returns>
        public IEnumerable<double> CalculateBatchExpressions(IEnumerable<string> expressions)
        {
            List<double> results = new List<double>();

            foreach (string exp in expressions)
            {
                double ans = CalculateExpression(exp);
                results.Add(ans);
            }

            return results;
        }

        /// <summary>
        /// Вычисляет значение выражения.
        /// </summary>
        /// <param name="expression"> Выражение. </param>
        /// <returns> Результат подсчёта. </returns>
        public double CalculateExpression(string expression)
        {
            expression = expression.Trim();
            // Добавляем все ключи в список.
            var keys = new List<char>();
            foreach (var key in operations.Keys)
            {
                keys.Add(key[0]);
            }

            // Находим индекс операции в строке.
            int sign = 1;
            if (expression[0] == '-' || expression[0] == '+')
            {
                sign = expression[0] == '-' ? -1 : 1;
                expression = expression.Substring(1);
            }
            char c = expression[expression.IndexOfAny(keys.ToArray())];

            // Раделяем строку по операции.
            string[] nums = expression.Split(c, 2);
           

            // Получаем числа.
            nums[0] = nums[0].Replace(',', '.');
            nums[1] = nums[1].Replace(',', '.');

            double num1 = double.Parse(nums[0], CultureInfo.InvariantCulture) * sign;
            double num2 = double.Parse(nums[1], CultureInfo.InvariantCulture);

            // Если они не удовлетворяют дополнительным условиям, то выбрасываем исключение.
            if ((num2 == 0 && c == '/') || num1 <= int.MinValue || num1 >= int.MaxValue
                || num2 <= int.MinValue || num2 >= int.MaxValue || nums.Length > 2||nums[1].StartsWith('+'))
            {
                throw new ArgumentException();
            }
            else
            {
                MathOperation op = operations[$"{c}"];
                //Console.WriteLine(num1);
                //Console.WriteLine(num2);
                //Console.WriteLine((long) (op(num1,num2) * 10000));
                //Console.WriteLine((op(num1, num2) * 10000));
                
                double expr = op(num1, num2);
                if (expr.ToString().Contains( ',' )|| expr.ToString().Contains('.'))
                {
                    string exprRes = op(num1, num2).ToString("0.0000000");
                    string[] exprs = exprRes.Split(new[] { '.', ',' });
                    if (exprRes.Contains('.'))
                    {
                        return double.Parse("" + exprs[0] + "." + exprs[1].Substring(0, 4));
                    }
                    else 
                    {
                        return double.Parse("" + exprs[0] + "," + exprs[1].Substring(0, 4));
                    }
                    


                }
                else { return expr; }

            }
        }
    }
}
