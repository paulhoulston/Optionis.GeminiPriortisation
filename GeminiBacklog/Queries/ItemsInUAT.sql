SELECT i.issueid AS IssueId
	,CASE 
		WHEN i.projid = 12
			THEN 'T-' + CONVERT(NVARCHAR(10), i.issueid)
		WHEN i.projid = 14
			THEN 'CR-' + CONVERT(NVARCHAR(10), i.issueid)
		ELSE 'PROB-' + CONVERT(NVARCHAR(10), i.issueid)
		END AS Issue
	,i.summary AS Summary
	,itl.typedesc AS Type
	,isl.statusdesc AS Status
	,i.startdate AS StartDate
	,CASE 
		WHEN i.duedate < (GETDATE())
			THEN '1 - Overdue'
		WHEN i.duedate BETWEEN GETDATE()
				AND (GETDATE() + 28)
			THEN '2 - Due Soon'
		ELSE '3 - Not Due'
		END AS DueSoon
	,i.duedate AS DueDate
	,i.created AS Created
    ,REPLACE(REPLACE(REPLACE(REPLACE(PriorityDesc, '1 - ', ''), '2 - ', ''), '3 - ', ''), '4 - ', '') AS [Priority]
FROM issueresource AS ir
INNER JOIN issues AS i ON ir.issueid = i.issueid
INNER JOIN issuestatuslut AS isl ON isl.statusid = i.issstatus
INNER JOIN issuetypelut AS itl ON itl.typeid = i.isstype
INNER JOIN issueprioritylut AS p ON p.priorityid = i.isspriority
WHERE 
    i.projid IN (12, 14, 17)
    AND isl.statusdesc In ('UAT', 'UAT Pending')
GROUP BY i.issueid
	,i.projid
	,i.summary
	,isl.statusdesc
	,itl.typedesc
	,i.startdate
	,i.duedate
	,i.created
	,p.prioritydesc
	,CASE 
		WHEN i.duedate < (GETDATE())
			THEN '1 - Overdue'
		WHEN i.duedate BETWEEN GETDATE()
				AND (GETDATE() + 28)
			THEN '2 - Due Soon'
		ELSE '3 - Not Due'
		END
ORDER BY
	isl.statusdesc
	,PriorityDesc
	,Created
