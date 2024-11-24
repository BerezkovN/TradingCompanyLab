using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using DAL.AdoNet;
using Microsoft.Extensions.Configuration;
using DTO;
namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        
        [TestMethod]
        public void Add_ShouldReturnCorrectSum()
        {
            
            var result = 5 + 3;

            
            Assert.AreEqual(8, result);
        }
        [TestMethod]
        public void Add_ShouldReturnCorrectSum2()
        {

            var result = 5 + 3;


            Assert.AreEqual(7, result);
        }
    }
}