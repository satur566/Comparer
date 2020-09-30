using System;
using System.Diagnostics;

namespace Comparer
{
    public class Program
    {
        static void Main(string[] args)
        {
            Stopwatch globalStopwatch = new Stopwatch();
            globalStopwatch.Start();
            bool isDetailed = false;
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
                    int discrepancyIndex = -1;
                    Comparing defaultComparer = new Comparing
                    {
                        ResultPath = args[0],
                        ReferencePath = args[1]
                    };
                    if (args.Length > 2)
                    {
                        defaultComparer.StartIndex = Convert.ToInt32(ArgumentTaker(args, "-begin"));
                        defaultComparer.EndIndex = Convert.ToInt32(ArgumentTaker(args, "-end"));
                        defaultComparer.SetIgnoreIndexes(ArgumentTaker(args, "-ignore"));
                    }
                    discrepancyIndex = defaultComparer.Compare(out string firstValue, out string secondValue);
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
                if (isDetailed)
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
        private static string ArgumentTaker(string[] args, string parameter)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals(parameter))
                {
                    switch (args[i])
                    {
                        case "-begin":
                        case "-end":
                            if (!int.TryParse(args[i + 1], out _))
                            {
                                throw new Exception($"{args[i]}: номер строки должен быть числом.");
                            }
                            if (args[i + 1].Equals(0))
                            {
                                throw new Exception($"{args[i]}: порядковый номер строки не может быть равен 0");
                            }
                            return args[i + 1];
                        case "-ignore":
                            string value  = args[i + 1].Replace(" ", "");
                            value = value.Trim(',');
                            return value;
                        case "-verbose":
                            return "true";
                        default:
                            throw new Exception($"Неизвестный параметр: {args[i]}");
                    }
                }
            }
            return "";
        }        
    }
}