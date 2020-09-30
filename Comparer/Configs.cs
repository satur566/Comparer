using System;
using System.Collections.Generic;
using System.IO;

namespace Comparer
{
    public class Configs
    {
        private string resultPath = "";
        private string referencePath = "";
        private int startIndex = 0;
        private List<int> ignoreIndexes = new List<int>();
        public string ResultPath 
        {
            get
            {
                return resultPath;
            }
            set
            {               
                resultPath = CheckFileAvailability(value);
            }
        }
        public string ReferencePath {
            get
            {
                return referencePath;
            }
            set
            {
                referencePath = CheckFileAvailability(value);
            }
        }
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
        } //TODO: return --value
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
        /// <summary>
        /// Проверяет использование файла другим процессом.
        /// </summary>
        /// <param name="filename">Полное имя файла.</param>
        /// <returns>Возвращает true, если файл заблокирован другим процессом. В противном случае возвращает false.</returns>
        private bool IsFileLocked(string filename)
        {
            bool Locked = false;
            try
            {
                FileStream fs =
                    File.Open(filename, FileMode.OpenOrCreate,
                    FileAccess.ReadWrite, FileShare.None);
                fs.Close();
            }
            catch
            {
                Locked = true;
            }
            return Locked;
        }
        /// <summary>
        /// Проверяет доступность и наличие файла.
        /// </summary>
        /// <param name="path">Полное имя файла.</param>
        private string CheckFileAvailability(string path)
        {
            if (!File.Exists(path))
            {
                throw new Exception($"Файл {path} не сушествует.");
            }
            else if (IsFileLocked(path))
            {
                throw new Exception($"Файл {path} занят другим процессом.");
            }
            return path;
        }
    }
}
