USE [master]
GO
/****** Object:  Database [WEMA]    Script Date: 25/02/2022 4:44:24 pm ******/
CREATE DATABASE [WEMA]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'WEMA', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\WEMA.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'WEMA_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\WEMA_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [WEMA] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [WEMA].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [WEMA] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [WEMA] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [WEMA] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [WEMA] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [WEMA] SET ARITHABORT OFF 
GO
ALTER DATABASE [WEMA] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [WEMA] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [WEMA] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [WEMA] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [WEMA] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [WEMA] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [WEMA] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [WEMA] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [WEMA] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [WEMA] SET  DISABLE_BROKER 
GO
ALTER DATABASE [WEMA] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [WEMA] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [WEMA] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [WEMA] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [WEMA] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [WEMA] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [WEMA] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [WEMA] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [WEMA] SET  MULTI_USER 
GO
ALTER DATABASE [WEMA] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [WEMA] SET DB_CHAINING OFF 
GO
ALTER DATABASE [WEMA] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [WEMA] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [WEMA] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [WEMA] SET QUERY_STORE = OFF
GO
USE [WEMA]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 25/02/2022 4:44:24 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Password] [varchar](600) NOT NULL,
	[PhoneNo] [varchar](20) NOT NULL,
	[OTP] [varchar](20) NULL,
	[isOnboard] [bit] NULL,
	[Location] [int] NOT NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lga]    Script Date: 25/02/2022 4:44:24 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lga](
	[LgaId] [int] IDENTITY(1,1) NOT NULL,
	[StateId] [int] NOT NULL,
	[LgaName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Lga] PRIMARY KEY CLUSTERED 
(
	[LgaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[States]    Script Date: 25/02/2022 4:44:24 pm ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[States](
	[StateId] [int] IDENTITY(1,1) NOT NULL,
	[StateName] [varchar](20) NOT NULL,
 CONSTRAINT [PK_States] PRIMARY KEY CLUSTERED 
(
	[StateId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [FK_Customers_States] FOREIGN KEY([Location])
REFERENCES [dbo].[States] ([StateId])
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [FK_Customers_States]
GO
ALTER TABLE [dbo].[Lga]  WITH CHECK ADD  CONSTRAINT [FK_Lga_States] FOREIGN KEY([StateId])
REFERENCES [dbo].[States] ([StateId])
GO
ALTER TABLE [dbo].[Lga] CHECK CONSTRAINT [FK_Lga_States]
GO
USE [master]
GO
ALTER DATABASE [WEMA] SET  READ_WRITE 
GO
