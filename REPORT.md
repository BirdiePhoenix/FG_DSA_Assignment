# REPORT.md - Individual Assignment

Keep this concise and specific to your implementation.

## 1. Initial analysis
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
    - By changing the data structure to a dictionary, the operation of finding the location will go from an O(n)
      operation to an O(1) operation.
    - After implementing this the baseline workload, elapsed time went down to 398ms.
- Changed Connection storage from list to dictionary with lists
    - Like with the change of location storage, the operation goes from O(n) to O(1). By sorting all the connections
      into lists of FromLocationId I've removed the need of looping through all the connections during lookup, instead
      there'll be smaller amount of data to work with and potentially loop through.
    - Here the elapsed time to look up went down to 0ms.
        - One issue that can occur is if there's a lot of connections. The size of the dictionary will grow quite large,
          but since the world creation time only grew from 1 ms to 3 ms, this will be worth the cost of making the lookup
          instant instead.

## 3. Algorithms and adaptation

-MinHeap
Explain your route finder at a useful high level and how the travel-budget query adapts the same ideas.

- The DijkstraRoutFinder
  - The algorithm starts with looking for if the start or goal has any connections inside the connection dictionary in 
    WorldData. If they don't, we know that the goal can't be reached from the start location.
  - During the algorithm we're constantly looking for if the current calculated cost to each node is lower than the 
    previously calculated cost. If the newer value is lower, we update the value inside the dictionary that stores the
    lowest costs.
  - When we have gone through all nodes which the current node is connected to, we move on to the next.
  - To know which node to check next we're using a MinPriorityQueue which makes sure that we can get access to the node 
    with the lowest travel cost. Since we're looking for the cheapest path, it's better to always move on to the node 
    with the lowest currentCost first. This makes sure that in case we find the goal that way, we can discard all paths 
    that would create a more expensive route.
  - When the cheapest route has been found, the algorithm creates a list containing the nodes from the path to the goal.
    During the algorithm loop a dictionary containing all nodes and from which node they descended from is being 
    updated. We then follow the road back from the goal to the start, and add all nodes to a List which we then reverse.
- ReachabilityQuery
  - The ReachabilityQuery works similar to Dijkstra, but since we're looking for all connections that we can reach
    without exceeding the given budget, we only update the lowestCost dictionary if the lowest cost is lower than the
    maxTravelCost. 
  - But the idea of having a starting node and then check the cost of each connected node is still the same.

## 4. Complexity

| Operation | Your representation/algorithm | Complexity | Short explanation |
| --- | --- | --- | --- |
| Find location by ID |Dictionary/Instant lookup by index |Expected to be O(1) on avarage |At setup loops through the array of locations and then adds to a Dictionary. Requires more memory, but faster lookup. |
| Get outgoing connections |Dictionary with Lists/Instant lookup by index, followed by iteration |O(1) on avarage but in worst case O(n) |At setup loops through the array of connections and then adds to a Dictionary of Lists. Instead of looping through all connection, a shorter list needs to be looped through|
| Priority queue enqueue |List/Adds and rearange if needed |Expected to be O(log n) |Adds element to the end of a List, and then sift it up until its priority is no longer smaller than its parents priority.|
| Priority queue dequeue-min |List/Removes top and then rearange |Ecpected to be O(log n) |Removes the top item from the queue, and then replaces it with the item on the last index. The last index is removed and then the top is sifted down until its priority is no longer larger than its childrens priorities |
| Route finding |Dictionaries and MinPriorityQueue/ Dijkstra's algorithm |Expected to be O((V+E)log V) (V = verices/E = edges) |Complexity depends on the amount of nodes and connections that needs to be checked before finding the shortest path to the goal |

## 5. Correctness and testing
-Test SiftUp
Important tests/edge cases and what they demonstrate.
-MinPriorityQueue
    -Implemented the test PriorityQueue_EnqueueDequeue_ReturnsInRightOrder() to make sure that the algorithm's
     SiftDown method works, and that it always returns the lowest value even after removing items.
-ReachabiltyQuery and Dijkstra
    -Implemented a test to check if the algorithms could handle if the start location lacked connections and one if the 
     goal location (for Dijkstra) lacked connections. This is useful in case there's a location that not always can be
     reached or, like in my case, to make sure that the algorithms can handle locations without connections.

## 6. Performance investigation
-Sorting of Locations and Connections:
 The original measurements was:
    - Baseline Workload:
      Locations: 2500
      Connections: 9800
      Iterations: 5000
      Elapsed world time: 1ms
      Elapsed lookup: 417ms
      Checksum: 6,287,100
 
 To lower the elapsed time of the lookup of locations and connections I decided to remake them from Arrays to 
 Dictionaries. By doing so the lookup time should go down since the lookup should go from O(n) to O(1).
 After changing the Location storage into a dictionary the lookup time went down to 398ms, and after also changing 
 the Connection storage the lookup time went down to 0ms. The time for creating the world only went up from 1ms to 3ms.
 As suspected I ended up with O(1) lookup complexity, and since the time to build the world only grew by 2ms this change 
 was worth it.


## 7. Development reflection

Describe one bug, wrong assumption, failed approach, or surprising result and how you investigated it.
-Missed to return in MinPriorityQueue if parent where lesser than or equal to its children
-When testing Dijkstra, it couldn't find the id inside the dictionary (101). = used wrong class
(WorldDatasetFactory/SampleWorldFactory)
-When testing Dijkstra, it couldn't find the id inside the dictionary (205)

## 8. Tools and AI assistance

Briefly describe relevant assistance, if any.
-I used AI assistance at one occasion, at a time when I was home and couldn't reach any of my classmates:
    -I had problems with getting my Dijkstra to work properly so I asked ChatGPT to point out the flaws in the code
     without showing me how to write the code. I did this when I was sick, so I had a hard time thinking clearly and
     to detect smaller flaws in the code.

## 9. VG extension

Only if attempting VG: heuristic, traversal constraints, correctness, and Dijkstra/A* comparison.
