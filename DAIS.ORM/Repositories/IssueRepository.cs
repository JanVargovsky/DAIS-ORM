using DAIS.ORM.DTO;
using DAIS.ORM.Framework;
using System;
using System.Collections.Generic;
using System.Data;
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

        public bool UpdateIssueStatus(long issueId, long userId, string statusName, string commentText, TimeSpan spentTime)
        {
            return UpdateIssueStatus(issueId, userId, statusName, commentText, spentTime.Ticks);
        }

        public bool UpdateIssueStatus(long issueId, long userId, string statusName, string commentText, long spentTime)
        {
            try
            {
                database.Open();
                using (var command = database.CreateSqlCommand("UpdateIssueStatus"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@issueId", issueId);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@statusName", statusName);
                    command.Parameters.AddWithValue("@commentText", commentText);
                    command.Parameters.AddWithValue("@spentTime", spentTime);

                    int result = command.ExecuteNonQuery();

                    return result == 3;
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

        public bool LogWork(long issueId, long userId, TimeSpan spentTime, string commentText = null)
        {
            return LogWork(issueId, userId, spentTime.Ticks, commentText);
        }

        public bool LogWork(long issueId, long userId, long spentTime, string commentText = null)
        {
            bool containsComment = !string.IsNullOrEmpty(commentText);

            try
            {
                database.Open();
                using (var command = database.CreateSqlCommand("LogWorkOnIssue"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@issueId", issueId);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@spentTime", spentTime);
                    if (containsComment)
                        command.Parameters.AddWithValue("@commentText", commentText);

                    int result = command.ExecuteNonQuery();

                    return result == 3 && containsComment || result == 2 && !containsComment;
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

        public bool CloseIssues(long issueId, long userId, string commentText)
        {
            try
            {
                database.Open();
                using (var command = database.CreateSqlCommand("CloseIssues"))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@issueId", issueId);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@commentText", commentText);

                    int result = command.ExecuteNonQuery();

                    return result > 2;
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
