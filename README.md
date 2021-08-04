# Task Manager Api

A demonstration of my workflow process on a sample project.

## Brief

- Task management system to track progress of work done by team members.

## Assumptions

- Tasks can only be assigned to one person.
- Tasks can only be complete/not complete, but will contain progress items to track the progress of a task.
  - You can mark a task as complete but still have pending progress items.
- Only the Manager and the team member who is assigned to a task can edit it or change progerss items.
- This will be company internal only, and the data it contains will not be sensitive in nature.
- An MVP that can scale is more useful than a complete solution over a longer time period.

## Architecture Choices

- Working with CQRS (Command Query Responsibility Segregation) to keep functionality isolated and easy to add to.
- `xUnit` with `Moq`, `AutoFixture` and `FluentAssertions` for testing.
- Implementing a personal twist on [Clean Architecture](https://jasontaylor.dev/clean-architecture-getting-started/) as described by [Jason Taylor](https://github.com/jasontaylordev), based on concepts from [Uncle Bob](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html).

## Testing Choices

- I've elected to try my hand at the `test-first` approach for this project. It led to some running around a little clueless on how to start and how far to define by base abstraction.
- This caused issues as I didn't know how complicated mocking DbSet would be. This lead to large refactors and the use of a glue library in the mean time. Jason Taylor makes use of Integration Tests to do this instead which is arguably a more elegant solution. The other option would to have a repository layer that I could mock the db sets into collections with but it'd create some hard coupling between db fetch logic and the command/query layer. Lesser of two evils problem.
- For now I'll limit tests to the command/query layer as expanding the tests is proving to be very time consuming on first setup. They should be near zero cost once a template exists but a from scratch build means its just too much time to ask for.

## Requirements

Copy of Visual Studio, VS Code, or `dotnet cli`.
The project is .NET 5, so you'll need the SDK to be installed.

## How to use

1. Clone
```bash
git clone https://github.com/M3ales/TaskManager.Api.git
```

2. Ensure WebApi is set as the Startup Project
3. Run WebApi using the WebApi launch settings
4. Navigate to `localhost:5001/api`
5. Select the Auth expander
6. Select Authenticate
7. Click Try it Out (on the top right)
8. Edit the request body from:
```json
{
  "refreshToken": "string"
}
```
to
```json
{
  "refreshToken": "ASDF"
}
```
9. Execute
10. Copy the response body section (without "") ie.
```jwt
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwibmJmIjoxNjI4MDM3NTk0LCJleHAiOjE2MjgwNDExOTQsImlhdCI6MTYyODAzNzU5NCwiaXNzIjoiVGFza01hbmFnZXIuQXBpIiwiYXVkIjoiVGFza01hbmFnZXIuQXBpIn0.yrolyQIkyme1nDYyIg7WYc9HKhErwAvlfW2bCgPa3pw
```
11. Scroll to the top of the page and on the top right click 'Authorize'.
12. Paste the JWT and hit Authorize
13. You now can perform any requests as an authenticated 'admin' user.

Note if you regenerate the key and lose it you will not be able to recover it. In Memory DB means that it resets to `ASDF` every time you restart the application.

## Difficulties

- EFCore's DbSet&lt;T&gt; concrete type is difficult to mock correctly.
- Tests needed to be refactored because of DbSet issue, it reduced their readability.
- Having tests impacted the turn around time significantly. But I am far more confident about the behaviour of the codebase.

## Authentication and Authorization

- POST your API Key to `/api/auth/token` to get a short lived jwt (3600 seconds) with which to do requests.
- Include the key in your header, with or without the `Bearer` prefix.
- You can easily do this from the generated Swagger Doc page at `/api` by clicking the Authorize lock button on the top right, and pasting your JWT there.
- JWT at the moment is just a hardcoded key for private/public pair. Ideally this will be refactored out at some point.

## Learning Points

- Test first is robust but requires significant infrastructure setup to produce tests that aren't overly repetative. It's possible that an even more developed infrastructure system may be better.
- xUnit + AutoMoq + AutoFixture + DbSet = Uncertainty
- Significantly better understanding of the pitfalls of this implementation of Clean Architecture, as well as the heavy coupling introduced by IApplicationDbContext.

## Things I still want to do but haven't been able to get to

- Extract the hardcoded keys out of the AuthService, and move it into either `appsettings.json` or environment variables.
- Write tests for all the Commands and Queries I skipped due to time constraints.
- Write integration tests for the authentication.
- Write claims based authentication and allow for team members to be provided logins which allow them to only see the work they need to.
- Write a way to map team members to accounts easily.
- Filtering of progress information and perhaps some form of 'this past week' summary/dashboard feature.
- Find a way to better mock DbSet or convert the commands and queries to integration tests (using full DI).
- Write a SignalR or GRPC WebApi layer to illustrate modularity.
- Seed Data

Time Spent: 30 hours.
