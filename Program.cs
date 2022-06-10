using System;
using System.Collections.Generic;
using Roommates.Models;
using Roommates.Repositories;

namespace Roommates
{
    class Program
    {
        //  This is the address of the database.
        //  We define it here as a constant since it will never change.
        private const string CONNECTION_STRING = @"server=localhost\SQLExpress;database=Roommates;integrated security=true;TrustServerCertificate=true;";

        static void Main(string[] args)
        {
            RoomRepository roomRepo = new RoomRepository(CONNECTION_STRING);
            ChoreRepository choreRepo = new ChoreRepository(CONNECTION_STRING);
            RoommateRepository roommateRepo = new RoommateRepository(CONNECTION_STRING);

            bool runProgram = true;
            while (runProgram)
            {
                string selection = GetMenuSelection();

                switch (selection)
                {
                    case ("Show all rooms"):
                        List<Room> rooms = roomRepo.GetAll();
                        foreach (Room r in rooms)
                        {
                            Console.WriteLine($"{r.Name} has an Id of {r.Id} and a max occupancy of {r.MaxOccupancy}");
                        }
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for room"):
                        Console.Write("Room Id: ");
                        int id = int.Parse(Console.ReadLine());

                        Room room = roomRepo.GetById(id);

                        Console.WriteLine($"{room.Id} - {room.Name} Max Occupancy({room.MaxOccupancy})");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a room"):
                        Console.Write("Room name: ");
                        string name = Console.ReadLine();

                        Console.Write("Max occupancy: ");
                        int max = int.Parse(Console.ReadLine());

                        Room roomToAdd = new Room()
                        {
                            Name = name,
                            MaxOccupancy = max
                        };

                        roomRepo.Insert(roomToAdd);

                        Console.WriteLine($"{roomToAdd.Name} has been added and assigned an Id of {roomToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Show all chores"):
                        List<Chore> chores = choreRepo.GetAll();
                        foreach (Chore c in chores)
                        {
                            Console.WriteLine($"{c.Id}) {c.Name} ");
                        }
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Search for chore"):
                        Console.Write("Chore Id: ");
                        int Id = int.Parse(Console.ReadLine());

                        Chore chore = choreRepo.GetById(Id);

                        Console.WriteLine($"{chore.Id} - {chore.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;
                    case ("Add a chore"):
                        Console.Write("Chore name: ");
                        string choreName = Console.ReadLine();

                        Chore choreToAdd = new Chore()
                        {
                            Name = choreName
                        };

                        choreRepo.Insert(choreToAdd);

                        Console.WriteLine($"{choreToAdd.Name} has been added and assigned an Id of {choreToAdd.Id}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Show all Unassigned Chores"):
                        List<Chore> unassignedChores = choreRepo.GetAll();
                        foreach (Chore c in unassignedChores)
                        {
                            Console.WriteLine($"{c.Id}) {c.Name}");
                        }
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Search for roommate"):
                        Console.Write("Roommate Id: ");
                        int roommateId = int.Parse(Console.ReadLine());

                        Roommate roommate = roommateRepo.GetById(roommateId);

                        Console.WriteLine($"Roommate #{roommate.Id} - Name: {roommate.FirstName}, Rent Portion: {roommate.RentPortion}, Room Name: {roommate.Room.Name}");
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Assign chore to roommate"):
                        List<Chore> allChores = choreRepo.GetAll();
                        foreach (Chore c in allChores)
                        {
                            Console.WriteLine($"{c.Id}) {c.Name}");
                        }
                        Console.Write("Chore Id: ");
                        int cId = int.Parse(Console.ReadLine());

                        List<Roommate> roommateList = roommateRepo.GetAll();
                        foreach (Roommate rm in roommateList)
                        {
                            Console.WriteLine($"{rm.Id}) {rm.FirstName} {rm.LastName}");
                        }
                        Console.Write("Roommate Id: ");
                        int rmId = int.Parse(Console.ReadLine());

                        choreRepo.AssignChore(rmId, cId);
                        Console.WriteLine("Successfully assigned chore to roommate.");
                        Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Get Chore Count by Roommate"):
                        List<ChoreReport> choreCountReport = choreRepo.GetChoreCounts();
                        Console.WriteLine();
                        foreach (ChoreReport cr in choreCountReport)
                        {
                            Console.WriteLine($"{cr.FirstName} {cr.LastName}: {cr.ChoreCount} chores ");
                        }
                        Console.WriteLine();
                        Console.Write("Press any key to continue");
                        Console.ReadKey();
                        break;

                    case ("Exit"):
                        runProgram = false;
                        break;
                }
            }


        }

        static string GetMenuSelection()
        {
            Console.Clear();

            List<string> options = new List<string>()
            {
                "Show all rooms",
                "Search for room",
                "Add a room",
                "Show all chores",
                "Search for chore",
                "Add a chore",
                "Show all Unassigned Chores",
                "Search for roommate",
                "Assign chore to roommate",
                "Get Chore Count by Roommate",
                "Exit"
            };

            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            while (true)
            {
                try
                {
                    Console.WriteLine();
                    Console.Write("Select an option > ");

                    string input = Console.ReadLine();
                    int index = int.Parse(input) - 1;
                    return options[index];
                }
                catch (Exception)
                {

                    continue;
                }
            }
        }
    }
}