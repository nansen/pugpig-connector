using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using EPiPugPigConnector.Editions.Factories;
using EPiPugPigConnector.Editions.Interfaces.Edition;
using EPiPugPigConnector.Editions.Interfaces.Editions;
using EPiPugPigConnector.Fakes.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPiPugPigConnector.Tests
{
    [TestClass]
    public class UnitTest_Template
    {
        [TestMethod]
        public void Test_MethodDescription_Does_This_And_That()
        {
            //Arrange 
            int leftInt = 1;
            int rightInt = 2;

            //Act
            int result = leftInt + rightInt;

            //Assert
            Assert.IsTrue(result == 3);
        }
    }
}