Entity Framework

Not a big fan of entity framework as I like to have greater control over the SQL commands and don't like how it uses sp_executesl stored procedure thus requiring high privlidges.
I also don't like how tightly coupled Entity framework is to the UI and how you have to add attributes to the model to dictate what SQL datatypes should be used. However, I used it here for speed of development as it's great for prototyping and putting small applications together.

Entity Framework Migrations have been used to create the database. To install the database run the following command on the 'Package Manager Console'

Update-Database

 