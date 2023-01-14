using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

namespace Ajay.Legend.App.CSharp.UITests
{
    public class SampleUITest : IDisposable
    {
        private readonly IWebDriver driver;

        public SampleUITest()
        {
            this.driver = new ChromeDriver();
        }

        [Fact]
        public void HomePageDisplaysWelcomeText()
        {
            this.driver.Url = Environment.GetEnvironmentVariable("Service_Frontend_Url");
            var el = this.driver.FindElement(By.Id("welcomeBanner")).Text;
            _ = el.Should().Be("Hello World");
        }

        public void Dispose()
        {
            this.driver.Quit();
            this.driver.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
