using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Comparer
{
    class Comparing
    {
        private int StartIndex { get; set; } = 0;
        public static int EndIndex { get; set; } = 0;
        private List<int> IgnoreIndexes { get; set; } = new List<int>();
        public Comparing() { }
        public Comparing(int beginsWith, int endsWith, List<int> ignoreItems)
        {
            StartIndex = beginsWith;
            EndIndex = endsWith;
            IgnoreIndexes = new List<int>(ignoreItems);
        }

        /// <summary>
        /// Определяет наиболее короткий по количеству строк файл.
        /// </summary>
        /// <param name="firstPath">Путь к файлу.</param>
        /// <param name="secondPath">Путь к файлу.</param>
        /// <returns>Возвращает количество строк наиболее короткого файла.</returns>
        private int ShortestLength (string firstPath, string secondPath)
        {
            int count = 0;
            try
            {
                using (StreamReader firstStream = new StreamReader(firstPath))
                using (StreamReader secondStream = new StreamReader(secondPath))
                {
                    while (true)
                    {
                        if (firstStream.ReadLine() == null || secondStream.ReadLine() == null)
                        {
                            break;
                        }
                        count++;
                    }
                }
            }
            catch
            {
                throw new Exception("Невозможно сравнить файлы. Процесс чтения был прерван.");
            }
            return count;
        }

        /// <summary>
        /// Сравнивает два файла между собой до первого различия.
        /// </summary>
        /// <param name="firstUnmatchedValue">Строка из первого файла с отличием.</param>
        /// <param name="secondUnmatchedValue">Строка из второго файла с отличием.</param>
        /// <returns>Возвращает -1 в случае, если различий не обнаружено. В противном случае возвращает номер строки, на которой обнаружилось первое отличие.</returns>
        public int Compare(out string firstUnmatchedValue, out string secondUnmatchedValue)
        {
            string firstPath = Configs.ResultPath;
            string secondPath = Configs.ReferencePath;
            int discrepancyIndex = -1;
            string unmatchedResultString = "";
            string unmatchedReferenceString = "";
            int shortestLength = ShortestLength(firstPath, secondPath);
            EndIndex = EndIndex != 0 && shortestLength > EndIndex ? EndIndex : shortestLength;
            if (StartIndex > EndIndex)
            {
                throw new Exception("Значение нижней границы диапазона поиска больше значения верхней границы диапазона поиска.");
            }
            try
            {
                using (StreamReader firstStream = new StreamReader(firstPath, GetEncoding(firstPath)))
                using (StreamReader secondStream = new StreamReader(secondPath, GetEncoding(secondPath)))
                {
                    for (int i = 0; i < EndIndex; i++)
                    {
                        string resultString = firstStream.ReadLine();
                        string referenceString = secondStream.ReadLine();
                        if (i < StartIndex || IgnoreIndexes.Contains(i))
                        {
                            continue;
                        }
                        if (!resultString.Equals(referenceString))
                        {
                            if (IsMaskCovered(resultString, referenceString))
                            {
                                continue;
                            }
                            else
                            {
                                discrepancyIndex = i;
                                unmatchedResultString = resultString;
                                unmatchedReferenceString = referenceString;
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                throw new Exception("Невозможно сравнить файлы. Процесс чтения был прерван.");
            }
            if (discrepancyIndex.Equals(-1))
            {
                firstUnmatchedValue = "";
                secondUnmatchedValue = "";
                return discrepancyIndex;
            }
            firstUnmatchedValue = unmatchedResultString;
            secondUnmatchedValue = unmatchedReferenceString;
            return discrepancyIndex;
        }

        /// <summary>
        /// Сравнивает две строки, любая из которых содержит маску *.
        /// </summary>
        /// <param name="firstValue">Строковое значение.</param>
        /// <param name="secondValue">Строковое значение.</param>
        /// <returns>Возвращает true, если строки совпадают с учетом маски. В противном случае - false.</returns>
        private static bool IsMaskCovered(string firstValue, string secondValue)
        {
            if (firstValue.Equals("*") || secondValue.Equals("*"))
            {
                return true;
            }
            bool condition = false;
            if (firstValue.Contains("*") && secondValue.Contains("*"))
            {
                string firstValueHead = firstValue.Substring(0, firstValue.IndexOf("*"));
                string firstValueTail = firstValue.Substring(firstValue.LastIndexOf("*") + 1);
                string secondValueHead = secondValue.Substring(0, secondValue.IndexOf("*"));
                string secondValueTail = secondValue.Substring(secondValue.LastIndexOf("*") + 1);
                string shortestHead = firstValueHead.Length > secondValueHead.Length ? secondValueHead : firstValueHead;
                string shortestTail = firstValueTail.Length > secondValueTail.Length ? secondValueTail : firstValueTail;
                string longestHead = firstValueHead.Length < secondValueHead.Length ? secondValueHead : firstValueHead;
                string longestTail = firstValueTail.Length < secondValueTail.Length ? secondValueTail : firstValueTail;
                if (longestHead.Contains(shortestHead) && longestTail.Contains(shortestTail))
                {
                    condition = true;
                }
            }
            else if (firstValue.Contains("*") || secondValue.Contains("*")) //REGEX!!!!!!! I dont know regex, but I made it myself and it's works fine!
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

        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="filename">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        private static Encoding GetEncoding(string filename) //Used this: https://stackoverflow.com/questions/3825390/effective-way-to-find-any-files-encoding
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return Encoding.UTF32; //UTF-32LE
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return new UTF32Encoding(true, true);  //UTF-32BE

            // We actually have no idea what the encoding is if we reach this point, so
            // you may wish to return null instead of defaulting to ASCII
            return Encoding.ASCII;
        }
    }
}
