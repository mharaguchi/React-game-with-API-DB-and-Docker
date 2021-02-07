CREATE DATABASE TicTacToeDB;
GO

USE TicTacToeDB;
GO

CREATE TABLE GameStates(
    ID UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
    GameID UNIQUEIDENTIFIER,
    History varchar(1000),
    TurnNumber int
)
GO

CREATE PROCEDURE UpdateOrInsertGameState
@GameID UNIQUEIDENTIFIER,
@History varchar(1000),
@TurnNumber int
AS
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
    BEGIN TRANSACTION;
        UPDATE GameStates SET History = @History, TurnNumber = @TurnNumber WHERE GameID = @GameID
        IF @@ROWCOUNT = 0
        BEGIN
            INSERT INTO GameStates(ID, GameID, History, TurnNumber) VALUES (default, @GameID, @History, @TurnNumber)
        END
    COMMIT TRANSACTION;
GO

CREATE PROCEDURE SelectGameState
@GameID UNIQUEIDENTIFIER
AS
SELECT * FROM GameStates WHERE GameID = @GameID