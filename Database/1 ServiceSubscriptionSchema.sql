/****** Object:  Table [dbo].[MyServices]    Script Date: 8/23/2019 8:21:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MyServices](
	[ServiceId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Amount] [decimal](18, 2) NULL,
	[ServiceTypeId] [int] NULL,
	[AddedOn] [datetime] NULL CONSTRAINT [DF_MyServices_AddedOn]  DEFAULT (getdate()),
 CONSTRAINT [PK_MyServices] PRIMARY KEY CLUSTERED 
(
	[ServiceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ServiceType]    Script Date: 8/23/2019 8:21:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceType](
	[ServiceTypeId] [int] IDENTITY(1,1) NOT NULL,
	[ServiceTypeName] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_Table_2] PRIMARY KEY CLUSTERED 
(
	[ServiceTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 8/23/2019 8:21:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](250) NULL,
	[LastName] [nvarchar](250) NULL,
	[UserName] [nvarchar](250) NULL,
	[Password] [nvarchar](250) NULL,
	[Role] [nvarchar](50) NULL,
	[EmailId] [nvarchar](50) NULL,
	[UnSubscribe] [bit] NULL CONSTRAINT [DF_User_UnSubscribe]  DEFAULT ((0)),
	[PasswordHash] [varbinary](64) NULL,
	[PasswordSalt] [varbinary](128) NULL,
 CONSTRAINT [PK_dbo.User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[usp_ServiceSubscription_AddUser]    Script Date: 8/23/2019 8:21:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_ServiceSubscription_AddUser] 
@FirstName NVARCHAR(250),
@LastName NVARCHAR(250),
@UserName NVARCHAR(250),
@Email NVARCHAR(50),
@PasswordHash VARBINARY(64),
@PasswordSalt VARBINARY(128),
@Role NVARCHAR(50)
AS
BEGIN
			INSERT INTO [dbo].[User]  
				  (
					  FirstName,
					  LastName,
					  UserName,
					  [Role],
					  EmailId,
					  PasswordHash,
					  PasswordSalt
				  )
				  VALUES
				  (
					 @FirstName,
					 @LastName,
					 @UserName,
					 @Role,
					 @Email,
					 @PasswordHash,
					 @PasswordSalt
				  )
END
GO
/****** Object:  StoredProcedure [dbo].[usp_ServiceSubscription_AddUserSubscription]    Script Date: 8/23/2019 8:21:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[dbo].[usp_ServiceSubscription_AddUserSubscription] 1,0
CREATE PROCEDURE [dbo].[usp_ServiceSubscription_AddUserSubscription] 
@UserId INT,
@SubscribeType INT,
@SubscriptionId INT OUTPUT
AS
BEGIN
			INSERT INTO [dbo].[MyServices]  
				  (
					  UserId,
					  Amount,
					  ServiceTypeId			  
				  )
				  VALUES
				  (
					  @UserId,				
					  CASE WHEN @SubscribeType = 1 THEN 60 ELSE 5 END,
					  @SubscribeType
				  )
			SET @SubscriptionId=SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [dbo].[usp_ServiceSubscription_GetAllUsers]    Script Date: 8/23/2019 8:21:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_ServiceSubscription_GetAllUsers]
AS
BEGIN
	SELECT UserId,FirstName,LastName,UserName,EmailId AS Email,[Password],[Role],UnSubscribe,PasswordHash,PasswordSalt FROM [User]
END
GO
/****** Object:  StoredProcedure [dbo].[usp_ServiceSubscription_GetReport]    Script Date: 8/23/2019 8:21:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--[dbo].[usp_ServiceSubscription_GetReport] 1, '8/22/2019 8:00:05 AM', '9/16/2019 8:00:05 AM'
CREATE PROCEDURE [dbo].[usp_ServiceSubscription_GetReport]
@UserId INT,
@FromDate DATETIME,
@ToDate DATETIME
AS
BEGIN 
	SELECT	UserId,
			Amount,
			ST.ServiceTypeName,
			CAST(AddedOn AS DATE) AS AddedOn
			FROM [dbo].[MyServices] AS MS
				INNER JOIN [dbo].[ServiceType] ST
				ON MS.ServiceTypeId = ST.ServiceTypeId
	WHERE  
			UserId = @UserId AND
			(CAST(AddedOn AS DATE) BETWEEN CAST(@FromDate AS DATE) AND 
                            CAST(@ToDate AS DATE))
END
GO
/****** Object:  StoredProcedure [dbo].[usp_ServiceSubscription_GetServiceDetailsByUserId]    Script Date: 8/23/2019 8:21:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_ServiceSubscription_GetServiceDetailsByUserId]
@UserId INT
AS
BEGIN 
	SELECT	UserId,
			Amount,
			ST.ServiceTypeName,
			CAST(AddedOn AS DATE) AS AddedOn
			FROM [dbo].[MyServices] AS MS
				INNER JOIN [dbo].[ServiceType] ST
				ON MS.ServiceTypeId = ST.ServiceTypeId
	WHERE  
			UserId = @UserId 
END
GO
/****** Object:  StoredProcedure [dbo].[usp_ServiceSubscription_GetUserDetailsById]    Script Date: 8/23/2019 8:21:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_ServiceSubscription_GetUserDetailsById] 
@UserId INT
AS
BEGIN
	SELECT UserId,FirstName,LastName,UserName,[Password],[Role], UnSubscribe FROM [User] WHERE UserId=@UserId
END
GO
/****** Object:  StoredProcedure [dbo].[usp_ServiceSubscription_GetUserEmailById]    Script Date: 8/23/2019 8:21:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_ServiceSubscription_GetUserEmailById] 
@UserId INT
AS
BEGIN
	SELECT EmailId FROM [User] WHERE UserId=@UserId
END
GO
/****** Object:  StoredProcedure [dbo].[usp_ServiceSubscription_HasUserSubscribed]    Script Date: 8/23/2019 8:21:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[dbo].[usp_ServiceSubscription_HasUserSubscribed] 1
CREATE PROCEDURE [dbo].[usp_ServiceSubscription_HasUserSubscribed] 
@UserId INT
AS
BEGIN
DECLARE @InstallationSubscribe BIT=0, @MaintenanceSubscribe BIT=0

	 IF EXISTS(SELECT 1 FROM [dbo].[MyServices] AS MS
						INNER JOIN [dbo].[User] AS US
						ON MS.UserId=US.UserId WHERE MS.UserId=@UserId and ServiceTypeId=1 AND US.UnSubscribe=0)
	 BEGIN
		SET @InstallationSubscribe = 1
	 END

	IF EXISTS(SELECT 1 FROM [dbo].[MyServices] AS MS
						INNER JOIN [dbo].[User] AS US
						ON MS.UserId=US.UserId WHERE MS.UserId=@UserId and ServiceTypeId=2 AND US.UnSubscribe=0)
	BEGIN
		DECLARE @AddedDate DATETIME = (SELECT TOP 1 AddedOn FROM [dbo].[MyServices] WHERE UserId=@UserId and ServiceTypeId=2 ORDER BY AddedOn DESC)
		DECLARE @NextDate DATETIME = DATEADD(MONTH, 1, @AddedDate)
		PRINT @NextDate
		IF(GETDATE() >= @NextDate)
			BEGIN
				SET @MaintenanceSubscribe = 0
			END
		ELSE
			BEGIN
				SET @MaintenanceSubscribe = 1
			END
	END
	SELECT @InstallationSubscribe AS InstallationSubscribe,@MaintenanceSubscribe AS MaintenanceSubscribe
END

GO
/****** Object:  StoredProcedure [dbo].[usp_ServiceSubscription_HasUserUnSubscribed]    Script Date: 8/23/2019 8:21:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_ServiceSubscription_HasUserUnSubscribed] 
@UserId INT
AS
BEGIN
	SELECT UnSubscribe AS HasSubscribed FROM [dbo].[User] WHERE UserId = @UserId
END
GO
/****** Object:  StoredProcedure [dbo].[usp_ServiceSubscription_IsEmailAddressExists]    Script Date: 8/23/2019 8:21:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_ServiceSubscription_IsEmailAddressExists] 
@UserEmail NVARCHAR(100)
AS
BEGIN
DECLARE @IsExist BIT = 0
	IF EXISTS (SELECT 1 FROM [dbo].[User] WHERE EmailId = @UserEmail)
		BEGIN
			SET @IsExist=1
		END
	SELECT @IsExist AS IsExist
END
GO
/****** Object:  StoredProcedure [dbo].[usp_ServiceSubscription_ReSubscribeService]    Script Date: 8/23/2019 8:21:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_ServiceSubscription_ReSubscribeService]
@UserId INT
AS
BEGIN
	UPDATE [dbo].[User] 
			SET UnSubscribe = 0
			WHERE UserId = @UserId
END
GO
/****** Object:  StoredProcedure [dbo].[usp_ServiceSubscription_UnSubscribeService]    Script Date: 8/23/2019 8:21:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_ServiceSubscription_UnSubscribeService]
@UserId INT
AS
BEGIN
	UPDATE [dbo].[User] 
			SET UnSubscribe = 1
			WHERE UserId = @UserId
END
GO
