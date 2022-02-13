# Miracle.FileZilla.Api

Managed API for FileZilla FTP server. Allows you to do basically the same as the FileZilla Server interface.
Target audience is everyone who wants to automate the administration of FileZilla server, particularly user/group management.

# Note! FileZilla server 1.x is NOT supported
This *not* my decision as the FileZilla team has made it almost impossible to maintain an API for the 1.x+ as the internal API will changes without notice. See [FileZilla Forums](https://forum.filezilla-project.org/viewtopic.php?f=6&t=54164&p=178144#p178147).

## Usage

Available as a NuGet package: [Miracle.FileZilla.Api](https://www.nuget.org/packages/Miracle.FileZilla.Api/)

## Features

* Get/Set Groups/Users using GetAccountSettings/SetAccountSettings methods.
* Get/Set server state using GetServerState/SetServerState methods.
* Get/Set server settings using GetSettings/SetSettings methods.
* Get active connections using GetConnections method.
* Kick connection using Kick method.
* Ban IP (and kick) using BanIp method.
* Use FileZillaServerProtocol for more advanced implemetations.

## FileZilla Server versions supported

All 0.9.x versions from 0.9.43+. 

1.x+ version is NOT supported. This not my decision as the FileZilla team has made it almost impossible to maintain an API for the 1.x+ as the internal API changes without  


### FileZilla Server versions tested during development

* 0.9.43 - Verified to work (Last FileZilla Server to support Windows XP/2003)
* 0.9.46 - First version supported
* 0.9.48 - Protocol changed to support 16M users
* 0.9.52 - Protocol changes mostly related to TLS
* 0.9.54 - User passwords hashed using Sha512
* 0.9.58 - UserControl size changed to 24 bit.
* 0.9.60 - No protocol changes.

Other versions than listed are supported provided that the FileZilla team has not changed the protocol version. An ApiException is thrown upon connect if that is the case. Let me know, and I will try to fix it.

## Example: Create user in 5 easy steps:

#### 1: Create instance of API

```csharp
// Using localhost and default port
var fileZillaApi = new FileZillaApi();

or
// Using specific server/port
var fileZillaApi = new FileZillaApi(IPAddress.Parse("192.168.0.42"), 54321);
```

#### 2: Connect to FileZilla server

```csharp
fileZillaApi.Connect(myAdminPassword);
```

#### 3: Get account settings which includes all users and groups

```csharp
var accountSettings = fileZillaApi.GetAccountSettings();
```

#### 4: Modify Users and Groups lists as desired (add/change/delete).

```csharp
var user = new User
{
	GroupName = "SomeGroup", // Reference to existing group
	UserName = "NewUser",
	Password = User.HashPassword("secret"),
	SharedFolders = new List<SharedFolder>()
	{
		new SharedFolder()
		{
			Directory = @"C:\Hello\World",
		}
	}
};
accountSettings.Users.Add(user);
```

#### 5: Save account settings. This will override all users and groups.

```csharp
fileZillaApi.SetAccountSettings(accountSettings);
```

## Complete example (C#):

```csharp
using (IFileZillaApi fileZillaApi = new FileZillaApi())
{
    fileZillaApi.Connect("FileZilla password");
    var accountSettings = fileZillaApi.GetAccountSettings();
    var user = new User
    {
        GroupName = "SomeGroup", // Reference to existing group
        UserName = "NewUser",
        SharedFolders = new List<SharedFolder>()
        {
            new SharedFolder()
            {
                Directory = @"C:\Hello\World",
                AccessRights = AccessRights.DirList | AccessRights.DirSubdirs | AccessRights.FileRead | AccessRights.FileWrite | AccessRights.IsHome
            }
        }
    };
		user.AssignPassword("secret", fileZillaApi.ProtocolVersion);
    accountSettings.Users.Add(user);
    fileZillaApi.SetAccountSettings(accountSettings);
}
```

## Complete example (VB.NET):

```vbnet
Using fileZillaApi As IFileZillaApi = New FileZillaApi()
    fileZillaApi.Connect("FileZilla password")
    Dim accountSettings = fileZillaApi.GetAccountSettings()
    ' Reference to existing group
    Dim user = New User()
    user.GroupName = "SomeGroup"
    user.UserName = "NewUser"
    user.SharedFolders = New List(Of SharedFolder)() From {
            New SharedFolder() With {
                .Directory = "C:\Hello\World"
				.AccessRights = AccessRights.DirList Or AccessRights.DirSubdirs Or AccessRights.FileRead Or AccessRights.FileWrite Or AccessRights.IsHome
            }
        }
    user.AssignPassword("secret", fileZillaApi.ProtocolVersion)
    accountSettings.Users.Add(user)
    fileZillaApi.SetAccountSettings(accountSettings)
End Using
```

Groups are managed just like Users using accountSettings.Groups.

#### See sample project for further information.
