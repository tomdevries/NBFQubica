SELECT playername, count(1) as aantal_strikes 
FROM nbf.game g
,    nbf.frame f
,    nbf.bowl b
where b.frameid = f.id
and   f.gameid = g.id
and b.isstrike is true
and g.startdatetime >= '2017-03-01' 
and g.startdatetime < '2017-04-01' 
group by g.playername
order by aantal_strikes DESC
limit 10