using DataAnalytic.Controllers;
using DataAnalytic.Services;
using FakeItEasy;
using KeyManagement.Services;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework.Constraints;

namespace DataAnalytic.Tests
{
    public class AnalyzeTextControllerTests
    {

        private readonly string _text = "The laptop performance is good. But the keyboard and mouse pad are awful! The camera is not good and it's zoom functionality is not working.";

        [Test]
        public async Task AnalyzeText_With_Amazon_ReturnJsonText()
        {
            //Arrange
            var factory = A.Fake<ProviderFactory>(a => a.WithArgumentsForConstructor(() => new ProviderFactory(new AzureKeyVaultService())));
            var controller = new AnalyzeTextController(factory);

            //Act
            var actionResult = await controller.AnalyzeText(_text, "amazon");

            //Assert
            var result = actionResult as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.TypeOf<string>());
        }

        [Test]
        public async Task AnalyzeText_With_Azure_ReturnJsonText()
        {
            //Arrange
            var factory = A.Fake<ProviderFactory>(a => a.WithArgumentsForConstructor(() => new ProviderFactory(new AzureKeyVaultService())));
            var controller = new AnalyzeTextController(factory);

            //Act
            var actionResult = await controller.AnalyzeText(_text, "azure");

            //Assert
            var result = actionResult as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.TypeOf<string>());
        }

        [Test]
        public async Task AnalyzeText_With_IBM_ReturnJsonText()
        {
            //Arrange
            var factory = A.Fake<ProviderFactory>(a => a.WithArgumentsForConstructor(() => new ProviderFactory(new AzureKeyVaultService())));
            var controller = new AnalyzeTextController(factory);

            //Act
            var actionResult = await controller.AnalyzeText(_text, "ibm");

            //Assert
            var result = actionResult as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.TypeOf<string>());
        }

        [Test]
        public async Task AnalyzeText_With_A_Provider_NotImplemented_Return_ArgumentOutOfRangeException()
        {
            //Arrange
            var factory = A.Fake<ProviderFactory>();
            var controller = new AnalyzeTextController(factory);

            //Act, Assert
            var exception = Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => controller.AnalyzeText(_text, "any"));
            Assert.That(exception.Message, new StartsWithConstraint("The provider name not found!"));
        }
    }
}