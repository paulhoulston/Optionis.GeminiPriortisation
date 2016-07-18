SELECT username
FROM issueresource ir
JOIN users u ON ir.userid = u.userid
WHERE ir.issueid = @issueId