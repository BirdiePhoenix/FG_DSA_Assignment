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
            MinPriorityQueue<int> frontier = new MinPriorityQueue<int>();
            HashSet<int> visited = new HashSet<int>();
            int expandedCount = 0;

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
                    if (currentBest < lowestCosts[connection.ToLocationId] ||
                        lowestCosts[connection.ToLocationId] == 0)
                    {
                        lowestCosts[connection.ToLocationId] = currentBest;
                    }
                    
                    frontier.Enqueue(connection.ToLocationId, lowestCosts[connection.ToLocationId]);
                }
                //Check if goal
                //Add route list
                visited.Add(currentId);
                expandedCount++;
            }
            
            //Return RouteResult
            
            throw new NotImplementedException();
        }
    }
}
