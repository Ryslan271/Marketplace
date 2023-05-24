using KazanNewShop.DataTypes.Enums;
using System.Linq;

namespace KazanNewShop.Database.Models
{
    public partial class User
    {
        public UserRole Role
        {
            get
            {
                if (Client is not null)
                    return UserRole.Client;

                else if (Employee is not null)
                    return UserRole.Employee;

                return UserRole.Selesman;
            }
        }

        private Client? _client;
        public Client? Client => _client ??= DatabaseContext.Entities.Clients.FirstOrDefault(c => c.IdUserNavigation == this);

        private Employee? _employee;
        public Employee? Employee => _employee ??= DatabaseContext.Entities.Employees.FirstOrDefault(c => c.IdUserNavigation == this);

        private Salesman? _selesman;
        public Salesman? Selesman => _selesman ??= DatabaseContext.Entities.Salesmen.FirstOrDefault(c => c.IdUserNavigation == this);
    }
}
