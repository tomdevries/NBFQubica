select playername, sum(total)
from
(
  SELECT playername, total,
    (@row:=if(@prev=playername, @row +1, if(@prev:= playername, 1, 1))) rn
  FROM nbf.game t
  CROSS JOIN (select @row:=0, @prev:=null) c
  order by playername, total desc 
) src
where rn <= 2
and  playername is not null
group by playername
order by total DESC, playername
limit 10
