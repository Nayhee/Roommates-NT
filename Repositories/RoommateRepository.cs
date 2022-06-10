using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;
using System;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "Select rm.Id as RoommateId, rm.FirstName, rm.LastName, rm.RentPortion, rm.MoveInDate, rm.RoomId, r.Name, r.MaxOccupancy FROM Roommate rm JOIN Room r on r.Id = rm.RoomId";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Roommate> roommates = new List<Roommate>();

                        while(reader.Read())
                        {
                            Roommate roommate = new Roommate
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("RoommateId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                MovedInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                                Room = new Room()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy")),
                                }
                            };
                            roommates.Add(roommate);
                        }
                        return roommates;
                    }
                }
            }
        }
        
        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT rm.Id, rm.FirstName, rm.LastName, rm.RentPortion, rm.MoveInDate, rm.RoomId, r.Name, r.MaxOccupancy FROM Roommate rm JOIN Room r on r.Id=RM.RoomId WHERE rm.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Roommate roommate = null;

                        if(reader.Read())
                        {
                            roommate = new Roommate
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                                MovedInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                                Room = new Room()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))
                                }
                            };
                      
                        }
                        return roommate;
                    }
                }
            }
        }

    }
}