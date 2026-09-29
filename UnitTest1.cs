using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Text.RegularExpressions;
using System;
using System.Collections;

namespace MyFirstAutomationTest
{
    public class FirefoxTest
    {
            int fa,fb,summ,substract,multiply,divide;
            int frac;
        private FirefoxDriver? _driver;

        private static IEnumerable<TestCaseData> SummCases()
        {
            var rand = new Random();
            int a = rand.Next(1000);
            int b = rand.Next(1000);
            yield return new TestCaseData(a, b, a + b);
        }

        [SetUp]
        public void Setup()
        {
            FirefoxOptions options = new FirefoxOptions();

            _driver =  new FirefoxDriver(options);
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            
            Random rand = new Random();

            fa = rand.Next();
            fb = rand.Next();
            frac = rand.Next(10);

            summ = fa+fb;

            _driver.Navigate().GoToUrl("https://svyatoslav.biz/testlab/calc/");
            IWebElement FirstNumber = _driver.FindElement(By.CssSelector("Input[name='a']"));
            FirstNumber.SendKeys(fa.ToString());
            IWebElement SecondNumber = _driver.FindElement(By.CssSelector("Input[name='b']"));
            SecondNumber.SendKeys(fb.ToString());
            IWebElement FracElem = _driver.FindElement(By.CssSelector("Input[name='f']"));
            FracElem.SendKeys(frac.ToString());
        
        }

        private void WaitForPageLoad()
        {
        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        wait.Until(d => ((IJavaScriptExecutor)d)
       .ExecuteScript("return document.readyState").ToString() == "complete");
        }

        [TestCaseSource(nameof(SummCases))]
        public void IntTestSumm(int a, int b, int expectedSum)
        {
          
          //  substract =  Math.Round(fa-fb,frac);
        //    multiply =  Math.Round(fa*fb,frac);
         //   divide =  Math.Round(fa/fb,frac);
            Console.WriteLine(summ);
            IWebElement SummRadio = _driver.FindElement(By.CssSelector("Input[value='+']"));
            SummRadio.Click();

            IWebElement SubmitButton = _driver.FindElement(By.CssSelector("Input[type='submit']"));
            SubmitButton.Click();
            
          
            IWebElement ResultText = _driver.FindElement(By.XPath("//b[starts-with(text(),'Result:')]"));
            string rawText = ResultText.Text;

            Console.WriteLine(rawText);
            Match match = Regex.Match(rawText, @"=\s*([0-9.]+)");
            rawText = match.Groups[1].Value;

            double dResult = double.Parse(rawText, System.Globalization.CultureInfo.InvariantCulture);
            Console.WriteLine(dResult);
            Assert.That(summ,Is.EqualTo(dResult));
            //var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            //wait.Until(d => d.Url.Contains("/wiki/"));
          //  WaitForPageLoad();

        }




    [TearDown]
        public void Teardown()
        {
            // Обязательно закрываем браузер и освобождаем память после теста
            if (_driver != null)
            {
                _driver.Quit();
                _driver.Dispose();
            }
        }
    }

}
