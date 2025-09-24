### ClearBank Technical test

# Key changes
1. Dependency Injection

Problem: 
The original PaymentService directly instantiated AccountDataStore and BackupAccountDataStore based on configuration, tightly coupling the service to specific implementations.

Changes:
Introduced abstractions and injected them via constructor. This decouples the service from concrete data store implementations and allows for easier testing and future extensibility.

Reason:
 - Adheres to the Dependency Inversion Principle (SOLID).
 - Improves testability by allowing mocks/substitutes in unit tests.
 - Increases flexibility for future data store changes.

2. Encapsulation and DDD

Problem:
There were inconsistent logic spread throughout the service for payment validation.

Changes:
Moved payment validation logic into Account class, encapsulating business logic and having a single source of truth.

Reason:
 - Improves code reuseability.
 - Prevent accidental modification of logic.

3. Error Handling and Result Reporting

Problem:
The original method only returned a bool to indicate sucess or failure.

Changes:
Return detailed error messages and failure types to indicate the type of failure

Reason:
 - Improves ability to debug issue.
 - Provides for more granular unit testing.

4. Project Structure

Problem:
Files were all contained within a single project. This can become hard to navigate as the project grows and more components are added.

Changes:
Organised solution into clearly defined projects and folders.

Reason:
 - Keeps large codebase manageable.
 - Promotes separation of concern.

5. Bank Transaction

Problem:
When a payment is made, there is no record of the type of payment made (or attempted), the amount and the reason for failure.

Changes:
Added BankTransaction class to log all payments that were attempted even if it failed.

Reason:
 - Provide clear audit trail for all payment operations
 - Enables tracking and troubleshooting of payment actitivty

 6. Tests
 
 Problem:
There were no tests written on the payment service so it didn't catch the flaw on payment logic

Changes:
Added unit test to Account and PaymentService class. Also adding mocking.

Reason: 
 - Ensure correctness of business logic
 - Provides safety net for future refactoring

# Future Improvements

1. Repository and database
 - Implement repository and database for storing payment information

2. Logging
 - Implement logging at various point for monitoring and troubleshooting

3. Async
 - Implement async operations when implementing datastore

4. Api and upstream
 - Create api endpoint and all processes upstream of PaymentService

5. Downstream
 - Implement notification system through events should payment succed or fail.

6. Integration tests
 - Create integration tests to ensure multiple services are working together correctly