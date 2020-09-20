using System;
using System.IO;
using Comparer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComparerTest
{
    [TestClass]
    public class IsMaskCoveredTest //TODO: thrown exception tests!
    {
        [TestMethod]
        public void IsMaskCoveredTest_1() //TODO: change two test files that way: make at least 1 string of 2 matching contains * and add that files like new files named "maskedLineContainer_N.
        {
            //Arrange
            int expectedValue = 9;
            string testFilesFolder = Path.Combine(Environment.CurrentDirectory.Replace("\\bin\\Debug", ""), @"test files");
            string firstTestFile = Path.Combine(testFilesFolder, @"sample1.txt"); 
            string secondTestFile = Path.Combine(testFilesFolder, @"sample2.txt");
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
