select g.playername, count(1) as m
from game g
,    frame f
,    bowl b
where g.id = f.gameid
and   b.frameid = f.id
and b.bowlNumber = 1
and g.startdatetime >= '2017-03-01'
and g.startdatetime < '2017-04-01'
and pins like '%9%'
group by g.playername
order by m desc
limit 10
