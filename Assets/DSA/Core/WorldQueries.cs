#nullable enable

using System;
using System.Collections.Generic;

namespace FrontierDispatch.Core
{
    public sealed class WorldQueries
    {
        private readonly WorldData _world;

        public WorldQueries(WorldData world)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
        }

        public Location? FindLocationById(int id)
        {
            _world.Locations.TryGetValue(id, out var location);
            return location;
        }

        public IReadOnlyList<Connection>? FindOutgoingConnections(int fromLocationId)
        {
            _world.Connections.TryGetValue(fromLocationId, out var connections);
            return connections;
        }
    }
}