using System;
using System.Collections.Generic;

namespace FrontierDispatch.Core
{
    public sealed class DijkstraRouteFinder
    {
        private readonly WorldData _world;

        public DijkstraRouteFinder(WorldData world)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
        }

        
        public RouteResult FindRoute(int startLocationId, int goalLocationId)
        {
            Dictionary<int, int> lowestCosts = new Dictionary<int, int>();
            Dictionary<int, int> cameFrom = new Dictionary<int, int>();
            MinPriorityQueue<int> frontier = new MinPriorityQueue<int>();
            HashSet<int> visited = new HashSet<int>();
            List<int> locationIds = new List<int>();
            int expandedCount = 0;
            int totalCost = 0;
            bool wasFound = false;

            lowestCosts[startLocationId] = 0;
            frontier.Enqueue(startLocationId, lowestCosts[startLocationId]);
            
            while (frontier.Count > 0)
            {
                int currentId = frontier.Dequeue();
                if (currentId == goalLocationId)
                {
                    locationIds.Add(currentId);
                    totalCost = 0;
                    return new RouteResult(true, locationIds, totalCost, expandedCount);
                }
                
                List<Connection> currentConnections = _world.Connections[currentId];

                foreach (Connection connection in currentConnections) 
                {
                    if(visited.Contains(connection.ToLocationId)) continue;
                    int currentBest = lowestCosts[currentId] + connection.TravelCost;
                    lowestCosts[connection.ToLocationId] = 0;
                    
                    if (currentBest < lowestCosts[connection.ToLocationId] || 
                        lowestCosts[connection.ToLocationId] == 0)
                    {
                        lowestCosts[connection.ToLocationId] = currentBest;
                        cameFrom[connection.ToLocationId] = currentId;
                    }

                    if (connection.ToLocationId == goalLocationId)
                    {
                        wasFound = true;
                        totalCost = currentBest;
                        break;
                    }
                    
                    frontier.Enqueue(connection.ToLocationId, lowestCosts[connection.ToLocationId]);
                }
                
                visited.Add(currentId);
                expandedCount++;
                if(wasFound) break;
            }

            int locationBacking = goalLocationId;
            while (true)
            {
                if(!wasFound) break;
                
                locationIds.Add(locationBacking);
                if(locationBacking == startLocationId) break;
                
                locationBacking = cameFrom[locationBacking];
            }
           
            return new RouteResult(wasFound, locationIds, totalCost, expandedCount);
        }
    }
}
