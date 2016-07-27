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
	,isl.statusdesc AS STATUS
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
	,p.prioritydesc AS PriorityDesc
	,i.estimatedays AS EstDays
	,i.estimatehours AS EstHours
	,c.fielddata AS ITOwner
	,c1.fielddata AS Strategic
	,c2.fielddata AS ROI
	,c3.fielddata AS CustomerJourney
	,c4.fielddata AS Risk
	,c5.fielddata AS Legislative
FROM issueresource AS ir
INNER JOIN issues AS i ON ir.issueid = i.issueid
INNER JOIN issuestatuslut AS isl ON isl.statusid = i.issstatus
INNER JOIN issuetypelut AS itl ON itl.typeid = i.isstype
INNER JOIN issueprioritylut AS p ON p.priorityid = i.isspriority
INNER JOIN customfielddata AS c ON i.issueid = c.issueid
INNER JOIN customfielddata AS c1 ON i.issueid = c1.issueid
INNER JOIN customfielddata AS c2 ON i.issueid = c2.issueid
INNER JOIN customfielddata AS c3 ON i.issueid = c3.issueid
INNER JOIN customfielddata AS c4 ON i.issueid = c4.issueid
INNER JOIN customfielddata AS c5 ON i.issueid = c5.issueid
WHERE c.customfieldid = 34
	AND c1.customfieldid = 68
	AND c2.customfieldid = 69
	AND c3.customfieldid = 70
	AND c4.customfieldid = 71
	AND c5.customfieldid = 72
	AND i.projid IN (
		12
		,14
		)
GROUP BY i.issueid
	,i.projid
	,i.summary
	,isl.statusdesc
	,itl.typedesc
	,i.startdate
	,i.duedate
	,i.created
	,p.prioritydesc
	,i.estimatedays
	,i.estimatehours
	,c.fielddata
	,c1.fielddata
	,c2.fielddata
	,c3.fielddata
	,c4.fielddata
	,c5.fielddata
	,CASE 
		WHEN i.duedate < (GETDATE())
			THEN '1 - Overdue'
		WHEN i.duedate BETWEEN GETDATE()
				AND (GETDATE() + 28)
			THEN '2 - Due Soon'
		ELSE '3 - Not Due'
		END
HAVING (NOT (isl.statusdesc IN (N'Closed')))
	AND (
		NOT (
			i.issueid IN (
				29017
				,29015
				,31860
				,44238
				,44239
				,44227
				,44034
				)
			)
		)
	AND (itl.typedesc IN (N'Applications Enhancement'))
ORDER BY 'DueSoon'
	,CASE 
		WHEN i.duedate < (GETDATE())
			THEN i.duedate
		WHEN i.duedate BETWEEN GETDATE()
				AND (GETDATE() + 28)
			THEN i.duedate
		ELSE NULL
		END
	,CASE 
		WHEN isl.statusdesc IN ('Pending Closure')
			THEN 1
		WHEN isl.statusdesc IN ('For Release')
			THEN 2
		WHEN isl.statusdesc LIKE ('%UAT%')
			THEN 3
		WHEN isl.statusdesc LIKE ('%Testing%')
			THEN 4
		WHEN isl.statusdesc IN (
				'Logged with supplier'
				,'waiting on customer'
				)
			THEN 5
		WHEN isl.statusdesc IN (
				'In Progress'
				,'Development'
				)
			THEN 6
		WHEN isl.statusdesc IN (
				'Development Pending'
				,'Design Pending'
				)
			THEN 7
		WHEN isl.statusdesc LIKE ('%analysis%')
			THEN 8
		ELSE 99
		END
	,PriorityDesc
	,STATUS
	,Created
