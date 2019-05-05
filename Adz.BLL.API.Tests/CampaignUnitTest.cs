using System;
using NUnit.Framework;
using ServiceStack.Testing;
using ServiceStack;
using ServiceStack.Web;
using Adz.BLL.API.ServiceInterface;
using Adz.BLL.API.ServiceModel;
using Adz.BLL.API.ServiceModel.Types;

namespace WebApplication2.Tests
{
    [TestFixture]
    public class CampaignUnitTests
    {
        private readonly ServiceStackHost appHost;

        public CampaignUnitTests()
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
        public void CampaignGetTest()
        {
            var service = appHost.Container.Resolve<CampaignServices>();

            var response = (CampaignResponse)service.Get(new CampaignGet
            {
                id = 14
            });
            
            Assert.That(response, Is.Not.Null);

            Assert.That(response.Result.CampaignId, Is.EqualTo(14));
            Assert.That(response.Result.MerchantId, Is.EqualTo(10));
            Assert.That(response.Result.Name, Is.EqualTo("Campaign2"));
        }        

    }
}
