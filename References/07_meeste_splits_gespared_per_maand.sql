select g.playername, b.total, b.pins
from game g
,    frame f
,    bowl b
where g.id = f.gameid
and   b.frameid = f.id
and (b.total = 10)
and g.startdatetime >= '2017-03-01'
and g.startdatetime < '2017-04-01'
and pins = '7,10'
group by playername
