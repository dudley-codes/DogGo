select o.Id,
o.Email,
o.Name,
o.Address,
o.Phone,
n.Name AS Neighborhood
from Owner o
join Neighborhood n on o.NeighborhoodId = n.Id 