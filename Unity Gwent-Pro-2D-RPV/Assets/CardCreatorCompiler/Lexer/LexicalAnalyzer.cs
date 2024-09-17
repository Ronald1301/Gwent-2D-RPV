//using UnityEngine;

using System;
using System.Collections.Generic;

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
        readonly List<Token> tokens;
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
                if ((Input[i] == ' ' || Input[i] == '\t' || Input[i] == '\n' || Input[i] == '\r' || Input[i] == '\0' || Input[i] == '\f' || Input[i] == '\v' || Input[i] == '\b' || Input[i] == '\a') && aux == "")
                {

                    continue;
                }

                //else if ((Input[i] == ' ' || Input[i] == ';') && aux != "")
                else if (Input[i] == ' ' && aux != "")
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
            DeleteWhiteSpace(tokens);
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
                else if (input[index] == '.')
                {
                    number += input[index];
                }
                else if (input[index] == 'f')
                {
                    number += input[index];
                    return number;
                }
                else if (char.IsLetter(input[index]))
                {
                    EngineCompiler.CreateError(ErrorCode.LexicalError, "Invalid token");
                    return number;
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
            EngineCompiler.CreateError(ErrorCode.LexicalError, "String not closed");
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

        private static void DeleteWhiteSpace(List<Token> tokens)
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i].Type == Token.TokenType.WhiteSpace)
                {
                    tokens.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}