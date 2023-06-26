declare @statusReasonSeed int
declare @refreshLeadId int

if not exists (select * from StatusReasons where name = 'Renew Lead') 
begin
	select @statusReasonSeed = max(sr.id) from dbo.StatusReasons sr
	DBCC CHECKIDENT ('StatusReasons', RESEED, @statusReasonSeed)

	insert StatusReasons (name) values ('Renew Lead')
	select @refreshLeadId = scope_identity()

	insert dbo.StatusReasonFlows(statusReasonId, domainId, fromTaskId, toTaskId, toStatusId, toDomainId, fromFlowId, toFlowId)
	select @refreshLeadId, 48, 87, 87, 1, 48, 1200, 1200
	union select @refreshLeadId, 48, 10, 87, 1, 48, 1203, 1200

	select f.id as "RenewHeldLead" from StatusReasonFlows f where statusReasonId = @statusReasonSeed and fromFlowId = 1200
	select f.id as "RenewCompletedLead" from StatusReasonFlows f where statusReasonId = @statusReasonSeed and fromFlowId = 1203
end
else
begin
	select @statusReasonSeed = sr.id from dbo.StatusReasons sr where sr.name = 'Renew Lead'
	select f.id as "RenewHeldLead" from StatusReasonFlows f where statusReasonId = @statusReasonSeed and fromFlowId = 1200
	select f.id as "RenewCompletedLead" from StatusReasonFlows f where statusReasonId = @statusReasonSeed and fromFlowId = 1203
end