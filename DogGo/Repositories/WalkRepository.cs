using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly IConfiguration _config;

        public WalkRepository(IConfiguration config)
        {
            _config = config;
        }
        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walk> GetWalksByWalkerId(int walkerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT w.Id,
                                        w.Date,
                                        w.Duration,
                                        o.Name as OwnerName,
                                        w.DogId,
                                        w.WalkerId
                                        FROM Walks w
                                        join Dog d ON d.Id = w.DogId
                                        join Owner o ON o.Id = d.OwnerId
                                        WHERE w.WalkerId = @walkerId;";

                    cmd.Parameters.AddWithValue("@walkerId", walkerId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walk> walks = new List<Walk>();

                    while (reader.Read())
                    {
                        Walk walk = new Walk()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")).ToString("MM/dd/yyyy"),
                            Duration = Convert.ToInt32(TimeSpan.FromSeconds(reader.GetInt32(reader.GetOrdinal("Duration"))).TotalMinutes),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            Owner = new Owner
                            {
                                Name = reader.GetString(reader.GetOrdinal("OwnerName"))
                            }

                        };

                        walks.Add(walk);
                    }
                    reader.Close();
                    int TotalHours = walks.Sum(w => TimeSpan.FromMinutes(Convert.ToInt32(w.Duration)).Hours);
                    int TotalMinutes = walks.Sum(w => TimeSpan.FromMinutes(Convert.ToInt32(w.Duration)).Minutes);
                    //walks.Add(TotalHours);
                    //walks.Add(TotalMinutes);
                    return walks;
                }
            }
        }
    }
}


