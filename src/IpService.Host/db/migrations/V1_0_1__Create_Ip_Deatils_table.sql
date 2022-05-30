USE [ipservice]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IpDetails](
    [Ip] [varchar](15) PRIMARY KEY,
    [City] [varchar](150) NOT NULL,
    [Country] [varchar](150) NOT NULL,
    [Continent] [varchar](150) NOT NULL,
    [Longitude] [float] NOT NULL,
    [Latitude] [float] NOT NULL
    )
GO