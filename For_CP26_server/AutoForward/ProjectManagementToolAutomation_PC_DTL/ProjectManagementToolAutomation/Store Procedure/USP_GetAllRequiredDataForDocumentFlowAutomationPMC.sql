
create procedure [dbo].[USP_GetAllRequiredDataForDocumentFlowAutomationPMC]
as
begin
select * From ActualDocuments where Doc_Type != 'Cover Letter' and DocumentUID in 
(select distinct DocumentUID From Documents Where FlowUID in (select FlowMasterUID From [dbo].[DocumentFlowDisplayMaster] Where Type='STP'))


;WITH cte AS
(
   SELECT *,
         ROW_NUMBER() OVER (PARTITION BY DocumentUID ORDER BY CreatedDate DESC) AS rn
    from DocumentStatus where Delete_Flag = 'N' and DocumentUID in (
select ActualDocumentUID From ActualDocuments where Doc_Type != 'Cover Letter' and DocumentUID in 
(select distinct DocumentUID From Documents Where FlowUID in (select FlowMasterUID From [dbo].[DocumentFlowDisplayMaster] Where Type='STP'))
)
)
SELECT StatusUID, DocumentUID, Current_Status
FROM cte
WHERE rn = 1


select distinct UserType, UID, FlowUID, Current_Status, Update_Status, ForFlow_Step, d.Flow_Name from UserTypeStatus u, DocumentFlowDisplayMaster d 
where u.FlowUID = d.FlowMasterUID and d.Type = 'STP' and u.Current_Status like '%Accepted%' and
(d.Flow_Name like '%Works A%' or d.Flow_Name like '%Works B%' or d.Flow_Name like '%Vendor Approval%')

end
GO


