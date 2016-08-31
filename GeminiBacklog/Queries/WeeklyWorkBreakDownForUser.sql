DECLARE @IssueTypes TABLE (IssueType VARCHAR(50) NOT NULL)

INSERT INTO @IssueTypes (IssueType)
VALUES ('Team Management')
	,('Absence')
	,('Professional Development')
	,('Research and Development')
	,('Admin and Team Meetings')
	,('Project')
	,('Applications Enhancement')
	,('BAU')

SELECT d.IssueType
	,ISNULL(SUM(CumulativeMinutes), 0) AS CumulativeMinutes
FROM @IssueTypes d
LEFT JOIN (
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
		,tt.hours * 60 + tt.minutes AS CumulativeMinutes
	FROM timetracking tt
	JOIN issues i ON i.issueid = tt.issueid
	JOIN issuetypelut it ON it.typeid = i.isstype
	JOIN issuecomponent ic ON ic.issueid = i.issueid
	JOIN components c ON ic.componentid = c.compid
	WHERE tt.userid = @UserId
		AND tt.timeentrydate >= @StartDate
		AND tt.timeentrydate < DATEADD(DAY, 7, @StartDate)
	) t ON d.IssueType = t.IssueType
GROUP BY d.IssueType
ORDER BY d.IssueType
