create database UserDB
GO

use UserDB 

CREATE TABLE [dbo].[Person](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](255) NOT NULL,
	[UserName] [varchar](255) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	PRIMARY KEY (ID)
);
GO

CREATE PROCEDURE [dbo].[Add]
	@FullName VARCHAR(255),
	@UserName VARCHAR(MAX),
	@Password VARCHAR(255)
AS 

BEGIN 
	SET NOCOUNT ON

	BEGIN
		INSERT INTO Person(FullName,UserName,Password)
		VALUES (@FullName,@UserName,@Password)

	END
	
END
GO

CREATE PROCEDURE [dbo].[UserByUsername]
	@UserName varchar(255)

	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT * From Person
	WHERE @UserName = UserName
END
GO

CREATE PROCEDURE [dbo].[Loginfo]
	@UserName varchar(255),
	@Password varchar(255)
	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT * From Person
	WHERE @UserName = UserName AND @Password = Password
END
GO