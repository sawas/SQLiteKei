#SQLite Kei
##Description
SQLite Kei is .NET based management tool for SQLite databases.

![alt tag](http://puu.sh/qOYxY/82cf766ecd.png)

##Existing Features
- Manage multiple databases simultaneously
- Manage tables, views, indexes and triggers
- Useful creator wizards that assist you in the creation of new tables, views, indexes, triggers and select queries
- View select data with the option to inline-edit or delete records from the result view
- Simple query editor to write plain SQL to your databases with the option to load and save .sql files
- Copy tables from one database to another
- Export tables and their values as SQL and CSV

##Technical infos & Technologies used
- C# on .NET Framework 4.5.2
- WPF
- SQLite
- log4net
- NUnit

##Contribution and participation
If you want to help out, feel free to test the newest versions and report any issues, may it be bugs or (feature) suggestions.

##Project Structure
- **SQLiteKei** - The main project which represents the UI and business layer.
- **SQLiteKei.DataAccess** - Provides functionality to access an sqlite database and to create SQL queries using custom QueryBuilders

###Test Projects
- **SQLiteKei.UnitTests** - The unit tests for the main project.
- **SQLiteKei.DataAccess.UnitTests** - The unit tests for the DataAccess project.
- **SQLiteKei.IntegrationTests** - The main project for integration tests of all projects. Note: tests need the NUnit 3 Runner to be started from VS locally. R# does not support the [OneTimeSetup] and [OneTimeTearDown] features of NUnit which replaced [TestFixtureSetup] and [TestFixtureTearDown] respectively.

##Trivia
- About the project name: I was looking for a working title and noticed that all the cool stuff like Manager, Management, Studio, Administrator, etcetera all were in use already. Since I like to use random Japanese words as working titles for my projects I picked the Japanese Kanji 系 (written as 'kei') which means "system" or "lineage".
- It started out as a side project for me to learn different technologies. Hooray for messy beginner code! This tool has **not** started out using the MVVM pattern, though I started using it midway-through and refactored it towards this as much as possible.
- I organized myself using [Taiga.io](https://taiga.io "Taiga.io"), an open source project management tool that supports Scrum and Kanban styled procedures. It's a really neat and powerful tool, you should check them out.
