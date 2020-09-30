using System;
using System.Collections.Generic;
using System.IO;
using Comparer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComparerTest
{
    [TestClass]
    public class IsMaskCoveredTests //TODO: thrown exception tests!
    {
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
        private void TestBody(int expectedValue, string firstFileName, string secondFileName, int beginIndex, int endIndex, string ignoreList, string expectedFirstLine, string expectedSecondLine)
        {
            //Arrange
            string testFilesFolder = Path.Combine(Environment.CurrentDirectory.Replace("\\bin\\Debug", ""), @"test files");
            string firstTestFile = Path.Combine(testFilesFolder, firstFileName);
            string secondTestFile = Path.Combine(testFilesFolder, secondFileName);
            Comparing compare = new Comparing();
            compare.ResultPath = firstTestFile;
            compare.ReferencePath = secondTestFile;
            compare.StartIndex = beginIndex;
            compare.EndIndex = endIndex;
            compare.SetIgnoreIndexes(ignoreList);
            //Act
            int actualValue = compare.Compare(out string actualFirstLine, out string actalSecondLine);
            //Assert
            Assert.AreEqual(expectedValue, actualValue, "Номер строки с отличием некорректен.");
            Assert.AreEqual(actualFirstLine, expectedFirstLine, "Значение несовпавшей строки результирующего файла некорректно.");
            Assert.AreEqual(actalSecondLine, expectedSecondLine, "Значение несовпавшей строки эталонного файла некорректно.");
        }
        [TestMethod]
        public void OneLineContainsOneMaskInside()
        {
            TestBody(-1, "MaskCoveredStrings1.txt", "MaskCoveredStrings2.txt", 1, 1, "10", null, null);
        }
        [TestMethod]
        public void OneLineContainsTwoMasksInside()
        {
            TestBody(-1, "MaskCoveredStrings1.txt", "MaskCoveredStrings2.txt", 2, 2, "10", null, null);
        }
        [TestMethod]
        public void BothLinesAreMask()
        {
            TestBody(-1, "MaskCoveredStrings1.txt", "MaskCoveredStrings2.txt", 3, 3, "10", null, null);
        }
        [TestMethod]
        public void BothLinecContainsMask()
        {
            TestBody(-1, "MaskCoveredStrings1.txt", "MaskCoveredStrings2.txt", 4, 4, "10", null, null);
        }
        [TestMethod]
        public void BothLinesContainsTwoMasks()
        {
            TestBody(-1, "MaskCoveredStrings1.txt", "MaskCoveredStrings2.txt", 5, 5, "10", null, null);
        }
        [TestMethod]
        public void OneLineContainsTailmask()
        {
            TestBody(-1, "MaskCoveredStrings1.txt", "MaskCoveredStrings2.txt", 6, 6, "10", null, null);
        }
        [TestMethod]
        public void OneLineIsMask_AnotherContainsMask()
        {
            TestBody(-1, "MaskCoveredStrings1.txt", "MaskCoveredStrings2.txt", 7, 7, "10", null, null);
        }
        [TestMethod]
        public void OneLineContainsHeadMask()
        {
            TestBody(-1, "MaskCoveredStrings1.txt", "MaskCoveredStrings2.txt", 8, 8, "20", null, null);
        }
        [TestMethod]
        public void OneLineContainsTailMask_AnotherContainsBodyMask()
        {
            TestBody(-1, "MaskCoveredStrings1.txt", "MaskCoveredStrings2.txt", 9, 9, "20", null, null);
        }
        [TestMethod]
        public void OneLineContainsHeadMask_AnotherContainsBodyMask()
        {
            TestBody(-1, "MaskCoveredStrings1.txt", "MaskCoveredStrings2.txt", 10, 10, "20", null, null);
        }
        [TestMethod]
        public void OneLineContainsCharBetweenMasks_AnotherLineDoesNotContainsThatChar()
        {
            TestBody(10, "MaskCoveredStrings1.txt", "MaskCoveredStrings2.txt", 11, 11, "20", "*f*", "akskskdlkaldklas");
        }
        [TestMethod]
        public void NoMasksSameLines()
        {
            TestBody(-1, "MaskCoveredStrings1.txt", "MaskCoveredStrings2.txt", 12, 12, "20", null, null);
        }
        [TestMethod]
        public void LinesContainsMasks_AsWellAsUnmatching()
        {
            TestBody(12, "MaskCoveredStrings1.txt", "MaskCoveredStrings2.txt", 13, 13, "20", "abc*f", "a*e");
        }
        [TestMethod]
        public void OneLineContainsCharBetweenMasks_AnotherContainsDifferentCharsAndmaskBetweenThem()
        {
            TestBody(-1, "MaskCoveredStrings1.txt", "MaskCoveredStrings2.txt", 14, 14, "20", null, null);
        }
        [TestMethod]
        public void OneLineContainsCharBetweenMasks_AnotherLineContainsAnotherCharBetweenMasks()
        {
            TestBody(-1, "MaskCoveredStrings1.txt", "MaskCoveredStrings2.txt", 15, 15, "20", null, null);
        }
    }
}
