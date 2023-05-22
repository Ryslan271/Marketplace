using Microsoft.EntityFrameworkCore;

namespace KazanNewShop.Database
{
    public partial class DatabaseContext
    {
        private static DatabaseContext _entities = null!;
        public static DatabaseContext Entities => _entities ??= new DatabaseContext();

        public static void LoadEntities()
        {
            Entities.Users.Load();
            Entities.Clients.Load();
            Entities.Employees.Load();
            Entities.Salesmen.Load();
        }
    }
}
