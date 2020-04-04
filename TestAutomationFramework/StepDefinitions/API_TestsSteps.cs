using NUnit.Framework;
using System;
using TechTalk.SpecFlow;
using TestAutomationFramework.Helpers;
using TestAutomationFramework.Models;

namespace TestAutomationFramework.StepDefinitions
{
    [Binding]
    public class API_TestsSteps
    {

        WebClient client = new WebClient();
        ResponseModel responseModel = null;
        RequestModel requestModel = null;
        string requestType, requestUrl, jsonString = "";


        [Given(@"I have a '(.*)' API '(.*)'")]
        public void GivenIHaveAAPI(string requestType, string requestURL)
        {
            this.requestType = requestType;
            this.requestUrl = requestURL;
        }

        [Given(@"I have a JSON input file '(.*)'")]
        public void GivenIHaveAJSONInputFile(string filePath)
        {
            //requestModel = new RequestModel();
            requestModel = client.ReadJsonFile<RequestModel>(filePath);

        }
        [Then(@"I execute the API")]
        public async System.Threading.Tasks.Task ThenIExecuteTheAPI()
        {
            switch (requestType.ToUpper())
            {
                case "POST":
                    //requestUrl += client.RequestParameters(jsonString);
                    responseModel = await client.PostAsyncHelper<ResponseModel>(requestUrl, requestModel) as ResponseModel;
                    break;
                case "GET":
                    responseModel = await client.GetAsyncHelper<Users>(requestUrl) as ResponseModel;
                    break;
            }
        }

         [Then(@"I expect status code '(.*)'")]
        public void ThenIExpectStatusCode(int p0)
        {
            Assert.AreEqual(responseModel.statusCode, p0);
        }
        
        [Then(@"I verify list of users")]
        public void ThenIVerifyListOfUsers()
        {
            Assert.IsNotNull(responseModel.value);
        }


        [Then(@"I should receive a token")]
        public void ThenIShouldReceiveAToken()
        {
            //ScenarioContext.Current.Pending();
        }

    }
}
