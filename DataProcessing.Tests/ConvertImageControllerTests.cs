using DataProcessing.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework.Constraints;

namespace DataProcessing.Tests
{
    public class ConvertImageControllerTests
    {
        private string _path = "C:\\Projects\\Internship\\DataProcessing\\DataProcessing\\Content\\Images\\Image2.jpg";

        [Test]
        public async Task ConvertImageToText_With_Azure_Provider_ReturnTextString()
        {
            //Arrange
            var controller = new ConvertImageController();

            //Act
            var actionResult = await controller.ConvertImageToText(_path, "azure");

            //Assert
            var result = actionResult as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.TypeOf<string>());
        }

        [Test]
        public async Task ConvertImageToText_With_Amazon_Provider_ReturnTextString()
        {
            //Arrange
            var controller = new ConvertImageController();

            //Act
            var actionResult = await controller.ConvertImageToText(_path, "amazon");

            //Assert
            var result = actionResult as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.TypeOf<string>());
        }

        [Test]
        public async Task ConvertImageToText_With_Any_Provider_PathNotFound_ReturnBadRequest()
        {
            //Arrange
            var controller = new ConvertImageController();

            //Act
            var actionResult = await controller.ConvertImageToText("Invalid path!", "any");

            //Assert
            var result = actionResult as BadRequestObjectResult;
            Assert.That(result.Value, Is.EqualTo("File not found"));
        }

        [Test]
        public async Task ConvertImageToText_With_A_Provider_NotImplemented_Return_ArgumentOutOfRangeException()
        {
            //Arrange
            var controller = new ConvertImageController();

            //Act, Assert
            var exception = Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => controller.ConvertImageToText(_path, "any"));
            Assert.That(exception.Message, new StartsWithConstraint("The provider name not found!"));

        }
    }
}