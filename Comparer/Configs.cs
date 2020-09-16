using System;
using System.Collections.Generic;
using System.IO;

namespace Comparer
{
    static class Configs
    {
        private static string resultPath = "";
        private static string referencePath = "";
        private static int startIndex = 0;
        private static int endIndex = 0;
        private static List<int> ignoreIndexes = new List<int>();
        public static string ResultPath 
        {
            get
            {
                return resultPath;
            }
            set
            {
                CheckFileAvailability(value); //TODO: check this!
                resultPath = value;
            }
        }
        public static string ResultEncoding { get; set; } //TODO: descript what encoding we get
        public static string ReferencePath {
            get
            {
                return referencePath;
            }
            set
            {
                CheckFileAvailability(value);
                referencePath = value;
            }
        } //TODO: some tryCatch
        public static string ReferenceEncoding { get; set; } //TODO: descript what encoding we get
        public static int StartIndex
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
        public static int EndIndex //TODO: autoproperty.
        {
            get
            {
                return endIndex;
            }
            set
            {
                endIndex = value;
            }
        }
        public static List<int> GetIgnoreIndexes()
        {
            return ignoreIndexes;
        }
        public static void SetIgnoreIndexes(string value)
        {
            string[] tempArray = value.Split(',');
            List<int> tempList = new List<int>();
            foreach (string item in tempArray)
            {
                if(!int.TryParse(item, out _))
                {
                    tempList.Clear();
                    throw new Exception("Номер строки должен быть числом."); //TODO: или просто проигнорировать? //TODO: НЕ РАБОТАЕТ, ЕСЛИ В КОНЦЕ ИЛИ В НАЧАЛЕ ЕСТЬ ,
                }
                tempList.Add(Convert.ToInt32(item) - 1);
            }
            ignoreIndexes = new List<int>(tempList);
        }
        private static bool IsFileLocked(string filename)
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
        private static void CheckFileAvailability(string path)
        {
            if (!File.Exists(path))
            {
                throw new Exception($"Файл {path} не сушествует.");
            }
            else if (IsFileLocked(path))
            {
                throw new Exception($"Файл {path} занят другим процессом.");
            }
        }
    }
}
