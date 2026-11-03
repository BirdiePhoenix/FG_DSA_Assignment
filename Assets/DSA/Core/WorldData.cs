using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontierDispatch.Core
{
    public sealed class WorldData
    {
        private readonly Dictionary<int, Location> _locations;
        private readonly Connection[] _connections;

        public WorldData(IEnumerable<Location> locations, IEnumerable<Connection> connections)
        {
            if (locations == null) throw new ArgumentNullException(nameof(locations));
            if (connections == null) throw new ArgumentNullException(nameof(connections));
            _locations = new List<Location>(locations).ToDictionary(keySelector: l => l.Id, elementSelector: l => l);
            _connections = new List<Connection>(connections).ToArray();
        }

        public IReadOnlyDictionary<int, Location> Locations => _locations;
        public IReadOnlyList<Connection> Connections => _connections;
    }
}
