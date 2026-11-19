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
            List<int> locationIds = new List<int>();
            int expandedCount = 0;
            int currentBest = 0;

            lowestCosts[startLocationId] = 0;
            frontier.Enqueue(startLocationId, lowestCosts[startLocationId]);
            
            while (frontier.Count > 0)
            {
                MinPriorityQueue<int>.Entry entry = frontier.Dequeue();
                int entryId = entry.Item;

                if (!lowestCosts.TryGetValue(entryId, out int bestCost) || entry.Priority != bestCost)
                {
                    continue;
                }
                
                expandedCount++;
                
                if (entryId == goalLocationId)
                {
                    locationIds = AddRoute(cameFrom, startLocationId, goalLocationId);
                    currentBest = lowestCosts[goalLocationId];
                    return new RouteResult(true, locationIds, currentBest, expandedCount);
                }
                
                List<Connection> currentConnections = _world.Connections[entryId];

                foreach (Connection connection in currentConnections) 
                {
                    currentBest = lowestCosts[entryId] + connection.TravelCost;
                    int locationId = connection.ToLocationId;
                    
                    if (!lowestCosts.TryGetValue(locationId, out int known) || currentBest < known)
                    {
                        lowestCosts[locationId] = currentBest;
                        cameFrom[locationId] = entryId;
                        frontier.Enqueue(locationId, lowestCosts[locationId]);
                    }
                }
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
            locationIds.Reverse();

            return locationIds;
        }
    }
}
