# NetForge Demo .NET 7

This is a demo web project that contains the database context to be used in the NetForge admin panel.

## Commands

This project contains some helper commands that can be used to initialize / interact with basic project data.

General syntax for executing a command (from command line): `Saritasa.NetForge.Demo.Net7.exe command-name parameters`

### Create User

Creates a new user in the system.

Arguments:
- `--first-name` - user's first name
- `--last-name` - user's last name
- `--email` - user's email
- `--password` user's password

Example usage:

`Saritasa.NetForge.Demo.Net7.exe create-user --first-name=John --last-name=Doe --email=jdoe@dotnet-forge.com --password=11111111Aa`

### Data seed

Seeds the data with some default values.

Arguments:
- `--name` - name of the data seeder to use
- `--count` - amount of objects the seeder should generate

Available seeders:
- `Products` - adds random products to the database.
- `Users` - adds random users to the database. You can set `--password` to set override the generated users' password (default is `11111111Aa`).
- `Addresses` - adds random addresses to the database.
- `ContactInfo` - adds random contact information to the database.
- `ProductTags` - adds random product tags to the database.
- `Shops` - adds random shops to the database.

Example usage:

`Saritasa.NetForge.Demo.Net7.exe seed --name=Products`
