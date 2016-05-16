SELECT
	i.issueid AS IssueId,
	i.summary AS Summary,
	isl.statusdesc AS [Status],
	itl.typedesc AS [Type],
	i.duedate AS DueDate,
	i.created AS Created,
	p.seq AS [Priority],
	p.prioritydesc AS PriorityDesc,
	CASE
		WHEN i.projid = 12 THEN 'Ticket'
		WHEN i.projid = 14 THEN 'CR'
		ELSE 'Problem'
	END AS IssueType
INTO #Temp
FROM dbo.issueresource ir
JOIN dbo.issues i on ir.issueid = i.issueid
JOIN dbo.issuestatuslut isl on isl.statusid = i.issstatus
JOIN dbo.issuetypelut itl on itl.typeid = i.isstype
JOIN dbo.issueprioritylut p on p.priorityid = i.isspriority
WHERE userid = 39
 AND isl.statusdesc NOT IN ('Closed', 'On Hold')

CREATE TABLE #Prioritised (
	Id INT PRIMARY KEY IDENTITY,
	IssueId INT,
	Summary NVARCHAR(255),
	[Status] NVARCHAR(255),
	[Type] NVARCHAR(255),
	DueDate DATETIME NULL,
	Created DATETIME,
	[Priority] INT,
	PriorityDesc NVARCHAR(255),
	IssueType NVARCHAR(255));

-- Prioritise project first
INSERT INTO #Prioritised
SELECT IssueId, Summary, [Status], [Type], DueDate, Created, [Priority], PriorityDesc, IssueType
FROM #Temp
WHERE [Type] = 'Project'
  AND IssueId NOT IN (SELECT IssueId FROM #Prioritised)
ORDER BY [Priority] DESC, ISNULL(DueDate, '01-Jan-3000'), Created

-- Prioritise tickets
INSERT INTO #Prioritised
SELECT IssueId, Summary, [Status], [Type], DueDate, Created, [Priority], PriorityDesc, IssueType
FROM #Temp
WHERE [IssueType] = 'Ticket'
  AND IssueId NOT IN (SELECT IssueId FROM #Prioritised)
ORDER BY [Priority] DESC, ISNULL(DueDate, '01-Jan-3000'), Created

-- Prioritise bugs
INSERT INTO #Prioritised
SELECT IssueId, Summary, [Status], [Type], DueDate, Created, [Priority], PriorityDesc, IssueType
FROM #Temp
WHERE [Type] = 'Bug' AND [IssueType] = 'CR'
  AND IssueId NOT IN (SELECT IssueId FROM #Prioritised)
ORDER BY [Priority] DESC, ISNULL(DueDate, '01-Jan-3000'), Created

-- Prioritise converted tickets
INSERT INTO #Prioritised
SELECT IssueId, Summary, [Status], [Type], DueDate, Created, [Priority], PriorityDesc, IssueType
FROM #Temp
WHERE [Type] = 'Query'
  AND IssueId NOT IN (SELECT IssueId FROM #Prioritised)
ORDER BY [Priority] DESC, ISNULL(DueDate, '01-Jan-3000'), Created

-- Insert everything left over
INSERT INTO #Prioritised
SELECT IssueId, Summary, [Status], [Type], DueDate, Created, [Priority], PriorityDesc, IssueType
FROM #Temp
WHERE IssueId NOT IN (SELECT IssueId FROM #Prioritised)
ORDER BY [Priority] DESC, ISNULL(DueDate, '01-Jan-3000'), Created

SELECT 
	CASE
		WHEN IssueType = 'Ticket' THEN 'T-' + CONVERT(NVARCHAR(10), IssueId)
		WHEN IssueType = 'CR' THEN 'CR-' + CONVERT(NVARCHAR(10), IssueId)
		ELSE 'PROB-' + CONVERT(NVARCHAR(10), IssueId)
	END AS Issue,
	Summary,
	[Status],
	[Type],
	CASE WHEN DueDate IS NULL THEN '' ELSE CONVERT(VARCHAR(10), DueDate, 103) END AS DueDate,
	CONVERT(VARCHAR(10), Created, 103) AS Created,
	REPLACE(REPLACE(REPLACE(REPLACE(PriorityDesc, '1 - ', ''), '2 - ', ''), '3 - ', ''), '4 - ', '') AS [Priority]
FROM #Prioritised
ORDER BY Id

DROP TABLE #Temp
DROP TABLE #Prioritised