using DataProcessing.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DataProcessing.Tests
{
    public class ConvertImageControllerTests
    {
        private string _path="C:\\Projects\\Internship\\DataProcessing\\DataProcessing\\Content\\Images\\Image2.jpg";

        [Test]
        public async Task ConvertImageToText_With_Azure_Provider_ReturnTextString()
        {
            //Arrange
            var controller= new ConvertImageController();
            
            //Act
            var actionResult=await controller.ConvertImageToText(_path, "azure");

            //Assert
            var result= actionResult as OkObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.TypeOf<string>());
        }
    }
}