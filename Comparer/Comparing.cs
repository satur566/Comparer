using System;
using System.IO;
using System.Text;

namespace Comparer
{
    public class Comparing: Configs
    {
        public Comparing() { }

        /// <summary>
        /// Сравнивает два файла между собой до первого различия.
        /// </summary>
        /// <param name="firstUnmatchedValue">Строка из первого файла с отличием.</param>
        /// <param name="secondUnmatchedValue">Строка из второго файла с отличием.</param>
        /// <returns>Возвращает -1 в случае, если различий не обнаружено. В противном случае возвращает номер строки, на которой обнаружилось первое отличие.</returns>
        public int Compare(out string firstUnmatchedValue, out string secondUnmatchedValue)
        {
            firstUnmatchedValue = null;
            secondUnmatchedValue = null;
            string firstPath = ResultPath;
            string secondPath = ReferencePath;
            int discrepancyIndex = -1;
            if (StartIndex > EndIndex)
            {
                throw new Exception("Значение нижней границы диапазона поиска больше значения верхней границы диапазона поиска.");
            }
            try
            {
                using (StreamReader firstStream = new StreamReader(firstPath, Encoding.ASCII))
                using (StreamReader secondStream = new StreamReader(secondPath, Encoding.ASCII))
                {
                    int i = 0;
                    string resultString = "";
                    string referenceString = "";
                    while (true)
                    {
                        resultString = firstStream.ReadLine();                        
                        referenceString = secondStream.ReadLine();
                        if (i < StartIndex || GetIgnoreIndexes().Contains(i))
                        {
                            i++;
                            continue;
                        }
                        if ((resultString != null && referenceString == null) ||
                            (resultString == null && referenceString != null))
                        {
                            firstUnmatchedValue = resultString;
                            secondUnmatchedValue = referenceString;
                            discrepancyIndex = i;
                            break;
                        }
                        if ((EndIndex != 0 && i == EndIndex) ||
                            (resultString == null && referenceString == null))
                        {
                            break;
                        }                        
                        if (!resultString.Equals(referenceString) &&
                            !IsMaskCovered(resultString, referenceString))
                        {
                            discrepancyIndex = i;
                            firstUnmatchedValue = resultString;
                            secondUnmatchedValue = referenceString;
                            break;
                        }
                        else
                        {
                            i++;
                            continue;
                        }                        
                    }                    
                }
            }
            catch
            {
                throw new Exception("Невозможно сравнить файлы. Процесс чтения был прерван.");
            }
            return discrepancyIndex;
        }

        /// <summary>
        /// Сравнивает две строки, любая из которых содержит маску *.
        /// </summary>
        /// <param name="firstValue">Строковое значение.</param>
        /// <param name="secondValue">Строковое значение.</param>
        /// <returns>Возвращает true, если строки совпадают с учетом маски. В противном случае - false.</returns>
        private bool IsMaskCovered(string firstValue, string secondValue)
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
                string longestHead = firstValueHead.Equals(shortestHead) ? secondValueHead : firstValueHead;
                string longestTail = firstValueTail.Equals(shortestTail) ? secondValueTail : firstValueTail;
                condition = longestHead.Contains(shortestHead) && longestTail.Contains(shortestTail);
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
    }
}
