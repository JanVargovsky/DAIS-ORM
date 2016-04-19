using DAIS.ORM.DTO;
using DAIS.ORM.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DAIS.ORM.Repositories
{
    public class UserRepository : RepositoryBase<UserDTO>
    {
        public UserRepository(IDatabase db) : base(db)
        {
        }

        public IEnumerable<ActiveUser> MostActiveUsersForLastNDays(int days)
        {
            var results = new List<ActiveUser>();

            try
            {
                StringBuilder query = new StringBuilder()
                    .AppendLine($"SELECT *")
                    .AppendLine($"FROM MostActiveUsersForLastNDays(@days)");

                database.Open();
                using (var command = database.CreateSqlCommand(query.ToString()))
                {
                    command.Parameters.AddWithValue("@days", days);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new ActiveUser
                            {
                                Id = reader.GetInt64(0),
                                TimeSpentTicks = reader.GetInt64(1)
                            });
                        }
                        return results;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                database.Close();
            }
        }
    }

    public class ActiveUser
    {
        public long Id { get; set; }
        public long TimeSpentTicks { get; set; }
        public TimeSpan TimeSpent => TimeSpan.FromTicks(TimeSpentTicks);
    }
}
