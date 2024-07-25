
create procedure USP_UpdateActualDocumentStatus
@ActualDocumentUID uniqueidentifier,
@UpdateStatus varchar(100)
as
begin
update ActualDocuments set ActualDocument_CurrentStatus = @UpdateStatus where ActualDocumentUID = @ActualDocumentUID
end

