Feature: StreamProcessing
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Test Stream processing test 
Given I have produced some messages to a 'producer'
Then I expect a list of messages from a 'consumer'
Then I expect that messages are delivered correctly
