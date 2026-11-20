using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontierDispatch.Core
{
    
    public sealed class ReachabilityQuery
    {
        private readonly WorldData _world;

        public ReachabilityQuery(WorldData world)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
        }

        public IReadOnlyDictionary<int, int> FindReachable(int startLocationId, int maxTravelCost)
        {
            Dictionary<int, int> lowestCosts = new Dictionary<int, int>();
            MinPriorityQueue<int> frontier = new MinPriorityQueue<int>();
            
            lowestCosts[startLocationId] = 0;
            if (!_world.Connections.TryGetValue(startLocationId, out var list))
            {
                return lowestCosts;
            }
            frontier.Enqueue(startLocationId, 0);

            while (frontier.Count > 0) 
            {
                MinPriorityQueue<int>.Entry entry = frontier.Dequeue();
                int entryId = entry.Item;
                
                if ( entry.Priority != lowestCosts[entryId])
                {
                    continue;
                }
                List<Connection> currentConnections = _world.Connections[entryId];

                foreach (Connection connection in currentConnections) 
                {
                    int currentBest = lowestCosts[entryId] + connection.TravelCost;
                    int locationId = connection.ToLocationId;
                    
                    if (currentBest <= maxTravelCost && 
                        (!lowestCosts.TryGetValue(locationId, out int known) || currentBest < known))
                    {
                        lowestCosts[locationId] = currentBest;
                        frontier.Enqueue(locationId, lowestCosts[locationId]);
                    }
                }
            }

            return lowestCosts;
        }
    }
}
