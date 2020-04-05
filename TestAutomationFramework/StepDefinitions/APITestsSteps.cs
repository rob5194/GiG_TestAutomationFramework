using NUnit.Framework;
using System;
using TechTalk.SpecFlow;
using TestAutomationFramework.Helpers;
using TestAutomationFramework.Models;

namespace TestAutomationFramework.StepDefinitions
{
    [Binding]
    public class APITestsSteps
    {

        WebClient client = new WebClient();
        CommonResponseModel responseModel = null;
        RequestModel requestModel = null;
        string requestType, requestUrl = "";


        [Given(@"I have a '(.*)' API '(.*)'")]
        public void GivenIHaveAAPI(string requestType, string requestURL)
        {
            this.requestType = requestType;
            this.requestUrl = requestURL;
        }

        [Given(@"I have a JSON input file '(.*)'")]
        public void GivenIHaveAJSONInputFile(string filePath)
        {
            requestModel = client.ReadJsonFile<RequestModel>(filePath);

        }
        [Then(@"I execute the API")]
        public async System.Threading.Tasks.Task ThenIExecuteTheAPI()
        {
            switch (requestType.ToUpper())
            {
                case "POST":
                    responseModel = await client.PostAsyncHelper<CommonResponseModel>(requestUrl, requestModel) as CommonResponseModel;
                    break;
                case "GET":
                    responseModel = await client.GetAsyncHelper<Users>(requestUrl) as CommonResponseModel;
                    break;
            }
        }

         [Then(@"I expect status code '(.*)'")]
        public void ThenIExpectStatusCode(int p0)
        {
            Assert.AreEqual(responseModel.StatusCode, p0);
        }
        
        [Then(@"I verify list of users")]
        public void ThenIVerifyListOfUsers()
        {
            Assert.IsNotNull(responseModel.Value);
        }


        [Then(@"I should receive a token")]
        public void ThenIShouldReceiveAToken()
        {
            Assert.IsNotNull(responseModel.Token);
        }

        [Then(@"I should receive an error")]
        public void ThenIShouldReceiveAnError()
        {
            Assert.IsNotNull(responseModel.Error);
        }


    }
}
