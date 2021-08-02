# Task Manager Api

A demonstration of my workflow process on a sample project.

## Brief

- Task management system to track progress of work done by team members.

## Assumptions

- Tasks can only be assigned to one person.
- Tasks can only be complete/not complete, but will contain progress items to track the progress of a task.
  - You can mark a task as complete but still have pending progress items.
- Only the Manager and the team member who is assigned to a task can edit it or change progerss items.

## Architecture Choices

- Working with CQRS (Command Query Responsibility Segregation) to keep functionality isolated and easy to add to.
- `xUnit` with `Moq`, `AutoFixture` and `FluentAssertions` for testing.
- Implementing a personal twist on [Clean Architecture](https://jasontaylor.dev/clean-architecture-getting-started/) as described by [Jason Taylor](https://github.com/jasontaylordev), based on concepts from [Uncle Bob](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html).

## Testing Choices

- I've elected to try my hand at the `test-first` approach for this project. It led to some running around a little clueless on how to start and how far to define by base abstraction.
- This caused issues as I didn't know how complicated mocking DbSet would be. This lead to large refactors and the use of a glue library in the mean time. Jason Taylor makes use of Integration Tests to do this instead which is arguably a more elegant solution. The other option would to have a repository layer that I could mock the db sets into collections with but it'd create some hard coupling between db fetch logic and the command/query layer. Lesser of two evils problem.
- For now I'll limit tests to the command/query layer as expanding the tests is proving to be very time consuming on first setup. They should be near zero cost once a template exists but a from scratch build means its just too much time to ask for.

## Difficulties

- EFCore's DbSet&lt;T&gt; concrete type is difficult to mock correctly.
- Tests needed to be refactored because of DbSet issue, it reduced their readability.
- Having tests impacted the turn around time significantly. But I am far more confident about the behaviour of the codebase.

Time Spent: 28 hours.
