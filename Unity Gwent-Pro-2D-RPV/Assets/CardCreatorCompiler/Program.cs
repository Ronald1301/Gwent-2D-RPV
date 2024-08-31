using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gwent
{
    public class Program
    {
        public string Code { get; set; }

        public Program(string code)
        {
            Code = code;
        }

        public void CompileCode()
        {
            /*
            // Compare this snippet from Lexer/LexerStack.cs:
            var tokensStack = LexerStack.Analyze(Code);
            var parserStack = new (tokensStack);
            var astStack = parserStack.Parsing();
            astStack.CheckSemantic();
            astStack.Evaluate();
            */

            // try
            {

                // Compare this snippet from Parser/Parser.cs:
                var lexer = new LexicalAnalyzer(Code);
                var tokensList = lexer.Analyze();

                var parser = new Parser(tokensList);
                var ast = parser.Parsing();
                ast.SetScope(new(null!, new(), new()));
                //s ast.CheckSemantic();
                System.Console.WriteLine(ast.Evaluate());
            }
            //catch (Exception e)
            {
                //   Console.WriteLine(e.Message);
            }

        }

        public static void Main(string[] args)
        {
            string code = "effect { Name: \"Damage \" , Params: { amount: Number } , Action: (targets,context) => { for target in targets { i=0; while(i++ < amount) i++; }; } }";

            string input1 = " i = 2; while ( i-- > 0) {context = 7 + 2; x  =  1; while (x-- > -1) { y = x + 1;} }";

            string input3 = "context.Hand.Find( (x) => x.Power == 9);";

            string input5 = "effect " +
                "{" +
                "Name: " + '\"' + "Draw" + '\"' + "," +
                "Action: (targets,context) => {" +
                "while( !(1 > -(90+8)) )" +
                "i = 0;" +
                "}" +
                "}";


            string input6 = "effect {" + "Params: {" +
                "amount: NUMBER" +
                "} ," +
                "Action: (targets,context) => {" +
                "for target in targets {" +
                "i=0;" +
                "while(i++ < amount)" +
                "    i++;" +
                "};" +
                "} ," +
                "" +
                "Name: " + '\"' + "Damage" + '\"' + " }";
            // -----------------------------------------------------------------------
            //int[] x = { 1, 2, 3, 4, 5 + 0 + 90 };
            //Predicate<int> predicate = x => !true || !false;

            string input8 = "card " +
                            "{" +
                            "Type: " + '\"' + "O" + '\"' + "@" + '\"' + "ro" + '\"' + "," +
                            "Name: " + '\"' + "Beluga" + '\"' + "," +
                            "Power: " + "-(-1-9)" + "," +
                            "Faction: " + '\"' + "Pokemon" + '\"' + "," +
                            "Range: " + "[" + '\"' + "Ranged" + '\"' + "," + '\"' + "Melee" + '\"' + ",9 + 8 - 4 == 0] ," +
                            "OnActivation: " +
                            "[" +
                            "{" +
                            "Effect:" +
                            "{" +
                            "Name: " + '\"' + "Damage" + '\"' + "," +
                            "amount: 7-2+(-1-9)^(1 - 1 + -1 +1) - 1" +
                            "}" +
                            "Selector:" +
                            "{" +
                            "Source: " + '\"' + "board" + '\"' + "," +
                            "Predicate: " + "(unit) => unit.Power == 9 , " +
                            "Single: " + "false || true && !( 5 > 0) " +
                            "}" +
                            "PostAction: " +
                            "{" +
                            "Type: " + '\"' + "O" + '\"' + "@" + '\"' + "ro" + '\"' + "," +
                            "Selector:" +
                            "{" +
                            "Source: " + '\"' + "parent" + '\"' + "," +
                            "Single: !(true || (false && -7 + 9 > 0))," +
                            "Predicate: " + "(unit) => unit.Power == 9" +
                            "}" +
                            "}" +
                            "}," +
                            "{" +
                            "Effect: " + '\"' + "Return Deck" + '\"' +
                            "}" +
                            "]" +
                            "}" +
                            "";



            Program program = new(code);
            program.CompileCode();


        }
    }

}