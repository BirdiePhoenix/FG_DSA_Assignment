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
            int currentBest = 0;

            lowestCosts[startLocationId] = 0;
            frontier.Enqueue(startLocationId, lowestCosts[startLocationId]);
            
            while (frontier.Count > 0)
            {
                int lastBestCost = frontier.Peek().Priority;
                int currentId = frontier.Dequeue();

                if (!lowestCosts.TryGetValue(currentId, out int bestCost) || lastBestCost != bestCost)
                {
                    continue;
                }
                
                expandedCount++;
                
                if (currentId == goalLocationId)
                {
                    locationIds = AddRoute(cameFrom, startLocationId, goalLocationId);
                    currentBest = lowestCosts[goalLocationId];
                    return new RouteResult(true, locationIds, currentBest, expandedCount);
                }
                
                List<Connection> currentConnections = _world.Connections[currentId];

                foreach (Connection connection in currentConnections) 
                {
                    if(visited.Contains(connection.ToLocationId)) continue;
                    currentBest = lowestCosts[currentId] + connection.TravelCost;
                    
                    if (!lowestCosts.TryGetValue(connection.ToLocationId, out int known) || currentBest < known)
                    {
                        lowestCosts[connection.ToLocationId] = currentBest;
                        cameFrom[connection.ToLocationId] = currentId;
                        frontier.Enqueue(connection.ToLocationId, lowestCosts[connection.ToLocationId]);
                    }
                }
                
                visited.Add(currentId);
            }
            return new RouteResult(false, locationIds, currentBest, expandedCount);
        }

        private List<int> AddRoute(Dictionary<int, int> cameFrom, int startLocationId, int goalLocationId)
        {
            int locationBacking = goalLocationId;
            List<int> locationIds = new List<int>();

            while (true)
            {
                locationIds.Add(locationBacking);
                if(locationBacking == startLocationId) break;
                    
                locationBacking = cameFrom[locationBacking];
            }

            return locationIds;
        }
    }
}
