# tests-tryout-1 branch.

## Identified Issues

- We can not test the LegacyTasksController because the file is located in a folder that the tests project has not access, and it do not have to has access.
- We can not test the TasksController because it depends on a connection to MongoDB and the test project do not has access to that resource and it do not have has to have access.
