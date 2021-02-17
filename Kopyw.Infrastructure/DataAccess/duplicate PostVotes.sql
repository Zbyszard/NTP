select PostId, UserId, count(*) num
from PostVotes
group by PostId, UserId
having count(*) > 1; 

WITH cte AS (
  SELECT PostId, UserId, 
     row_number() OVER(PARTITION BY PostId, UserId ORDER BY Id) AS [rn]
  FROM PostVotes
)
DELETE cte WHERE [rn] > 1

select * from PostVotes;