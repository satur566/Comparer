using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

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
                if (args.Length == 2)
                {
                    Configs.ResultPath = args[0];
                    Configs.ReferencePath = args[1];
                    Comparing defaultComparer = new Comparing();
                    Console.WriteLine(defaultComparer.Compare());
                }
                if (args.Length > 2)
                {
                    Configs.ResultPath = args[0];
                    Configs.ReferencePath = args[1];
                    ReadArgs(args);
                    Comparing argsComparer = new Comparing(Configs.StartIndex, Configs.EndIndex, Configs.GetIgnoreIndexes());                    
                    Console.WriteLine(argsComparer.Compare());
                }
                if (args.Length <= 1)
                {
                    Console.WriteLine("Недостаточно данных для работы программы.\n");
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
            }
            Console.ReadKey();
        }
        /// <summary>
        /// Выводит подскизку по использованию консольного приложения.
        /// </summary>
        private static void ShowHelp()
        {
            Console.WriteLine($"\n" +
                $"Сравнивает два файла между собой до нахождения первого различия в их содержимом.\n\n" +
                $"Использование:\n" +
                $"Comparer source source [[-begin] index] [[-end] index] [[ignore] index_1, index_2, ... , index_N] [-verbose]\n\n" +
                $"Обязательные параметры:\n" +
                $"source\t\tПуть к одному из сравниваемых файлов.\n\n" +
                $"Дополнительные параметры:\n\n" +
                $"-begin\t\tНомер строки, с которой будет начато сравнение.\n" +
                $"-end\t\tНомер строки, на которой сравнение будет законено.\n" +
                $"-ignore\t\tНомера строк, которые будут пропущены в процессе сравнения.\n" +
                $"-verbose\tДополнительно отобразить время в миллисекундах, затраченное на сравнение файлов.\n");
        }

        /// <summary>
        /// Проверяет корректность применяемого к параметру аргумента.
        /// </summary>
        /// <param name="args">Массив аргументов.</param>
        /// <param name="i">Индекс массива.</param>
        /// <returns>Возвращает значение аргумента.</returns>
        private static string ArgumentTaker(ref string[] args, ref int i)
        {
            if (i + 1 >= args.Length)
            {
                throw new Exception($"Отсутствуют аргументы параметра {args[i]}.");
            }
            switch (args[i])
            {
                case "-begin":
                case "-end":
                    if (!int.TryParse(args[i + 1], out _))
                    {
                        throw new Exception($"{args[i]}: номер строки должен быть числом.");
                    }
                    break;
                case "-ignore":
                    args[i + 1] = args[i + 1].Replace(" ", "");
                    args[i + 1] = args[i + 1].Trim(',');
                    break;
                default:
                    break;
            }
            return args[++i];
        }

        /// <summary>
        /// Считывает аргументы, подаваемые на вход приложению.
        /// </summary>
        /// <param name="args">Массив аргуиентов.</param>
        private static void ReadArgs(string[] args)
        {
            for (int i = 2; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-begin":
                        Configs.StartIndex = Convert.ToInt32(ArgumentTaker(ref args, ref i));
                        break;
                    case "-verbose":
                        Configs.IsDetailed = true;
                        break;
                    case "-end":
                        Configs.EndIndex = Convert.ToInt32(ArgumentTaker(ref args, ref i));
                        break;
                    case "-help":
                    case "/?":
                    case "-?":
                        ShowHelp();
                        break;
                    case "-ignore":
                        Configs.SetIgnoreIndexes(ArgumentTaker(ref args, ref i));
                        break;                    
                    default:
                        throw new Exception($"Неизвестный параметр: {args[i]}");
                }
            }
        }
    }
}
