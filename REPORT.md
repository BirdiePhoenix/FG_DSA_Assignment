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
    - Locations:
      Locations are represented by an id, it's name, it's position and a location type.
    - Connections:
      Connections are being represented by a fromLocationId, a toLocationId and a travelCost.
    - Both the locations and the connections are being stored in arrays which are being initialized inside WorldData,
      and both the locations and the connection are being created inside WorldDatasetFactory.
        - To get the locations and the connection for-loops are being used, which are O(n) operations. The location loop
          compares the id's and return the right location, while the connection loop is comparing fromLocationId's and adds
          all to a list.

- Which operations appear frequent?
    - The comparing of location id and connection fromId.
- Which operation(s) do you predict may scale poorly?
    - Both the searching of location and connection may scale poorly, especially the searching for connection, since
      we always have to loop through the whole list.
- What will you investigate first?
    - The first thing I will investigate is if there is more efficient ways to store the data, since this may lead me
      to which search operation will be most efficient.

## 2. Data representation

Describe important changes/retained choices, operations affected, and time/memory trade-offs.

- Changed Location storage from list to dictionary
    - By changing the data structure to a dictionary, the operation of finding the location will go from an O(n) operation
      to an O(1) operation.
    - After implementing this the baseline workload, elapsed time went down to 398ms.
- Changed Connection storage from list to dictionary with lists
    - Like with the change of location storage, the operation goes from O(n) to O(1). By sorting all the connections into
      lists of FromLocationId I've removed the need of looping through all the connections during lookup, instead there'll
      be smaller amount of data to work with and potentially loop through.
    - Here the elapsed time to look up went down to 0ms.
    - One issue that can occure is if there's alot of connections, the size of the dictionary will grow quite big, but I
      decided that this will be worth the cost of making the lookup instant instead.

## 3. Algorithms and adaptation

-MinHeap
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
-Test SiftUp
Important tests/edge cases and what they demonstrate.
-MinPriorityQueue
-Enqueue and Dequeue:
-Implemented a test to see if inserting more elements with the same priority works. It does.
-Implemented PriorityQueue_EnqueueDequeue_ReturnsNegativePriority(), which checks if enqueueing elements with
negative priorities will return the element with the lowest negative priority.

## 6. Performance investigation

Record reproducible before/after measurements for loading/preparation, repeated runtime work, and relevant allocation behavior. Explain what changed and why.

## 7. Development reflection

Describe one bug, wrong assumption, failed approach, or surprising result and how you investigated it.
-Missed to return in MinPriorityQueueu if parent where lesser than or equal to it's children
-When testing Dijsktra, it couldn't find the id inside the dictionary (101). = used wrong class
(WorldDatasetFactory/SampleWorldFactory)
-When testing Dijsktra, it couldn't find the id inside the dictionary (205)

## 8. Tools and AI assistance

Briefly describe relevant assistance, if any.
-Used chatgpt to ask about things I hadn't learned about, How to instantiate theMinPriorityQueue<T>

## 9. VG extension

Only if attempting VG: heuristic, traversal constraints, correctness, and Dijkstra/A* comparison.
