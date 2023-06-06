using System.ComponentModel.DataAnnotations.Schema;

namespace KazanNewShop.Database.Models
{
    public partial class Client
    {
        private string _fullname = null!;
        [NotMapped]
        public string Fullname
        {
            get => _fullname ??= Surname + " " + Name + " " + Patronymic;
            set
            {
                _fullname = value;
            }
        }
    }
}
