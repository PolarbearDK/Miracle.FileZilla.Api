using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Miracle.FileZilla.Api.Test
{
    [TestFixture]
    public class PasswordTests
    {
        /// <summary>
        /// Test requires user named Foo with password Bar
        /// </summary>
        [Test]
        [Category("Integration")]
        public void PasswordTest()
        {
            const string UserName = "Foo";
            const string Password = "Bar";

            using (IFileZillaApi fileZillaApi = new FileZillaApi() {Log = Console.Out})
            {
                fileZillaApi.Connect("");

                var accountSettings = fileZillaApi.GetAccountSettings();

                var user = accountSettings.Users.Find(x => x.UserName == UserName);
                Assert.That(user, Is.Not.Null);

                Console.WriteLine("User.Password({0}):{1}", user.Password.Length, user.Password);
                Console.WriteLine("Salt({0}):{1}", user.Salt.Length, user.Salt);

                var password = User.HashPasswordSha512(Password, user.Salt);
                Console.WriteLine("Calculated password ({0}):{1}", password.Length, password);

                Assert.That(password, Is.EqualTo(user.Password));
            }
        }
        /// <summary>
        /// </summary>
        [Test]
        [Category("Integration")]
        public void UserPasswordTest()
        {
            const string UserName = "TestUser";
            const string Password = "TestPassword";

            using (IFileZillaApi fileZillaApi = new FileZillaApi() { Log = Console.Out })
            {
                fileZillaApi.Connect("");

                var accountSettings = fileZillaApi.GetAccountSettings();

                var user = new User
                {
                    UserName = UserName,
                    SharedFolders = new List<SharedFolder>()
                    {
                        new SharedFolder()
                        {
                            Directory = @"C:\Foo\Bar",
                            AccessRights = AccessRights.DirList | AccessRights.DirSubdirs | AccessRights.FileRead | AccessRights.FileWrite | AccessRights.IsHome
                        }
                    }
                };
                user.AssignPassword(Password, fileZillaApi.ProtocolVersion);

                Console.WriteLine("User.Password({0}):{1}", user.Password.Length, user.Password);
                Console.WriteLine("Salt({0}):{1}", user.Salt.Length, user.Salt);

                accountSettings.Users.RemoveAll(x => x.UserName == UserName);
                accountSettings.Users.Add(user);
                fileZillaApi.SetAccountSettings(accountSettings);

                accountSettings = fileZillaApi.GetAccountSettings();

                user = accountSettings.Users.Find(x => x.UserName == UserName);
                Assert.That(user, Is.Not.Null);

                Console.WriteLine("User.Password({0}):{1}", user.Password.Length, user.Password);
                Console.WriteLine("Salt({0}):{1}", user.Salt.Length, user.Salt);

                var password = User.HashPasswordSha512(Password, user.Salt);
                Console.WriteLine("Calculated password ({0}):{1}", password.Length, password);

                Assert.That(password, Is.EqualTo(user.Password));
            }
        }
    }
}
