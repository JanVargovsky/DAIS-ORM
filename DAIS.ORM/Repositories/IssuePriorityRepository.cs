using DAIS.ORM.DTO;
using DAIS.ORM.Framework;

namespace DAIS.ORM.Repositories
{
    public class IssuePriorityRepository : RepositoryBase<IssuePriorityDTO>
    {
        public IssuePriorityRepository(IDatabase db) : base(db)
        {
        }
    }
}
