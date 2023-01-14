using Xunit;
using Pose;
using Ajay.Legend.App.CSharp.Controllers;
using Moq;
using Microsoft.Extensions.Logging;
using Ajay.Legend.App.CSharp.Models;
using Defra.Cdp.Telemetry.Loggers;

namespace Ajay.Legend.App.Tests;

public class IndexPageTests
{
    
    [Fact]
    public void ControllerShouldReturnStaticText()
    {
        var homeController = new HomeController(new Mock<IStandardLogger>().Object);

        var result = homeController.Index() as Microsoft.AspNetCore.Mvc.ViewResult;

        var model = result?.Model as IndexModel;
        Assert.Equal("Hello World", model?.HeaderText);
    }


}