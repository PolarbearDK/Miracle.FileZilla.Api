Miracle.FileZilla.Api
=====================

Managed api for FileZilla FTP server. Primarily for automated user/group creation and deletion.

##Create user in 5 easy steps:

####1: Create api
```csharp
// Using localhost and default port
var fileZillaApi = new FileZillaApi();

or
// Using specific server/port
var fileZillaApi = new FileZillaApi(IPAddress.Parse("192.168.0.42"), 54321);
```

####2: Connect to FileZilla server
```csharp
fileZillaApi.Connect("FileZilla password");
```

####3: Get account settings including all users and groups
```csharp
var accountSettings = fileZillaApi.GetAccountSettings();
```

####4: Modify Users and Groups lists as desired (add/change/delete). 
```csharp
var user = new User
{
	GroupName = "SomeGroup", // Reference to group
	UserName = "NewUser",
	Password = User.HashPassword("secret"),
	Permissions = new List<Permission>()
	{
		new Permission()
		{
			Directory = @"C:\Hello\World",
		}
	}
};
accountSettings.Users.Add(user);
```
####5: Save account settings. This will override all users and groups. 
```csharp
fileZillaApi.SetAccountSettings(accountSettings);
```

##Complete example:
```csharp
using (var fileZillaApi = new FileZillaApi())
{
    // Optionally set buffer size
    fileZillaApi.BufferSize = 100*1024*1024;
    fileZillaApi.Connect("FileZilla password");
    var accountSettings = fileZillaApi.GetAccountSettings();
    var user = new User
    {
        GroupName = "SomeGroup", // Reference to group
        UserName = "NewUser",
        Password = User.HashPassword("secret"),
        Permissions = new List<Permission>()
        {
            new Permission()
            {
                Directory = @"C:\Hello\World",
            }
        }
    };
    accountSettings.Users.Add(user);
    fileZillaApi.SetAccountSettings(accountSettings);
}
```
####See sample project for further information.

##Other features
* Get server state using GetServerState()
* Get current connections using GetConnections()
* Kick connection using Kick(connectionId)
* Ban IP (and kick) using BanIp(connectionId)



##Note! 
* If you handle a lot of users, you will need to increase the size of the input buffer (BufferSize property)
* Miracle.FileZilla.Api was developed using FileZilla Server version 0.9.46 beta for Windows, and is only testet with that version.
