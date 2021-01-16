USE [master]
GO
/****** Object:  Database [Chat]    Script Date: 13-Jan-21 11:51:26 PM ******/
CREATE DATABASE [Chat]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Chat', FILENAME = N'E:\Database\Chat\Chat.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Chat_log', FILENAME = N'E:\Database\Chat\Chat_log.ldf' , SIZE = 2048KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Chat] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Chat].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Chat] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Chat] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Chat] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Chat] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Chat] SET ARITHABORT OFF 
GO
ALTER DATABASE [Chat] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Chat] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Chat] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Chat] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Chat] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Chat] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Chat] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Chat] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Chat] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Chat] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Chat] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Chat] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Chat] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Chat] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Chat] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Chat] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Chat] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Chat] SET RECOVERY FULL 
GO
ALTER DATABASE [Chat] SET  MULTI_USER 
GO
ALTER DATABASE [Chat] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Chat] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Chat] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Chat] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [Chat] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Chat', N'ON'
GO
USE [Chat]
GO
/****** Object:  Table [dbo].[tblLogedinStatus]    Script Date: 13-Jan-21 11:51:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblLogedinStatus](
	[LoginStatusId] [bigint] NOT NULL,
	[IpAddress] [varchar](50) NULL,
	[IsLoged] [bit] NOT NULL CONSTRAINT [DF_LogedinStatus_IsLoged]  DEFAULT ((1)),
	[UserId] [bigint] NOT NULL,
	[Date] [date] NOT NULL,
	[Time] [varchar](15) NOT NULL,
	[LogOutDate] [date] NULL,
	[LogOutTime] [varchar](15) NULL,
 CONSTRAINT [PK_LogedinStatus] PRIMARY KEY CLUSTERED 
(
	[LoginStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblMessage]    Script Date: 13-Jan-21 11:51:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblMessage](
	[ChatId] [bigint] NOT NULL,
	[SenderId] [bigint] NOT NULL,
	[ReceiverId] [bigint] NOT NULL,
	[Message] [varchar](max) NOT NULL,
	[IsEdit] [bit] NOT NULL CONSTRAINT [DF_tblMessage_IsEdit]  DEFAULT ((0)),
	[Date] [date] NOT NULL CONSTRAINT [DF_tblMessage_Date]  DEFAULT (getdate()),
	[Time] [varchar](15) NOT NULL,
	[LoginStatusId] [bigint] NULL,
	[IsDeleteFromReceiver] [bit] NOT NULL CONSTRAINT [DF_tblMessage_IsDeleteFromReceiver]  DEFAULT ((0)),
	[IsDeleteFromSender] [bit] NOT NULL CONSTRAINT [DF_tblMessage_IsDeleteFromSender]  DEFAULT ((0)),
 CONSTRAINT [PK_Message] PRIMARY KEY CLUSTERED 
(
	[ChatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblUser]    Script Date: 13-Jan-21 11:51:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblUser](
	[UserId] [bigint] NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](50) NULL,
	[Email] [varchar](50) NOT NULL,
	[CreateDate] [date] NOT NULL CONSTRAINT [DF_tblUser_CreateDate]  DEFAULT (getdate()),
	[ModifyDate] [date] NULL CONSTRAINT [DF_tblUser_ModifyDate]  DEFAULT (getdate()),
	[IpAddress] [varchar](50) NULL,
 CONSTRAINT [PK_tblUser] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [u_email] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
USE [master]
GO
ALTER DATABASE [Chat] SET  READ_WRITE 
GO
