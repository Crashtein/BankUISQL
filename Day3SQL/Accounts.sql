CREATE TABLE [dbo].[Accounts]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY , 
    [UserName] TEXT NOT NULL, 
    [Password] TEXT NULL, 
    [Balance] FLOAT NOT NULL
)
