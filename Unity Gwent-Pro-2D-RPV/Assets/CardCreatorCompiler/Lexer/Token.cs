using System.Globalization;
using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Gwent
{
  public class Token
  {
    public TokenType Type { get; set; } 
    public string Value { get; set; } 

    public Token(TokenType type, string value)
    {
      Type = type;
      Value = value;
    }
    public enum TokenType
    {
      //Keyword general

      //keywords Effect
      Token_effect_Declaration, Token_Name, Token_Params, Token_Action, /*Token_targets, Token_Context,*/
      Token_TriggerPlayer, Token_Board, Token_HandOfPlayer, Token_FieldOfPlayer, Token_GraveyardOfPlayer, Token_DeckOfPlayer,
      Token_Find, Token_Push, Token_SendBottom, Token_Pop, Token_Remove, Token_Shuffle,
      Token_Owner, Token_Hand, Token_Field, Token_Graveyard, Token_Deck,
      //keywords Cards
      Token_card, Token_Type, Token_Faction, Token_Power, Token_Range, Token_OnActivation, Token_Effect,
      Token_Selector, Token_Source, Token_Single, Token_Predicate, Token_PostAction,

      //keywords Language
      Token_Else, Token_In, Token_If, Token_Function, Token_Then, Token_While, Token_For,

      Identifier,

      //Operator
      Token_Sum, Token_Dif, Token_Multi, Token_Div, Token_Equal, Token_Mod,
      Token_Less, Token_More, Token_LessOrEqual, Token_MoreOrEqual, Token_DoubleEqual, Token_NotEqual,

      Token_SumEqual, Token_DifEqual, Token_MultiEqual, Token_DivEqual, Token_SumSum, Token_DifDif,

      //Logic Operators 
      Token_Not, Token_And, Token_Or,

      //Specials Characters 
      WhiteSpace, Open_Paren, Close_Paren, Open_Block, Close_Block, Open_Key, Close_Key, PointAndComma, EndLine,
      EndProgram, Comma, TwoPoint, Point,

      //Booleans
      Token_True, Token_False,

      Number, String, Boolean, Card, IEnumerableCards,//DataType
      Chain_Literals, Number_Literal,//entero

      //Functions
      Token_Log, Token_Sen, Token_Cos, Token_Tan, Token_Cot, Token_Sqrt, Token_Pow, Token_PI,

      Token_SpaceLine, Token_Lambda, Token_Concat, Token_DoubleConcat,
      Token_Parent,
            Token_AndAnd,
            Token_OrOr
        }

    public static Dictionary<string, Token> AllTokens = new Dictionary<string, Token>
    {
      ["effect"] = new Token(TokenType.Token_effect_Declaration, "effect"),
      ["Name"] = new Token(TokenType.Token_Name, "Name"),
      ["Params"] = new Token(TokenType.Token_Params, "Params"),
      ["Number"] = new Token(TokenType.Number, "Number"),
      ["String"] = new Token(TokenType.String, "String"),
      ["Boolean"] = new Token(TokenType.Boolean, "Boolean"),

      ["Action"] = new Token(TokenType.Token_Action, "Action"),
      //["targets"]= new Token(TokenType.Token_targets, "targets"),
      //["Context"]= new Token(TokenType.Token_Context, "Context"),
      ["TriggerPlayer"] = new Token(TokenType.Token_TriggerPlayer, "TriggerPlayer"),
      ["Board"] = new Token(TokenType.Token_Board, "Board"),
      ["Hand"] = new Token(TokenType.Token_Hand, "Hand"),
      ["Field"] = new Token(TokenType.Token_Field, "Field"),
      ["Graveyard"] = new Token(TokenType.Token_Graveyard, "Graveyard"),
      ["Deck"] = new Token(TokenType.Token_Deck, "Deck"),
      ["HandOfPlayer"] = new Token(TokenType.Token_HandOfPlayer, "HandOfPlayer"),
      ["FieldOfPlayer"] = new Token(TokenType.Token_FieldOfPlayer, "FieldOfPlayer"),
      ["GraveyardOfPlayer"] = new Token(TokenType.Token_GraveyardOfPlayer, "GraveyardOfPlayer"),
      ["DeckOfPlayer"] = new Token(TokenType.Token_DeckOfPlayer, "DeckOfPlayer"),
      ["Find"] = new Token(TokenType.Token_Find, "Find"),
      ["Push"] = new Token(TokenType.Token_Push, "Push"),
      ["SendBottom"] = new Token(TokenType.Token_SendBottom, "SendBottom"),
      ["Pop"] = new Token(TokenType.Token_Pop, "Pop"),
      ["Remove"] = new Token(TokenType.Token_Remove, "Remove"),
      ["Shuffle"] = new Token(TokenType.Token_Shuffle, "Shuffle"),

      ["owner"] = new Token(TokenType.Token_Owner, "owner"),


      //Cards
      ["card"] = new Token(TokenType.Token_card, "card"),
      ["Type"] = new Token(TokenType.Token_Type, "Type"),
      ["Faction"] = new Token(TokenType.Token_Faction, "Faction"),
      ["Power"] = new Token(TokenType.Token_Power, "Power"),
      ["Range"] = new Token(TokenType.Token_Range, "Range"),
      ["OnActivation"] = new Token(TokenType.Token_OnActivation, "OnActivation"),
      ["Effect"] = new Token(TokenType.Token_Effect, "Effect"),
      ["Selector"] = new Token(TokenType.Token_Selector, "Selector"),
      ["Source"] = new Token(TokenType.Token_Source, "Source"),
      ["Single"] = new Token(TokenType.Token_Single, "Single"),
      ["Predicate"] = new Token(TokenType.Token_Predicate, "Predicate"),
      ["PostAction"] = new Token(TokenType.Token_PostAction, "PostAction"),

      //Keywords Language
      ["if"] = new Token(TokenType.Token_If, "if"),
      ["else"] = new Token(TokenType.Token_Else, "else"),
      ["in"] = new Token(TokenType.Token_In, "in"),
      //["then"]= new Token(TokenType.Token_Then, "then"),
      ["while"] = new Token(TokenType.Token_While, "while"),
      ["For"] = new Token(TokenType.Token_For, "For"),
      ["function"] = new Token(TokenType.Token_Function, "function"),


      //Operators arithmetics
      ["+"] = new Token(TokenType.Token_Sum, "+"),
      ["-"] = new Token(TokenType.Token_Dif, "-"),
      ["*"] = new Token(TokenType.Token_Multi, "*"),
      ["/"] = new Token(TokenType.Token_Div, "/"),
      ["="] = new Token(TokenType.Token_Equal, "="),
      ["%"] = new Token(TokenType.Token_Mod, "%"),

      ["+="] = new Token(TokenType.Token_SumEqual, "+="),
      ["-="] = new Token(TokenType.Token_DifEqual, "-="),
      ["*="] = new Token(TokenType.Token_MultiEqual, "*="),
      ["/="] = new Token(TokenType.Token_DivEqual, "/="),
      ["++"] = new Token(TokenType.Token_SumSum, "++"),
      ["--"] = new Token(TokenType.Token_DifDif, "--"),

      //Operators de comparative
      ["<"] = new Token(TokenType.Token_Less, "<"),
      [">"] = new Token(TokenType.Token_More, ">"),
      ["<="] = new Token(TokenType.Token_LessOrEqual, "<="),
      [">="] = new Token(TokenType.Token_MoreOrEqual, ">="),
      ["=="] = new Token(TokenType.Token_DoubleEqual, "=="),
      ["!="] = new Token(TokenType.Token_NotEqual, "!="),

      //Operators Logics
      ["!"] = new Token(TokenType.Token_Not, "!"),
      ["&"] = new Token(TokenType.Token_And, "&"),
      ["&&"] = new Token(TokenType.Token_AndAnd, "&&"),
      ["|"] = new Token(TokenType.Token_Or, "|"),
      ["||"] = new Token(TokenType.Token_OrOr, "||"),

      //Characters Specials
      //[" "]= new Token(TokenType.WhiteSpace," "),
      ["("] = new Token(TokenType.Open_Paren, "("),
      [")"] = new Token(TokenType.Close_Paren, ")"),
      ["["] = new Token(TokenType.Open_Block, "["),
      ["]"] = new Token(TokenType.Close_Block, "]"),
      ["{"] = new Token(TokenType.Open_Key, "{"),
      ["}"] = new Token(TokenType.Close_Key, "}"),
      [";"] = new Token(TokenType.PointAndComma, ";"),
      ["."] = new Token(TokenType.Point, "."),
      [":"] = new Token(TokenType.TwoPoint, ":"),
      [","] = new Token(TokenType.Comma, ","),
      ["\n"] = new Token(TokenType.EndLine, "\n"),
      ["\0"] = new Token(TokenType.EndProgram, "\0"),

      //Booleans
      ["true"] = new Token(TokenType.Token_True, "true"),
      ["false"] = new Token(TokenType.Token_False, "false"),

      //Functions
      ["log"] = new Token(TokenType.Token_Log, "log"),
      ["sin"] = new Token(TokenType.Token_Sen, "sin"),
      ["cos"] = new Token(TokenType.Token_Cos, "cos"),
      ["tan"] = new Token(TokenType.Token_Tan, "tan"),
      ["cot"] = new Token(TokenType.Token_Cot, "cot"),
      ["^"] = new Token(TokenType.Token_Pow, "^"),
      ["sqrt"] = new Token(TokenType.Token_Sqrt, "sqrt"),
      ["Pi"] = new Token(TokenType.Token_PI, "PI"),

      //Others
      [" "] = new Token(TokenType.WhiteSpace, " "),
      ["\t"] = new Token(TokenType.WhiteSpace, "\t"),
      ["\n"] = new Token(TokenType.WhiteSpace, "\n"),
      ["\r"] = new Token(TokenType.WhiteSpace, "\r"),
      ["\0"] = new Token(TokenType.WhiteSpace, "\0"),
      ["\f"] = new Token(TokenType.WhiteSpace, "\f"),
      ["\v"] = new Token(TokenType.WhiteSpace, "\v"),
      ["\b"] = new Token(TokenType.WhiteSpace, "\b"),
      ["\a"] = new Token(TokenType.WhiteSpace, "\a"),


      ["=>"] = new Token(TokenType.Token_Lambda, "=>"),
      ["@"] = new Token(TokenType.Token_Concat, "@"),
      ["@@"] = new Token(TokenType.Token_DoubleConcat, "@@"),

      /*
        //Keyword
        //effect
        {"effect", new Token(TokenType.Token_effect_Declaration, "effect")},
        {"Name", new Token(TokenType.Token_Name, "Name")},
        {"Params", new Token(TokenType.Token_Params, "Params")},
        {"Number",new Token(TokenType.Number,"Number")},
        {"String",new Token(TokenType.String,"String")},
        {"Boolean",new Token(TokenType.Boolean,"Boolean")},

        {"Action", new Token(TokenType.Token_Action, "Action")},
         //{"targets", new Token(TokenType.Token_targets, "targets")},
         //{"Context", new Token(TokenType.Token_Context, "Context")},
          {"TriggerPlayer", new Token(TokenType.Token_TriggerPlayer, "TriggerPlayer")},
          {"Board", new Token(TokenType.Token_Board, "Board")},
          {"Hand", new Token(TokenType.Token_Hand, "Hand")},
          {"Field", new Token(TokenType.Token_Field, "Field")},
          {"Graveyard", new Token(TokenType.Token_Graveyard, "Graveyard")},
          {"Deck", new Token(TokenType.Token_Deck, "Deck")},
          {"HandOfPlayer", new Token(TokenType.Token_HandOfPlayer, "HandOfPlayer")},
          {"FieldOfPlayer", new Token(TokenType.Token_FieldOfPlayer, "FieldOfPlayer")},
          {"GraveyardOfPlayer", new Token(TokenType.Token_GraveyardOfPlayer, "GraveyardOfPlayer")},
          {"DeckOfPlayer", new Token(TokenType.Token_DeckOfPlayer, "DeckOfPlayer")},
          {"Find", new Token(TokenType.Token_Find, "Find")},
          {"Push", new Token(TokenType.Token_Push, "Push")},
          {"SendBottom", new Token(TokenType.Token_SendBottom, "SendBottom")},
          {"Pop", new Token(TokenType.Token_Pop, "Pop")},
          {"Remove", new Token(TokenType.Token_Remove, "Remove")},
          {"Shuffle", new Token(TokenType.Token_Shuffle, "Shuffle")},

          {"owner", new Token(TokenType.Token_Owner, "owner")},


        //Cards
        {"card", new Token(TokenType.Token_card, "card")},
        {"Type", new Token(TokenType.Token_Type, "Type")},
        {"Faction", new Token(TokenType.Token_Faction, "Faction")},
        {"Power", new Token(TokenType.Token_Power, "Power")},
        {"Range", new Token(TokenType.Token_Range, "Range")},
        {"OnActivation", new Token(TokenType.Token_OnActivation, "OnActivation")},
         {"Effect", new Token(TokenType.Token_Effect, "Effect")},
         {"Selector", new Token(TokenType.Token_Selector, "Selector")},
          {"Source", new Token(TokenType.Token_Source, "Source")},
          {"Single", new Token(TokenType.Token_Single, "Single")},
          {"Predicate", new Token(TokenType.Token_Predicate, "Predicate")},
         {"PostAction", new Token(TokenType.Token_PostAction, "PostAction")},

        //Keywords Language
        {"if", new Token(TokenType.Token_If, "if")},
        {"else", new Token(TokenType.Token_Else, "else")},
        {"in", new Token(TokenType.Token_In, "in")},
        //{"then", new Token(TokenType.Token_Then, "then")},
        {"while", new Token(TokenType.Token_While, "while")},
        {"For", new Token(TokenType.Token_For, "For")},
        {"function", new Token(TokenType.Token_Function, "function")},


        //Operators arithmetics
        {"+", new Token(TokenType.Token_Sum, "+")},
        {"-", new Token(TokenType.Token_Dif, "-")},
        {"*", new Token(TokenType.Token_Multi, "*")},
        {"/", new Token(TokenType.Token_Div, "/")},
        {"=", new Token(TokenType.Token_Equal, "=")},
        {"%", new Token(TokenType.Token_Mod, "%")},

        {"+=", new Token(TokenType.Token_SumEqual, "+=")},
        {"-=", new Token(TokenType.Token_DifEqual, "-=")},
        {"*=", new Token(TokenType.Token_MultiEqual, "*=")},
        {"/=", new Token(TokenType.Token_DivEqual, "/=")},
        {"++", new Token(TokenType.Token_SumSum, "++")},
        {"--", new Token(TokenType.Token_DifDif, "--")},

        //Operators de comparative
        {"<", new Token(TokenType.Token_Less,"<")},
        {">", new Token(TokenType.Token_More,">")},
        {"<=", new Token(TokenType.Token_LessOrEqual,"<=")},
        {">=", new Token(TokenType.Token_MoreOrEqual,">=")},
        {"==", new Token(TokenType.Token_DoubleEqual,"==")},
        {"!=", new Token(TokenType.Token_NotEqual,"!=")},

       //Operators Logics
        {"!", new Token(TokenType.Token_Not,"!")},
        {"&", new Token(TokenType.Token_And,"&")},
        {"|", new Token(TokenType.Token_Or,"|")},

        //Characters Specials
        //{" ", new Token(TokenType.WhiteSpace," ")},
        {"(", new Token(TokenType.Open_Paren,"(")},
        {")", new Token(TokenType.Close_Paren,")")},
        {"[", new Token(TokenType.Open_Block,"[")},
        {"]", new Token(TokenType.Close_Block,"]")},
        {"{", new Token(TokenType.Open_Key,"{")},
        {"}",new Token(TokenType.Close_Key,"}")},
        {";",new Token(TokenType.PointAndComma,";")},
        {".",new Token(TokenType.Point,".")},
        {":",new Token(TokenType.TwoPoint,":")},
        {",", new Token(TokenType.Comma,",")},
        {"\n", new Token(TokenType.EndLine,"\n")},
        {"\0", new Token(TokenType.EndProgram,"\0")},

        //Booleans
        {"true", new Token(TokenType.Token_True, "true")},
        {"false", new Token(TokenType.Token_False, "false")},

        //Functions
        {"log",new Token(TokenType.Token_Log,"log")},
        {"sin", new Token(TokenType.Token_Sen,"sin")},
        {"cos", new Token(TokenType.Token_Cos,"cos")},
        {"tan", new Token(TokenType.Token_Tan,"tan")},
        {"cot", new Token(TokenType.Token_Cot,"cot")},
        {"^",new Token(TokenType.Token_Pow,"^")},
        {"sqrt", new Token(TokenType.Token_Sqrt,"sqrt")},
        {"Pi", new Token(TokenType.Token_PI, "PI")},

        //Others
        {" ", new Token(TokenType.WhiteSpace, " ")},
        {"\t", new Token(TokenType.WhiteSpace, "\t")},
        {"\n", new Token(TokenType.WhiteSpace, "\n")},
        {"\r", new Token(TokenType.WhiteSpace, "\r")},
        {"\0", new Token(TokenType.WhiteSpace, "\0")},
        {"\f", new Token(TokenType.WhiteSpace, "\f")},
        {"\v", new Token(TokenType.WhiteSpace, "\v")},
        {"\b", new Token(TokenType.WhiteSpace, "\b")},
        {"\a", new Token(TokenType.WhiteSpace, "\a")},


        {"=>", new Token(TokenType.Token_Lambda, "=>")},
        {"@", new Token(TokenType.Token_Concat, "@")},
        {"@@", new Token(TokenType.Token_ConcatTwo, "@@")},
        */
    };

  }
}


