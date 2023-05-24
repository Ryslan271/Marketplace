using KazanNewShop.Database;
using KazanNewShop.Database.Models;
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
            if (DatabaseContext.Entities.Users.Local.Any(x => x.Login == login && x.Password == password) == true)
                return false;

            User user = new()
            {
                Login = login,
                Password = password,
                Removed = false
            };

            App.CarrentUser = user;

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
        public static void FilledClientData(string name, string surname, string patronymic)
        {

            DatabaseContext.Entities.Users.Local.Add(App.CarrentUser);

            DatabaseContext.Entities.Clients.Local.Add(
                new Client()
                {
                    Name = name,
                    Surname = surname,
                    Patronymic = patronymic,
                    IdUserNavigation = App.CarrentUser
                });

            DatabaseContext.Entities.SaveChanges();
        }

        public static void FilledSalesmanData(string description, DateTime dateOnMarketplace, string companyName)
        {
            DatabaseContext.Entities.Users.Local.Add(App.CarrentUser);

            DatabaseContext.Entities.Salesmen.Local.Add(
                new Salesman()
                {
                    DateOnMarketplace = dateOnMarketplace,
                    NameCompany = companyName,
                    Description = description,
                    IdUserNavigation = App.CarrentUser
                });

            DatabaseContext.Entities.SaveChanges();
        }
        #endregion
    }
}
