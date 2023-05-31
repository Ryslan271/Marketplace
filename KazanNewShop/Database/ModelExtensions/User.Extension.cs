using KazanNewShop.DataTypes.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace KazanNewShop.Database.Models
{
    public partial class User
    {
        [NotMapped]
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
        [NotMapped]
        public Client? Client
        {
            get => _client;
            set
            {
                _client = value;
            }
        }

        private Employee? _employee;
        [NotMapped]
        public Employee? Employee
        {
            get => _employee;
            set
            {
                _employee = value;
            }
        }

        private Salesman? _salesman;
        [NotMapped]
        public Salesman? Salesman
        {
            get => _salesman;
            set
            {
                _salesman = value;
            }
        }
    }
}
