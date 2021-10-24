# TrainLine
 
# Description
 This application gets two currencies abbreviation and a price then tries to convert the price with the target currency.
 This is an .NET Core 5 API using xUnit.
# How to setup 
  Please install .Net 5.0 from this link https://dotnet.microsoft.com/download/dotnet/5.0
  Then clone this repository `git clone https://github.com/Elham-Rahimi/TrainLine.git `.
  After that go to the Project by `cd Trainline`
## Run application
   To run the application from the project repository go to the `cd TrainLine/TrainLine.CurrencyConvertert/TrainLine.CurrencyConverter`
   Enter command `dotnet run` now the project should be visible at localhost:5000. 
   You can test the project by `swagger` in `http://localhost:5000/index.html`
## Run test
   To run the test for application from the project repository go to the 
	`cd TrainLine/TrainLine.CurrencyConvertert/TrainLine.CurrencyConverterTeset`
   Enter command `dotnet test`
# Pre-assumptions
- User only enters the currency codes, not any signs or names. for instance, for US Dollars only USD is acceptable.
- User can execute the endpoint via Swagger.
- Negative numbers are ok for users to enter as they can be converted either.

# Improvments
- [] Add better error for regex failure. 
- [] Handle third-party API for exchange rate timeout.
- [] Add test for the middleware.
- [] Add more details for middleware exceptions messages. 
- [] Be able to accept multiple target currencies at once.
- [] Change directory levels of gir repo
