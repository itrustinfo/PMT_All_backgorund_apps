
create procedure [dbo].[USP_GetDocumentStatus]
(
@DocumentUID uniqueidentifier
)
as begin
select * from DocumentStatus where DocumentUID = @DocumentUID order by CreatedDate desc
end
GO


