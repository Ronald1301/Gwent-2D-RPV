using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Gwent
{
    public class LexicalAnalyzer
    {
        public LexicalAnalyzer(string input)
        {
            Input = input;
            tokens = new();
        }
        readonly string Input;
        readonly List<Token> tokens ;
        string aux = "";
        /// <summary>
        ///Analiza la entra y la covierte en tokens
        /// </summary>
        /// <param name="Input"></param>
        /// <returns>Una lista de tokens</returns>///
        public List<Token> Analyze()
        {
            for (int i = 0; i < Input.Length; i++)
            {
                if (Input[i] == ' ' && aux == "") continue;

                else if ((Input[i] == ' ' || Input[i] == ';') && aux != "")
                {
                    tokens.Add(GetToken(aux));
                    aux = "";
                    if (i != Input.Length - 1) continue;
                }

                else if (Char.IsDigit(Input[i]))
                {
                    if (aux == "")
                    {
                        string number = GetNumber(ref i, Input);
                        i--;
                        tokens.Add(new Token(Token.TokenType.Number_Literal, number));
                    }
                    else aux += Input[i];
                    continue;
                }

                else if (Input[i] == '\"' || Input[i] == '"')
                {
                    if (aux != "") tokens.Add(GetToken(aux));
                    i++;
                    string string_result = GetString(ref i, Input);
                    tokens.Add(new Token(Token.TokenType.Chain_Literals, string_result));
                    aux = "";
                    continue;
                }

                else if (!char.IsLetter(Input[i]))
                {
                    if (aux != "") tokens.Add(GetToken(aux));
                    string symbol = GetOperator(ref i, Input);
                    // if(aux!="/n")
                    {
                        tokens.Add(GetToken(symbol));
                    }
                    aux = "";
                    continue;
                }

                else
                    aux += Input[i];
            }
            tokens.Add(new Token(Token.TokenType.EndProgram, "EOF"));
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
        private static string GetNumber(ref int index, string input)
        {
            string number = "";
            while (index < input.Length)
            {
                if (Char.IsDigit(input[index]))
                {
                    number += input[index];
                }
                else
                {
                    return number;
                }
                index++;
            }
            return number;
        }
        private static string GetString(ref int index, string input)
        {
            string str = "";
            while (index < input.Length)
            {
                if (input[index] == '\"' || input[index] == '"')
                {
                    return str;
                }
                str += input[index];
                index++;
            }
            return str;
        }
        private static string GetOperator(ref int index, string input)
        {
            string opera = $"{input[index]}";
            if (index < input.Length - 1)
            {
                if (Token.AllTokens.ContainsKey(opera + input[index + 1]))
                {
                    index++;
                    opera += input[index];
                    return opera;
                }
            }
            return opera;
        }
    }
}