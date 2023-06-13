
ALTER procedure [dbo].[USP_GetSubmittedMiltipleUser]
(
@DocumentUID uniqueidentifier,
@ActualDocumentUID uniqueidentifier,
@Step int,
@Current_Status varchar(50)
)
as
begin
select distinct s.Step, s.UserUID, u.FirstName, u.LastName from Submittal_MultipleUsers s, UserDetails u 
where s.UserUID = u.UserUID and s.SubmittalUID = @DocumentUID and Step = @Step
and s.UserUID not in (select AcivityUserUID from DocumentStatus where DocumentUID = @ActualDocumentUID and Current_Status = @Current_Status
)
end
GO
