using System;
using System.IO;
using Comparer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComparerTest
{
    [TestClass]
    public class ComparingTest_1
    {
        [TestMethod]
        public void CompareTest()
        {
            //Arrange
            int expectedValue = 4;
            string testFilesFolder = Environment.CurrentDirectory.Replace("\\bin\\Debug", "");
            string firstTestFile = Path.Combine(testFilesFolder, @"test files", @"sample1.txt");
            string secondTestFile = Path.Combine(testFilesFolder, @"test files", @"sample3.txt");
            string expectedFirstLine = "01.09.2020 21:49:34 - Reading html: SUCCESS.";
            string expectedSecondLine = "01.09.2020 21:49:34 - Reading html: SUCCESSFULL.";
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
