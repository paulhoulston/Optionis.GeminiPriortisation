SELECT userid
	,username
	,firstname
	,surname
FROM users
WHERE userid IN @UserIds
