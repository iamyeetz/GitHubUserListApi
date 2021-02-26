


1. Used Moq and Xunit
2. Has 3 testing for UserController
- TestSuccessfullProcessByPassingValidUsernames = Test for the result if given usernames are valid
- ReturnOutOfRangeArgumentExceptionIfUsernameIsMoreThanTen = Test if Api will return OutofRangeArgument Exception if username supplied is morethat 10
- TestRequestWithInvalidUserName = Test for usernames that doesnt exist