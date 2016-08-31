DECLARE @IssueTypes TABLE (
	IssueType VARCHAR(50) NOT NULL
	,DisplayOrder INT NOT NULL
	)

INSERT INTO @IssueTypes (IssueType,DisplayOrder)
VALUES ('Team Management',8)
	,('Absence',1)
	,('Professional Development',5)
	,('Research and Development',7)
	,('Admin and Team Meetings',2)
	,('Project',6)
	,('Applications Enhancement',3)
	,('BAU',4)
	,('Total',1000)

SELECT CASE 
		WHEN i.issueid = 44227
			THEN 'Team Management'
		WHEN i.issueid = 29015
			THEN 'Absence'
		WHEN i.issueid = 44239
			THEN 'Professional Development'
		WHEN i.issueid = 44238
			THEN 'Research and Development'
		WHEN i.issueid = 29017
			THEN 'Admin and Team Meetings'
		WHEN c.compname = 'Project'
			THEN 'Project'
		WHEN it.typedesc = 'Applications Enhancement'
			THEN 'Applications Enhancement'
		ELSE 'BAU'
		END AS IssueType
	,CASE 
		WHEN (
				tt.timeentrydate >= DATEADD(DAY, 0, @StartDate)
				AND tt.timeentrydate < DATEADD(DAY, 7, @StartDate)
				)
			THEN tt.hours * 60 + tt.minutes
		ELSE 0
		END AS CumulativeMinutesWeek1
	,CASE 
		WHEN (
				tt.timeentrydate >= DATEADD(DAY, - 7, @StartDate)
				AND tt.timeentrydate < DATEADD(DAY, 0, @StartDate)
				)
			THEN tt.hours * 60 + tt.minutes
		ELSE 0
		END AS CumulativeMinutesWeek2
	,CASE 
		WHEN (
				tt.timeentrydate >= DATEADD(DAY, - 14, @StartDate)
				AND tt.timeentrydate < DATEADD(DAY, - 7, @StartDate)
				)
			THEN tt.hours * 60 + tt.minutes
		ELSE 0
		END AS CumulativeMinutesWeek3
	,CASE 
		WHEN (
				tt.timeentrydate >= DATEADD(DAY, - 21, @StartDate)
				AND tt.timeentrydate < DATEADD(DAY, - 14, @StartDate)
				)
			THEN tt.hours * 60 + tt.minutes
		ELSE 0
		END AS CumulativeMinutesWeek4
INTO #temp
FROM timetracking tt
JOIN issues i ON i.issueid = tt.issueid
JOIN issuetypelut it ON it.typeid = i.isstype
JOIN issuecomponent ic ON ic.issueid = i.issueid
JOIN components c ON ic.componentid = c.compid
WHERE tt.userid IN @UserIds
	AND tt.timeentrydate >= DATEADD(DAY, - 21, @StartDate)
	AND tt.timeentrydate < DATEADD(DAY, 7, @StartDate)

INSERT INTO #Temp(IssueType, CumulativeMinutesWeek1, CumulativeMinutesWeek2, CumulativeMinutesWeek3, CumulativeMinutesWeek4)
SELECT 'Total' AS IssueType
	,sum(CumulativeMinutesWeek1) AS CumulativeMinutesWeek1
	,sum(CumulativeMinutesWeek2) AS CumulativeMinutesWeek2
	,sum(CumulativeMinutesWeek3) AS CumulativeMinutesWeek3
	,sum(CumulativeMinutesWeek4) AS CumulativeMinutesWeek4
FROM #temp

SELECT d.IssueType
	,ISNULL(SUM(t.CumulativeMinutesWeek1), 0) AS CumulativeMinutesWeek1
	,ISNULL(SUM(t.CumulativeMinutesWeek2), 0) AS CumulativeMinutesWeek2
	,ISNULL(SUM(t.CumulativeMinutesWeek3), 0) AS CumulativeMinutesWeek3
	,ISNULL(SUM(t.CumulativeMinutesWeek4), 0) AS CumulativeMinutesWeek4
FROM @IssueTypes d
LEFT JOIN #temp t ON d.IssueType = t.IssueType
GROUP BY d.IssueType
ORDER BY d.IssueType

DROP TABLE #temp
