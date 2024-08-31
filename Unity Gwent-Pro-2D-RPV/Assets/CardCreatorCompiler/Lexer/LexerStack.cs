namespace Gwent{
    
}
/*
public static class LexerStack
{
    /// <summary>
    ///Analiza la entra y la covierte en tokens
    /// </summary>
    /// <param name="input"></param>
    /// <returns>Una Pila de tokens</returns>
    public static Stack<Token> Analyze(string input)
    {
        Stack<Token> tokens = [];

        string aux = "";

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == ' ' && aux == "") continue;

            else if ((input[i] == ' ' || input[i] == ';') && aux != "")
            {
                tokens.Push(GetToken(aux));
                aux = "";
                if (i != input.Length - 1) continue;
            }

            else if (Char.IsDigit(input[i]))
            {
                if (aux == "")
                {
                    (int, string) number = GetNumber(i, input);
                    i = number.Item1 - 1;
                    tokens.Push(new Token(Token.TokenType.Number_Literal, number.Item2));
                }
                else aux += input[i];
                continue;
            }

            else if (input[i] == '\"' || input[i] == '"')
            {
                if (aux != "") tokens.Push(GetToken(aux));

                (int, string) string_result = GetString(i + 1, input);
                tokens.Push(new Token(Token.TokenType.Chain_Literals, string_result.Item2));
                i = string_result.Item1 - 1;
                aux = "";
                continue;
            }

            else if (!char.IsLetter(input[i]))
            {
                if (aux != "") tokens.Push(GetToken(aux));
                (int, string) symbol = GetOperator(i, input);
                i = symbol.Item1;
               // if(aux!="/n")
                {
                tokens.Push(GetToken(symbol.Item2));
                }
                aux = "";
                continue;
            }

            else
                aux += input[i];
        }

        return tokens;
    }

    private static Token GetToken(string aux)
    {
        if (Token.AllTokens.TryGetValue(aux, out Token? value))
        {
            return value;
        }
        else
        {
            return new Token(Token.TokenType.Identifier, aux);
        }
    }
    private static (int, string) GetNumber(int i, string input)
    {
        string number = "";
        for (int j = i; j < input.Length; j++)
        {
            if (Char.IsDigit(input[j]))
            {
                number += input[j];
            }
            else
            {
                return (j, number);
            }
        }
        return (input.Length, number);
    }

    private static (int, string) GetString(int v, string input)
    {
        string str = "";
        for (int i = v; i < input.Length; i++)
        {
            if (input[i] == '\"' || input[i] == '"')
            {
                return (i, str);
            }
            str += input[i];
        }
        return (input.Length, str);
    }
    private static (int, string) GetOperator(int start, string input)
    {
        string opera = $"{input[start]}";
        if (start < input.Length - 1)
        {
            if (Token.AllTokens.ContainsKey(opera + input[start + 1])) return (start + 1, opera += input[start + 1]);
        }
        return (start, opera);
    }
}
*/