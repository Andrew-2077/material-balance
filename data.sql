USE MaterialBalanceDB;
GO

-- DELETE FROM Measurements WHERE StreamId = 'S8';
    
-- DELETE FROM Streams WHERE Id = 'S8';
-- GO

-- INSERT INTO Streams (Id, Name, SourceNodeId, DestinationNodeId, MinBound, MaxBound)
-- VALUES
-- ('S8', N'Доп. поток', 'Node1', NULL, 0, 10000);
-- GO

-- INSERT INTO Measurements (PeriodId, StreamId, Value, Tolerance, IsMeasured)
-- VALUES
-- (1, 'S8', 6.66666, 0.666666, 1);
-- GO

-- UPDATE Measurements SET Value = 10.005491934148900000 WHERE StreamId = 'S1';
-- UPDATE Measurements SET Value = 3.032657950247490000 WHERE StreamId = 'S2';
GO

-- -- 1. Добавляем данные в таблицу Periods
-- INSERT INTO Periods (Name, Timestamp)
-- VALUES (N'Тестовый период', '2026-05-02 21:00:00');
-- GO

-- -- 2. Добавляем данные в таблицу Streams
-- INSERT INTO Streams (Id, Name, SourceNodeId, DestinationNodeId, MinBound, MaxBound)
-- VALUES 
-- ('S1', N'Входной поток', NULL, 'Node1', 0, 10000),
-- ('S2', N'Прокачка в цех 1', 'Node1', NULL, 0, 10000),
-- ('S3', N'Перекачка 1', 'Node1', 'Node2', 0, 10000),
-- ('S4', N'Прокачка в цех 2', 'Node2', NULL, 0, 10000),
-- ('S5', N'Перекачка 2', 'Node2', 'Node3', 0, 10000),
-- ('S6', N'Выходной поток 1', 'Node3', NULL, 0, 10000),
-- ('S7', N'Выходной поток 2', 'Node3', NULL, 0, 10000),
-- ('S8', N'Доп. поток', 'Node1', NULL, 0, 10000);
-- GO

-- -- 3. Добавляем данные в таблицу Measurements
-- -- Здесь мы предполагаем, что созданный выше период получил Id = 1
-- INSERT INTO Measurements (PeriodId, StreamId, Value, Tolerance, IsMeasured)
-- VALUES 
-- (1, 'S1', 10.0054919341489, 0.200109838682978, 1),
-- (1, 'S2', 3.03265795024749, 0.1213063180098996, 1),
-- (1, 'S3', 6.83122010827837, 0.683122010827837, 1),
-- (1, 'S4', 1.98478460320379, 0.0396956920640758, 1),
-- (1, 'S5', 5.09293357450987, 0.1018586714901974, 1),
-- (1, 'S6', 4.05721328676762, 0.0811442657353524, 1),
-- (1, 'S7', 0.991215230484718, 0.01982430460969436, 1),
-- (1, 'S8', 6.66666, 0.666666, 1);
-- GO