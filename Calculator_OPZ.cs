using System.Text.RegularExpressions;

namespace TelegramBot_Arithmetic_Calculator
{
    internal class Calculator_OPZ
    {
        private Stack<string> result;

        private bool IsArithmetic(string input)
        {
            Regex regex = new Regex("[^0-9\\+\\-\\*\\/\\^\\%\\)\\(\\,\\.]");
            return !regex.IsMatch(input);
        }
        private bool IsNumber(string input)
        {
            Regex regex = new Regex("([\\d\\.\\d]+)");
            return regex.IsMatch(input);
        }
        private MatchCollection get_elements(string input)
        {
            Regex args = new Regex("([\\+|\\-|\\*|\\/|\\^|\\%|\\(|\\)]|[\\d\\,\\.\\d]+)");
            return args.Matches(input);
        }

        public string Calculate_poliz(ref Stack<string> stack)
        {
            if (stack.Count == 0)
            {
                return "Error01";
            }
            List<string> poliz = stack.ToList();
            poliz.Reverse();
            Stack<string> temp_stack = new Stack<string>();
            foreach(var element in poliz)
            {
                if (IsNumber(element))
                {
                    temp_stack.Push(element);
                }
                if (!IsNumber(element))
                {
                    double[] arg = { 0, 0 };
                    try
                    {
                        arg[1] = Convert.ToDouble(temp_stack.Pop());
                        arg[0] = Convert.ToDouble(temp_stack.Pop());
                    }
                    catch
                    {
                        return "Обнаружена некорректная запись числа!";
                    }
                    switch(element)
                    {
                        case "+": temp_stack.Push((arg[0] + arg[1]).ToString());
                            break;
                        case "-": temp_stack.Push((arg[0] - arg[1]).ToString());
                            break;
                        case "*": temp_stack.Push((arg[0] * arg[1]).ToString());
                            break;
                        case "/": temp_stack.Push((arg[0] / arg[1]).ToString());
                            break;
                        case "^": temp_stack.Push((Math.Pow(arg[0], arg[1])).ToString());
                            break;
                        case "%": temp_stack.Push((arg[0] * arg[1] / 100).ToString());
                            break;
                    }
                }
            }
            return temp_stack.Pop();
        }
        public ref Stack<string> Parser_poliz(string input_string)
        {
            result = new Stack<string>();
            if (!IsArithmetic(input_string)) return ref result;
            input_string = input_string.Trim();//Remove whitespaces
            if(input_string.Contains("**")) input_string.Replace("**", "^");//Replace two char operator with one char
            MatchCollection elements = get_elements(input_string);//Parse input string with regex
            Stack<string> stack = new Stack<string>();
            foreach (Match match in elements)
            {
                if (IsNumber(match.Value))
                {
                    result.Push(match.Value);
                }
                else
                {
                    //Low priority
                    if (match.Value == "+" || match.Value == "-")
                    {
                        if(stack.Count>0)
                        {
                            while (stack.Peek() == "+" || stack.Peek() == "-" ||
                            stack.Peek() == "*" || stack.Peek() == "/" ||
                            stack.Peek() == "^" || stack.Peek() == "%")
                            {
                                result.Push(stack.Pop());
                                if (stack.Count == 0) break;
                            }
                        }
                        stack.Push(match.Value);
                        continue;
                    }
                    //Medium priority
                    if (match.Value == "*" || match.Value == "/")
                    {
                        if (stack.Count > 0)
                        {
                            while (stack.Peek() == "*" || stack.Peek() == "/" ||
                            stack.Peek() == "^" || stack.Peek() == "%")
                            {
                                result.Push(stack.Pop());
                                if (stack.Count == 0) break;
                            }
                        }
                        stack.Push(match.Value);
                        continue;
                    }
                    //High priotity
                    if (match.Value == "^" || match.Value == "(" ||
                        match.Value == "%")
                    {
                        if (stack.Count > 0 && 
                            (match.Value == "^" || match.Value == "%"))
                        {
                            while (stack.Peek() == "^" || stack.Peek() == "%")
                            {
                                result.Push(stack.Pop());
                                if (stack.Count == 0) break;
                            }
                        }
                        stack.Push(match.Value);
                        continue;
                    }
                    // Close (
                    if (match.Value == ")")
                    {
                        if (stack.Count > 0)
                        {
                            while (stack.Peek() != "(")
                            {
                                result.Push(stack.Pop());
                                if (stack.Count == 0) break;

                            }
                            if (stack.Count > 0) stack.Pop();
                            continue;
                        }
                    }
                }
            }
            while (stack.Count > 0)
            {
                result.Push(stack.Pop());
            }
            return ref result;
        }
    }
}