SELECT
	CAST(tt.timeentrydate AS DATE) AS WorkDate,
	CASE
		WHEN i.projid = 12 THEN 'T-' + CAST(i.issueid AS VARCHAR)
		WHEN i.projid = 14 THEN 'CR-' + CAST(i.issueid AS VARCHAR)
		ELSE 'PROB-' + CAST(i.issueid AS VARCHAR)
	END AS GeminiRef,
	i.summary AS Issue,
	SUM(tt.hours*60 + tt.minutes) AS WorkTimeInMinutes,
	tt.comment AS Comments
FROM
	[dbo].[timetracking] tt
JOIN dbo.issues i ON i.issueid = tt.issueid
WHERE tt.timeentrydate >= @StartDate
AND tt.timeentrydate < DATEADD(DAY, 7, @StartDate)
AND	tt.userid = @UserId
GROUP BY i.projid, i.issueid, i.summary, tt.timeentrydate, tt.comment