select g.playername, count(1) as missed
from game g
,    frame f
,    bowl b
where g.id = f.gameid
and   b.frameid = f.id
and (isgutter is true or (b.total <> 10 and pins is null))
and g.startdatetime >= '2017-03-01'
and g.startdatetime < '2017-04-01'
group by playername
order by missed
