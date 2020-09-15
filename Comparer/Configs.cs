using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comparer
{
    static class Configs
    {
        private static string resultPath;
        private static string referencePath;
        private static int startIndex = 0;
        private static int endIndex = 0;
        private static int[] ignoreIndex;
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
                if (value.GetType() != typeof(int))
                {
                    throw new Exception("Номер строки должен быть числом.");
                }
                startIndex = --value;
            }
        }
        public static int EndIndex 
        {
            get
            {
                return endIndex;
            }
            set
            {
                if (value.GetType() != typeof(int))
                {
                    throw new Exception("Номер строки должен быть числом.");
                }
                endIndex = --value;
            }
        }
        public static int[] IgnoreIndexes { get; set; } //TODO: -1 to set
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
