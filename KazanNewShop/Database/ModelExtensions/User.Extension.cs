using KazanNewShop.DataTypes.Enums;
using System.ComponentModel.DataAnnotations.Schema;

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
    }
}
