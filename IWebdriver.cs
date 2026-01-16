using OpenQA.Selenium;

namespace FinalProject
{
    internal interface IWebdriver
    {
        string url { get; set; }

        object FindElement(By by);
    }
}