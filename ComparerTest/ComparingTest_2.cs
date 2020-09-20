using System;
using System.IO;
using Comparer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComparerTest
{
    [TestClass]
    public class ComparingTest_2
    {
        [TestMethod]
        public void CompareTest()
        {
            //Arrange
            int expectedValue = 9;
            string testFilesFolder = Environment.CurrentDirectory.Replace("\\bin\\Debug", "");
            string firstTestFile = Path.Combine(testFilesFolder, @"test files", @"sample1.txt");
            string secondTestFile = Path.Combine(testFilesFolder, @"test files", @"sample2.txt");
            string expectedFirstLine = null;
            string expectedSecondLine = "1";
            Comparing compare = new Comparing(firstTestFile, secondTestFile);
            //Act
            int actualValue = compare.Compare(out string actualFirstLine, out string actalSecondLine);
            //Assert
            Assert.AreEqual(expectedValue, actualValue, "Unmached string number are not correct.");
            Assert.AreEqual(actualFirstLine, expectedFirstLine, "Unmached result string value are not correct.");
            Assert.AreEqual(actalSecondLine, expectedSecondLine, "Unmached reference string value are not correct.");
        }
    }
}
