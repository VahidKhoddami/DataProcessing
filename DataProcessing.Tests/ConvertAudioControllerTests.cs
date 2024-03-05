using DataProcessing.Controllers;
using DataProcessing.Services;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework.Constraints;

namespace DataProcessing.Tests
{
    public class ConversionControllerTests
    {
        private string _path = "C:\\Projects\\Internship\\DataProcessing\\DataProcessing\\Content\\Audios\\Audio1(Noisy).wav";

        [Test]
        public async Task ConvertAudio_With_AssemblyAI_Provider_ReturnTextString()
        {
            //Arrange
            var factory = A.Fake<ProviderFactory>(a => a.WithArgumentsForConstructor(() => new ProviderFactory(new AzureKeyVaultService())));
            var controller = new ConvertAudioController(factory);

            //Act
            var actionResult = await controller.ConvertToText(_path, "assemblyai");

            //Assert
            var result = actionResult as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.TypeOf<string>());
        }

        [Test]
        public async Task ConvertAudio_With_Any_Provider_PathNotFound_ReturnBadRequest()
        {
            //Arrange
            var factory = A.Fake<ProviderFactory>();
            var controller = new ConvertAudioController(factory);

            //Act
            var actionResult = await controller.ConvertToText("Invalid path!", "any");

            //Assert
            var result = actionResult as BadRequestObjectResult;
            Assert.That(result.Value, Is.EqualTo("File not found"));
        }

        [Test]
        public async Task ConvertAudio_With_A_Provider_NotImplemented_Return_ArgumentOutOfRangeException()
        {
            //Arrange
            var factory = A.Fake<ProviderFactory>();
            var controller = new ConvertAudioController(factory);

            //Act, Assert
            var exception = Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => controller.ConvertToText(_path, "any"));
            Assert.That(exception.Message, new StartsWithConstraint("The provider name not found!"));

        }
    }
}