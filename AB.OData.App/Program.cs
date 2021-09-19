using ConsoleTableExt;
using ConsoleTables;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Terminal.Gui;
using Trippin;

namespace AB.OData.App
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            bool run = true;
            while (run)
            {
                var peopleService = host.Services.GetRequiredService<IPeopleService>();

                Console.Clear();

                Console.WriteLine("Choose an option:");
                Console.WriteLine("1) List people");
                Console.WriteLine("2) Search people by name");
                Console.WriteLine("3) Show person detail");
                Console.WriteLine("4) Update person age");

                Console.WriteLine("\r\n0) Exit");

                Console.Write("\r\nSelect an option: ");

                switch (Console.ReadLine().ToLower())
                {
                    case "1":
                        var allPeople = (await peopleService.GetAllPeople())
                            .Select(AsSimplifiedPerson);

                        PrintPeople(allPeople);
                        break;

                    case "2":
                        Console.Write("\r\nEnter name: ");

                        var peopleByName = (await peopleService.GetPeopleByName(Console.ReadLine()))
                            .Select(AsSimplifiedPerson);

                        PrintPeople(peopleByName);

                        break;

                    case "3":
                        Console.Write("\r\nEnter username: ");

                        var person = (await peopleService.GetPersonByUsername(Console.ReadLine()));
                        Console.WriteLine(JsonConvert.SerializeObject(person, Formatting.Indented));

                        break;

                    case "4":
                        Console.Write("\r\nEnter username: ");

                        var personToUpdate = (await peopleService.GetPersonByUsername(Console.ReadLine()));
                        if (personToUpdate == null) break;

                        Console.Write("\r\nEnter age: ");
                        personToUpdate.Age = Convert.ToInt64(Console.ReadLine());

                        try
                        {
                            await peopleService.UpdatePerson(personToUpdate);
                            Console.WriteLine(JsonConvert.SerializeObject(personToUpdate, Formatting.Indented));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed to update.");
                            Console.WriteLine(ex);
                        }

                        break;

                    case "0":
                        run = false;
                        break;
                }

                if (run)
                {
                    Console.Write("\r\nPress any key to clear... ");
                    Console.ReadKey();
                }
            }

            // Not needed
            //await host.RunAsync();
        }

        private static void PrintPeople(IEnumerable<SimplifiedPerson> allPeople)
        {
            ConsoleTable
                .From(allPeople)
                .Configure(o => o.NumberAlignment = Alignment.Right)
                .Write(Format.Alternative);
        }

        static SimplifiedPerson AsSimplifiedPerson(Person person)
        {
            return new SimplifiedPerson
            {
                UserName = person.UserName,
                FirstName = person.FirstName,
                MiddleName = person.MiddleName,
                LastName = person.LastName,
                Gender = person.Gender,
                Age = person.Age,
            };
        }

        static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddTransient<IPeopleService, PeopleService>();
                });
        }

        class SimplifiedPerson
        {
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public PersonGender Gender { get; set; }
            public long? Age { get; internal set; }
        }
    }
}
