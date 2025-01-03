using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LoxInterpreter.Util
{
    /// <summary>
    /// Manual compilation
    /// "C:\Program Files\Microsoft Visual Studio\2022\Community\Msbuild\Current\Bin\Roslyn\csc.exe" /langversion:9 /target:exe /out:ASTGenerator.exe ASTGenerator.cs
    /// </summary>
    public class ASTGenerator
    {
        public static void FakeMain(string[] args)
        {

            if (args.Length < 0)
            {
                Console.Error.WriteLine("Usage: astgenerate <output_directory>");
                Environment.Exit(64);
            }
            var procid = Process.GetCurrentProcess().Id;
            Console.WriteLine("Oigan Esta Process {0}.\n Press Any Key to Verify", procid.ToString()); Console.ReadKey();
            string outdir = args[0];

            /*  Generator substitutions
             *  Expr -> ExprBase
             *  operator -> op
            */
            List<string> exprnodelist = new List<string>() {
                "Binary   : ExprBase left, Token oper, ExprBase right",
                "Grouping : ExprBase expression",
                "Literal  : Object value",
                "Unary    : Token oper, ExprBase right"
            };

            List<string> stmtnodelist = new List<string>() {
                "Expression    : ExprBase expression",
                "Print : ExprBase expression",
            };

            DefineAST(outdir, "ExprBase", exprnodelist);
            
        }


        private static void DefineAST(string outdir, string basename, List<string> types)
        {
            var stdout = new StreamWriter(Console.OpenStandardOutput(), Console.Out.Encoding) { AutoFlush = true };
            string writepath = Path.Combine(outdir, basename + ".cs");
            Console.SetOut(new StreamWriter(writepath));

            Console.WriteLine("\n\n");
            Console.WriteLine("namespace LoxInterpreter.Parser { \n");
            Console.WriteLine("");
            //Console.WriteLine("\t abstract class " + basename + " : ExprBase { \n");
            DefineVisitor(basename, types);

            foreach (var type in types)
            {
                string classname = type.Split(':')[0].Trim();
                string fields = type.Split(':')[1].Trim();
                DefineType(basename, classname, fields);
            }

            // Run all final closers 
            Console.WriteLine("");
            Console.WriteLine("\n }");
            Console.Out.Flush();
            Console.Out.Close();
            Console.SetOut(stdout);
        }

        private static void DefineVisitor(string basename, List<string> typeslist)
        {
            var types = typeslist
                                    .Select(f => f.Trim())
                                    .ToArray();

            Console.WriteLine("  interface Visitor<R> {");
            foreach (string type in types)
            {
                String typename = type.Split(':')[0].Trim();
                Console.WriteLine("     void visit" + typename + basename + "(" +
                    typename + " " + basename.ToLower() + ");");
            }

            Console.WriteLine("  }");
        
        }

        private static void DefineType(string basename, string classname, string fieldslist)
        {
            // Convert the fieldlist to fields array. Done early on cuz of my preference for field members before the constructor.
            // When using the CSC compiler wthout .Core references. Need to rely on older techniques for BW compatibility
            string[] fields = fieldslist.Split(new[] { ',' })
                                        .Select(f => f.Trim())
                                        .ToArray();

            // Classes are public non-static due to C# requirment to that static classes must derive from objects
            Console.WriteLine("public class " + classname + " : " + basename + " {");
            Console.WriteLine("");

            // Constructor
            Console.WriteLine("    " + classname + "(" + fieldslist + ") {");
            foreach (string field in fields)
            {
                string name = field.Split(' ')[1];
                Console.WriteLine("      this." + name + " = " + name + ";");
            }
            Console.WriteLine("    }");
            Console.WriteLine();

            // Visitor Pattern implementt=ing in each subclass

            Console.WriteLine("    public override");
            Console.WriteLine("    void Accept(Visitor visitor) {");
            Console.WriteLine("      return visitor.visit" + classname + basename + "(this);");
            Console.WriteLine("    }");

            // Structure Field Members. put at the bottom
            foreach (string field in fields)
            {
                Console.WriteLine("    private " + field + ";");
            }
            Console.WriteLine("  }");

        }
    }
}