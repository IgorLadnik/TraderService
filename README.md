# TraderService
TraderService test



TraderService

The service provides a very simple handling (base CRUD operations) of traders list.
It is based on GraphQL technology [1]. 
GraphQL allows to retrive and submit stored data in a very ordered and at the same time flexible manner.
It provides schema acting as a contract between client and server.
The schema also defines retrival procedure in the server.

Usage of GraphQL brings about a so called N + 1 problem [2].
Efficient application of the technology implies reasonable solution to this issue.
In this solution I implemented my original code solving of N + 1 problem for GraphQL.
In few words it may be formulated as following.
In "naive" solution in handler of every field call to database to retrieve data is used.
In optimazed solution the first field handler on each level retrieve from database data for all fields on this level and
stores them in a cache attached to GraphQL context object available to all foeld handlers.
Then all other field handlers on this level retrieve data from the cache and not from database. 
The difference is illustrated by Figs. 1 and 2.
Similar approach was implemented in Node.js in my article [3].

TraderService provides two controllers.
GqlController processes all GraphQL requests, whereas
TraderController processes parameterless GET request responsing with some text, and
another GET request with Trader id as a parameter.
This request is internally processed as an ordinary GraphQL request with hardcoded query.
TraderController serves just illustrative purpose.

The service referred to several general purposes libraries (DLLs).
They are located in directory Libs.
RepoInterfacesLib provides interfaces to deal with data repository.
RepoLib implements IRepo<T> interface from RepoInterfacesLib for EntityFramework.
It equips save procedure with a transaction.
GraphQlHelperLib contains general GraphQL related software including code for data caching discussed above.
And AsyncLockLib provides locking possibility for async/await methods particularly used in imlpementation of the caching.

Directory Model contains TraderModelLib project.
The project provides code specific for the given problem.

Directory Test includes TraderServiceTest project provideing integration tests for TraderService.
These tests are based on the concept of in-memory service.
Such as approach allows developer effortlessly test actual service code. 


How to Run?

Prerequisites (for Windows):
	- Local SQL Server (please see connection string in file appsettings.json of the service),
	- Visual Studio (VS) 2019 with .NET 5 support
	
1. Open solution TraderService.sln with VS and build it.

2. Start TraderService.
It may be done from VS either as a service or under IIS Express.
Browser with Playground Web application for GraphQL stats automatically.
Alternatively, the service may be started activating 
..\TraderService\TraderService\bin\Debug {or Release}\net5.0\TraderService.exe .
In the latter case browser should be started manually browsing on https://localhost:5001/playground .
In Playgroumd Web page you may see GraphQL schema and play with different quesries and mutations
(some predefined query and mutation may be copied from QueriesSample.txt file).

Playground application does not call GqlController that is used by clients in production.
To work with GqlController you may use Postman.
From Postman make a POST to https://localhost:5001/gql
with Body -> GraphQL providing in QUERY textbox you actual GraphQL query / mutation.

You may also use OpenApi (a.k.a. Swagger):
browse to https://localhost:5001/swagger and activate POST /Gql .
In Postman press Code link in the upper-right corner, copy query to Swagger's Request body textbox and execute method.

In all cases you may use unsafe call to http://localhost:5000 (allowed for illustration and debugging).


Further Development

- Database handling enhancement.
  For easy start, Code First approach to database adopted.
  Ids to tables records are generated from code - which is not acceptable in real world application, e.g.
  due to possible insertion to database by several instances of a service or by several services.
  
- Authentication (I have already implemented JSON Web Token (JWT) authentication for .NET 5 services).

- Deployment with Docker.


Technologies Used

- .NET 5
- ASP.MVC
- GraphQL, including solution to N + 1 problem
- ORM EntityFrameworkCore
- Repo pattern
- In-memory service for integration testing
- Synchronization for async/await methods
- Dependency injection
- Transport Layer Security (TLS)







​	![playground](C:\prj\TraderService\_docs\playground.png)



![swagger](C:\prj\TraderService\_docs\swagger.png)





![postman](C:\prj\TraderService\_docs\postman.png)





![postman-code](C:\prj\TraderService\_docs\postman-code.png)