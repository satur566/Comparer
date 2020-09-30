using System;
using System.Collections.Generic;
using System.IO;

namespace Comparer
{
    public class Configs
    {
        private int startIndex = 0;
        private List<int> ignoreIndexes = new List<int>();
        public string ResultPath { get; set; }
        public string ReferencePath { get; set; }
        public int StartIndex
        {
            get
            {
                return startIndex;
            }
            set
            {
                startIndex = --value;
            }
        }
        public int EndIndex { get; set; } = 0;
        public List<int> GetIgnoreIndexes()
        {
            return ignoreIndexes;
        }
        public void SetIgnoreIndexes(string value)
        {
            string[] tempArray = value.Split(',');           
            List<int> tempList = new List<int>();
            for (int i = 0; i < tempArray.Length; i++)            
            {
                if(!int.TryParse(tempArray[i], out _))
                {
                    tempList.Clear();
                    throw new Exception($"-ignore: номер строки на позиции {i + 1} должен быть числом.");
                }
                tempList.Add(Convert.ToInt32(tempArray[i]) - 1);
            }
            ignoreIndexes = new List<int>(tempList);
        }        
    }
}