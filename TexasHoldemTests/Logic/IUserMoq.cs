using Microsoft.VisualStudio.TestTools.UnitTesting;
using Client.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TexasHoldem.Logic.Users;
using TexasHoldemShared;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace Client.Logic.Tests
{

    [TestClass()]
    public class IUserMoq
    {
        [TestMethod()]
        public void returnExcpectedIdTest()
        {
            int excpected = 305509069;
            var mock = new Mock<IUser>(MockBehavior.Strict);
            Player p = new Player(mock.Object, 100, 100);
            mock.Setup(foo => foo.Id()).Returns(excpected);
            var results = p.user.Id();
            Assert.IsTrue(results == excpected);
            
        }
        [TestMethod()]
        public void CanReduceMoneyTest()
        {
            int excpected = 200;

            var mock = new Mock<IUser>(MockBehavior.Strict);
            Player p = new Player(mock.Object, 100, 100);
            mock.Setup(foo => foo.ReduceMoneyIfPossible(1)).Returns(false);

            //should be true... cause it's enoungh money
            Assert.IsTrue(!p.user.ReduceMoneyIfPossible(1));
           
        }

        [TestMethod()]
        public void WinNumCallBackTest()
        {
            var mock = new Mock<IUser>(MockBehavior.Strict);
            Player p = new Player(mock.Object, 100, 100);
            int calls = 100;
            mock.Setup(foo => foo.WinNum)
                .Returns(() => calls)
                .Callback(() => calls++);

            Assert.IsTrue(p.user.WinNum==100);
            Assert.IsTrue(p.user.WinNum== 101);

        }

        [TestMethod()]
        public void WinTest()
        {
            var mock = new Mock<IUser>(MockBehavior.Strict);
            Player p = new Player(mock.Object, 100, 100);
            mock.Setup(foo => foo.IncWinNum()).Returns(true);
            mock.Setup(foo => foo.EditUserPoints(10100)).Returns(true);
            mock.Setup(foo => foo.Money()).Returns(10000);
            mock.Setup(foo => foo.WinNum).Returns(100);


            Assert.IsTrue(p.Win(1000));

        }

    }


}