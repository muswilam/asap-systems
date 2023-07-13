

NOTE ==> To create databse:

CLI:

    - Navigate to API project. (cd AsapSystems.API)
    - Run command: dotnet ef database update

PM:

    - Set API project as startup project.
    - update-database


Steps:

    1. build project structure (Onion, Repository Pattern, UnitOfWork):
        I Domain (core)
            - Entities.
            - Repositories Declarations (Interfaces).
            - UnitOfWork Declaration.	
            - Any Other Abstraction.
        II Persistence (infrastruction)
            - Migrations.
            - Repositories Implementation (classes).
            - UnitOfWork Implementation.
            - DB Context.
        III Service (bll)
            - Declarations and implementations.
        IV API
            - endpoints.
    
    2. Implement Auth Service:
        - Register, Login, Refresh Token, Logout
        - Using JWT Token.
        - Authorize all endpoints except auth endpoints.

    3. Implement Person Service:
        - Get persons with filter, search, pagination.

    4. Implement Address Service:
        - All CRUD operations: 
            * Get addresses with filter, search, pagination.
            * Get by id.
            * Create.
            * Update.
            * Delete.

    5. Implement validation. (Just focus on required inputs and business validation.)