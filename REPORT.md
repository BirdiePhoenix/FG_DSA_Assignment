# REPORT.md - Individual Assignment

Keep this concise and specific to your implementation.

## 1. Initial analysis
- Baseline Workload:
    Locations: 2500
    Connections: 9800
    Iterations: 5000
    Elapsed: 417ms
    Checksum: 6,287,100
- How are locations and connections represented in the baseline?
  - Locations
    Locations are represented by an id, it's name, it's position and a location type.
  - Connections
    Connections are being represented by a fromLocationId, a toLocationId and a travelCost.
  - Both the locations and the connections are being stored in arrays which are being initialized inside WorldData,
    and both the locations and the connection are being created inside WorldDatasetFactory.
    - To get the locations and the connection for-loops are being used, which are O(n) operations. The location loop
      compares the id's and return the right location, while the connection loop is comparing fromLocationId's and adds
      all to a list.
    
- Which operations appear frequent?
  - The comparing of location id and connection from id.
- Which operation(s) do you predict may scale poorly?
  - Both the searching of location and connection may scale poorly, especially the searching for connection, since
    we always have to loop through the whole list. 
- What will you investigate first?
  - The first thing I will investigate is if there is more efficient ways to store the data, since this may lead me
    to wich search operation will be most efficient.

## 2. Data representation

Describe important changes/retained choices, operations affected, and time/memory trade-offs.

## 3. Algorithms and adaptation

Explain your route finder at a useful high level and how the travel-budget query adapts the same ideas.

## 4. Complexity

| Operation | Your representation/algorithm | Complexity | Short explanation |
| --- | --- | --- | --- |
| Find location by ID | | | |
| Get outgoing connections | | | |
| Priority queue enqueue | | | |
| Priority queue dequeue-min | | | |
| Route finding | | | |

## 5. Correctness and testing

Important tests/edge cases and what they demonstrate.

## 6. Performance investigation

Record reproducible before/after measurements for loading/preparation, repeated runtime work, and relevant allocation behavior. Explain what changed and why.

## 7. Development reflection

Describe one bug, wrong assumption, failed approach, or surprising result and how you investigated it.

## 8. Tools and AI assistance

Briefly describe relevant assistance, if any.

## 9. VG extension

Only if attempting VG: heuristic, traversal constraints, correctness, and Dijkstra/A* comparison.
