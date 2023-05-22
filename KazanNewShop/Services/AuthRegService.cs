using KazanNewShop.Database;
using KazanNewShop.Database.Models;
using KazanNewShop.DataTypes.Enums;
using System;
using System.Linq;

namespace KazanNewShop.Services
{
    public static class AuthRegService
    {
        /// <summary>
        /// Проверка на существование User
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        public static User? AuthorizeUser(string login, string password) =>
            DatabaseContext.Entities.Users.Local.FirstOrDefault(x => x.Login == login && x.Password == password);

        /// <summary>
        /// Создание нового User
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        public static bool RegisterUser(string login, string password)
        {
            if (DatabaseContext.Entities.Users.Local.Any(x => x.Login == login && x.Password == password) == true)
                return false;

            User user = new()
            {
                Login = login,
                Password = password
            };

            App.CarrentUser = user;

            return true;
        }

        /// <summary>
        /// Создание и заполнение новой сущности Client
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="username">Фамилия</param>
        /// <param name="patronymic">Отчество</param>
        public static void FilledClientData(string name, string username, string patronymic)
        {

            DatabaseContext.Entities.Users.Local.Add(App.CarrentUser);

            DatabaseContext.Entities.Clients.Local.Add(
                new Client()
                {
                    Name = name,
                    Username = username,
                    Patronymic = patronymic,
                    User = App.CarrentUser,
                    Removed = false
                });

            DatabaseContext.Entities.SaveChanges();
        }

        public static void FilledSalesmanData(string name, string username, string patronymic,
                                              string description, DateTime dateOnMarketplace, string organizationName)
        {
            DatabaseContext.Entities.Users.Local.Add(App.CarrentUser);

            DatabaseContext.Entities.Salesmen.Local.Add(
                new Salesman()
                {
                    Name = name,
                    Username = username,
                    Patronymic = patronymic,
                    DateOnMarketplace = dateOnMarketplace,
                    OrganizationName = organizationName,
                    Description = description,
                    User = App.CarrentUser,
                    Removed = false
                });

            DatabaseContext.Entities.SaveChanges();
        }
    }
}
