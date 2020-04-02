Feature: API_Tests
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
#Scenario: Add two numbers
#Given I have provided 70 and 20 as the inputs
#When I press add
#Then the result should be 90

Scenario: Get list of Users 
Given I have invoked a 'GET' API 'https://reqres.in/api/users'
Then I expect status code '200'
Then I verify list of users
