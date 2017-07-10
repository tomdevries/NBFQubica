SELECT PLAYERNAME, total FROM nbf.game
where startdatetime >= '2017-03-01'
and   startdatetime < '2017-04-01'
order by total DESC
limit 10;