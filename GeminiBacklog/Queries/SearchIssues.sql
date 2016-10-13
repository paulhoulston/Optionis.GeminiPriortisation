SELECT i.issueid AS IssueId
	,i.projid AS Project
	,i.summary AS Summary
	,isl.statusdesc AS [Status]
	,itl.typedesc AS [Type]
	,i.duedate AS DueDate
	,i.created AS Created
	,p.seq AS [Priority]
	,p.prioritydesc AS PriorityDesc
	,CASE 
		WHEN i.projid = 12
			THEN 'Ticket'
		WHEN i.projid = 14
			THEN 'CR'
		ELSE 'Problem'
		END AS IssueType
FROM dbo.issues i
JOIN dbo.issuestatuslut isl ON isl.statusid = i.issstatus
JOIN dbo.issuetypelut itl ON itl.typeid = i.isstype
JOIN dbo.issueprioritylut p ON p.priorityid = i.isspriority
WHERE i.issueid IN (
		SELECT issueid
		FROM dbo.issues
		WHERE summary LIKE '%' + @searchTerm + '%'
			OR longdesc LIKE '%' + @searchTerm + '%'
		
		UNION
		
		SELECT issueid
		FROM dbo.issuecomments
		WHERE comment LIKE '%' + @searchTerm + '%'
		)

