using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontierDispatch.Core
{
    // Assignment Phase 5. Adapt shortest-path reasoning to a travel-cost budget.

    /// <summary>
    /// Finds locations that can be reached from a starting location without
    /// exceeding a maximum total travel cost.
    ///
    /// This is an adaptation of shortest-path reasoning: instead of searching
    /// for one goal, the query determines the cheapest known cost to every
    /// location that fits within the supplied budget.
    ///
    /// This class is intentionally incomplete in the assignment starter.
    ///
    /// What do we mean by "query"? In this context, a query is simply a one-time search for information.
    /// </summary>
    public sealed class ReachabilityQuery
    {
        private readonly WorldData _world;

        /// <summary>
        /// Creates a reachability query operating on the supplied world.
        /// </summary>
        public ReachabilityQuery(WorldData world)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
        }

        /// <summary>
        /// Returns reachable location IDs mapped to their cheapest known
        /// travel cost from the starting location.
        ///
        /// Only locations whose cheapest cost is within maxTravelCost should
        /// be included. The starting location has cost 0.
        /// </summary>
        public IReadOnlyDictionary<int, int> FindReachable(int startLocationId, int maxTravelCost)
        {
            Dictionary<int, int> reachableLocations = new Dictionary<int, int>();
            Stack<int> locationsToSearch = new Stack<int>();
            locationsToSearch.Push(startLocationId);
            if (!_world.Locations.TryGetValue(startLocationId, out var value) || maxTravelCost == 0)
            {
                reachableLocations.Add(startLocationId, 0);
                return reachableLocations;
            }

            while (locationsToSearch.Count > 0)
            {
                int currentId = locationsToSearch.Pop();
                List<Connection> connections = _world.Connections[currentId];

                foreach (Connection connection in connections)
                {
                    RouteResult cheapestResult = FindRoute(currentId, connection.ToLocationId, maxTravelCost);

                    if (cheapestResult.TotalCost <= maxTravelCost)
                    {
                        int id = connection.ToLocationId;
                        reachableLocations[id] = cheapestResult.TotalCost;
                        locationsToSearch.Push(id);
                    }
                }
            }

            return reachableLocations;
        }
        
        private RouteResult FindRoute(int startLocationId, int goalLocationId, int maxTravelCost)
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

                if (!lowestCosts.TryGetValue(currentId, out int bestCost) || lastBestCost != bestCost || 
                    bestCost > maxTravelCost)
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
            locationIds.Reverse();

            return locationIds;
        }
        
    }
}
