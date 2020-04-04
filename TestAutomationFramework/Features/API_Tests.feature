Feature: API_Tests
	A test to ensure that POST and GET APIs work
	1. Successful registration with code 200 and token
	2. Unsuccessful registration with code 400 and error
	3. Get a list of Users with code 200


@API_TESTS
Scenario: Get list of Users 
Given I have a 'GET' API 'https://reqres.in/api/users'
Then I execute the API
Then I expect status code '200'
Then I verify list of users

Scenario: : Successful registration
Given I have a 'POST' API 'https://reqres.in/api/register' 
Given I have a JSON input file '..\..\..\TestData\SuccessfulRegistration.json'
Then I execute the API
Then I should receive a token 
Then I expect status code '200'
