using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.DataTypes.Enums;
using System;
using System.Linq;

namespace KazanNewShop.Services
{
    public static class AuthRegService
    {
        #region Проверка на существование User

        /// <summary>
        /// Проверка на существование User
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        public static User? AuthorizeUser(string login, string password) =>
            DatabaseContext.Entities.Users.Local.FirstOrDefault(u => u.Login == login && u.Password == password && u.Removed == false);

        #endregion

        #region Создание нового User

        /// <summary>
        /// Создание нового User
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        public static bool RegisterUser(string login, string password)
        {
            if (DatabaseContext.Entities.Users.Local.Any(x => x.Login == login) == true)
                return false;

            User user = new()
            {
                Login = login,
                Password = password,
                Removed = false
            };

            DatabaseContext.Entities.Users.Local.Add(user);

            DatabaseContext.Entities.SaveChanges();

            App.CurrentUser = DatabaseContext.Entities.Users.Local.LastOrDefault()!;

            return true;
        }
        #endregion

        #region Заполнение данных в сущности Client, Salesman

        /// <summary>
        /// Создание и заполнение новой сущности Client
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="surname">Фамилия</param>
        /// <param name="patronymic">Отчество</param>
        public static void FillClientData(string name, string surname, string patronymic)
        {
            Client client = new()
            {
                Name = name,
                Surname = surname,
                Patronymic = patronymic,
                User = App.CurrentUser
            };

            DatabaseContext.Entities.Clients.Local.Add(client);

            DatabaseContext.Entities.SaveChanges();
        }

        public static void FillSalesmanData(string description, DateTime dateOnMarketplace, string companyName)
        {
            Salesman salesman = new()
            {
                DateOnMarketplace = dateOnMarketplace,
                NameCompany = companyName,
                Description = description,
                User = App.CurrentUser
            };

            DatabaseContext.Entities.Salesmen.Local.Add(salesman);

            DatabaseContext.Entities.SaveChanges();
        }
        #endregion
    }
}
