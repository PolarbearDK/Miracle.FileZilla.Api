using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Miracle.FileZilla.Api.Samples
{
    internal class Program
    {
        private const string Ip = "127.0.0.1";
        private const int Port = 14147;
        private const string ServerPassword = "master";
        private const int MaxGroups = 1000;
        private const string GroupName = "MachineGroup";
        private const int MaxUsers16M = 10000;
        private const int MaxUsers64K = 10000;
        private const string UserName = "MachineUser";

        private static readonly DebugTextWriter DebugLog = new DebugTextWriter();

        private static void Main(string[] args)
        {
            GetConnections();
            KickFirstConnection();
            GetServerState();
            SetServerState();
            GetSettings();
            SetSettings();
            CreateUserAndGroup();
            DeleteUser();
            CreateLotsOfUsersAndGroups();
            DeleteLotsOfUsersAndGroups();
            ReuseConnection();
            GetMessagesLoop();
        }

        private static void GetServerState()
        {
            Console.WriteLine("\n-- {0} --", MethodBase.GetCurrentMethod().Name);
            using (IFileZillaApi fileZillaApi = new FileZillaApi(IPAddress.Parse(Ip), Port) { Log = DebugLog })
            {
                var stopWatch = Stopwatch2.StartNew();

                fileZillaApi.Connect(ServerPassword);
                var serverState = fileZillaApi.GetServerState();
                Console.WriteLine("Connected in {0}. State is {1}", stopWatch.GetDelta(), serverState);
            }
        }

        private static void SetServerState()
        {
            Console.WriteLine("\n-- {0} --", MethodBase.GetCurrentMethod().Name);
            using (IFileZillaApi fileZillaApi = new FileZillaApi(IPAddress.Parse(Ip), Port) { Log = DebugLog })
            {
                fileZillaApi.Connect(ServerPassword);
                var serverState = fileZillaApi.GetServerState();
                Console.WriteLine("State is {0}", serverState);

                // Go offiline
                serverState = fileZillaApi.SetServerState(ServerState.GoOfflineNow);
                Console.WriteLine("GoOfflineNow State is {0}", serverState);
                Thread.Sleep(TimeSpan.FromSeconds(5));
                serverState = fileZillaApi.GetServerState();
                Console.WriteLine("State is {0}", serverState);

                // Go online
                serverState = fileZillaApi.SetServerState(ServerState.Online);
                Console.WriteLine("State is {0}", serverState);

                // Lock server
                serverState = fileZillaApi.SetServerState(ServerState.Online | ServerState.Locked);
                Console.WriteLine("Lock State is {0}", serverState);

                // Unlock
                serverState = fileZillaApi.SetServerState(ServerState.Online);
                Console.WriteLine("State is {0}", serverState);
            }
        }

        private static void GetSettings()
        {
            Console.WriteLine("\n-- {0} --", MethodBase.GetCurrentMethod().Name);
            using (IFileZillaApi fileZillaApi = new FileZillaApi(IPAddress.Parse(Ip), Port) { Log = DebugLog })
            {
                var stopWatch = Stopwatch2.StartNew();

                fileZillaApi.Connect(ServerPassword);
                var settings = fileZillaApi.GetSettings();
                Console.WriteLine("Settings retrieved in {0}.", stopWatch.GetDelta());
                int optionNumber = 0;
                foreach (var o in settings.Options)
                {
                    Console.WriteLine("  {0} = {1}",
                        o.Label,
                        o.OptionType == OptionType.Text
                            ? o.TextValue
                            : o.NumericValue.ToString()
                        );
                    optionNumber++;
                }
            }
        }

        private static void SetSettings()
        {
            Console.WriteLine("\n-- {0} --", MethodBase.GetCurrentMethod().Name);
            using (IFileZillaApi fileZillaApi = new FileZillaApi(IPAddress.Parse(Ip), Port) { Log = DebugLog })
            {
                var stopWatch = Stopwatch2.StartNew();

                fileZillaApi.Connect(ServerPassword);
                var settings = fileZillaApi.GetSettings();
                Console.WriteLine("Settings retrieved in {0}.", stopWatch.GetDelta());

                // Select option to modify
                var option = settings.GetOption(OptionId.WELCOMEMESSAGE);

                // Modify
                string originalTextValue = option.TextValue;
                const string newMessage = "Hello world";
                option.TextValue = newMessage;

                // Save
                if (!fileZillaApi.SetSettings(settings)) throw new Exception("Uh uh");
                var settings2 = fileZillaApi.GetSettings();

                // Verify 
                if (settings.Options.Count() != settings2.Options.Count()) throw new Exception("Uh uh");
                for (int i = 0; i < settings.Options.Count(); i++)
                {
                    if (settings.Options[i].Label != settings2.Options[i].Label) throw new Exception("Uh uh");
                    if (settings.Options[i].NotRemotelyChangeable != settings2.Options[i].NotRemotelyChangeable) throw new Exception("Uh uh");
                    if (settings.Options[i].OptionType != settings2.Options[i].OptionType) throw new Exception("Uh uh");
                    if (settings.Options[i].NumericValue != settings2.Options[i].NumericValue)
                    {
                        // Numeric value of "No Transfer Timeout" is bumped up to 600 by the server. Ignore that.
                        if (!(settings.Options[i].Label == "No Transfer Timeout" && settings.Options[i].NumericValue < 600 && settings2.Options[i].NumericValue == 600))
                            throw new Exception("Uh uh");
                    }

                    if (settings.Options[i].TextValue != settings2.Options[i].TextValue)
                    {
                        // Admin Password is sent as "*" when not set
                        if (!(settings.Options[i].Label == "Admin Password" && settings.Options[i].TextValue == "*" && settings2.Options[i].TextValue == null))
                            throw new Exception("Uh uh");
                    }
                }

                // Restore 
                settings.GetOption(OptionId.WELCOMEMESSAGE).TextValue = originalTextValue;
                fileZillaApi.SetSettings(settings);
            }
        }

        private static void GetConnections()
        {
            Console.WriteLine("\n-- {0} --", MethodBase.GetCurrentMethod().Name);
            using (IFileZillaApi fileZillaApi = new FileZillaApi(IPAddress.Parse(Ip), Port) { Log = DebugLog })
            {

                var stopWatch = Stopwatch2.StartNew();

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
            Console.WriteLine("\n-- {0} --", MethodBase.GetCurrentMethod().Name);
            using (IFileZillaApi fileZillaApi = new FileZillaApi(IPAddress.Parse(Ip), Port) { Log = DebugLog })
            {
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
            Console.WriteLine("\n-- {0} --", MethodBase.GetCurrentMethod().Name);
            using (IFileZillaApi fileZillaApi = new FileZillaApi(IPAddress.Parse(Ip), Port) { Log = DebugLog })
            {
                var stopWatch = Stopwatch2.StartNew();

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
                    SharedFolders = new List<SharedFolder>()
                    {
                        new SharedFolder()
                        {
                            Directory = @"C:\Group\Shared",
                            AccessRights = AccessRights.DirList | AccessRights.DirSubdirs | AccessRights.FileRead | AccessRights.IsHome
                        },
                        new SharedFolder()
                        {
                            Directory = @"C:\foo\bar",
                            AccessRights = AccessRights.DirList | AccessRights.DirSubdirs | AccessRights.FileRead
                        }
                    },
                    AllowedIPs = new List<string>() { "127.0.0.1", "10.10.10.10", "42.42.42.42", "::1" },
                    DisallowedIPs = new List<string>() { "172.0.0.0" },
                    ForceSsl = TriState.Yes,
                    Comment = "The quick brown fox jumps over the lazy dog",
                    BypassUserLimit = TriState.No,
                };
                accountSettings.Groups.RemoveAll(x => x.GroupName == GroupName);
                accountSettings.Groups.Add(@group);

                var user = new User
                {
                    GroupName = GroupName, // Reference to group
                    UserName = UserName,
                    SharedFolders = new List<SharedFolder>()
                    {
                        new SharedFolder()
                        {
                            Directory = @"C:\UserX\Home",
                            AccessRights = AccessRights.DirList | AccessRights.DirSubdirs | AccessRights.FileRead | AccessRights.FileWrite | AccessRights.IsHome
                        },
                        new SharedFolder()
                        {
                            Directory = @"C:\Shared\foo\bar",
                            AccessRights = AccessRights.DirList | AccessRights.DirSubdirs | AccessRights.FileRead
                        }
                    }
                };
                user.AssignPassword("test42", fileZillaApi.ProtocolVersion);
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
            Console.WriteLine("\n-- {0} --", MethodBase.GetCurrentMethod().Name);
            using (IFileZillaApi fileZillaApi = new FileZillaApi(IPAddress.Parse(Ip), Port) { Log = DebugLog })
            {
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
            Console.WriteLine("\n-- {0} --", MethodBase.GetCurrentMethod().Name);
            using (IFileZillaApi fileZillaApi = new FileZillaApi(IPAddress.Parse(Ip), Port) { Log = DebugLog })
            {
                var stopWatch = Stopwatch2.StartNew();

                fileZillaApi.Connect(ServerPassword);
                var serverState = fileZillaApi.GetServerState();
                Console.WriteLine("Connected in {0}. State is {1}", stopWatch.GetDelta(), serverState);
                var accountSettings = fileZillaApi.GetAccountSettings();
                Console.WriteLine("Account settings with {0} groups and {1} users fetched in {2}.",
                    accountSettings.Groups.Count,
                    accountSettings.Users.Count,
                    stopWatch.GetDelta());

                accountSettings.Groups.RemoveAll(x => x.GroupName.StartsWith(GroupName));
                accountSettings.Users.RemoveAll(x => x.UserName.StartsWith(UserName));

                for (var i = 0; i < MaxGroups; i++)
                {
                    var group = new Group()
                    {
                        GroupName = GroupName + i,
                        SharedFolders = new List<SharedFolder>()
                        {
                            new SharedFolder()
                            {
                                Directory = @"C:\Group" + i + @"\Shared",
                                AccessRights = AccessRights.DirList | AccessRights.DirSubdirs | AccessRights.FileRead | AccessRights.IsHome
                            }
                        },
                    };

                    accountSettings.Groups.Add(group);
                }

                var maxUsers = fileZillaApi.ProtocolVersion < ProtocolVersions.User16M ? MaxUsers64K : MaxUsers16M;
                for (var i = 0; i < maxUsers; i++)
                {
                    var user = new User
                    {
                        GroupName = GroupName + (i % MaxGroups), // Reference to group
                        UserName = UserName + i,
                        SharedFolders = new List<SharedFolder>()
                        {
                            new SharedFolder()
                            {
                                Directory = @"C:\User" + i + @"\Private",
                                AccessRights = AccessRights.DirList | AccessRights.DirSubdirs | AccessRights.FileRead | AccessRights.IsHome
                            }
                        },
                    };
                    user.AssignPassword("LonglongPasswordwithnumber" + i, fileZillaApi.ProtocolVersion);

                    accountSettings.Users.Add(user);
                }

                Console.WriteLine("Created {0} groups and {1} users in {2}.",
                    MaxGroups,
                    maxUsers,
                    stopWatch.GetDelta());

                fileZillaApi.SetAccountSettings(accountSettings);
                Console.WriteLine("Finished saving account settings in {0}.", stopWatch.GetDelta());
            }
        }

        private static void DeleteLotsOfUsersAndGroups()
        {
            Console.WriteLine("\n-- {0} --", MethodBase.GetCurrentMethod().Name);
            using (IFileZillaApi fileZillaApi = new FileZillaApi(IPAddress.Parse(Ip), Port) { Log = DebugLog })
            {
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

        private static void ReuseConnection()
        {
            Console.WriteLine("\n-- {0} --", MethodBase.GetCurrentMethod().Name);
            using (IFileZillaApi fileZillaApi = new FileZillaApi(IPAddress.Parse(Ip), Port) { Log = DebugLog })
            {
                var stopWatch = Stopwatch2.StartNew();

                fileZillaApi.Connect(ServerPassword);
                var serverState = fileZillaApi.GetServerState();
                Console.WriteLine("Connected in {0}. State is {1}", stopWatch.GetDelta(), serverState);

                // Get to _socket variable using reflection.
                var field= typeof(SocketCommunication).GetField("_socket", BindingFlags.NonPublic | BindingFlags.Instance);
                var socket = (Socket)field.GetValue(fileZillaApi);
                // Disconnect socket to simulate that connection has been broken.
                socket.Disconnect(false);

                if(fileZillaApi.IsConnected) throw new ApplicationException("Hmm, it is supposed to be disconnected");
                fileZillaApi.Connect(ServerPassword);
                serverState = fileZillaApi.GetServerState();
                Console.WriteLine("Connected in {0}. State is {1}", stopWatch.GetDelta(), serverState);
            }
        }

        private static void GetMessagesLoop()
        {
            Console.WriteLine("\n-- {0} --", MethodBase.GetCurrentMethod().Name);
            using (var serverProtocol = new FileZillaServerProtocol(IPAddress.Parse(Ip), Port) { Log = DebugLog })
            {
                serverProtocol.Connect(ServerPassword);
                Console.WriteLine("Listening to FileZilla server. Connect to server with FTP client now... (CTRL-C to exit)");
                Console.WriteLine("Causing a deliberate error to see error response");
                serverProtocol.SendCommand((MessageType)42); // Send deliberate unknown command
                while (true)
                {
                    serverProtocol.SendCommand(MessageType.Loopback);
                    var messages = serverProtocol.ReceiveMessages();
                    foreach (var message in messages)
                    {
                        switch (message.MessageType)
                        {
                            case MessageType.Loopback:
                                break;
                            case MessageType.Error:
                                var error = (Error)message.Body;
                                Console.WriteLine("Error Message:{0} TextType:{1}", error.Message, error.TextType);
                                break;
                            default:
                                Console.WriteLine("{0} {1} ({2}){3}",
                                    message.MessageOrigin,
                                    message.MessageType,
                                    message.Body != null ? message.Body.GetType().Name : "null",
                                    message.Body != null ? JsonConvert.SerializeObject(message.Body) : "");
                                break;
                        }
                    }

					Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }
        }
    }
}
