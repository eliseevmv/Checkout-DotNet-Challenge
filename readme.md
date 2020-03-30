# 1. Payment Gateway design considerations
    
## 1.1. Architecture and tech stack
    
I've decided to implement Payment Gateway as REST API and to use JSON format. I've also decided to implement it 
using .NET Core and to host it on Azure. I assume it is acceptable.
    
## 1.2. Endpoints

According to requirements, Payment Gateway
    
-   should provide merchants a way to process a payment 
-   should also allow a merchant to retrieve details of previously made payment using its identifier. 
      
A standard way to implement these requirements in REST API is to treat each payment as a separate resource with a unique URL. 
The URL should include resource type (payment) and its identifier. Example:  

    {paymentGatewayBaseUrl}/payments/{paymentIdentifier}
    
When API receives GET request to resource's URL, it will return the resource representation in JSON format. 
Clients can create new payment resources by sending POST requests to {paymentGatewayBaseUrl}/payments.
Payment details are passed in POST request body.

## 1.3. Data store

In order to meet requirements to allow a merchant to retrieve payment details, Payment Gateway needs to store payment details 
in a persistent data store. Data store should support saving payment details and retrieving payment details by identifier.

It can be done by using a relational database or a NoSQL store. NoSQL store (in particular a document database) could be 
a good choice for these requirements, especially in case there is no other requirements for data joining and querying. 
Relational database is also a good choice. 

I have decided to use a relational database (SQL Server) because I have more experience with SQL Server than with NoSQL databases.
I have also decided to use Dapper to access SQL server database because of simplicity and high performance.

## 1.4. Status codes
    
According to the requirements, a merchant should be able to process a payment and receive either a successful or 
unsuccessful response. Details of a previously made payment should include a status code which indicates the result of the payment.
    
REST APIs return HTTP status code to indicate if an operation was successful. For example, Payment Gateway will use 200 
to show that an operation was successful, 422 to indicate that validation has failed and 500 for an internal server error.
    
Unfortunately, it is possible that not every payment failure scenario has a corresponding HTTP status code.
In case a payment has failed, Payment Gateway will assign its own status code and return it to the merchant in the response body.
It will also store the status code in the data store together with payment details. 

in case the payment failure has happened in the Acquiring Bank and it returns its own error code, it is a good idea to add this
information to the payment details. However, the Payment Gateway will not store bank's error code directly. Instead, its own 
list of status codes should have values which correspond to bank error codes. Payment Gateway will map bank's error code to these 
status codes. This is done to protect Payment Gateway and its clients from unexpected code changes in Acquiring Bank.

## 1.6. Encryption

### Data at rest 

According to the requirements, the response should include a masked card number and card details. 
Current version of Payment Gateway stores the masked card number and does not store the complete card number.

This is not production-ready. PCI-DSS should be taken into consideration.
In particular, card details should be encrypted according to the appropriate security standards.
As an alternatiive it could be possible to use a secure 3rd party service to store the credit card details.
    
It is important to prevent writing credit card details to log files, including logs of requests and responses.

### Data in transit

Current versions of Payment Gateway enforces HTTPS (by using app.UseHttpsRedirection command) in order ensure that card 
details are not transmitted between Merchant and Payment Gateway unencrypted. It also uses HTTPS when calling 
Acquiring Bank.

## 1.7. Payment identifier

Payment identifier generated by the Acquiring Bank is not always available. It is possible that the Acquring Bank system 
throws an exception and returns an error message, which does not contain payment identifier. 
Payment Gateway should generate its own payment identifier, return it to the merchant and use this identifier in GET payment 
endpoint URL. Since payment identifier is exposed to external systems, it is practical to use GUID data type. 
    
Identifier returned by the bank can be stored as an optional property of payment resource. It can be useful for incident 
investigation purposes. 
  
## 1.8. Entities and services

Business logic is contained in the core of the system, which consist of services and entities. 
API, data access code and Acquiring Bank client should not contain business logic. They are interface adapters between 
the core and the external systems. They convert data from the format convenient to external systems to the entities.

# 2. Scenarios for the payment processing endpoint

## 2.1. Happy path

    Merchant 
        submits a request to the payment gateway
    Payment gateway 
        validates the request 
        saves the request in its data store
        forwards payment request to the acquiring bank
    Acquiring bank 
        validates the request
        processes the payment 
        returns 200
    Payment gateway 
        saves the response in its data store
        returns 200 to the merchant

This scenario is implemented as a component test and an integration test.

## 2.2. Validation failure in Payment Gateway

    Merchant 
        submits a request to the payment gateway
    Payment gateway 
        validates the request, validation fails 
        returns 4xx to the merchant

I have made an assumption that invalid requests should not be stored in data store.
This scenario is implemented as an integration test.

## 2.3. Validation failure in Bank

    Merchant 
        submits a request to the payment gateway
    Payment gateway 
        validates the request 
        saves the request in its data store
        forwards payment request to the acquiring bank
    Acquiring bank 
        validates the request, validation fails
        does not process the payment 
        returns 4xx
    Payment gateway 
        saves the response in its data store
        returns 4xx to the merchant

This scenario is implemented as a component test and an integration test.

## 2.4. Server error in Bank

    Merchant 
        submits a request to the payment gateway
    Payment gateway 
        validates the request 
        saves the request in its data store
        forwards payment request to the acquiring bank
    Acquiring bank 
        validates the request
        tries to process the payment but fails
        returns 5xx
    Payment gateway 
        saves the response in its data store
        returns 5xx to the merchant

This scenario is implemented as a component test and an integration test.

## 2.5. Server error in Bank, non-JSON response

    Merchant 
        submits a request to the payment gateway
    Payment gateway 
        validates the request 
        saves the request in its data store
        forwards payment request to the acquiring bank
    Acquiring bank 
        returns 5xx with non-JSON response
    Payment gateway 
        saves error details (empty payment identifier and error code) in its data store
        returns 5xx to the merchant

This scenario shows that payment identifier generated by the bank is not always available.
This scenario is not implemented as part of this exercise, but a production system should be ready for this scenario.

## 2.6. Bank timeout

Same as scenario above but bank does not respond
This scenario is not implemented as part of this exercise, but a production system should be ready for this scenario.

## 2.7. Database exception before calling bank

    Merchant 
        submits a request to the payment gateway
    Payment gateway 
        tries to saves the request in its data store
        data store returns exception
        returns 500 to the merchant

This scenario is implemented as a component test.

## 2.8. Database exception after calling bank

    Merchant 
        submits a request to the payment gateway
    Payment gateway 
        validates the request 
        saves the request in its data store
        forwards payment request to the acquiring bank
    Acquiring bank 
        validates the request
        processes the payment 
        returns 200
    Payment gateway 
        tries to saves the response in its data store
        data store returns an exception
        returns 200 to the merchant

This scenario is implemented as a component test

It is unclear what status code should Payment Gateway return to the merchant if the Bank processed the payment but 
the data store returned an error. It depends on whether it is safe to re-send thepayment request to the Bank endpoint.

I have made an assumption that rhe Bank endpoint is not idempotent and it is not safe to re-send payment request to the Bank.
In this case Payment Gateway should catch the exception from DB and return 200 to the merchant to ensure the merchant 
does not retry the same payment.

As a result of that, the merchant will have a correct response code. However the Payment Gateway data store will have 
an incorrect status because the DB update failed.

Payment Gateway should ideally notify the support team (eg by raising an alert) to ensure support team fixes the issue. 
It is also possible to make DB update asyncronous by using message queue. That will ensure DB will be correctly automatically updated at some point.
        
# 3. Scenarios for the retrieving payment details endpoint

## 3.1. Happy path

    Merchant 
        submits a request to the payment gateway
    Payment gateway 
        retrieves the payment details from its data store
        returns 200 and the payment details to the merchant

This scenario is implemented as an integration test.

## 3.2. Incorrect payment id

    Merchant 
        submits a request with incorrect payment id to the payment gateway
    Payment gateway 
        tries to retrieves the payment details from its data store but does not find it
        returns 404 to the merchant

This scenario is implemented as a component test and an integration test.

## 3.3. Database exception

    Merchant 
        submits a request to the payment gateway
    Payment gateway 
        tries to retrieves the payment details from its data store 
        data store returns an exception
        returns 500 to the merchant
        
If the data store returns an exception, Payment Gateway should retry several times. 
It can help resolve errors related to intermittent connectivity issues and make this scenario less likely.

This scenario is not implemented as part of this exercise, but a production system should be ready for this scenario.

# 4. Testing

Ideally Payment Gateway should have a set of tests which follows the idea of the test pyramid.
    
Most classes should have a separate set of unit tests which test each class in isolation of any dependencies.
Dependencies must be replaced by test doubles, eg mocks. Unit tests should do detailed low-level testing of classes.

Payment Gateway should also have a set of component tests, which run PaymentGateway inside an in-memory test web server and 
replace external dependencies (eg database and Bank) by mocks. These tests allow to tests Payment Gateway as a whole.
They allow to simulate correct and incorrect responses from external systems and test how Payment Gateway processes these
responses. There should be fewer component tests that unit tests.
    
Payment Gateway should have a set of integration tests, which run against Payment Gateway deployed on a test environment,
with real dependencies, including real database and Bank Simulator. Ideally number of these tests should be lower than
number of component tests, because these tests are slower and could be more brittle.

# 5. Bank Simulator

Assumptions : acquiring bank exposes an REST API which 
- uses JSON format
- returns 200 and payment identifier for successful requests, 
- returns 4xx, payment identifier and error code for requests which failed validation
- returns 5xx, payment identifier and error code when payment is not possible because of technical issues 
    (eg one of dependencies is down)
- returns 5xx and non-JSON response when it experiences serious issues (eg whole system is down and proxy returned 503/504) 

If the acquiring bank endpoint used a different technology (eg a SOAP web service), Payment Gateway would be able 
to support that but code changes would be required.

# 6. Hosting

I have deployed Payment Gateway, its database and Bank Simulator to Azure.
Payment Gateway uses Application Insights service for logging and metrics.
The Postman collection (which runs against Azure) is in Documentation folder: [Link](Documentation/Checkout%20test.postman_collection.json)

# 7. Configuration

appsettings.json contains tokens (eg "#{DbConnectionString}#") which are replaced by the environment-specific configuration values 
by the CD release pipeline in Azure DevOps (see below). Example: [Link](https://dev.azure.com/maximeliseev/Checkout-PaymentGateway/_releaseProgress?releaseId=12&environmentId=12&itemType=VariableGroups&_a=release-environment-logs)

Release pipeline gets environment-specific configuration values from an environment-specific variable group in Azure DevOps. Example: [Link](https://dev.azure.com/maximeliseev/Checkout-PaymentGateway/_library?itemType=VariableGroups&view=VariableGroupView&variableGroupId=1&path=Dev)

I have configured the DB connection string as a "secret" to ensure users cannot read or copy the value.
    
# 8. Extra mile bonus points

## 8.1. Application logging 

Logging is configured and logs are available in Application Insights. 
The application still needs an ability to log requests and responses (from/to merchant and to/from bank), however
it is only safe to do when card details are hidden or masked.

## 8.2. Application metrics

Application metric are available in Application Insights. 
Examples: 
[End to end transaction details](Documentation/Application%20Insights%20-%20end%202%20end%20transaction%20details.PNG)
[Live metrics](Documentation/Application%20Insights%20-%20Live%20Metrics.PNG)
[Load test - 1000 requests](Documentation/Application%20Insights%20-%201000%20requests.PNG)
    
## 8.3. Containerization

Not done

## 8.4. Authentication

Not done

## 8.5. API client

See [PaymentGateway.Client project](PaymentGateway.Client/).
Further improvements - I would update the build pipeline (see below) to publish the client as a nuget package.

## 8.6. Build script / CI
    
The [build pipeline](https://dev.azure.com/maximeliseev/Checkout-PaymentGateway/_build?definitionId=2&_a=summary) 
- builds the solution, 
- runs unit tests and component tests 
- publishes the artifact.

[Build script](azure-pipelines.yml)
    
The [release pipeline](https://dev.azure.com/maximeliseev/Checkout-PaymentGateway/_release?_a=releases&view=mine&definitionId=1)
- replaces tokens in appsettings.json by values from the [environment-specific variable group](https://dev.azure.com/maximeliseev/Checkout-PaymentGateway/_library?itemType=VariableGroups&view=VariableGroupView&variableGroupId=1&path=Dev)
- releases Payment Gateway to Azure App Services
- runs the integration tests against the deployed application.

Please note that the current implementation of CI/CD represents build and deployment to a dev/test environment. 
    
Further improvements - change the build pipeline so it publishes an API artifact and an integration test artifact separately.
At the moment it publishes one artifact which includes both API and tests which is not the best approach.
    
## 8.7. Performance testing

I did a very basic performance test - ran a process payment request from Postman against Payment Gateway deployed to Azure, 
in a loop (1000 times). According to Application Insights, average execution time was 31ms, 99th percentile was 65ms.
See [Application Insights screenshot](Documentation/Application%20Insights%20-%201000%20requests.PNG)

Note that Payment Gateway, Bank Simulator and the database were deployed on cheap "free tier" resources (F1 virtual machines, S0 10 DTU DB)
    
## 8.8. Encryption

Not done

## 8.9. Data storage

Payment Gateway uses a SQL database deployed on Azure. The database has one table. 
[SQL script to create the table](Data/dbo.Payments.sql)

## 9. Future improvements
    
- Improve the way how code reads configuration - including implementing options pattern
- Used Polly policies for outbound calls eg to retry in case transient errors have happened
- Production code should use a circuit breaker when calling Acquiring Bank 
- Logging could be improved
- API default page, which should include API version
- Swagger documentation
- Create a database project for automatic deployment of database changes
- A policy on GitHub repo which only allows to merge to master from a pull request, and uses squash commits by default.
    Pull request should have at least 2 code reviews
- GDPR
- I would like to separate core (entities and services) from infrastructure (data access and service clients). 
    Core project should not have reference to infrastructure projects. This is similar to Hexagonal Arctitecture approach.
   
   

