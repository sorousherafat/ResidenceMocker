# Residence Mocker
A database schema and a C# console application that inserts mocked data into the database using Entity Framework Core.

## Getting Started

First, install dotnet version 7.0:

```shell
sudo apt install dotnet7
```

Now clone this repository:

```shell
git clone https://github.com/sorousherafat/ResidenceMocker.git && cd ResidenceMocker
```

Then, restore the dependencies:

```shell
dotnet restore
```

Now update the `appsettings.json` file and add your database connection string and mock quantity:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "<connection-string>"
  },
  "Mock": {
    "AddressCount": 2000,
    "CityCount": 100,
    "ComplaintCount": 1000,
    "DamageReportCount": 1000,
    "GuestCount": 10000,
    "HostCount": 100,
    "MessageCount": 10000,
    "PriceChangeCount": 500,
    "ProvinceCount": 20,
    "RentCount": 20000,
    "RentalRequestCount": 25000,
    "ResidenceCount": 200,
    "ReviewCount": 10000,
    "UnavailabilityCount": 100
  }
}
```

Run the `schema.sql` query in your database to create the schema.

And at last, run the program:

```shell
dotnet run
```