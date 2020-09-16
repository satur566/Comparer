using System;
using System.Collections.Generic;
using System.IO;

namespace Comparer
{
    class Comparing
    {
        private int StartIndex { get; set; }
        public static int EndIndex { get; set; }
        private List<int> IgnoreIndexes { get; set; }
        private string[] ResultArray { get; set; }
        private string[] ReferenceArray { get; set; }

        public Comparing(string firstValue, string secondValue, int beginsWith, int endsWith, List<int> ignoreItems)
        {
            ResultArray = File.ReadAllLines(firstValue);
            ReferenceArray = File.ReadAllLines(secondValue);
            StartIndex = beginsWith;
            EndIndex = endsWith;
            IgnoreIndexes = new List<int>(ignoreItems);
        }

        public string Compare()
        {
            int shortestLengs = ResultArray.Length > ReferenceArray.Length ? ReferenceArray.Length : ResultArray.Length;
            EndIndex = EndIndex != 0 && shortestLengs > EndIndex ? EndIndex : shortestLengs;
            if (StartIndex > EndIndex)
            {
                throw new Exception("Значение нижней границы диапазона поиска больше значения верхней границы диапазона поиска.");
            }
            int discrepancyIndex = -1;
            for (int i = StartIndex; i < EndIndex; i++)
            {
                if (IgnoreIndexes.Contains(i))
                {
                    continue;
                }
                if (!ResultArray[i].Equals(ReferenceArray[i]))
                {
                    if (IsMaskCovered(ResultArray[i], ReferenceArray[i]))
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
                return "Содержимое файлов идентично.";
            }
            return $"Первое различие встретилось на {discrepancyIndex + 1} строке:\n" +
                $"Эталон: \t{ ReferenceArray[discrepancyIndex]}\nРезультат: \t{ ResultArray[discrepancyIndex]}";
        }

        private bool IsMaskCovered(string firstValue, string secondValue) //REGEX!!!!!!! I dont know regex =(
        {
            if (firstValue.Equals("*") || secondValue.Equals("*"))
            {
                return true;
            }
            bool condition = false;
            if (firstValue.Contains("*") || secondValue.Contains("*"))
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
