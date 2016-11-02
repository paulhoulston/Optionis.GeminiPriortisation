SELECT        
       i.issueid AS IssueId,
       CASE
            WHEN i.projid = 12 THEN 'T-' + CONVERT(NVARCHAR(10), i.issueid)
            WHEN i.projid = 14 THEN 'CR-' + CONVERT(NVARCHAR(10), i.issueid)
            ELSE 'PROB-' + CONVERT(NVARCHAR(10), i.issueid)
       END AS Issue, 
       i.summary AS Summary, 
       CASE   
              WHEN isl.statusdesc IN ('Pending Closure','For Release') THEN 1
              WHEN isl.statusdesc LIKE ('%UAT%') THEN 2
              WHEN isl.statusdesc LIKE ('%Testing%') THEN 3
              WHEN isl.statusdesc In ('Logged with supplier', 'waiting on customer') THEN 4
              WHEN isl.statusdesc In  ('In Progress', 'Development')THEN 5
              WHEN isl.statusdesc In ('Development Pending', 'Design Pending') THEN 6
              WHEN isl.statusdesc LIKE ('%analysis%') THEN 7
              ELSE 99
       END AS StatusOrder,
       isl.statusdesc AS Status, 
       itl.typedesc AS Type, 
       CASE 
              WHEN i.duedate < (GETDATE()) THEN '1 - Overdue' 
              WHEN i.duedate between GETDATE () AND (GETDATE() + 28) THEN '2 - Due Soon'
              ELSE '3 - Not Due' 
       END AS DueSoon, 
       i.duedate AS DueDate, 
       i.created AS Created, 
       p.prioritydesc AS PriorityDesc, 
       i.estimatedays AS EstDays, 
    i.estimatehours AS EstHours, 
       c.fielddata AS ITOwner
FROM            
       issueresource AS ir 
       INNER JOIN issues AS i ON ir.issueid = i.issueid 
       INNER JOIN issuestatuslut AS isl ON isl.statusid = i.issstatus 
       INNER JOIN issuetypelut AS itl ON itl.typeid = i.isstype 
       INNER JOIN issueprioritylut AS p ON p.priorityid = i.isspriority 
       INNER JOIN customfielddata AS c ON i.issueid = c.issueid
WHERE  
       c.customfieldid = 34     
       AND i.projid = 14 
       AND (@Filter IS NULL OR @Filter = '' OR (@Filter IS NOT NULL AND @Filter <> '' AND isl.statusdesc = @Filter))

GROUP BY 
       i.issueid,
       i.projid,
       i.summary, 
       isl.statusdesc, 
       itl.typedesc, 
       i.duedate, 
       i.created, 
       p.prioritydesc, 
       i.estimatedays, 
       i.estimatehours, 
       c.fielddata, 
       CASE 
              WHEN i.duedate < (GETDATE()) THEN '1 - Overdue' 
              WHEN i.duedate between GETDATE () AND (GETDATE() + 28) THEN '2 - Due Soon'
              ELSE '3 - Not Due' 
       END

HAVING        
       (NOT (isl.statusdesc IN (N'Closed', N'On Hold'))) 
       AND (NOT (i.issueid IN (29017, 29015, 31860, 44238, 44239, 44227, 44034))) 
       AND (itl.typedesc IN (N'BAU Applications Support','BUG') )
       
ORDER BY 
       'DueSoon?',
       CASE 
              WHEN i.duedate < (GETDATE()) THEN i.duedate
              WHEN i.duedate between GETDATE () AND (GETDATE() + 28) THEN i.duedate
              ELSE NULL
       END,
       PriorityDesc, 
       CASE 
              WHEN isl.statusdesc IN ('Pending Closure', 'For Release') THEN 1 
              WHEN isl.statusdesc LIKE ('%UAT%') THEN 2 
              WHEN isl.statusdesc LIKE ('%Testing%') THEN 3 
              WHEN isl.statusdesc IN ('Logged with supplier', 'waiting on customer') THEN 4 
              WHEN isl.statusdesc IN ('In Progress', 'Development') THEN 5 
              WHEN isl.statusdesc IN ('Development Pending', 'Design Pending') THEN 6 
              WHEN isl.statusdesc LIKE ('%analysis%') THEN 7
              ELSE 99 
       END, 
       Created
