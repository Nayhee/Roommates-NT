using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    public class ChoreRepository : BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }

        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name) OUTPUT INSERTED.Id VALUES (@name)";
                    cmd.Parameters.AddWithValue("@name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                }
            }
        }

        public List<ChoreReport> GetChoreCounts()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Count(rmc.ChoreId) as NumChores, rm.Id, rm.FirstName, rm.LastName 
                                        FROM RoommateChore rmc 
                                        JOIN Roommate rm on rm.Id = rmc.RoommateId 
                                        GROUP BY rm.Id, rm.FirstName, rm.LastName;";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<ChoreReport> choreReports = new List<ChoreReport>();

                        while (reader.Read())
                        {
                            ChoreReport choreReport = new ChoreReport
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                ChoreCount = reader.GetInt32(reader.GetOrdinal("NumChores")),
                            };
                            choreReports.Add(choreReport);
                        }
                        return choreReports;
                    }
                }
            }
        }

        public void AssignChore(int roommateId, int choreId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO RoommateChore (RoommateId, ChoreId) VALUES (@RoommateId, @ChoreId)";
                    cmd.Parameters.AddWithValue("@RoommateId", roommateId);
                    cmd.Parameters.AddWithValue("@ChoreId", choreId);
                    cmd.ExecuteScalar();
                }
            }
        }

        public List<Chore> GetUnassignedChores ()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT c.Id, c.Name 
                                        FROM Chore c 
                                        LEFT JOIN RoommateChore rmc on rmc.ChoreId = c.Id 
                                        WHERE rmc.ChoreId is null";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Chore> chores = new List<Chore>();

                        while (reader.Read())
                        {
                            Chore chore = new Chore
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                            };

                            chores.Add(chore);
                        }
                        return chores;
                    }
                }
            }
        }

        public Chore GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Name FROM Chore WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Chore chore = null;
                        if(reader.Read())
                        {
                            chore = new Chore
                            {
                                Id = id,
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                        }
                        return chore;
                    }
                }
            }
        }

        public List<Chore> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Chore";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Chore> chores = new List<Chore>();

                        while (reader.Read())
                        {
                            int idColumnPosition = reader.GetOrdinal("Id");
                            int idValue = reader.GetInt32(idColumnPosition);
                            int nameColumnPosition = reader.GetOrdinal("Name");
                            string nameValue = reader.GetString(nameColumnPosition);

                            Chore chore = new Chore
                            {
                                Id = idValue,
                                Name = nameValue
                            };

                            chores.Add(chore);
                        }
                        return chores;
                    }
                }
            }
        }



    }
}