using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontierDispatch.Core
{
    public sealed class WorldData
    {
        private readonly Dictionary<int, Location> _locations;
        private readonly Dictionary<int, List<Connection>> _connections;

        public WorldData(IEnumerable<Location> locations, IEnumerable<Connection> connections)
        {
            if (locations == null) throw new ArgumentNullException(nameof(locations));
            if (connections == null) throw new ArgumentNullException(nameof(connections));
            _locations = new List<Location>(locations).ToDictionary(keySelector: l => l.Id, elementSelector: l => l);
            _connections = new Dictionary<int, List<Connection>>();
            SortConnectionLists(connections);
        }

        private void SortConnectionLists(IEnumerable<Connection> connections)
        {
            foreach (Connection connection in connections) 
            {
                if (!Connections.ContainsKey(connection.FromLocationId))
                {
                    _connections.Add(connection.FromLocationId, new List<Connection>());
                }
                
                _connections[connection.FromLocationId].Add(connection);
            }
        }

        public IReadOnlyDictionary<int, Location> Locations => _locations;
        public IReadOnlyDictionary<int, List<Connection>> Connections => _connections;
    }
}
