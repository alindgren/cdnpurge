using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PurgeCDN;

namespace PurgeCDN.Tests
{
    [TestClass]
    public class CdnPurgerTests
    {
        [TestMethod]
        public void TestBuildPostData()
        {
            string postData = CdnPurger.BuildPostData("goyabeta-57a7.kxcdn.com", new string[] { "test.html", "hello" });
            Console.WriteLine(postData);
            Assert.IsTrue("{\"urls\":[\"goyabeta-57a7.kxcdn.com/test.html\",\"goyabeta-57a7.kxcdn.com/hello\"]}" == postData);
        }

        [TestMethod]
        public void TestBuildPostData2()
        {
            string postData = CdnPurger.BuildPostData("goyabeta-57a7.kxcdn.com", new string[] { "test.html" });
            Console.WriteLine(postData);
            Assert.IsTrue("{\"urls\":[\"goyabeta-57a7.kxcdn.com/test.html\"]}" == postData);
        }

        [TestMethod]
        public void TestBuildPostDataForTags()
        {
            string postData = CdnPurger.BuildPostData(new string[] { "tag1" });
            Console.WriteLine(postData);
            Assert.IsTrue("{\"tags\":[\"tag1\"]}" == postData);
        }

        [TestMethod]
        public void TestBuildPostDataForTags2()
        {
            string postData = CdnPurger.BuildPostData(new string[] { "tag1", "tag2" });
            Console.WriteLine(postData);
            Assert.IsTrue("{\"tags\":[\"tag1\",\"tag2\"]}" == postData);
        }
    }
}
