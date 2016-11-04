SELECT DISTINCT
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
    REPLACE(REPLACE(REPLACE(REPLACE(p.prioritydesc, '1 - ', ''), '2 - ', ''), '3 - ', ''), '4 - ', '') AS[Priority],
 u.username AS ITOwner,
 isl.seq
FROM dbo.issueresource ir
JOIN dbo.issues i on ir.issueid = i.issueid
JOIN dbo.issuestatuslut isl on isl.statusid = i.issstatus
JOIN dbo.issuetypelut itl on itl.typeid = i.isstype
JOIN dbo.issueprioritylut p on p.priorityid = i.isspriority
JOIN dbo.users u on u.userid = ir.userid
WHERE ir.userid IN @UserIds
 AND isl.statusdesc NOT IN ('Closed')
 AND (@Filter IS NULL OR @Filter = '' OR (@Filter IS NOT NULL AND @Filter <> '' AND isl.statusdesc = @Filter))
 AND i.issueid NOT IN (29015, 29017, 44227, 44238, 44239, 29459, 31860)
ORDER BY isl.seq DESC, u.username, i.issueid