using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length > 0)
                {
                    for (int i = 0; i < args.Length; i++)
                    {
                        switch (args[i])
                        {
                            case "-result":
                                ExceptionMaker(ref args, ref i);
                                Configs.ResultPath = args[++i];
                                if (++i < args.Length &&
                                    !args[i].StartsWith("-"))
                                    {
                                        Configs.ResultEncoding = args[i];
                                    }
                                break;
                            case "-reference":
                                ExceptionMaker(ref args, ref i);
                                Configs.ReferencePath = args[++i];
                                if (++i < args.Length &&
                                    !args[i].StartsWith("-"))
                                {
                                    Configs.ReferenceEncoding = args[i];
                                }
                                break;
                            case "-init":
                                ExceptionMaker(ref args, ref i);
                                Configs.StartIndex = Convert.ToInt32(args[++i]);
                                break;
                            case "-end":
                                ExceptionMaker(ref args, ref i);
                                Configs.EndIndex = Convert.ToInt32(args[++i]);
                                break;
                            case "-ignore":
                                //TODO: code it
                                break;
                            case "-help":
                                ShowHelp();
                                break;
                            default:
                                break;
                        }
                    }
                    if (args.Contains("-run"))
                    {
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        BeginCompare();
                        stopwatch.Stop();
                        Console.WriteLine($"Потрачено времени (миллисекунд): {stopwatch.ElapsedMilliseconds}.");
                    }
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
            Console.ReadKey();
        }

        private static void ShowHelp()
        {
            //TODO: code helpBox!
        }

        private static void BeginCompare()
        {
            //TODO: some code.
            string[] resultArray = File.ReadAllLines(Configs.ResultPath);
            string[] referenceArray = File.ReadAllLines(Configs.ReferencePath);
            int compareLength = resultArray.Length > referenceArray.Length ? referenceArray.Length : resultArray.Length;
            Configs.EndIndex = Configs.EndIndex != 0 && compareLength > Configs.EndIndex ? Configs.EndIndex : compareLength;
            int discrepancyIndex = -1;
            for (int i = Configs.StartIndex; i < Configs.EndIndex; i++)
            {
                if (!resultArray[i].Equals(referenceArray[i]))
                {
                    discrepancyIndex = i;
                    break;
                }
            }
            if (discrepancyIndex.Equals(-1))
            {
                Console.WriteLine("Содержимое файлов идентично.");
                return;
            }
            Console.WriteLine($"Первое различие встретилось на {discrepancyIndex + 1} строке:\n" +
                $"Эталон: \t{ referenceArray[discrepancyIndex]}\nРезультат: \t{ resultArray[discrepancyIndex]}");
        }

        private static void ExceptionMaker(ref string[] array, ref int index)
        {
            if (index + 1 >= array.Length)
            {
                throw new Exception($"Отсутствуют аргументы параметра {array[index]}.");
            }
            if (array[index].Equals("-init") ||
                array[index].Equals("-end"))
            {
                if (array[++index].GetType() != typeof(int))
                {
                    throw new Exception("Номер строки должен быть числом.");
                }
            }
        }
    }
}
