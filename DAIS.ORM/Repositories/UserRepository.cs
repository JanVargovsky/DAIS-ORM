using DAIS.ORM.DTO;
using DAIS.ORM.Framework;

namespace DAIS.ORM.Repositories
{
    public class UserRepository : RepositoryBase<UserDTO>
    {
        public UserRepository(IDatabase db) : base(db)
        {
        }
    }
}
