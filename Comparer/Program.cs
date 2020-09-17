using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace Comparer
{
    class Program
    {
        static void Main(string[] args) //TODO: ULTIMATE BUG! -begin > -end by 1 looks equals
        {
            Stopwatch globalStopwatch = new Stopwatch();
            globalStopwatch.Start();
            try
            {
                if (args.Length >= 2)
                {
                    Configs.ResultPath = args[0];
                    Configs.ReferencePath = args[1];
                    ReadArgs(args);
                    Comparing comparer = new Comparing(Configs.ResultPath, Configs.ReferencePath, Configs.StartIndex, Configs.EndIndex, Configs.GetIgnoreIndexes());                    
                    Console.WriteLine(comparer.Compare());
                }
                else
                {
                    ShowHelp();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                globalStopwatch.Stop();
                if (Configs.IsDetailed)
                {
                    Console.WriteLine($"Потрачено времени (миллисекунд) всего: {globalStopwatch.ElapsedMilliseconds}.");
                }
                Console.ReadKey();
            }
        }

        private static void ShowHelp()
        {
            //TODO: code helpBox!
        }

        private static string ArgumentTaker(ref string[] args, ref int i)
        {
            if (i + 1 >= args.Length)
            {
                throw new Exception($"Отсутствуют аргументы параметра {args[i]}.");
            }
            if (args[i].Equals("-begin") ||
                args[i].Equals("-end"))
            {
                if (!int.TryParse(args[i + 1], out _))
                {
                    throw new Exception($"{args[i]}: номер строки должен быть числом.");
                }
            }
            if (args[i].Equals("-ignore"))
            {
                args[i + 1] = args[i + 1].Replace(" ", "");
                args[i + 1] = args[i + 1].Trim(',');
            }
            return args[++i];
        }

        private static void ReadArgs(string[] args)
        {
            for (int i = 2; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-begin":
                        Configs.StartIndex = Convert.ToInt32(ArgumentTaker(ref args, ref i));
                        break;
                    case "-detailed":
                        Configs.IsDetailed = true;
                        break;
                    case "-end":
                        Configs.EndIndex = Convert.ToInt32(ArgumentTaker(ref args, ref i));
                        break;
                    case "-help":
                        ShowHelp();
                        break;
                    case "-ignore":
                        Configs.SetIgnoreIndexes(ArgumentTaker(ref args, ref i));
                        break;                    
                    default:
                        break;
                }
            }
        }
    }
}
