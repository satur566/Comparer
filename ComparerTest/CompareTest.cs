using System;
using System.Collections.Generic;
using System.IO;
using Comparer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComparerTest
{
    [TestClass]
    public class CompareTest //TODO: more test for the GOD OF TESTS!
    {
        /// <summary>
        /// Проводит тестирование метода int Compare(out string firstUnmatchedValue, out string secondUnmatchedValue).
        /// </summary>
        /// <param name="expectedValue">Ожидаемое возвращаемое значение.</param>
        /// <param name="firstFileName">Путь к первому проверяемому файлу.</param>
        /// <param name="secondFileName">Путь ко второму проверяемому файлу.</param>
        /// <param name="expectedFirstLine">Ожидаемое возвращаемое значение отличающейся строки результирующего файла.</param>
        /// <param name="expectedSecondLine">Ожидаемое возвращаемое значение отличающейся строки эталонного файла.</param>
        private void TestBody(int expectedValue, string firstFileName, string secondFileName, string expectedFirstLine, string expectedSecondLine)
        {
            //Arrange
            string testFilesFolder = Path.Combine(Environment.CurrentDirectory.Replace("\\bin\\Debug", ""), @"test files");
            string firstTestFile = Path.Combine(testFilesFolder, firstFileName);
            string secondTestFile = Path.Combine(testFilesFolder, secondFileName);
            Comparing compare = new Comparing(firstTestFile, secondTestFile);
            //Act
            int actualValue = compare.Compare(out string actualFirstLine, out string actualSecondLine);
            //Assert
            Assert.AreEqual(expectedValue, actualValue, "Номер строки с отличием некорректен.");
            Assert.AreEqual(actualFirstLine, expectedFirstLine, "Значение несовпавшей строки результирующего файла некорректно.");
            Assert.AreEqual(actualSecondLine, expectedSecondLine, "Значение несовпавшей строки эталонного файла некорректно.");
        }

        /// <summary>
        /// Проводит тестирование метода int Compare(out string firstUnmatchedValue, out string secondUnmatchedValue).
        /// </summary>
        /// <param name="expectedValue">Ожидаемое возвращаемое значение.</param>
        /// <param name="firstFileName">Путь к первому проверяемому файлу.</param>
        /// <param name="secondFileName">Путь ко второму проверяемому файлу.</param>
        /// <param name="beginIndex">Номер строки с которой требуется начать проверку.</param>
        /// <param name="endIndex">Номер строки, на которой требуется закончить проверку.</param>
        /// <param name="array">Номера строк, которые необходимо пропустить при проверке.</param>
        /// <param name="expectedFirstLine">Ожидаемое возвращаемое значение отличающейся строки результирующего файла.</param>
        /// <param name="expectedSecondLine">Ожидаемое возвращаемое значение отличающейся строки эталонного файла.</param>
        private void TestBody(int expectedValue, string firstFileName, string secondFileName, int beginIndex, int endIndex, int[] ignoreList, string expectedFirstLine, string expectedSecondLine)
        {
            //Arrange
            string testFilesFolder = Path.Combine(Environment.CurrentDirectory.Replace("\\bin\\Debug", ""), @"test files");
            string firstTestFile = Path.Combine(testFilesFolder, firstFileName);
            string secondTestFile = Path.Combine(testFilesFolder, secondFileName);
            Comparing compare = new Comparing(firstTestFile, secondTestFile, beginIndex, endIndex, new List<int>(ignoreList));
            //Act
            int actualValue = compare.Compare(out string actualFirstLine, out string actalSecondLine);
            //Assert
            Assert.AreEqual(expectedValue, actualValue, "Номер строки с отличием некорректен.");
            Assert.AreEqual(actualFirstLine, expectedFirstLine, "Значение несовпавшей строки результирующего файла некорректно.");
            Assert.AreEqual(actalSecondLine, expectedSecondLine, "Значение несовпавшей строки эталонного файла некорректно.");
        }

        [TestMethod]
        public void BasicUnmatchingInSimilarFileLength()
        {
            TestBody(4, "sample1.txt", "sample3.txt", "01.09.2020 21:49:34 - Reading html: SUCCESS.", "01.09.2020 21:49:34 - Reading html: SUCCESSFULL.");
        }
        [TestMethod]
        public void LinesCountDifferenceUnmatching()
        {
            TestBody(9, "sample1.txt", "sample2.txt", null, "1");
        }
        [TestMethod]
        public void OneLineFullMasked()
        {
            TestBody(-1, "sample1.txt", "sample4.txt", "", "");
        }
        [TestMethod]
        public void OneLinePartialMaskedMatching()
        {
            TestBody(-1, "sample1.txt", "sample5.txt", "", "");
        }
        [TestMethod]
        public void OneLinePartialMaskedUnmatching()
        {
            TestBody(6, "sample1.txt", "sample6.cfg", "01.09.2020 21:51:03 - ", "01.*:23 - ");
        }
        [TestMethod]
        public void DifferentEncodingMatchingContent()
        {
            TestBody(-1, "sample1.txt", "sample7.file", "", "");
        }
    }
}
