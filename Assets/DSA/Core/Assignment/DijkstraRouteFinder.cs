using System;
using System.Collections.Generic;

namespace FrontierDispatch.Core
{
    // Assignment Phase 4.

    /// <summary>
    /// Finds minimum-total-cost routes through a Frontier Dispatch world
    /// using Dijkstra's shortest-path algorithm.
    ///
    /// This class is intentionally incomplete in the assignment starter.
    /// It should eventually use the student's prepared world representation
    /// and MinPriorityQueue implementation.
    /// </summary>
    public sealed class DijkstraRouteFinder
    {
        private readonly WorldData _world;

        /// <summary>
        /// Constructor. Creates a route finder operating on the supplied world.
        /// </summary>
        public DijkstraRouteFinder(WorldData world)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
        }

        
        public RouteResult FindRoute(int startLocationId, int goalLocationId)
        {
            // TODO: implement Dijkstra using your prepared world representation
            // and MinPriorityQueue<T>.

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
                List<Connection> currentConnections = _world.Connections[currentId];

                foreach (Connection connection in currentConnections) 
                {
                    if(visited.Contains(connection.ToLocationId)) continue;
                    int currentBest = lowestCosts[currentId] + connection.TravelCost;
                    
                    if (currentBest < lowestCosts[connection.ToLocationId])
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
            
            //Return RouteResult
            return new RouteResult(wasFound, locationIds, totalCost, expandedCount);
        }
    }
}
