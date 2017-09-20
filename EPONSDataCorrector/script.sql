CREATE PROCEDURE [EPONS].[RemoveDuplicatePatient] 
@keepPatientId UNIQUEIDENTIFIER,
@removePatientId UNIQUEIDENTIFIER
AS

UPDATE [Audit].[PatientImpairmentGroup]
SET [PatientId] = @keepPatientId
WHERE [PatientId] = @removePatientId;

UPDATE [Patient].[MeasurementTools]
SET [PatientId] = @keepPatientId
WHERE [PatientId] = @removePatientId;

UPDATE [Patient].[SupportServices]
SET [PatientId] = @keepPatientId
WHERE [PatientId] = @removePatientId;

UPDATE [Patient].[TeamMembers]
SET [PatientId] = @keepPatientId
WHERE [PatientId] = @removePatientId;

UPDATE [Visit].[Details]
SET [PatientId] = @keepPatientId
WHERE [PatientId] = @removePatientId;

UPDATE [Patient].[EpisodesOfCare]
SET [PatientId] = @keepPatientId
WHERE [PatientId] = @removePatientId;

UPDATE [Message].[Details]
SET [PatientId] = @keepPatientId
WHERE [PatientId] = @removePatientId;

UPDATE [Survey].[Results]
SET [PatientId] = @keepPatientId
WHERE [PatientId] = @removePatientId;

--DELETE FROM [Audit].[PatientImpairmentGroup]
--WHERE [PatientId] = @removePatientId;

--DELETE FROM [Patient].[MeasurementTools]
--WHERE [PatientId] = @removePatientId;

--DELETE FROM [Patient].[SupportServices]
--WHERE [PatientId] = @removePatientId;

--DELETE FROM [Patient].[TeamMembers]
--WHERE [PatientId] = @removePatientId;

--DELETE [scoreValue] FROM [Visit].[ScoreValues] AS [scoreValue]
--INNER JOIN [Visit].[Details] AS [visit]
--ON [visit].[VisitId] = [scoreValue].[VisitId]
--WHERE [PatientId] = @removePatientId;

--DELETE FROM [Visit].[Details]
--WHERE [PatientId] = @removePatientId;

DELETE FROM [Patient].[Details]
WHERE [PatientId] = @removePatientId;