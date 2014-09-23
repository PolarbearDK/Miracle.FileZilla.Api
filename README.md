Miracle.FileZilla.Api
=====================

Managed api for FileZilla FTP server. Allows you to do basically the same as the FileZilla Server interface.
Target audience is everyone who wants to automate the administration of FileZilla server, particularly user/group management.

##Features
* Get/Set Groups/Users using GetAccountSettings/SetAccountSettings methods.
* Get/Set server state using GetServerState/SetServerState methods.
* Get/Set server settings using GetSettings/SetSettings methods.
* Get active connections using GetConnections method.
* Kick connection using Kick method.
* Ban IP (and kick) using BanIp method.
* Use FileZillaServerProtocol for more advanced implemetations.

##Example: Create user in 5 easy steps:

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

####3: Get account settings which includes all users and groups
```csharp
var accountSettings = fileZillaApi.GetAccountSettings();
```

####4: Modify Users and Groups lists as desired (add/change/delete). 
```csharp
var user = new User
{
	GroupName = "SomeGroup", // Reference to existing group
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
using (IFileZillaApi fileZillaApi = new FileZillaApi())
{
    // Optionally set buffer size
    fileZillaApi.BufferSize = 100*1024*1024;
    fileZillaApi.Connect("FileZilla password");
    var accountSettings = fileZillaApi.GetAccountSettings();
    var user = new User
    {
        GroupName = "SomeGroup", // Reference to existing group
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

Groups are managed just like Users using accountSettings.Groups.

####See sample project for further information.

##Note! 
* If you handle a lot of users, you will need to increase the size of the input buffer (BufferSize property), otherwise an ApiException is thrown.
* Miracle.FileZilla.Api was developed using FileZilla Server version 0.9.46 beta for Windows, and is only testet with that version. 
