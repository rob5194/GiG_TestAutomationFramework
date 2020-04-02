using NUnit.Framework;
using System;
using TechTalk.SpecFlow;
using TestAutomationFramework.Enums;
using TestAutomationFramework.Helpers;
using TestAutomationFramework.Models;

namespace TestAutomationFramework.StepDefinitions
{
    [Binding]
    public class API_TestsSteps
    {

        WebClient client = new WebClient();
        ResponseModel u = null;

        [Given(@"I have invoked a '(.*)' API '(.*)'")]
        public async System.Threading.Tasks.Task GivenIHaveInvokedAAPIAsync(string requestType, string requestURL)
        {
            RequestTypes rt = (RequestTypes)Enum.Parse(typeof(RequestTypes), requestType);
             u = await client.InvokeAsyncHelper<ResponseModel,Users>(requestURL, null, rt) as ResponseModel;
        }

         [Then(@"I expect status code '(.*)'")]
        public void ThenIExpectStatusCode(int p0)
        {
            Assert.AreEqual(u.StatusCode, p0);
        }
        
        [Then(@"I verify list of users")]
        public void ThenIVerifyListOfUsers()
        {
            Assert.IsNotNull(u.Value);
        }
    }
}
