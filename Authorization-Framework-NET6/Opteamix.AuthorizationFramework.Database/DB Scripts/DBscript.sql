USE [master]
GO
/****** Object:  Database [AuthFramework]    Script Date: 06/12/2021 06:00:00 PM ******/
CREATE DATABASE [AuthFramework]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'authframework', FILENAME = N'D:\Documents\Secure 360\Database\authframework.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'authframework_log', FILENAME = N'D:\Documents\Secure 360\Database\authframework_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [AuthFramework] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AuthFramework].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AuthFramework] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AuthFramework] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AuthFramework] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AuthFramework] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AuthFramework] SET ARITHABORT OFF 
GO
ALTER DATABASE [AuthFramework] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [AuthFramework] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AuthFramework] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AuthFramework] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AuthFramework] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AuthFramework] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AuthFramework] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AuthFramework] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AuthFramework] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AuthFramework] SET  ENABLE_BROKER 
GO
ALTER DATABASE [AuthFramework] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AuthFramework] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AuthFramework] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AuthFramework] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AuthFramework] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AuthFramework] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AuthFramework] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AuthFramework] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [AuthFramework] SET  MULTI_USER 
GO
ALTER DATABASE [AuthFramework] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AuthFramework] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AuthFramework] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AuthFramework] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [AuthFramework] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [AuthFramework] SET QUERY_STORE = OFF
GO
USE [AuthFramework]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO


USE [AuthFramework]
GO
/****** Object:  Table [dbo].[Entity]    Script Date: 09-03-2021 10:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Entity](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DisplayName] [varchar](50) NOT NULL,
	[ModuleId] [int] NULL,
	[ParentPermissionId] [int] NULL,
	[Abbreviation] [varchar](50) NULL,
	[PermissionType] [int] NULL,
	[DisplayOrder] [int] NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Module]    Script Date: 09-03-2021 10:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Module](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Application_Id] [int] NOT NULL,
	[Name] [varchar](20) NOT NULL,
	[Description] [varchar](50) NULL,
	[Abbrevation] [varchar](10) NULL,
	[Code] [varchar](50) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Modules] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 09-03-2021 10:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](20) NOT NULL,
	[ShortName] [varchar](50) NULL,
	[RoleDescription] [varchar](100) NULL,
	[Application_Id] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Client]    Script Date: 09-03-2021 10:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Client](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[ShortName] [varchar](10) NOT NULL,
	[ClientStatus] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Client] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UK_Client_ShortName] UNIQUE NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EntityPrivilege]    Script Date: 09-03-2021 10:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EntityPrivilege](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Permission_Id] [int] NOT NULL,
	[Privilege_Id] [int] NOT NULL,
 CONSTRAINT [PK_PermissionPrivileges] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Application]    Script Date: 09-03-2021 10:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Application](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](20) NULL,
	[Abbreviation] [varchar](10) NOT NULL,
	[Code] [varchar](50) NULL,
	[Description] [varchar](50) NULL,
	[Active] [bit] NULL,
	[ClientId] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[LogoImage] varbinary(MAX) NULL,
	[LogoImageName] varchar(500) NULL,
	[LogoImageType] varchar(30) NULL
	CONSTRAINT [PK_Application] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleEntity]    Script Date: 09-03-2021 10:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleEntity](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Role_Id] [int] NOT NULL,
	[Permissionprivilege_Id] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RolePermission] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Privilege]    Script Date: 09-03-2021 10:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Privilege](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrivilegeName] [varchar](20) NOT NULL,
	[PrivilegeType] [varchar](10) NULL,
	[DisplayOrder] [int] NULL,
	[AppId] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Privileges] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_EntityRolePrivilege]    Script Date: 09-03-2021 10:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





--to fetch access for entities
CREATE VIEW [dbo].[vw_EntityRolePrivilege]
AS
select  ap.Abbreviation as ApplicationName, cl.ShortName as ClientName, ro.ShortName as RoleName,
en.Abbreviation + '_' + pr.PrivilegeName as Access 
from RoleEntity re
join EntityPrivilege ep
on ep.id = re.permissionprivilege_id
join Entity en
on en.id = ep.Permission_Id
join Module mo
on mo.Id = en.ModuleId
join Privilege pr
on pr.Id = ep.Privilege_Id
join role ro
on ro.Id = re.Role_Id
join Application ap
on ap.Id = ro.Application_Id
join Client cl
on cl.Id = ap.ClientId
where ap.IsDeleted = 0
GO
/****** Object:  Table [dbo].[Audit]    Script Date: 09-03-2021 10:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Audit](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Action] [varchar](50) NULL,
	[Entity] [varchar](50) NULL,
	[Data] [varchar](1000) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_dbo.Audit] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


ALTER TABLE [dbo].[Application] ADD  CONSTRAINT [DF_Application_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Client] ADD  CONSTRAINT [DF_Client_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Application]  WITH CHECK ADD  CONSTRAINT [FK_Application_Client] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Client] ([Id])
GO
ALTER TABLE [dbo].[Application] CHECK CONSTRAINT [FK_Application_Client]
GO
ALTER TABLE [dbo].[Entity]  WITH CHECK ADD  CONSTRAINT [FK_Entity_Entity] FOREIGN KEY([ParentPermissionId])
REFERENCES [dbo].[Entity] ([Id])
GO
ALTER TABLE [dbo].[Entity] CHECK CONSTRAINT [FK_Entity_Entity]
GO
ALTER TABLE [dbo].[Entity]  WITH CHECK ADD  CONSTRAINT [FK_Permissions_Modules] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Module] ([Id])
GO
ALTER TABLE [dbo].[Entity] CHECK CONSTRAINT [FK_Permissions_Modules]
GO
ALTER TABLE [dbo].[EntityPrivilege]  WITH CHECK ADD  CONSTRAINT [FK_PermissionPrivileges_Permissions] FOREIGN KEY([Permission_Id])
REFERENCES [dbo].[Entity] ([Id])
GO
ALTER TABLE [dbo].[EntityPrivilege] CHECK CONSTRAINT [FK_PermissionPrivileges_Permissions]
GO
ALTER TABLE [dbo].[EntityPrivilege]  WITH CHECK ADD  CONSTRAINT [FK_PermissionPrivileges_Privileges] FOREIGN KEY([Privilege_Id])
REFERENCES [dbo].[Privilege] ([Id])
GO
ALTER TABLE [dbo].[EntityPrivilege] CHECK CONSTRAINT [FK_PermissionPrivileges_Privileges]
GO
ALTER TABLE [dbo].[Module]  WITH CHECK ADD  CONSTRAINT [FK_Modules_Application] FOREIGN KEY([Application_Id])
REFERENCES [dbo].[Application] ([Id])
GO
ALTER TABLE [dbo].[Module] CHECK CONSTRAINT [FK_Modules_Application]
GO
ALTER TABLE [dbo].[Privilege]  WITH CHECK ADD  CONSTRAINT [FK_Privilege_Application] FOREIGN KEY([AppId])
REFERENCES [dbo].[Application] ([Id])
GO
ALTER TABLE [dbo].[Privilege] CHECK CONSTRAINT [FK_Privilege_Application]
GO
ALTER TABLE [dbo].[Role]  WITH CHECK ADD  CONSTRAINT [FK_Roles_Application] FOREIGN KEY([Application_Id])
REFERENCES [dbo].[Application] ([Id])
GO
ALTER TABLE [dbo].[Role] CHECK CONSTRAINT [FK_Roles_Application]
GO
ALTER TABLE [dbo].[RoleEntity]  WITH CHECK ADD  CONSTRAINT [FK_RolePermission_PermissionPrivilege] FOREIGN KEY([Permissionprivilege_Id])
REFERENCES [dbo].[EntityPrivilege] ([Id])
GO
ALTER TABLE [dbo].[RoleEntity] CHECK CONSTRAINT [FK_RolePermission_PermissionPrivilege]
GO
ALTER TABLE [dbo].[RoleEntity]  WITH CHECK ADD  CONSTRAINT [FK_RolePermission_Role] FOREIGN KEY([Role_Id])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[RoleEntity] CHECK CONSTRAINT [FK_RolePermission_Role]
GO


/****** Object:  Table [dbo].[User]    Script Date: 09-03-2021 10:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[FullName] [varchar](50) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Password] [varchar](600) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Userinfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[UserEntity]    Script Date: 09-03-2021 10:37:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserEntity](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[User_Id] [int] NOT NULL,
	[Manager_Id] [int] NULL,
	[Application_Id] [int] NULL,
	[RoleEntity_Id] [int] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_UserEntity] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserEntity]  WITH CHECK ADD  CONSTRAINT [FK_UserEntity_Application] FOREIGN KEY([Application_Id])
REFERENCES [dbo].[Application] ([Id])
GO
ALTER TABLE [dbo].[UserEntity] CHECK CONSTRAINT [FK_UserEntity_Application]
GO

ALTER TABLE [dbo].[UserEntity]  WITH CHECK ADD  CONSTRAINT [FK_UserEntity_User] FOREIGN KEY([User_Id])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[UserEntity] CHECK CONSTRAINT [FK_UserEntity_User]
GO

ALTER TABLE [dbo].[UserEntity]  WITH CHECK ADD  CONSTRAINT [FK_UserEntity_Manager] FOREIGN KEY([Manager_Id])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[UserEntity] CHECK CONSTRAINT [FK_UserEntity_Manager]
GO

ALTER TABLE [dbo].[UserEntity]  WITH CHECK ADD  CONSTRAINT [FK_UserEntity_Role] FOREIGN KEY([RoleEntity_Id])
REFERENCES [dbo].[RoleEntity] ([Id])
GO
ALTER TABLE [dbo].[UserEntity] CHECK CONSTRAINT [FK_UserEntity_Role]
GO
--  Added By Vasudev
ALTER TABLE [dbo].[Role] ADD IsDeleted bit null
Go
ALTER TABLE [dbo].[EntityPrivilege]  WITH CHECK ADD  CONSTRAINT [FK_EntityPrivilege_Module] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Module] ([Id])
GO
ALTER TABLE [dbo].[EntityPrivilege] CHECK CONSTRAINT [FK_EntityPrivilege_Module]
GO
--  Added By Bhoomika
ALTER TABLE [dbo].[Privilege] ADD IsDeleted bit null;
Go
ALTER TABLE [dbo].[Privilege] ADD Description varchar(500) null;
Go
--Added By Nishi
ALTER TABLE [dbo].[Application]
ADD LogoImage varbinary(MAX) null;
Go
ALTER TABLE [dbo].[Application]
ADD LogoImageName varchar(500) null;
Go
ALTER TABLE [dbo].[Application]
ADD  LogoImageType varchar(30) null;
Go

-- Added By Keerthi
ALTER TABLE [dbo].[Client]
ADD ContactPerson varchar(100) null;
GO

-- Altering Module table by Joshua 
ALTER TABLE [dbo].[Module]
DROP COLUMN Code
GO

ALTER TABLE [dbo].[Module]
EXEC sp_RENAME '[Module].Abbrevation','Abbreviation', 'COLUMN'
GO

ALTER TABLE [dbo].[Module]
EXEC sp_RENAME '[Module].Application_Id','ApplicationId', 'COLUMN'
GO
--Added By Helen
ALTER TABLE [dbo].[Client]
ADD ClientLogoImage varbinary(MAX) ;
GO
ALTER TABLE [dbo].[Client]
ADD ClientLogoName varchar(500);
GO
ALTER TABLE [dbo].[Client]
ADD  ClientLogoImageType varchar(30);
GO
ALTER TABLE [dbo].[Client]
ADD  AddressLine1 varchar(500) null;
GO
ALTER TABLE [dbo].[Client]
ADD  AddressLine2 varchar(500) null;
GO
ALTER TABLE [dbo].[Client]
ADD  City varchar(50) null;
GO
ALTER TABLE [dbo].[Client]
ADD  WebsiteAdress varchar(100) null;
GO
ALTER TABLE [dbo].[Client]
ADD  EmailAdress varchar(100) null;
GO
ALTER TABLE [dbo].[Client]
ADD  ContactPersonDetails varchar(200) null;
GO
ALTER TABLE [dbo].[Client]
ADD  ContactPersonEmailAddress varchar(100) null;
GO
ALTER TABLE [dbo].[Client]
ADD  ContactPersonPhoneNumber int null;
GO
ALTER TABLE [dbo].[Client]
ADD  AddressLine2 varchar(500) null;
GO
ALTER TABLE [dbo].[Client]
ADD  City varchar(50) null;
GO
ALTER TABLE [dbo].[Client]
ADD  WebsiteAdress varchar(100) null;
GO
ALTER TABLE [dbo].[Client]
ADD  EmailAdress varchar(100) null;
GO
ALTER TABLE [dbo].[Client]
ADD  ContactPersonDetails varchar(200) null;
GO
ALTER TABLE [dbo].[Client]
ADD  ContactPersonEmailAddress varchar(100) null;
GO
ALTER TABLE [dbo].[Client]
ADD  ContactPersonPhoneNumber int null;
GO

--Added by Keerthi
ALTER TABLE [dbo].[Client]
ALTER COLUMN Name varchar(100) NOT NULL;

--Added By Vasudeva NS
ALTER TABLE [dbo].[Role] ADD IsDeleted bit null;

--Added by Helen Harit
ALTER TABLE [dbo].[Module] ADD ModuleType int 

ALTER TABLE [dbo].[User] ADD Manager int null

ALTER TABLE [dbo].[User] ADD Role int 

ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Manager] FOREIGN KEY([ManagerId])
REFERENCES [dbo].[User] ([Id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Manager]
GO

ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([Id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO

ALTER TABLE [dbo].[User] ADD IsDeleted bit 

ALTER TABLE [dbo].[User] ADD ClientId int 

ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Client] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Client] ([Id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Client]
GO
--Nishikanta
ALTER table Module Alter COLUMN Name varchar (500);
GO
ALTER table Module Alter COLUMN Description varchar (500);
GO
ALTER table Module Alter COLUMN Abbreviation varchar (500);
GO
ALTER table Privilege Alter COLUMN PrivilegeName varchar (500);
GO
ALTER table Role Alter COLUMN RoleName varchar (500);
GO
ALTER table Role Alter COLUMN RoleDescription varchar (500);
GO
ALTER table Role Alter COLUMN ShortName varchar (500);
GO