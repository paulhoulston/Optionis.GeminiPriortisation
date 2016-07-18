SELECT
 i.issueId AS IssueId,
 CASE
     WHEN i.projid = 12 THEN 'T-' + CONVERT(NVARCHAR(10), i.issueid)
		WHEN i.projid = 14 THEN 'CR-' + CONVERT(NVARCHAR(10), i.issueid)
		ELSE 'PROB-' + CONVERT(NVARCHAR(10), i.issueid)
	END AS Issue,
 i.projid AS Project,
	i.summary AS Summary,
	isl.statusdesc AS[Status],
    itl.typedesc AS[Type],
    CASE WHEN i.duedate IS NULL THEN NULL ELSE CONVERT(VARCHAR(10), i.duedate, 103) END AS DueDate,
    CONVERT(VARCHAR(10), i.created, 103) AS Created,
    REPLACE(REPLACE(REPLACE(REPLACE(p.prioritydesc, '1 - ', ''), '2 - ', ''), '3 - ', ''), '4 - ', '') AS[Priority]
FROM dbo.issueresource ir
JOIN dbo.issues i on ir.issueid = i.issueid
JOIN dbo.issuestatuslut isl on isl.statusid = i.issstatus
JOIN dbo.issuetypelut itl on itl.typeid = i.isstype
JOIN dbo.issueprioritylut p on p.priorityid = i.isspriority
WHERE userid = @userId
 AND isl.statusdesc NOT IN ('Closed')
 AND i.issueid NOT IN (29015, 29017, 44227, 44238, 44239, 29459)
ORDER BY i.issueid