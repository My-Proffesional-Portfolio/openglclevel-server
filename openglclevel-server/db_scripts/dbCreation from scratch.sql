/****** Object:  Table [dbo].[MealEventItems]    Script Date: 17/01/2023 05:20:20 p. m. ******/
USE TEST_DB

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MealEventItems](
	[Id] [uniqueidentifier] NOT NULL,
	[MealEventId] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[MealItemID] [uniqueidentifier] NOT NULL,
	[Unit] [int] NOT NULL,
 CONSTRAINT [PK_MealEventItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MealEvents]    Script Date: 17/01/2023 05:20:20 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MealEvents](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[MealDate] [datetime] NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[GlcLevel] [int] NOT NULL,
	[Notes] [nvarchar](max) NULL,
	[MealAtDay] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_MealEvent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MealItems]    Script Date: 17/01/2023 05:20:20 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MealItems](
	[Id] [uniqueidentifier] NOT NULL,
	[MealName] [nvarchar](max) NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_MealItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 17/01/2023 05:20:20 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](max) NOT NULL,
	[HashedPassword] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NULL,
	[Salt] [nvarchar](max) NOT NULL,
	[UserNumber] [bigint] IDENTITY(1000,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[FirstName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[MealEventItems]  WITH CHECK ADD FOREIGN KEY([MealEventId])
REFERENCES [dbo].[MealEvents] ([Id])
GO
ALTER TABLE [dbo].[MealEventItems]  WITH CHECK ADD FOREIGN KEY([MealItemID])
REFERENCES [dbo].[MealItems] ([Id])
GO
ALTER TABLE [dbo].[MealEvents]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[MealItems]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([Id])
GO
