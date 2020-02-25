using Microsoft.VisualStudio.TestTools.UnitTesting;
using WowLauncher.Utils;

namespace WowSuite.UnitTests
{
    [TestClass]
    public class UrlHelperUnitTest
    {
        [TestMethod]
        public void UrlHelperCombineTest()
        {
            const string pattern = "http://127.0.0.1/part1/part2/part3.txt";

            string result1 = UrlHelper.Combine("http://127.0.0.1", "//part1///", "//part2///", "part3.txt");
            Assert.AreEqual(pattern, result1); //сверяем результат выполнения метода с ожидаемым шаблоном.

            string result2 = UrlHelper.Combine("http://127.0.0.1", "part1", "part2", "part3.txt");
            Assert.AreEqual(pattern, result2); 

            string result3 = UrlHelper.Combine("http://127.0.0.1", "/part1/", "/part2/", "/part3.txt/");
            Assert.AreEqual(pattern, result3); 

            string result4 = UrlHelper.Combine("http://127.0.0.1//", "///part1/", "part2", "////part3.txt//");
            Assert.AreEqual(pattern, result4); 
        }
    }
}
