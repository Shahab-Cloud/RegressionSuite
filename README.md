# RegressionSuite
Its is basic practice suite to show the work done on the application.

In this test cases inputs are present in .xlsx file and derived from their and having same name as the test class name (test case name). After test run project uses extent report for reporting the test results and its steps screenshot.


## Folder Structure


**Data** - Contains .xlsx input files with the naming covention same as the test case name.

**ExtentReport** - Contains .html output report with the naming covention same as the test case name with the created date and time.

**MainResources** - Contains methods which are related to database and common to all test cases to give instructions to the test cases. Example:- can run script on dev and preprod by just changing the environment string value  in the properties.cs file.

**MainUtils** - Contains classes which are used in all test cases or we can say that it contains method which are building block for all test cases.

**Test** - This folder is design on the basis of POM (Page Object Model). In this three folders are available as per below:-

1. *Pages* - Conatins of xpath or css of elements as per the page name.
1. *Steps* - Contains method performing on the pages during test run.
1. *Tests* - Contains Regression Test in it.

**TestResources** - Others things required by the projects are URL, Database connection strings, DB Quiries etc are available in this folder.

### Language

C#

### Tools

Selenium, Nunit, Extent Report, AzureDevops, RestSharp

### Database

SQL, AzureDevops (Blob-Containers, Storage Browser)

