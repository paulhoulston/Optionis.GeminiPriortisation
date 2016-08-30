SELECT i.issueid
	,isnull(isl.statusdesc, REPLACE(history, 'Issue status changed to ', '')) AS [status]
	,count(isl.statusid) AS StatusChangeCount
INTO #IssueHistory
FROM issuehistory ih
JOIN issuestatuslut isl ON isl.statusdesc = REPLACE(history, 'Issue status changed to ', '')
JOIN issues i ON i.issueid = ih.issueid
WHERE i.projid = 14
	AND isl.statusdesc = 'Closed'
GROUP BY ih.issueid,ih.history,isl.statusdesc,i.issueid
HAVING MAX(ih.created) >= @DateFrom

SELECT DISTINCT d.issueid
	,ISNULL(q5.StatusChangeCount, 0) AS 'Closed'
INTO #StatusChangeCounts
FROM #IssueHistory d
	LEFT JOIN (SELECT issueid,StatusChangeCount FROM #IssueHistory WHERE [status] = 'Closed') AS q5 ON q5.issueid = d.issueid

SELECT i.issueid
    ,i.summary
	,CASE
		WHEN i.projid = 12 THEN 'T-' + CONVERT(NVARCHAR(10), i.issueid)
		WHEN i.projid = 14 THEN 'CR-' + CONVERT(NVARCHAR(10), i.issueid)
		ELSE 'PROB-' + CONVERT(NVARCHAR(10), i.issueid)
	END AS Issue
FROM #StatusChangeCounts c
JOIN issues i ON i.issueid = c.issueid
WHERE Closed > 1

DROP TABLE #IssueHistory
DROP TABLE #StatusChangeCounts
