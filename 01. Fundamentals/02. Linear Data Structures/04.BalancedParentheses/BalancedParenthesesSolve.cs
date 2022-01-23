namespace Problem04.BalancedParentheses
{
    using System;
    using System.Collections.Generic;

    public class BalancedParenthesesSolve : ISolvable
    {
        public bool AreBalanced(string parentheses)
        {
            Stack<char> parenthes = new Stack<char>(parentheses);
            
            for (int i = 0; i < parentheses.Length; i++)
            {
                if (parentheses[i] == 40 || parentheses[i] == 91 || parentheses[i] == 123)
                {
                    parenthes.Push(parentheses[i]);
                }
                else
                {
                    if (parenthes.Count > 0)
                    {
                        if (parentheses[i] == 41)
                        {
                            if (parenthes.Pop() != 40)
                            {
                                return false;
                            }
                        }
                        else if (parentheses[i] == 93)
                        {
                            if (parenthes.Pop() != 91)
                            {
                                return false;
                            }
                        }
                        else if (parentheses[i] == 125)
                        {
                            if (parenthes.Pop() != 123)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }
    }
}
