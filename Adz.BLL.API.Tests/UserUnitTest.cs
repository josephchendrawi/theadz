using System;
using NUnit.Framework;
using ServiceStack.Testing;
using ServiceStack;
using Adz.BLL.API.ServiceInterface;
using Adz.BLL.API.ServiceModel;
using Adz.BLL.API.ServiceModel.Types;
using ServiceStack.Auth;

namespace WebApplication2.Tests
{
    [TestFixture]
    public class UserUnitTests
    {
        private readonly ServiceStackHost appHost;

        public UserUnitTests()
        {
            appHost = new BasicAppHost(typeof(MyServices).Assembly)
            {
                ConfigureContainer = container =>
                {
                    //Add your IoC dependencies here
                }
            }
            .Init();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            appHost.Dispose();
        }

        /// <summary>
        /// add extension "NUnit Test Adapter" first before unit testing.
        /// </summary>
        
        [Test]
        public void LoginTest()
        {
            var service = appHost.Container.Resolve<UserServices>();

            var response = (UserResponse)service.Post(new Login
            {
                Username = "hello2@appstream.com.my",
                Password = "hahahaha",
                DeviceInfo = new DeviceInfo()
                {
                    OS = "Android",
                    OS_Version = "5.0",
                    Model = "HTC One M9",
                    UniqueId = "a1s2d3f5g6h7j8k9l0"
                }
            });

            Assert.That(response, Is.Not.Null);

            Assert.That(response.Result.UserId, Is.EqualTo(17));
            Assert.That(response.Result.FirstName, Is.EqualTo("Hello"));
            Assert.That(response.Result.LastName, Is.EqualTo("Low"));
            Assert.That(response.Result.Email, Is.EqualTo("hello2@appstream.com.my"));
        }

    }
}
