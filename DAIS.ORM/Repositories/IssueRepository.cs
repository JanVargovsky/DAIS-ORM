using DAIS.ORM.DTO;
using DAIS.ORM.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAIS.ORM.Repositories
{
    public class IssueRepository : RepositoryBase<IssueDTO>
    {
        public IssueRepository(IDatabase db) : base(db)
        {
        }

        public IEnumerable<NonOpenIssue> NonOpenIssuesForLastNDays(int days)
        {
            var results = new List<NonOpenIssue>();

            try
            {
                StringBuilder query = new StringBuilder()
                    .AppendLine($"SELECT *")
                    .AppendLine($"FROM NonOpenIssuesForLastNDays(@days)");

                database.Open();
                using (var command = database.CreateSqlCommand(query.ToString()))
                {
                    command.Parameters.AddWithValue("@days", days);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            results.Add(new NonOpenIssue
                            {
                                Id = reader.GetInt64(0),
                                Summary = reader.GetString(1),
                                Description = reader.GetString(2)
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

    public class NonOpenIssue
    {
        public long Id { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
    }
}
