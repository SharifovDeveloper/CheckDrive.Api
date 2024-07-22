using Bogus;
using CheckDrive.Domain.Entities;
using CheckDrive.Infrastructure.Persistence;

namespace CheckDrive.Api.Extensions
{
    public static class DatabaseSeeder
    {
        private static Faker _faker = new Faker();

        public static void SeedDatabase(this IServiceCollection _, IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<CheckDriveDbContext>();

            CreateRoles(context);
            CreateAccounts(context);
            //CreateCars(context);
            //CreateDrivers(context);
            //CreateDoctors(context);
            //CreateOperators(context);
            //CreateDispatchers(context);
            //CreateMechanics(context);
            //CreateDoctorReviews(context);
            //CreateMechanicHandovers(context);
            //CreateOperatorReviews(context);
            //CreateMechanicAcceptance(context);
            //CreateDispatcherReviews(context);
        }

        private static void CreateRoles(CheckDriveDbContext context)
        {
            if (context.Roles.Any()) return;

            List<Role> roles = new()
            {
                new Role()
                {
                    Name = "Manager"
                },
                new Role()
                {
                    Name = "Haydovchi"
                },
                new Role()
                {
                    Name = "Shifokor"
                },
                new Role()
                {
                    Name = "Operator"
                },
                new Role()
                {
                    Name = "Dispetcher"
                },
                new Role()
                {
                    Name = "Mexanik"
                }
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();
        }

        private static void CreateAccounts(CheckDriveDbContext context)
        {
            if (context.Accounts.Any()) return;
            
            var account = new Account()
                    {
                        Login = "mamager",
                        Password = "12345678",
                        PhoneNumber = "+998945242132",
                        FirstName = "Azamat",
                        LastName = "G`iyosov",
                        Bithdate = DateTime.UtcNow,
                        RoleId = 1,
                    };
                

            context.Accounts.Add(account);
            context.SaveChanges();
        }

        private static void CreateCars(CheckDriveDbContext context)
        {
            if (context.Cars.Any()) return;

            List<Car> cars = new()
            {
                new Car()
                {
                    Model = "Gentra",
                    Color = "Black",
                    Number = "01 145 PBA",
                    Mileage = 157,
                    MeduimFuelConsumption = 8.2,
                    FuelTankCapacity = 60,
                    ManufacturedYear = 2019,
                    RemainingFuel = 12,
                },
                new Car()
                {
                    Model = "Cobalt",
                    Color = "Black",
                    Number = "01 312 PBA",
                    Mileage = 157,
                    MeduimFuelConsumption = 8.2,
                    FuelTankCapacity = 60,
                    ManufacturedYear = 2019,
                    RemainingFuel = 12,
                },
                new Car()
                {
                    Model = "Matiz",
                    Color = "White",
                    Number = "01 146 PBA",
                    Mileage = 128,
                    MeduimFuelConsumption = 6.4,
                    FuelTankCapacity = 35,
                    ManufacturedYear = 2015,
                    RemainingFuel = 14,
                },
                new Car()
                {
                    Model = "Nexia 2",
                    Color = "Gray",
                    Number = "01 147 PBA",
                    Mileage = 198,
                    MeduimFuelConsumption = 8.2,
                    FuelTankCapacity = 45,
                    ManufacturedYear = 2018,
                    RemainingFuel = 25,
                },
                new Car()
                {
                    Model = "Spark",
                    Color = "Blue",
                    Number = "01 148 PBA",
                    Mileage = 228,
                    MeduimFuelConsumption = 6.6,
                    FuelTankCapacity = 35,
                    ManufacturedYear = 2019,
                    RemainingFuel = 10,
                },
                new Car()
                {
                    Model = "Prado",
                    Color = "Black",
                    Number = "01 149 PBA",
                    Mileage = 119,
                    MeduimFuelConsumption = 11.4,
                    FuelTankCapacity = 90,
                    ManufacturedYear = 2019,
                    RemainingFuel = 6,
                },
                new Car()
                {
                    Model = "Cadillac",
                    Color = "Black",
                    Number = "01 150 PBA",
                    Mileage = 28,
                    MeduimFuelConsumption = 10.7,
                    FuelTankCapacity = 117,
                    ManufacturedYear = 2019,
                    RemainingFuel = 11,
                },
            };

            context.Cars.AddRange(cars);
            context.SaveChanges();
        }

        private static void CreateDrivers(CheckDriveDbContext context)
        {
            if (context.Drivers.Any()) return;

            var accounts = context.Accounts.ToList();
            var roles = context.Roles.ToList();
            List<Driver> drivers = new();

            foreach (var account in accounts)
            {
                var driverRole = roles.FirstOrDefault(r => r.Name == "Haydovchi");
                if (driverRole != null && account.RoleId == driverRole.Id)
                {
                    drivers.Add(new Driver()
                    {
                        AccountId = account.Id,
                    });
                }
            }

            context.Drivers.AddRange(drivers);
            context.SaveChanges();
        }

        private static void CreateDoctors(CheckDriveDbContext context)
        {
            if (context.Doctors.Any()) return;

            var accounts = context.Accounts.ToList();
            var roles = context.Roles.ToList();
            List<Doctor> doctors = new();

            foreach (var account in accounts)
            {
                var doctorRole = roles.FirstOrDefault(r => r.Name == "Shifokor");
                if (doctorRole != null && account.RoleId == doctorRole.Id)
                {
                    doctors.Add(new Doctor()
                    {
                        AccountId = account.Id,
                    });
                }
            }

            context.Doctors.AddRange(doctors);
            context.SaveChanges();
        }

        private static void CreateOperators(CheckDriveDbContext context)
        {
            if (context.Operators.Any()) return;

            var accounts = context.Accounts.ToList();
            var roles = context.Roles.ToList();
            List<Operator> operators = new();

            foreach (var account in accounts)
            {
                var operatorRole = roles.FirstOrDefault(r => r.Name == "Operator");
                if (operatorRole != null && account.RoleId == operatorRole.Id)
                {
                    operators.Add(new Operator()
                    {
                        AccountId = account.Id,
                    });
                }
            }

            context.Operators.AddRange(operators);
            context.SaveChanges();
        }

        private static void CreateDispatchers(CheckDriveDbContext context)
        {
            if (context.Dispatchers.Any()) return;

            var accounts = context.Accounts.ToList();
            var roles = context.Roles.ToList();
            List<Dispatcher> dispatchers = new();

            foreach (var account in accounts)
            {
                var dispatcherRole = roles.FirstOrDefault(r => r.Name == "Dispetcher");
                if (dispatcherRole != null && account.RoleId == dispatcherRole.Id)
                {
                    dispatchers.Add(new Dispatcher()
                    {
                        AccountId = account.Id,
                    });
                }
            }

            context.Dispatchers.AddRange(dispatchers);
            context.SaveChanges();
        }

        private static void CreateMechanics(CheckDriveDbContext context)
        {
            if (context.Mechanics.Any()) return;

            var accounts = context.Accounts.ToList();
            var roles = context.Roles.ToList();
            List<Mechanic> mechanics = new();

            foreach (var account in accounts)
            {
                var mechanicRole = roles.FirstOrDefault(r => r.Name == "Mexanik");
                if (mechanicRole != null && account.RoleId == mechanicRole.Id)
                {
                    mechanics.Add(new Mechanic()
                    {
                        AccountId = account.Id,
                    });
                }
            }

            context.Mechanics.AddRange(mechanics);
            context.SaveChanges();
        }

        private static void CreateDoctorReviews(CheckDriveDbContext context)
        {
            if (context.DoctorReviews.Any()) return;

            var drivers = context.Drivers.ToList();
            var doctors = context.Doctors.ToList();
            List<DoctorReview> doctorReviews = new();

            foreach (var doctor in doctors)
            {
                int doctorReviewsCount = new Random().Next(5, 10);

                for (int i = 0; i < doctorReviewsCount; i++)
                {
                    var randomDriver = _faker.PickRandom(drivers);
                    var isHealthy = _faker.Random.Bool();
                    var comments = isHealthy ? "" : _faker.Lorem.Sentence();

                    doctorReviews.Add(new DoctorReview()
                    {
                        IsHealthy = isHealthy,
                        Date = _faker.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now),
                        Comments = comments,
                        DoctorId = doctor.Id,
                        DriverId = randomDriver.Id,
                    });
                }
            }

            context.DoctorReviews.AddRange(doctorReviews);
            context.SaveChanges();
        }

        private static void CreateMechanicHandovers(CheckDriveDbContext context)
        {
            if (context.MechanicsHandovers.Any()) return;

            var cars = context.Cars.ToList();
            var drivers = context.Drivers.ToList();
            var mechanics = context.Mechanics.ToList();
            List<MechanicHandover> mechanicHandovers = new();

            foreach (var mechanic in mechanics)
            {
                var mechanicHandoversCount = new Random().Next(5, 10);
                for (int i = 0; i < mechanicHandoversCount; i++)
                {
                    var randomDriver = _faker.PickRandom(drivers);
                    var randomCar = _faker.PickRandom(cars);
                    var isHanded = _faker.Random.Bool();
                    var comments = isHanded ? "" : _faker.Lorem.Sentence();
                    var status = _faker.Random.Enum<Status>();

                    mechanicHandovers.Add(new MechanicHandover()
                    {
                        IsHanded = isHanded,
                        Comments = comments,
                        Date = _faker.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now),
                        Status = status,
                        Distance = _faker.Random.Int(50, 100),
                        MechanicId = mechanic.Id,
                        DriverId = randomDriver.Id,
                        CarId = randomCar.Id,
                    });
                }
            }

            context.MechanicsHandovers.AddRange(mechanicHandovers);
            context.SaveChanges();
        }

        private static void CreateOperatorReviews(CheckDriveDbContext context)
        {
            if (context.OperatorReviews.Any()) return;

            var operators = context.Operators.ToList();
            var drivers = context.Drivers.ToList();
            var cars = context.Cars.ToList();
            List<OperatorReview> operatorReviews = new();

            foreach (var operatorr in operators)
            {
                var operatorReviewsCount = new Random().Next(5, 10);
                for (int i = 0; i < operatorReviewsCount; i++)
                {
                    var randomDriver = _faker.PickRandom(drivers);
                    var randomCar = _faker.PickRandom(cars);
                    var status = _faker.Random.Enum<Status>();
                    var oilMarks = _faker.Random.Enum<OilMarks>();
                    var isGiven = _faker.Random.Bool();
                    var comments = isGiven ? "" : _faker.Lorem.Sentence();

                    operatorReviews.Add(new OperatorReview()
                    {
                        OilAmount = _faker.Random.Double(10, 20),
                        Date = _faker.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now),
                        Status = status,
                        OilMarks = oilMarks,
                        IsGiven = isGiven,
                        Comments = comments,
                        OperatorId = operatorr.Id,
                        DriverId = randomDriver.Id,
                        CarId = randomCar.Id
                    });
                }
            }

            context.OperatorReviews.AddRange(operatorReviews);
            context.SaveChanges();
        }

        private static void CreateMechanicAcceptance(CheckDriveDbContext context)
        {
            if (context.MechanicsAcceptances.Any()) return;

            var cars = context.Cars.ToList();
            var drivers = context.Drivers.ToList();
            var mechanics = context.Mechanics.ToList();
            List<MechanicAcceptance> mechanicAcceptances = new();

            foreach (var mechanic in mechanics)
            {
                var mechanicAcceptancesCount = new Random().Next(5, 10);
                for (int i = 0; i < mechanicAcceptancesCount; i++)
                {
                    var randomDriver = _faker.PickRandom(drivers);
                    var randomCar = _faker.PickRandom(cars);
                    var isAccepted = _faker.Random.Bool();
                    var comments = isAccepted ? "" : _faker.Lorem.Sentence();
                    var status = _faker.Random.Enum<Status>();

                    mechanicAcceptances.Add(new MechanicAcceptance()
                    {
                        IsAccepted = isAccepted,
                        Comments = comments,
                        Date = _faker.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now),
                        Status = status,
                        Distance = _faker.Random.Int(50, 100),
                        MechanicId = mechanic.Id,
                        DriverId = randomDriver.Id,
                        CarId = randomCar.Id,
                    });
                }
            }

            context.MechanicsAcceptances.AddRange(mechanicAcceptances);
            context.SaveChanges();
        }

        private static void CreateDispatcherReviews(CheckDriveDbContext context)
        {
            if (context.DispatchersReviews.Any()) return;

            var dispatchers = context.Dispatchers.ToList();
            var mechanics = context.Mechanics.ToList();
            var drivers = context.Drivers.ToList();
            var operators = context.Operators.ToList();
            var cars = context.Cars.ToList();
            var mechanicHandovers = context.MechanicsHandovers.ToList();
            var mechanicAcceptances = context.MechanicsAcceptances.ToList();
            var operatorReviews = context.OperatorReviews.ToList();
            List<DispatcherReview> dispatcherReviews = new();

            foreach (var dispatcher in dispatchers)
            {
                var dispatcherReviewsCount = new Random().Next(5, 10);
                for (int i = 0; i < dispatcherReviewsCount; i++)
                {
                    var randomDriver = _faker.PickRandom(drivers);
                    var randomOperator = _faker.PickRandom(operators);
                    var randomMechanics = _faker.PickRandom(mechanics);
                    var randomCar = _faker.PickRandom(cars);
                    var randomMechanicHandover = _faker.PickRandom(mechanicHandovers);
                    var randomMechanicAcceptance = _faker.PickRandom(mechanicAcceptances);
                    var randomOperatorReview = _faker.PickRandom(operatorReviews);

                    dispatcherReviews.Add(new DispatcherReview()
                    {
                        Date = _faker.Date.Between(DateTime.Now.AddYears(-1), DateTime.Now),
                        FuelSpended = _faker.Random.Double(10, 20),
                        DistanceCovered = _faker.Random.Int(50, 100),
                        DispatcherId = dispatcher.Id,
                        DriverId = randomDriver.Id,
                        OperatorId = randomOperator.Id,
                        MechanicId = randomMechanics.Id,
                        CarId = randomCar.Id,
                        MechanicAcceptanceId = randomMechanicAcceptance.Id,
                        MechanicHandoverId = randomMechanicHandover.Id,
                        OperatorReviewId = randomOperatorReview.Id,
                    });
                }
            }

            context.DispatchersReviews.AddRange(dispatcherReviews);
            context.SaveChanges();
        }
    }
}
