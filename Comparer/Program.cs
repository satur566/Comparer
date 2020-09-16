using System;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace Comparer
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch globalStopwatch = new Stopwatch();
            globalStopwatch.Start();
            try
            {
                if (args.Length > 0)
                {
                    ReadArgs(args);
                    //WRONG! At least two path. Try do following: try read 2 firs args as 2 file path, if path1 and path2 != "" then BeginCompare
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    BeginCompare();
                    stopwatch.Stop();
                    Console.WriteLine($"Потрачено времени (миллисекунд): {stopwatch.ElapsedMilliseconds}.");
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
                Console.WriteLine($"Потрачено времени (миллисекунд) всего: {globalStopwatch.ElapsedMilliseconds}.");
                Console.ReadKey();
            }
        }

        private static void ShowHelp()
        {
            //TODO: code helpBox!
        }

        private static void BeginCompare()
        {
            string[] resultArray = File.ReadAllLines(Configs.ResultPath, Encoding.Default);
            string[] referenceArray = File.ReadAllLines(Configs.ReferencePath, Encoding.Default);
            int shortestLengs = resultArray.Length > referenceArray.Length ? referenceArray.Length : resultArray.Length;
            Configs.EndIndex = Configs.EndIndex != 0 && shortestLengs > Configs.EndIndex ? Configs.EndIndex : shortestLengs;
            if (Configs.StartIndex > Configs.EndIndex)
            {
                throw new Exception("Значение нижней границы диапазона поиска больше значения верхней границы диапазона.");
            }
            int discrepancyIndex = -1;
            for (int i = Configs.StartIndex; i < Configs.EndIndex; i++)
            {
                if (Configs.GetIgnoreIndexes().Contains(i))
                {
                    continue;
                }
                if (!resultArray[i].Equals(referenceArray[i])) 
                {
                    if (isMaskCovered(resultArray[i], referenceArray[i])) //TODO: ULTRAPRIORITY!!!! MAKE IT WORK WITH *
                    {                        
                        continue;
                    }
                    else
                    {
                        discrepancyIndex = i;
                        break;
                    }
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

        private static string ArgumentTaker(ref string[] args, ref int i)
        {
            if (i + 1 >= args.Length)
            {
                throw new Exception($"Отсутствуют аргументы параметра {args[i]}.");
            }
            if (args[i].Equals("-init") ||
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
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-result":
                        Configs.ResultPath = ArgumentTaker(ref args, ref i);
                        if (i + 1 < args.Length &&
                            !args[i + 1].StartsWith("-"))
                        {
                            Configs.ResultEncoding = args[++i];
                        }
                        break;
                    case "-reference":
                        Configs.ReferencePath = ArgumentTaker(ref args, ref i);
                        if (i + 1 < args.Length &&
                            !args[i + 1].StartsWith("-"))
                        {
                            Configs.ReferenceEncoding = args[++i];
                        }
                        break;
                    case "-init":
                        Configs.StartIndex = Convert.ToInt32(ArgumentTaker(ref args, ref i));
                        break;
                    case "-end":
                        Configs.EndIndex = Convert.ToInt32(ArgumentTaker(ref args, ref i));
                        break;
                    case "-ignore":
                        Configs.SetIgnoreIndexes(ArgumentTaker(ref args, ref i));
                        break;
                    case "-help":
                        ShowHelp();
                        break;
                    default:
                        break;
                }
            }
        }

        private static bool isMaskCovered(string firstValue, string secondValue)
        {
            if (firstValue.Equals("*") || secondValue.Equals("*"))
            {
                return true;
            }
            bool condition = false;
            if(firstValue.Contains("*") || secondValue.Contains("*"))
            {                
                string maskedValue = firstValue.Contains("*") ? firstValue : secondValue;
                string clearValue = secondValue.Contains("*") ? firstValue : secondValue;
                string[] maskedArray = maskedValue.Split('*');
                maskedValue = maskedValue.Replace("*", "");
                string unmaskedValue = "";
                for (int i = 0; i < maskedArray.Length; i++)
                {
                    if (clearValue.Contains(maskedArray[i]))
                    {
                        int matchIndex = clearValue.IndexOf(maskedArray[i]);
                        unmaskedValue += clearValue.Substring(matchIndex, maskedArray[i].Length);
                        clearValue = clearValue.Substring(matchIndex + maskedArray[i].Length);
                    }
                }
                condition = maskedValue.Equals(unmaskedValue);
            }
            return condition;
        }
    }
}
