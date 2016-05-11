SELECT
	CAST(tt.timeentrydate AS DATE) AS WorkDate,
	CASE
		WHEN i.projid = 12 THEN 'T-' + CAST(i.issueid AS VARCHAR)
		WHEN i.projid = 14 THEN 'CR-' + CAST(i.issueid AS VARCHAR)
	END AS GeminiRef,
	i.summary AS Issue,
	SUM(tt.hours*60 + tt.minutes) AS WorkTimeInMinutes
INTO #TimeLogs
FROM
	[dbo].[timetracking] tt
JOIN dbo.issues i ON i.issueid = tt.issueid
WHERE tt.timeentrydate >= @StartDate
AND tt.timeentrydate < DATEADD(DAY, 7, @StartDate)
AND	tt.userid = @UserId
GROUP BY i.projid, i.issueid, i.summary, tt.timeentrydate

SELECT Issue, GeminiRef, SUM(Monday) AS Monday, SUM(Tuesday) AS Tuesday, SUM(Wednesday) AS Wednesday, SUM(Thursday) AS Thursday, SUM(Friday) AS Friday
FROM (
	SELECT Issue, GeminiRef, WorkTimeInMinutes AS Monday, 0 AS Tuesday, 0 AS Wednesday, 0 AS Thursday, 0 AS Friday FROM #TimeLogs WHERE WorkDate = @StartDate
	UNION ALL
	SELECT Issue, GeminiRef, 0 AS Monday, WorkTimeInMinutes AS Tuesday, 0 AS Wednesday, 0 AS Thursday, 0 AS Friday FROM #TimeLogs WHERE WorkDate = DATEADD(DAY, 1, @StartDate)
	UNION ALL
	SELECT Issue, GeminiRef, 0 AS Monday, 0 AS Tuesday, WorkTimeInMinutes AS Wednesday, 0 AS Thursday, 0 AS Friday FROM #TimeLogs WHERE WorkDate = DATEADD(DAY, 2, @StartDate)
	UNION ALL
	SELECT Issue, GeminiRef, 0 AS Monday, 0 AS Tuesday, 0 AS Wednesday, WorkTimeInMinutes AS Thursday, 0 AS Friday FROM #TimeLogs WHERE WorkDate = DATEADD(DAY, 3, @StartDate)
	UNION ALL
	SELECT Issue, GeminiRef, 0 AS Monday, 0 AS Tuesday, 0 AS Wednesday, 0 AS Thursday, WorkTimeInMinutes AS Friday FROM #TimeLogs WHERE WorkDate = DATEADD(DAY, 4, @StartDate)
) AS t
GROUP BY t.Issue, t.GeminiRef

DROP TABLE #TimeLogs
