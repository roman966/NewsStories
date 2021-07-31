create database StoryDB
GO
use StoryDB 

CREATE TABLE Story (
    ID int NOT NULL IDENTITY(1,1),
    Title varchar(255) NOT NULL,
    Body varchar(Max),
	Date  Datetime,
    Author varchar(255),
    PRIMARY KEY (ID)
);
GO

CREATE PROCEDURE [dbo].[AllStory]

AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT * From Story
END
GO
CREATE PROCEDURE [dbo].[DeleteBYId]
	@ID INT

	
AS
BEGIN
	
	SET NOCOUNT ON;

	DELETE Story
	WHERE @ID = ID
END
GO
CREATE PROCEDURE [dbo].[AddorEdit]
	@Title VARCHAR(255),
	@Body VARCHAR(MAX),
	@Datetime VARCHAR(255),
	@Author VARCHAR(255),
	@ID INT
AS 

BEGIN 
	SET NOCOUNT ON

	If @ID=0
	BEGIN
		INSERT INTO Story(Title,Body,Date,Author)
		VALUES (@Title,@Body,@Datetime,@Author)

	END
	ELSE
	BEGIN 
		UPDATE Story
		SET 
			Title=@Title,
			Body=@Body,
			Date=@Datetime,
			Author=@Author
		WHERE ID=@ID
	END
END
GO
 CREATE PROCEDURE [dbo].[StoryBYId]
	@ID INT
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT * From Story
	WHERE @ID = ID
END
GO

CREATE PROCEDURE [dbo].[Search]
	@String varchar(255)

	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT * From Story
	WHERE Title LIKE '%@String%' OR Body LIKE '%@String%'
END
GO