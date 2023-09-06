# tests-tryout-1

## "Fix"

- One way of bypass the LegacyTasksController testing "problems" is to add the local db file to the output folder. However this is not a good solution because if the db is a file, it will probably be located in any other place than the application binaries.
- One way of bypass the TasksController testing "problems" is to add the ENVIRONMENT VARIABLE on the test class before all the test methods. However this is not a good solution because you will be using the same database as the project or will need to use a copy of the database.
