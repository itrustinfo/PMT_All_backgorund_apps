

CREATE procedure [dbo].[USP_InsertDocumentStatusTable]
(
@DocumentStatus TT_DocumentStatusType readonly 
)
as
begin
insert into DocumentStatus(StatusUID, DocumentUID, ActivityType, Current_Status, ActivityDate, DocumentDate, Status_Comments, Delete_Flag, Origin, Forwarded, Version, AcivityUserUID)
select StatusUID, DocumentUID, ActivityType, ActivityType, ActivityDate, DocumentDate,Status_Comments, 'N', 'BWSSB', 'Y', '1','419D71BD-FDD4-4A92-898C-A044BFA7803D' from @DocumentStatus
end
GO


