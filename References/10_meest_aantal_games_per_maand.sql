select g.playername, count(1) t
from game g
where g.startdatetime >= '2017-03-01'
and g.startdatetime < '2017-04-01'
group by playername
order by t desc
limit 10
