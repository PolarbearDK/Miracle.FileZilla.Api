using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Miracle.FileZilla.Api.Elements;

namespace Miracle.FileZilla.Api.Samples
{
    internal class Program
    {
        private const string Ip = "127.0.0.1";
        private const int Port = 14242;
        private const string ServerPassword = "master";
        private const int MaxGroups = 1000;
        private const string GroupName = "MachineGroup";
        private const int MaxUsers = 10000;
        private const string UserName = "MachineUser";

        private static void Main(string[] args)
        {
            GetServerState();
            GetConnections();
            KickFirstConnection();
            CreateUserAndGroup();
            DeleteUser();
            CreateLotsOfUsersAndGroups();
            DeleteLotsOfUsersAndGroups();
            
            GetConnectionsLoop();
        }

        private static void GetServerState()
        {
            using (var fileZillaApi = new FileZillaApi(IPAddress.Parse(Ip), Port))
            {
                var stopWatch = Stopwatch2.StartNew();

#if DEBUG
                fileZillaApi.Trace = true;
#endif
                fileZillaApi.Connect(ServerPassword);
                var serverState = fileZillaApi.GetServerState();
                Console.WriteLine("Connected in {0}. State is {1}", stopWatch.GetDelta(), serverState);
            }
        }

        private static void GetConnections()
        {
            using (var fileZillaApi = new FileZillaApi(IPAddress.Parse(Ip), Port))
            {

                var stopWatch = Stopwatch2.StartNew();

#if DEBUG
                fileZillaApi.Trace = true;
#endif
                fileZillaApi.Connect(ServerPassword);
                var serverState = fileZillaApi.GetServerState();
                Console.WriteLine("Connected in {0}. State is {1}", stopWatch.GetDelta(), serverState);
                var conections = fileZillaApi.GetConnections();
                Console.WriteLine("Got {0} connections in {1}", conections.Count, stopWatch.GetDelta());
            }
        }

        /// <summary>
        /// Kick first connection
        /// </summary>
        private static void KickFirstConnection()
        {
            using (var fileZillaApi = new FileZillaApi(IPAddress.Parse(Ip), Port))
            {
#if DEBUG
                fileZillaApi.Trace = true;
#endif
                fileZillaApi.Connect(ServerPassword);

                var connections = fileZillaApi.GetConnections();
                if (connections.Any())
                {
                    if (fileZillaApi.Kick(connections.First().ConnectionId))
                        Console.WriteLine("Connection was kicked");
                    else
                        Console.WriteLine("Connection was NOT kicked");
                }
                else
                    Console.WriteLine("No connections available");
            }
        }

        private static void CreateUserAndGroup()
        {
            using (var fileZillaApi = new FileZillaApi(IPAddress.Parse(Ip), Port))
            {
                var stopWatch = Stopwatch2.StartNew();

#if DEBUG
                fileZillaApi.Trace = true;
#endif
                fileZillaApi.BufferSize = 100 * 1024 * 1024;

                fileZillaApi.Connect(ServerPassword);
                var serverState = fileZillaApi.GetServerState();
                Console.WriteLine("Connected in {0}. State is {1}", stopWatch.GetDelta(), serverState);
                
                var accountSettings = fileZillaApi.GetAccountSettings();
                Console.WriteLine("Account settings with {0} groups and {1} users fetched in {2}.",
                    accountSettings.Groups.Count,
                    accountSettings.Users.Count,
                    stopWatch.GetDelta());

                var group = new Group()
                {
                    GroupName = GroupName,
                    Permissions = new List<Permission>()
                    {
                        new Permission()
                        {
                            Directory = @"C:\Group\Shared",
                            AccessRights = AccessRights.DirList | AccessRights.DirSubdirs | AccessRights.FileRead | AccessRights.IsHome
                        }
                    },
                };
                accountSettings.Groups.RemoveAll(x => x.GroupName == GroupName);
                accountSettings.Groups.Add(@group);

                var user = new User
                {
                    GroupName = GroupName, // Reference to group
                    UserName = UserName,
                    Password = User.HashPassword("test42"),
                    Permissions = new List<Permission>()
                    {
                        new Permission()
                        {
                            Directory = @"C:\Hello\World",
                            AccessRights = AccessRights.DirList | AccessRights.DirSubdirs | AccessRights.FileRead | AccessRights.IsHome
                        }
                    },
                };
                accountSettings.Users.RemoveAll(x => x.UserName == UserName);
                accountSettings.Users.Add(user);

                Console.WriteLine("Created {0} groups and {1} users in {2}.",
                    1,
                    1,
                    stopWatch.GetDelta());

                fileZillaApi.SetAccountSettings(accountSettings);
                Console.WriteLine("Finished saving account settings in {0}.", stopWatch.GetDelta());
            }
        }

        private static void DeleteUser()
        {
            using (var fileZillaApi = new FileZillaApi(IPAddress.Parse(Ip), Port))
            {
#if DEBUG
                fileZillaApi.Trace = true;
#endif
                fileZillaApi.BufferSize = 100 * 1024 * 1024;

                var stopWatch = Stopwatch2.StartNew();
                
                fileZillaApi.Connect(ServerPassword);
                var serverState = fileZillaApi.GetServerState();
                Console.WriteLine("Connected in {0}. State is {1}", stopWatch.GetDelta(), serverState);

                var accountSettings = fileZillaApi.GetAccountSettings();
                Console.WriteLine("Account settings with {0} groups and {1} users fetched in {2}.",
                    accountSettings.Groups.Count,
                    accountSettings.Users.Count,
                    stopWatch.GetDelta());

                accountSettings.Users.RemoveAll(x => x.UserName == UserName);

                fileZillaApi.SetAccountSettings(accountSettings);
                Console.WriteLine("Finished saving account settings in {0}.", stopWatch.GetDelta());
            }
        }

        private static void CreateLotsOfUsersAndGroups()
        {
            using (var fileZillaApi = new FileZillaApi(IPAddress.Parse(Ip), Port))
            {
#if DEBUG
                fileZillaApi.Trace = true;
#endif
                fileZillaApi.BufferSize = 100 * 1024 * 1024;

                var stopWatch = Stopwatch2.StartNew();

                fileZillaApi.Connect(ServerPassword);
                var serverState = fileZillaApi.GetServerState();
                Console.WriteLine("Connected in {0}. State is {1}", stopWatch.GetDelta(), serverState);
                var accountSettings = fileZillaApi.GetAccountSettings();
                Console.WriteLine("Account settings with {0} groups and {1} users fetched in {2}.",
                    accountSettings.Groups.Count,
                    accountSettings.Users.Count,
                    stopWatch.GetDelta());

                for (int i = 0; i < MaxGroups; i++)
                {
                    var group = new Group()
                    {
                        GroupName = GroupName + i,
                        Permissions = new List<Permission>()
                        {
                            new Permission()
                            {
                                Directory = @"C:\Group" + i + @"\Shared",
                                AccessRights = AccessRights.DirList | AccessRights.DirSubdirs | AccessRights.FileRead | AccessRights.IsHome
                            }
                        },
                    };

                    accountSettings.Groups.RemoveAll(x=>x.GroupName == group.GroupName);
                    accountSettings.Groups.Add(group);
                }

                for (int i = 0; i < MaxUsers; i++)
                {
                    var user = new User
                    {
                        GroupName = GroupName + (i%MaxGroups), // Reference to group
                        UserName = UserName + i,
                        Password = User.HashPassword("LonglongPasswordwithnumber" + i),
                        Permissions = new List<Permission>()
                        {
                            new Permission()
                            {
                                Directory = @"C:\User" + i + @"\Private",
                                AccessRights = AccessRights.DirList | AccessRights.DirSubdirs | AccessRights.FileRead | AccessRights.IsHome
                            }
                        },
                    };
                    accountSettings.Groups.RemoveAll(x => x.GroupName == user.UserName);
                    accountSettings.Users.Add(user);
                }

                Console.WriteLine("Created {0} groups and {1} users in {2}.",
                    MaxGroups,
                    MaxUsers,
                    stopWatch.GetDelta());
 
                fileZillaApi.SetAccountSettings(accountSettings);
                Console.WriteLine("Finished saving account settings in {0}.", stopWatch.GetDelta());
            }
        }

        private static void DeleteLotsOfUsersAndGroups()
        {
            using (var fileZillaApi = new FileZillaApi(IPAddress.Parse(Ip), Port))
            {
#if DEBUG
                fileZillaApi.Trace = true;
#endif
                fileZillaApi.BufferSize = 100 * 1024 * 1024;

                var stopWatch = Stopwatch2.StartNew();

                fileZillaApi.Connect(ServerPassword);
                var serverState = fileZillaApi.GetServerState();
                Console.WriteLine("Connected in {0}. State is {1}", stopWatch.GetDelta(), serverState);

                var accountSettings = fileZillaApi.GetAccountSettings();
                Console.WriteLine("Account settings with {0} groups and {1} users fetched in {2}.",
                    accountSettings.Groups.Count,
                    accountSettings.Users.Count,
                    stopWatch.GetDelta());

                accountSettings.Users.RemoveAll(x => x.UserName.StartsWith(UserName));
                accountSettings.Groups.RemoveAll(x => x.GroupName.StartsWith(GroupName));

                fileZillaApi.SetAccountSettings(accountSettings);
                Console.WriteLine("Finished saving account settings in {0}.", stopWatch.GetDelta());
            }
        }

        private static void GetConnectionsLoop()
        {
            using (var fileZillaApi = new FileZillaApi(IPAddress.Parse(Ip), Port))
            {
#if DEBUG
                fileZillaApi.Trace = true;
#endif
                fileZillaApi.Connect(ServerPassword);
                while (true)
                {
                    Console.WriteLine("Polling");
                    var connections = fileZillaApi.GetConnections();
                    foreach (var connection in connections)
                    {
                        Console.WriteLine("{0} {1} {2}", connection.TransferMode, connection.PhysicalFile, connection.UserName);
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
        }
    }
}
