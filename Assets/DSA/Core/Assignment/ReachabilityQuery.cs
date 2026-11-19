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
            frontier.Enqueue(startLocationId, 0);

            while (frontier.Count > 0) 
            {
                MinPriorityQueue<int>.Entry entry = frontier.Dequeue();
                
                if ( entry.Priority != lowestCosts[entry.Item])
                {
                    continue;
                }
                List<Connection> currentConnections = _world.Connections[entry.Item];

                foreach (Connection connection in currentConnections) 
                {
                    int currentBest = lowestCosts[entry.Item] + connection.TravelCost;
                    
                    if (currentBest <= maxTravelCost && 
                        (!lowestCosts.TryGetValue(connection.ToLocationId, out int known) || currentBest < known))
                    {
                        lowestCosts[connection.ToLocationId] = currentBest;
                        frontier.Enqueue(connection.ToLocationId, lowestCosts[connection.ToLocationId]);
                    }
                }
            }

            return lowestCosts;
        }
    }
}
