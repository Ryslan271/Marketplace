using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KazanNewShop.Database
{
    public partial class DatabaseContext
    {
        private static DatabaseContext _entities = null!;
        public static DatabaseContext Entities => _entities ??= new DatabaseContext();

        public static bool LoadingFlag = false;

        /// <summary>
        /// Загрузка таблиц для регистрации и авторизации
        /// </summary>
        public static void LoadEntitesForAuthReg()
        {
            Entities.Clients.Load();
            Entities.Salesmen.Load();
            Entities.Employees.Load();
            Entities.Users.Load();
        }

        /// <summary>
        /// Загрузка таблиц для основной части
        /// </summary>
        public static void LoadEntitesForMarketplace()
        {
            Entities.Baskets.Load();
            Entities.ProductLists.Load();
            Entities.Products.Load();
            Entities.Categories.Load();
            Entities.PhotoProducts.Load();
            Entities.Orders.Load();
            Entities.Statuses.Load();
            Entities.PointOfIssues.Load();
            Entities.ProductListOrders.Load();

            LoadingFlag = true;
        }
    }
}
