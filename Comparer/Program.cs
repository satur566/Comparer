using System;
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
                if (args.Equals(null) || args.Length <= 1)
                {
                    if (!args.Equals(null) && args[0] == "-help")
                    {
                        ShowHelp();
                    }
                    else
                    {
                        Console.WriteLine("Недостаточно данных для работы программы.\n");
                        ShowHelp();
                    }
                }
                else
                {
                    string firstValue = "";
                    string secondValue = "";
                    int discrepancyIndex = -1;
                    if (args.Length == 2)
                    {
                        Configs.ResultPath = args[0];
                        Configs.ReferencePath = args[1];
                        Comparing defaultComparer = new Comparing();
                        discrepancyIndex = defaultComparer.Compare(out firstValue, out secondValue);
                    }
                    else
                    {
                        Configs.ResultPath = args[0];
                        Configs.ReferencePath = args[1];
                        ReadArgs(args);
                        Comparing argsComparer = new Comparing(Configs.StartIndex, Configs.EndIndex, Configs.GetIgnoreIndexes());
                        discrepancyIndex = argsComparer.Compare(out firstValue, out secondValue);
                    }
                    if (discrepancyIndex > 0)
                    {
                        Console.WriteLine($"Первое различие встретилось на {discrepancyIndex + 1} строке:\n" +
                            $"Эталон: \t{firstValue}\nРезультат: \t{secondValue}");
                    }
                    else
                    {
                        Console.WriteLine("Файлы идентичны.");
                    }
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
        /// Проверяет корректность значения аргумента.
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
