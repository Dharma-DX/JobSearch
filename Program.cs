
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Set up the Chrome driver
        IWebDriver driver = new ChromeDriver();

        try
        {
            // Navigate to LinkedIn login page
            driver.Navigate().GoToUrl("https://www.linkedin.com/login");

            // Find the username and password fields and enter your credentials
            // Retrieve credentials from environment variables
            string username = "ranjith1901@gmail.com";
            string password = "Robo1901@";

            // Find the username and password fields and enter your credentials
            driver.FindElement(By.Id("username")).SendKeys(username);
            driver.FindElement(By.Id("password")).SendKeys(password);

            // Click the login button
            driver.FindElement(By.XPath("//button[@type='submit']")).Click();

            // Wait for the page to load
            Thread.Sleep(5000);

            // Navigate to the jobs section
            driver.Navigate().GoToUrl("https://www.linkedin.com/jobs/");

            // Wait for the jobs page to load
            Thread.Sleep(5000);

            IWebElement titleField = driver.FindElement(By.CssSelector("input[placeholder='Title, skill or company']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", titleField);
            titleField.SendKeys("Senior .NET Developer");

            IWebElement locationField = driver.FindElement(By.CssSelector("input[placeholder='Search jobs / European Union']"));
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", locationField);
            locationField.SendKeys("Europe");

            driver.FindElement(By.CssSelector("button.jobs-search-box__submit-button")).Click();
            // Enter job title and location
            //driver.FindElement(By.CssSelector("input[placeholder='Title, skill or company']")).SendKeys("Senior .NET Developer");
            //driver.FindElement(By.CssSelector("input[placeholder='Search jobs / European Union']")).SendKeys("Europe");
            //driver.FindElement(By.CssSelector("button.jobs-search-box__submit-button")).Click();
            Thread.Sleep(5000);
            // Example: Click on the first job listing
            driver.FindElement(By.CssSelector(".job-card-container__link")).Click();
            Thread.Sleep(5000);
            // Get all job listings
            // Find all job cards
            IList<IWebElement> jobCards = driver.FindElements(By.CssSelector(".job-card-container"));

            foreach (var jobListing in jobCards)
            {
                try
                {
                    if (jobListing.Text.Contains("Easy Apply"))
                    {
                        // Click on the job listing
                        jobListing.Click();

                        // Wait for the job details page to load
                        Thread.Sleep(5000);

                        // Check if the "Easy Apply" button is available
                        var easyApplyButton = driver.FindElements(By.CssSelector(".jobs-apply-button")).FirstOrDefault();
                        if (easyApplyButton != null)
                        {
                            easyApplyButton.Click();
                            // Add more steps to fill out the application form

                            // Method to click a button using JavaScript
                            void ClickButtonUsingJavaScript(string buttonText)
                            {
                                IWebElement button = null;
                                try
                                {
                                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                                    button = driver.FindElement(By.XPath($"//button[.//span[text()='{buttonText}']]"));
                                    if (button != null && button.Displayed)
                                    {
                                        button.Click();
                                    }
                                    if (button.Displayed)
                                    {
                                        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", button);
                                    }
                                }
                                catch (WebDriverTimeoutException)
                                {
                                    Console.WriteLine($"{buttonText} button was not found.");
                                    LogError($"{buttonText} button was not found.");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"An error occurred while clicking {buttonText} button: " + ex.Message);
                                    LogError($"An error occurred while clicking {buttonText} button: " + ex.Message);
                                }
                            }

                            // Method to repeatedly click the "Next" button until it is no longer available
                            void ClickNextButtonMultipleTimes()
                            {
                                while (true)
                                {
                                    try
                                    {
                                        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                                        IWebElement nextButton = null;

                                        try
                                        {
                                            nextButton = driver.FindElement(By.XPath("//button[.//span[text()='Next']]"));
                                            if (nextButton != null && nextButton.Displayed)
                                            {
                                                nextButton.Click();
                                            }
                                        }
                                        catch (WebDriverTimeoutException)
                                        {
                                            // Handle the timeout exception if the element is not found within the specified time
                                            Console.WriteLine("No more 'Next' buttons found. Moving to 'Review' or 'Submit'.");
                                            break;
                                        }
                                        catch (NoSuchElementException)
                                        {
                                            // Handle the case where the element is not found
                                            Console.WriteLine("No 'Next' button found.");
                                            break;
                                        }

                                        if (nextButton != null && nextButton.Displayed)
                                        {
                                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", nextButton);
                                            Thread.Sleep(2000); // Wait for the next section to load
                                        }
                                        else
                                        {
                                            Console.WriteLine("No more 'Next' buttons found. Moving to 'Review' or 'Submit'.");
                                            // Attempt to click the "Review" button
                                            ClickButtonUsingJavaScript("Review");

                                            // Attempt to click the "Submit" button
                                            ClickButtonUsingJavaScript("Submit");
                                            break;
                                        }
                                    }
                                    catch (WebDriverTimeoutException)
                                    {
                                        Console.WriteLine("No more 'Next' buttons found. Moving to 'Review' or 'Submit'.");
                                        // Attempt to click the "Review" button
                                        ClickButtonUsingJavaScript("Review");

                                        // Attempt to click the "Submit" button
                                        ClickButtonUsingJavaScript("Submit");
                                        break;
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("An error occurred while clicking 'Next' button: " + ex.Message);
                                        LogError("An error occurred while clicking 'Next' button: " + ex.Message);
                                        break;
                                    }
                                }
                            }

                            // Check and click "Next" button multiple times
                            ClickNextButtonMultipleTimes();

                            // Check and click "Review" button using JavaScript
                            ClickButtonUsingJavaScript("Review");

                            // Finally, submit the form using JavaScript
                            ClickButtonUsingJavaScript("Submit");
                        }

                        // Go back to the job listings page
                        driver.Navigate().Back();
                        Thread.Sleep(5000);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    LogError("An error occurred: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
            LogError("An error occurred: " + ex.Message);
        }
        finally
        {
            // Close the browser
            driver.Quit();
        }

        // Keep the command line open
        Console.WriteLine("Press Enter to close...");
        Console.ReadLine();
    }

    static void LogError(string message)
    {
        string logFilePath = "error_log.txt";
        using (StreamWriter writer = new StreamWriter(logFilePath, true))
        {
            writer.WriteLine($"{DateTime.Now}: {message}");
        }
    }
}
