Assumptions

Acquiring bank exposes an REST API which 
    - uses JSON format
    - returns 200 and payment identifier for successful requests, 
    - returns 4xx, payment identifier and error code for requests which failed validation
    - returns 5xx, payment identifier and error code when payment is not possible because of technical issues (eg one of dependencies is down)
    - returns 5xx and non-JSON response when it experiences serious issues (eg whole system is down and proxy returned 503/504) 

If the acquiring bank endpoint used a different technology (eg a SOAP web service), Payment Gateway should be able to support that.
(See a section about sync/async implementation)

Payment Gateway design considerations
    
    1. Architecture and tech stack
    
    I'd like to implement Payment Gateway as REST API and to use JSON format. I'd also like to implement it 
    using .NET Core and to host it on Azure. I assume it is acceptable.
    
    2. Endpoints

    According to requirements, Payment Gateway 
            -   should provide merchants a way to process a payment 
            -   should also allow a merchant to retrieve details of previously made payment using its identifier. 
      
      A standard way to implement these requirements in REST API is to treat each payment as a separate resource with a unique URL. 
      The URL should include resource type (payment) and its identifier. Example:  {paymentGatewayBaseUrl}/payments/{paymentIdentifier}
      When API receives GET request to resource's URL, it will return resource representation in JSON format. 
      Clients can create new payment resources by sending POST requests to {paymentGatewayBaseUrl}/payments.
      Payment details are passed in POST request body.

    3. Data store

    In order to meet requirements to allow a merchant to retrieve payment details, Payment Gateway needs to store payment details 
    in a persistent data store. Data store should support saving payment details and retrieving payment details by identifier.
    It can be done by using a relational database or a NoSQL store. NoSQL store (in particular a document database) could be 
    a good choice for these requirements, especially in case there is no other requirements for data joining and querying. 
    Relational database is also a good choice. I have decided to use a relational database (SQL Server) because I have more 
    experience with SQL Server than with NoSQL databases.

    3. Status codes
    
    According to the requirements, the response should include the status code which indicates the result of the payment.

    In case when payment processing failed, PaymentGateway should return a status co


    REST APIs return HTTP status code to indicate if an operation was successful. For example, Payment Gateway will use 200 to show that an operation
    was successful, 422 to indicate that validation has failed and 500 for an internal server error.
    

    Unfortunately, it is possible that not all payment failure scenario has a corresponding HTTP status code.
    
    s do not cover all possible scenarios. Payment Gateway will 
    
      According to the requirements, the response should include a masked card number and card details along with the status code which 
      indicates the result of the payment.

    
todo 

sync vs async (does payment endpoint wait for bank response)


Scenarios for the payment processing endpoint

1. Happy path

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

2. Validation failure in Payment Gateway

    Merchant 
        submits a request to the payment gateway
    Payment gateway 
        saves the request in its data store
        validates the request, validation fails 
        returns 4xx to the merchant

    I have assumed that invalid payment requests should be stored in the data store.

3. Validation failure in Bank

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

4. Server error in Bank

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

5. Server error in Bank, non-JSON response

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
    However

5. Bank timeout

6. Database exception