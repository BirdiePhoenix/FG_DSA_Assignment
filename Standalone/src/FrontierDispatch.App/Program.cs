using System;
using System.Collections.Generic;
using System.Diagnostics;
using FrontierDispatch.Core;

if (args.Length > 0 && args[0] == "--baseline")
{
    RunBaseline();
    return;
}

// Small sanity-check application.
// This is separate from the larger performance workload below.
WorldData world = SampleWorldFactory.CreateSmall();
var queries = new WorldQueries(world);

Console.WriteLine("Frontier Dispatch - assignment starter");
Console.WriteLine($"Small demo locations: {world.Locations.Count}");
Console.WriteLine($"Find 734: {queries.FindLocationById(734)?.Name ?? "missing"}");
Console.WriteLine();
Console.WriteLine("Run with --baseline to record the supplied baseline workload.");

static void RunBaseline()
{
    // Build the dataset before timing the repeated runtime queries.
    // This baseline therefore measures query work, not world construction.
    var swM = Stopwatch.StartNew();
    WorldData world = WorldDatasetFactory.CreateMedium();
    swM.Stop();
    var queries = new WorldQueries(world);

    
    // The checksum provides a stable sanity check that the workload still
    // produces equivalent results after implementation changes.
    int checksum = 0;

    const int iterations = 5000;

    var sw = Stopwatch.StartNew();

    for (int i = 0; i < iterations; ++i)
    {
        // Deterministically visit many different location IDs rather than
        // repeatedly querying the same location.
        int id = (i * 997) % world.Locations.Count;

        Location? location = queries.FindLocationById(id);
        checksum += location?.Id ?? 0;
        checksum += queries.FindOutgoingConnections(id).Count;
    }

    sw.Stop();

    WorldData routeWorld = SampleWorldFactory.CreateSmall();
    ReachabilityQuery reachability = new ReachabilityQuery(routeWorld);
    int checksum2 = 0;

    var swR = Stopwatch.StartNew();

    for (int i = 0; i < iterations; ++i)
    {
        foreach (KeyValuePair<int, Location> location in routeWorld.Locations)
        {
            reachability.FindReachable(location.Key, 100);
            checksum2++;
        }
    }
    swR.Stop();

    DijkstraRouteFinder dijkstra = new DijkstraRouteFinder(routeWorld);
    int checksum3 = 0;
    
    var swD = Stopwatch.StartNew();

    for (int i = 0; i < iterations; ++i)
    {
        foreach (KeyValuePair<int, Location> location in routeWorld.Locations)
        {
            dijkstra.FindRoute(101, location.Key);
            checksum3++;
        }
    }
    
    swD.Stop();

    Console.WriteLine("Baseline world-query workload");
    Console.WriteLine($"Locations: {world.Locations.Count}");
    Console.WriteLine($"Connections: {world.Connections.Count}");
    Console.WriteLine($"Iterations: {iterations}");
    Console.WriteLine($"Elapsed world: {swM.ElapsedMilliseconds} ms");
    Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");
    Console.WriteLine($"Checksum: {checksum}");
    Console.WriteLine($"Rout finding time: {swR.ElapsedMilliseconds} ms");
    Console.WriteLine($"Rout checksum: {checksum2}");
    Console.WriteLine($"Dijkstra time: {swD.ElapsedMilliseconds} ms");
    Console.WriteLine($"Dijkstra checksum: {checksum3}");
    Console.WriteLine("Absolute time varies by machine. Keep this result for before/after comparison.");
}
