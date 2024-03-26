using DataProcessing.Controllers;
using DataProcessing.Services;
using FakeItEasy;
using KeyManagement.Services;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework.Constraints;

namespace DataProcessing.Tests
{
    public class ConvertImageControllerTests
    {
        private string _path = "C:\\Projects\\Internship\\DataProcessing\\Data\\Images\\Image2.jpg";

        [Test]
        public async Task ConvertImage_With_Azure_Provider_ReturnTextString()
        {
            //Arrange
            var factory = A.Fake<ProviderFactory>(a => a.WithArgumentsForConstructor(() => new ProviderFactory(new AzureKeyVaultService())));
            var controller = new ConvertImageController(factory);

            //Act
            var actionResult = await controller.ConvertToText(_path, "azure");

            //Assert
            var result = actionResult as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.TypeOf<string>());
        }

        [Test]
        public async Task ConvertImage_With_Amazon_Provider_ReturnTextString()
        {
            //Arrange
            var factory = A.Fake<ProviderFactory>(a => a.WithArgumentsForConstructor(() => new ProviderFactory(new AzureKeyVaultService())));
            var controller = new ConvertImageController(factory);

            //Act
            var actionResult = await controller.ConvertToText(_path, "amazon");

            //Assert
            var result = actionResult as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.TypeOf<string>());
        }

        [Test]
        public async Task ConvertImage_With_Any_Provider_PathNotFound_ReturnBadRequest()
        {
            //Arrange
            var factory = A.Fake<ProviderFactory>(a => a.WithArgumentsForConstructor(() => new ProviderFactory(new AzureKeyVaultService())));
            var controller = new ConvertImageController(factory);

            //Act
            var actionResult = await controller.ConvertToText("Invalid path!", "any");

            //Assert
            var result = actionResult as BadRequestObjectResult;
            Assert.That(result.Value, Is.EqualTo("File not found"));
        }

        [Test]
        public async Task ConvertImage_With_A_Provider_NotImplemented_Return_ArgumentOutOfRangeException()
        {
            //Arrange
            var factory = A.Fake<ProviderFactory>(a => a.WithArgumentsForConstructor(() => new ProviderFactory(new AzureKeyVaultService())));
            var controller = new ConvertImageController(factory);

            //Act, Assert
            var exception = Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => controller.ConvertToText(_path, "any"));
            Assert.That(exception.Message, new StartsWithConstraint("The provider name not found!"));

        }
    }
}