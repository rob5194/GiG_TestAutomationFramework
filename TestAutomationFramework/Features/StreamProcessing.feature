Feature: StreamProcessing
	Create a Producer to send some messages
	Create a Consumer to receive the messages
	Check that messages produced and consumed are the same

@STREAM PROCESSING TESTS
Scenario: Test Stream processing test 
Given I have produced some messages to a 'producer'
Then I expect a list of messages from a 'consumer'
Then I expect that messages are delivered correctly
