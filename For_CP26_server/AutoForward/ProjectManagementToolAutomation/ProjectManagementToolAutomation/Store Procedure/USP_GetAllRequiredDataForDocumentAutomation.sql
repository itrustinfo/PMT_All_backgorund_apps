
CREATE procedure [dbo].[USP_GetAllRequiredDataForDocumentFlowAutomation]
as
begin
select * From ActualDocuments where Doc_Type != 'Cover Letter' and DocumentUID in 
(select distinct DocumentUID From Documents Where FlowUID in 
(select FlowMasterUID From [dbo].[DocumentFlowDisplayMaster] Where Type='STP'))


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

select distinct  FlowUID, Current_Status, Update_Status, ForFlow_Step From UserTypeStatus where 
FlowUID in (select FlowUID From [dbo].[DocumentFlowDisplayMaster] Where Type='STP') and
(
Current_Status like '%Network Design DTL Reviewed%' or
Current_Status like '%ONTB DTL Verified%' or
Current_Status like '%AE Approval' or 
Current_Status like '%AEE Approval' or
Current_Status like '%EE Approval' or
Current_Status like '%ACE Approval'
)
end
GO


