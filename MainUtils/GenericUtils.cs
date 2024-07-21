using JM.MainUtils;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OpenQA.Selenium;
using System.Dynamic;
using System.Globalization;
using System.Reflection;
using System.Text;


namespace JMAutomation.MainUtils
{
    public class GenericUtils : WebdriverSession
    {
        public static string getRandomString(int length)
        {

            // creating a StringBuilder object()
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            System.Console.WriteLine(str_build.ToString());
            return str_build.ToString();
        }

        public static IEnumerable<object[]> ReadExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Creat worksheet object

            using (ExcelPackage package = new ExcelPackage(new FileInfo("C:\\Users\\mohds\\source\\repos\\JMRegressionSuite\\JMAutomation\\Data\\Data.xlsx")))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];
                int rowCount = worksheet.Dimension.End.Row;
                for (int i = 2; i <= rowCount; i++)
                {

                    yield return new object[]
                    {
                        worksheet.Cells[i,1].Value?.ToString().Trim(), //Id Number
                        worksheet.Cells[i,2].Value?.ToString().Trim(), //First Name
                        worksheet.Cells[i,3].Value?.ToString().Trim(), //Surname
                        worksheet.Cells[i,4].Value?.ToString().Trim(), //CellPhonenumber
                        worksheet.Cells[i,5].Value?.ToString().Trim()  // websource
                    };
                }

            }
        }

        public static IEnumerable<object[]> ReadExcelData(string classname)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Create worksheet object

            using (ExcelPackage package = new ExcelPackage(new FileInfo("C:\\Users\\mohds\\source\\repos\\JMRegressionSuite\\JMAutomation\\Data\\" + classname + ".xlsx")))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];
                int rowCount = worksheet.Dimension.End.Row;
                for (int i = 2; i <= rowCount; i++)
                {

                    yield return new object[]
                    {
                        worksheet.Cells[i,1].Value?.ToString().Trim(), //Id Number
                        worksheet.Cells[i,2].Value?.ToString().Trim(), //Password
                        worksheet.Cells[i,3].Value?.ToString().Trim(), //Salary                        
                        worksheet.Cells[i,4].Value?.ToString().Trim()  //RepaymentValue

                    };
                }

            }
        }
        public static IEnumerable<object[]> ReadExcelDataForDTCC634(string classname)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Create worksheet object

            using (ExcelPackage package = new ExcelPackage(new FileInfo("C:\\Users\\mohds\\source\\repos\\JMRegressionSuite\\JMAutomation\\Data\\" + classname + ".xlsx")))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];
                int rowCount = worksheet.Dimension.End.Row;
                for (int i = 2; i <= rowCount; i++)
                {

                    yield return new object[]
                    {
                        worksheet.Cells[i,1].Value?.ToString().Trim(), //Id Number
                        worksheet.Cells[i,2].Value?.ToString().Trim(), //Password
                        worksheet.Cells[i,3].Value?.ToString().Trim()  //Credit score
                    };
                }

            }
        }

        public static string splitString(string inputString, string splitBy, int index)
        {

            string iString = inputString;
            string delimiter = splitBy;

            string[] substrings = inputString.Split(delimiter);

            return substrings[index];

        }


        public static string getCurrentDate(string ddMMYYYY_HHmmss)
        {

            return DateTime.Now.ToString(ddMMYYYY_HHmmss).Trim();


        }

        public static IEnumerable<object[]> ReadExcelDataAsPerColumnsInput(string classname, int no_of_column_inputs)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Create worksheet object
            string path = getDataPath("Data");
            using (ExcelPackage package = new ExcelPackage(new FileInfo(path + "\\" + classname + ".xlsx")))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];
                int rowCount = worksheet.Dimension.End.Row;
                for (int i = 2;
                    i <= rowCount; i++)
                {
                    if (no_of_column_inputs == 7)
                    {
                        yield return new object[]
                        {
                        worksheet.Cells[i,1].Value?.ToString().Trim(), //Id Number
                        worksheet.Cells[i,2].Value?.ToString().Trim(), //firstname
                        worksheet.Cells[i,3].Value?.ToString().Trim(), //lastname                        
                        worksheet.Cells[i,4].Value?.ToString().Trim(), //cell number
                        worksheet.Cells[i,5].Value?.ToString().Trim(), //activated by
                        worksheet.Cells[i,6].Value?.ToString().Trim(), //salary
                        worksheet.Cells[i,7].Value?.ToString().Trim()  //second set of security questions
                        };
                    }

                    else if (no_of_column_inputs == 6)
                    {
                        yield return new object[]
                        {
                        worksheet.Cells[i,1].Value?.ToString().Trim(), //Id Number
                        worksheet.Cells[i,2].Value?.ToString().Trim(), //firstname
                        worksheet.Cells[i,3].Value?.ToString().Trim(), //lastname                        
                        worksheet.Cells[i,4].Value?.ToString().Trim(), //cell number
                        worksheet.Cells[i,5].Value?.ToString().Trim(), //activated by
                        worksheet.Cells[i,6].Value?.ToString().Trim()  //salary
                        };
                    }
                    else if (no_of_column_inputs == 5)
                    {
                        yield return new object[]
                        {
                        worksheet.Cells[i,1].Value?.ToString().Trim(), //Id Number
                        worksheet.Cells[i,2].Value?.ToString().Trim(), //Password
                        worksheet.Cells[i,3].Value?.ToString().Trim(), //Salary                        
                        worksheet.Cells[i,4].Value?.ToString().Trim(),  //RepaymentValue
                        worksheet.Cells[i,5].Value?.ToString().Trim()
                        };
                    }

                    else if (no_of_column_inputs == 4)
                    {
                        yield return new object[]
                        {
                        worksheet.Cells[i,1].Value?.ToString().Trim(), //Id Number
                        worksheet.Cells[i,2].Value?.ToString().Trim(), //Password
                        worksheet.Cells[i,3].Value?.ToString().Trim(), //Salary                        
                        worksheet.Cells[i,4].Value?.ToString().Trim()  //RepaymentValue

                        };
                    }
                    else if (no_of_column_inputs == 3)
                    {
                        yield return new object[]
                        {
                        worksheet.Cells[i,1].Value?.ToString().Trim(), //Id Number
                        worksheet.Cells[i,2].Value?.ToString().Trim(), //Password
                        worksheet.Cells[i,3].Value?.ToString().Trim()  //any field

                        };
                    }
                    else if (no_of_column_inputs == 2)
                    {
                        yield return new object[]
                        {
                        worksheet.Cells[i,1].Value?.ToString().Trim(), //Id Number
                        worksheet.Cells[i,2].Value?.ToString().Trim()  //Password
                        };
                    }
                    else if (no_of_column_inputs == 1)
                    {
                        yield return new object[]
                        {
                        worksheet.Cells[i,1].Value?.ToString().Trim() //Id Number
                       
                        };
                    }
                }

            }
        }

        public static string getDataPath(string folderName)
        {
            // Get the path of the current executing assembly
            string assemblyPath = Assembly.GetExecutingAssembly().Location;

            // Get the directory of the assembly
            string assemblyDirectory = Path.GetDirectoryName(assemblyPath);

            // Get the parent directory of the assembly directory (i.e. the project folder)
            string projectDirectory = Directory.GetParent(assemblyDirectory).Parent.Parent.FullName;

            // Combine the project directory path with the "data" folder name to get the "data" folder path
            string dataFolderPath = Path.Combine(projectDirectory, folderName);
            return dataFolderPath;
        }

        public static void mouseHoverOverElement(IWebElement element, double pointWidth, double pointHeight)
        {

            int x = element.Location.X;
            int y = element.Location.Y;
            int width = element.Size.Width;
            int height = element.Size.Height;

            // Calculate the coordinates of the point you want to click
            int pointX = x + (int)(width * pointWidth); // 50% from left
            int pointY = y + (int)(height * pointHeight); // 50% from top

            // Use the JavaScriptExecutor to simulate a mouse click at the desired coordinates
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)WebdriverSession.Driver;
            jsExecutor.ExecuteScript($"var evt = new MouseEvent('click', {{ bubbles: true, cancelable: true, view: window, clientX: {pointX}, clientY: {pointY} }}); arguments[0].dispatchEvent(evt);", element);

            // Wait for the click to be processed
            BaseStep.wait.genericWait(1000);

        }

        public static int randomInteger(int uptomaxValuedigits)
        {

            Random random = new Random();
            int randomNumber = random.Next(1, uptomaxValuedigits + 1);
            return randomNumber;
        }

        public static int randomInteger(int minvalue, int uptomaxValuedigits)
        {

            Random random = new Random();
            int randomNumber = random.Next(minvalue, uptomaxValuedigits + 1);
            return randomNumber;
        }

        public static IEnumerable<object[]> GetUser(string application, string classname)
        {
            string path = getDataPath("InputData");
            string json = File.ReadAllText(path + "\\" + classname + ".json");

            var jsonObject = JObject.Parse(json);
            var appUsers = jsonObject[application];



            foreach (var user in appUsers)
            {
                var userId = (string)user["id"];
                var firstName = (string)user["firstname"];
                var surName = (string)user["surname"];
                var number = (string)user["number"];
                var creditcoachscore_personalloan = (string)user["creditcoachscore_personalloan"];
                var les_decision = (string)user["les_decision"];
                var creditcoachscore_creditcard = (string)user["creditcoachscore_creditcard"];
                var creditcoachscoreperc = (string)user["creditcoachscoreperc"];

                yield return new object[] { userId, firstName, surName, number, creditcoachscore_personalloan, les_decision, creditcoachscore_creditcard, creditcoachscoreperc };
            }
        }


        public static IEnumerable<dynamic> GetUsers(string application, string classname)
        {
            string path = getDataPath("InputData");
            string json = File.ReadAllText(path + "\\" + classname + ".json");

            
                var jsonObject = JObject.Parse(json);
                var splUsers = jsonObject[application];

                foreach (var user in splUsers)
                {
                    var userObj = new ExpandoObject() as IDictionary<string, object>;
                    foreach (JProperty property in user)
                    {
                        userObj[property.Name] = property.Value;
                    }
                    yield return userObj;
                }
            }

            public static JObject GetJson(string jsonFilePath)
        {

            string jsonString = File.ReadAllText(jsonFilePath);
            JObject json = JObject.Parse(jsonString);
            return json;

        }

        public static string Encrypt(string plainText, int shift)
        {
            string encryptedText = "";

            foreach (char c in plainText)
            {
                if (char.IsLetter(c))
                {
                    char encryptedChar = (char)(((int)char.ToLower(c) - 97 + shift) % 26 + 97);
                    encryptedText += char.IsUpper(c) ? char.ToUpper(encryptedChar) : encryptedChar;
                }
                else
                {
                    encryptedText += c;
                }
            }

            return encryptedText;
        }

        public static string Decrypt(string encryptedText, int shift)
        {
            return Encrypt(encryptedText, 26 - shift); // Decrypting is equivalent to shifting in the opposite direction
        }

        public static void scrollTillHalfPage()

        {
            // Scroll halfway using JavaScript
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
            long scrollHeight = (long)jsExecutor.ExecuteScript("return document.documentElement.scrollHeight");
            long halfScrollHeight = scrollHeight / 2;
            jsExecutor.ExecuteScript($"window.scrollTo(0, {halfScrollHeight});");
        }

        public static void scrollAtTopOfThePage()

        {
            // Scroll at top
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("window.scrollTo(0, 0);");
        }

        public static void scrollTillFullPage()

        {
            // Scroll halfway using JavaScript
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)Driver;
            long scrollHeight = (long)jsExecutor.ExecuteScript("return document.documentElement.scrollHeight");
            long halfScrollHeight = scrollHeight;
            jsExecutor.ExecuteScript($"window.scrollTo(0, {halfScrollHeight});");
        }

        public static IEnumerable<object[]> ReadExcelDataAsPerRowInput(string classname, int inputAsPerRow, int noOfInputsReq)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Create worksheet object
            string path = getDataPath("Data");
            using (ExcelPackage package = new ExcelPackage(new FileInfo(path + "\\" + classname + ".xlsx")))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets["Sheet1"];
                if (noOfInputsReq == 7)
                {
                    yield return new object[]
                    {
                        worksheet.Cells[inputAsPerRow,1].Value?.ToString().Trim(), //Id Number
                        worksheet.Cells[inputAsPerRow,2].Value?.ToString().Trim(), //firstname
                        worksheet.Cells[inputAsPerRow,3].Value?.ToString().Trim(), //lastname                        
                        worksheet.Cells[inputAsPerRow,4].Value?.ToString().Trim(), //cell number
                        worksheet.Cells[inputAsPerRow,5].Value?.ToString().Trim(), //isAutoregreq                        
                        worksheet.Cells[inputAsPerRow,6].Value?.ToString().Trim(), //websource
                        worksheet.Cells[inputAsPerRow,7].Value?.ToString().Trim() //secondPhoneNumber
                    };
                }

               else if (noOfInputsReq == 4)
                {
                    yield return new object[]
                    {
                        worksheet.Cells[inputAsPerRow,1].Value?.ToString().Trim(), //Id Number
                        worksheet.Cells[inputAsPerRow,2].Value?.ToString().Trim(), //firstname
                        worksheet.Cells[inputAsPerRow,3].Value?.ToString().Trim(), //lastname                        
                        worksheet.Cells[inputAsPerRow,4].Value?.ToString().Trim()  //phonenumber
                    };
                }
            }
        }

        public static dynamic GetUserAtIndex(string application, string classname, int index)
        {
            string path = getDataPath("InputData");
            string json = File.ReadAllText(path + "\\" + classname + ".json");

            var jsonObject = JObject.Parse(json);
            var splUsers = jsonObject[application];

            if (index >= 0 && index < splUsers.Count())
            {
                var user = splUsers[index];
                var userObj = new ExpandoObject() as IDictionary<string, object>;

                foreach (JProperty property in user)
                {
                    userObj[property.Name] = property.Value;
                }

                return userObj;
            }
            else
            {
                // If the index is out of range, return null or handle the error as per your requirements.
                return null;
            }
        }

        public static DateTime formatTheSystemDateIntoUTC(double v)
        {
            

            // Convert to UTC
            DateTime utcDateTime = currentDateTime.ToUniversalTime();

            // Add 24 hours to the UTC time
            DateTime utcDateTimePlus24Hours = utcDateTime.AddHours(24);

            // Format the UTC time in the desired format
            string formattedUtcDateTime = utcDateTimePlus24Hours.ToString("yyyy-MM-dd HH:mm:ss.fffffff");

            DateTime parsedResponseTime = DateTime.ParseExact(formattedUtcDateTime, "yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
            return parsedResponseTime;
        }
    }
}
