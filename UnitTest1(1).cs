using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace OrangeHRMTests
{
    [TestClass]
    public class OrangeHRMTests
    {
        #region Initialization and cleanup
        public TestContext instance;
        public TestContext TestContext
        {
            set { instance = value; }
            get { return instance; }
        }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            Console.WriteLine("AssemblyInitialize");
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Console.WriteLine("AssemblyCleanup");
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Console.WriteLine("ClassInitialize");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            Console.WriteLine("ClassCleanup");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            Console.WriteLine("TestInitialize");
            Setup();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Console.WriteLine("TestCleanup");
            TearDown();
        }
        #endregion

        IWebDriver driver;
        string baseUrl = "https://opensource-demo.orangehrmlive.com/web/index.php/auth/login";

        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = baseUrl;
            Thread.Sleep(3000); // Wait for page to load
        }

        public void TearDown()
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }

        // Test Case 1: Verify Successful Login
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC01_LoginSuccess", DataAccessMethod.Sequential)]
        public void TC01_LoginSuccess()
        {
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            string pageTitle = driver.FindElement(By.ClassName("oxd-topbar-header-breadcrumb-module")).Text;
            Assert.AreEqual("Dashboard", pageTitle, "Login failed");
        }

        // Test Case 2: Verify Login with Invalid Password
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC02_LoginInvalidPassword", DataAccessMethod.Sequential)]
        public void TC02_LoginInvalidPassword()
        {
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("wrongpassword");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(2000);

            string errorMessage = driver.FindElement(By.ClassName("oxd-alert-content-text")).Text;
            Assert.IsTrue(errorMessage.Contains("Invalid"), "Error message not shown");
        }

        // Test Case 3: Verify Login with Empty Username
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC03_LoginEmptyUsername", DataAccessMethod.Sequential)]
        public void TC03_LoginEmptyUsername()
        {
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(2000);

            bool validationShown = driver.FindElements(By.ClassName("oxd-input-field-error-message")).Count > 0;
            Assert.IsTrue(validationShown, "Validation not shown for empty username");
        }

        // Test Case 4: Verify Login with Empty Password
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC04_LoginEmptyPassword", DataAccessMethod.Sequential)]
        public void TC04_LoginEmptyPassword()
        {
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(2000);

            bool validationShown = driver.FindElements(By.ClassName("oxd-input-field-error-message")).Count > 0;
            Assert.IsTrue(validationShown, "Validation not shown for empty password");
        }

        // Test Case 5: Verify Logout Functionality
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC05_LogoutSuccess", DataAccessMethod.Sequential)]
        public void TC05_LogoutSuccess()
        {
            // Login first
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Logout
            driver.FindElement(By.ClassName("oxd-userdropdown-tab")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.LinkText("Logout")).Click();
            Thread.Sleep(2000);

            bool loginButton = driver.FindElement(By.CssSelector("button[type='submit']")).Displayed;
            Assert.IsTrue(loginButton, "Logout failed");
        }

        // Test Case 6: Verify Forgot Password Link
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC06_ForgotPasswordLink", DataAccessMethod.Sequential)]
        public void TC06_ForgotPasswordLink()
        {
            driver.FindElement(By.ClassName("orangehrm-login-forgot-header")).Click();
            Thread.Sleep(2000);

            string pageTitle = driver.FindElement(By.ClassName("orangehrm-forgot-password-title")).Text;
            Assert.AreEqual("Reset Password", pageTitle, "Forgot password page not opened");
        }

        // Test Case 7: Verify Dashboard Widgets Display
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC07_DashboardWidgetsDisplay", DataAccessMethod.Sequential)]
        public void TC07_DashboardWidgetsDisplay()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            int widgetCount = driver.FindElements(By.ClassName("orangehrm-dashboard-widget")).Count;
            Assert.IsTrue(widgetCount > 0, "No dashboard widgets displayed");
        }

        // Test Case 8: Verify Admin Menu Navigation
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC08_AdminMenuNavigation", DataAccessMethod.Sequential)]
        public void TC08_AdminMenuNavigation()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Navigate to Admin
            driver.FindElement(By.LinkText("Admin")).Click();
            Thread.Sleep(2000);

            string pageTitle = driver.FindElement(By.ClassName("oxd-topbar-header-breadcrumb-module")).Text;
            Assert.AreEqual("Admin", pageTitle, "Admin page not loaded");
        }

        // Test Case 9: Verify PIM Menu Navigation
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC09_PIMMenuNavigation", DataAccessMethod.Sequential)]
        public void TC09_PIMMenuNavigation()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Navigate to PIM
            driver.FindElement(By.LinkText("PIM")).Click();
            Thread.Sleep(2000);

            string pageTitle = driver.FindElement(By.ClassName("oxd-topbar-header-breadcrumb-module")).Text;
            Assert.AreEqual("PIM", pageTitle, "PIM page not loaded");
        }

        // Test Case 10: Verify Leave Menu Navigation
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC10_LeaveMenuNavigation", DataAccessMethod.Sequential)]
        public void TC10_LeaveMenuNavigation()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Navigate to Leave
            driver.FindElement(By.LinkText("Leave")).Click();
            Thread.Sleep(2000);

            string pageTitle = driver.FindElement(By.ClassName("oxd-topbar-header-breadcrumb-module")).Text;
            Assert.AreEqual("Leave", pageTitle, "Leave page not loaded");
        }

        // Test Case 11: Verify My Info Menu Navigation
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC11_MyInfoMenuNavigation", DataAccessMethod.Sequential)]
        public void TC11_MyInfoMenuNavigation()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Navigate to My Info
            driver.FindElement(By.LinkText("My Info")).Click();
            Thread.Sleep(2000);

            string pageHeader = driver.FindElement(By.ClassName("orangehrm-main-title")).Text;
            Assert.AreEqual("Personal Details", pageHeader, "My Info page not loaded");
        }

        // Test Case 12: Verify Time Menu Navigation
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC12_TimeMenuNavigation", DataAccessMethod.Sequential)]
        public void TC12_TimeMenuNavigation()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Navigate to Time
            driver.FindElement(By.LinkText("Time")).Click();
            Thread.Sleep(2000);

            string pageTitle = driver.FindElement(By.ClassName("oxd-topbar-header-breadcrumb-module")).Text;
            Assert.AreEqual("Time", pageTitle, "Time page not loaded");
        }

        // Test Case 13: Verify Recruitment Menu Navigation
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC13_RecruitmentMenuNavigation", DataAccessMethod.Sequential)]
        public void TC13_RecruitmentMenuNavigation()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Navigate to Recruitment
            driver.FindElement(By.LinkText("Recruitment")).Click();
            Thread.Sleep(2000);

            string pageTitle = driver.FindElement(By.ClassName("oxd-topbar-header-breadcrumb-module")).Text;
            Assert.AreEqual("Recruitment", pageTitle, "Recruitment page not loaded");
        }

        // Test Case 14: Verify Performance Menu Navigation
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC14_PerformanceMenuNavigation", DataAccessMethod.Sequential)]
        public void TC14_PerformanceMenuNavigation()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Navigate to Performance
            driver.FindElement(By.LinkText("Performance")).Click();
            Thread.Sleep(2000);

            string pageTitle = driver.FindElement(By.ClassName("oxd-topbar-header-breadcrumb-module")).Text;
            Assert.AreEqual("Performance", pageTitle, "Performance page not loaded");
        }

        // Test Case 15: Verify Directory Menu Navigation
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC15_DirectoryMenuNavigation", DataAccessMethod.Sequential)]
        public void TC15_DirectoryMenuNavigation()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Navigate to Directory
            driver.FindElement(By.LinkText("Directory")).Click();
            Thread.Sleep(2000);

            string pageTitle = driver.FindElement(By.ClassName("oxd-topbar-header-breadcrumb-module")).Text;
            Assert.AreEqual("Directory", pageTitle, "Directory page not loaded");
        }

        // Test Case 16: Verify Maintenance Menu Navigation
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC16_MaintenanceMenuNavigation", DataAccessMethod.Sequential)]
        public void TC16_MaintenanceMenuNavigation()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Navigate to Maintenance
            driver.FindElement(By.LinkText("Maintenance")).Click();
            Thread.Sleep(2000);

            // Enter password
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(2000);

            string pageTitle = driver.FindElement(By.ClassName("oxd-topbar-header-breadcrumb-module")).Text;
            Assert.AreEqual("Maintenance", pageTitle, "Maintenance page not loaded");
        }

        // Test Case 17: Verify Buzz Menu Navigation
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC17_BuzzMenuNavigation", DataAccessMethod.Sequential)]
        public void TC17_BuzzMenuNavigation()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Navigate to Buzz
            driver.FindElement(By.LinkText("Buzz")).Click();
            Thread.Sleep(2000);

            string pageTitle = driver.FindElement(By.ClassName("oxd-topbar-header-breadcrumb-module")).Text;
            Assert.AreEqual("Buzz", pageTitle, "Buzz page not loaded");
        }

        // Test Case 18: Verify Add Employee Function
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC18_AddEmployee", DataAccessMethod.Sequential)]
        public void TC18_AddEmployee()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Navigate to PIM
            driver.FindElement(By.LinkText("PIM")).Click();
            Thread.Sleep(2000);

            // Click Add Employee
            driver.FindElement(By.LinkText("Add Employee")).Click();
            Thread.Sleep(2000);

            // Enter employee details
            string timestamp = DateTime.Now.ToString("MMddHHmmss");
            driver.FindElement(By.Name("firstName")).SendKeys("John" + timestamp);
            driver.FindElement(By.Name("lastName")).SendKeys("Doe" + timestamp);

            // Save employee
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            string pageHeader = driver.FindElement(By.ClassName("orangehrm-main-title")).Text;
            Assert.AreEqual("Personal Details", pageHeader, "Employee not added successfully");
        }

        // Test Case 19: Verify Search Employee
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC19_SearchEmployee", DataAccessMethod.Sequential)]
        public void TC19_SearchEmployee()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Navigate to PIM
            driver.FindElement(By.LinkText("PIM")).Click();
            Thread.Sleep(2000);

            // Search employee
            driver.FindElement(By.XPath("//input[@placeholder='Type for hints...']")).SendKeys("John");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(2000);

            bool recordsFound = driver.PageSource.Contains("Records Found");
            Assert.IsTrue(recordsFound, "Employee search failed");
        }

        // Test Case 20: Verify User Profile Dropdown
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC20_UserProfileDropdown", DataAccessMethod.Sequential)]
        public void TC20_UserProfileDropdown()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Click user profile
            driver.FindElement(By.ClassName("oxd-userdropdown-tab")).Click();
            Thread.Sleep(1000);

            // Verify dropdown options
            bool aboutOption = driver.PageSource.Contains("About");
            bool supportOption = driver.PageSource.Contains("Support");
            bool logoutOption = driver.PageSource.Contains("Logout");

            Assert.IsTrue(aboutOption && supportOption && logoutOption, "User profile dropdown options not displayed");
        }

        // Test Case 21: Verify Dashboard Menu Items
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC21_DashboardMenuItems", DataAccessMethod.Sequential)]
        public void TC21_DashboardMenuItems()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Verify menu items
            string[] menuItems = { "Admin", "PIM", "Leave", "Time", "Recruitment", "My Info", "Performance", "Dashboard", "Directory", "Maintenance", "Buzz" };

            foreach (string item in menuItems)
            {
                bool itemExists = driver.FindElements(By.LinkText(item)).Count > 0;
                Assert.IsTrue(itemExists, $"Menu item '{item}' not found");
            }
        }

        // Test Case 22: Verify Reset Password Cancel Button
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC22_ResetPasswordCancel", DataAccessMethod.Sequential)]
        public void TC22_ResetPasswordCancel()
        {
            // Go to forgot password page
            driver.FindElement(By.ClassName("orangehrm-login-forgot-header")).Click();
            Thread.Sleep(2000);

            // Click cancel button
            driver.FindElement(By.ClassName("orangehrm-forgot-password-button--cancel")).Click();
            Thread.Sleep(2000);

            bool loginButton = driver.FindElement(By.CssSelector("button[type='submit']")).Displayed;
            Assert.IsTrue(loginButton, "Cancel button did not return to login page");
        }

        // Test Case 23: Verify Logo Display
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC23_LogoDisplay", DataAccessMethod.Sequential)]
        public void TC23_LogoDisplay()
        {
            bool logoDisplayed = driver.FindElement(By.ClassName("orangehrm-login-branding")).Displayed;
            Assert.IsTrue(logoDisplayed, "Logo not displayed on login page");
        }

        // Test Case 24: Verify Copyright Text
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC24_CopyrightText", DataAccessMethod.Sequential)]
        public void TC24_CopyrightText()
        {
            string copyrightText = driver.FindElement(By.ClassName("orangehrm-copyright-wrapper")).Text;
            Assert.IsTrue(copyrightText.Contains("OrangeHRM"), "Copyright text not displayed correctly");
        }

        // Test Case 25: Verify Login Page Title
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC25_LoginPageTitle", DataAccessMethod.Sequential)]
        public void TC25_LoginPageTitle()
        {
            string pageTitle = driver.Title;
            Assert.IsTrue(pageTitle.Contains("OrangeHRM"), "Login page title incorrect");
        }

        // Test Case 26: Verify Leave Apply Option
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC26_LeaveApplyOption", DataAccessMethod.Sequential)]
        public void TC26_LeaveApplyOption()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Navigate to Leave
            driver.FindElement(By.LinkText("Leave")).Click();
            Thread.Sleep(2000);

            // Click Apply
            driver.FindElement(By.LinkText("Apply")).Click();
            Thread.Sleep(2000);

            string pageHeader = driver.FindElement(By.ClassName("orangehrm-main-title")).Text;
            Assert.AreEqual("Apply Leave", pageHeader, "Apply leave page not loaded");
        }

        // Test Case 27: Verify My Leave Option
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC27_MyLeaveOption", DataAccessMethod.Sequential)]
        public void TC27_MyLeaveOption()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Navigate to Leave
            driver.FindElement(By.LinkText("Leave")).Click();
            Thread.Sleep(2000);

            // Click My Leave
            driver.FindElement(By.LinkText("My Leave")).Click();
            Thread.Sleep(2000);

            string pageHeader = driver.FindElement(By.ClassName("orangehrm-main-title")).Text;
            Assert.AreEqual("My Leave List", pageHeader, "My leave page not loaded");
        }

        // Test Case 28: Verify Attendance Option
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC28_AttendanceOption", DataAccessMethod.Sequential)]
        public void TC28_AttendanceOption()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Navigate to Time
            driver.FindElement(By.LinkText("Time")).Click();
            Thread.Sleep(2000);

            // Click Attendance
            driver.FindElement(By.LinkText("Attendance")).Click();
            Thread.Sleep(2000);

            string pageHeader = driver.FindElement(By.ClassName("orangehrm-main-title")).Text;
            Assert.AreEqual("Employee Records", pageHeader, "Attendance page not loaded");
        }

        // Test Case 29: Verify Vacancies Option
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC29_VacanciesOption", DataAccessMethod.Sequential)]
        public void TC29_VacanciesOption()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Navigate to Recruitment
            driver.FindElement(By.LinkText("Recruitment")).Click();
            Thread.Sleep(2000);

            // Click Vacancies
            driver.FindElement(By.LinkText("Vacancies")).Click();
            Thread.Sleep(2000);

            string pageHeader = driver.FindElement(By.ClassName("orangehrm-main-title")).Text;
            Assert.AreEqual("Vacancies", pageHeader, "Vacancies page not loaded");
        }

        // Test Case 30: Verify Candidates Option
        [TestMethod]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "TestData.xml", "TC30_CandidatesOption", DataAccessMethod.Sequential)]
        public void TC30_CandidatesOption()
        {
            // Login
            driver.FindElement(By.Name("username")).SendKeys("Admin");
            driver.FindElement(By.Name("password")).SendKeys("admin123");
            driver.FindElement(By.CssSelector("button[type='submit']")).Click();
            Thread.Sleep(3000);

            // Navigate to Recruitment
            driver.FindElement(By.LinkText("Recruitment")).Click();
            Thread.Sleep(2000);

            // Click Candidates
            driver.FindElement(By.LinkText("Candidates")).Click();
            Thread.Sleep(2000);

            string pageHeader = driver.FindElement(By.ClassName("orangehrm-main-title")).Text;
            Assert.AreEqual("Candidates", pageHeader, "Candidates page not loaded");
        }
    }
}