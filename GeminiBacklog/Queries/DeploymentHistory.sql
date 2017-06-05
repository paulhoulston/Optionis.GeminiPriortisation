SELECT d.deploymentid AS DeploymentId
	,d.deploymentdate AS DeploymentDate
	,r.title AS ReleaseTitle
	,r.applicationdescription AS Application
	,r.comments AS Comments
	,u.username AS DeployedBy
	,ds.statusdesc AS DeploymentStatus
FROM [kpi].[deployments] d
JOIN kpi.releases r ON r.releaseid = d.releaseid
JOIN kpi.deploymentstatus ds ON ds.deploymentstatusid = d.deploymentstatusid
JOIN users u ON u.userid = r.createdbyuserid
ORDER BY d.deploymentdate DESC

SELECT d.deploymentid AS DeploymentId
	,i.projid AS ProjectId
	,i.issueid AS IssueId
	,CASE 
		WHEN i.projid = 12
			THEN 'T-'
		WHEN i.projid = 14
			THEN 'CR-'
		ELSE 'PROB'
		END + cast(i.issueid AS VARCHAR(10)) AS IssueDescription
    ,i.summary AS Summary
FROM [kpi].[deployments] d
JOIN kpi.releaseissues ri ON ri.releaseid = d.releaseid
JOIN issues i ON i.issueid = ri.issueid
JOIN kpi.deploymentstatus ds ON ds.deploymentstatusid = d.deploymentstatusid
ORDER BY i.projid
	,i.issueid
