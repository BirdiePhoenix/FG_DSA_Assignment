# REPORT.md - Individual Assignment

Keep this concise and specific to your implementation.

## 1. Initial analysis
- How are locations and connections represented in the baseline?
    - Locations:
      Locations are represented by an id, its name, its position and a location type.
    - Connections:
      Connections are represented by a fromLocationId, a toLocationId and a travelCost.
    - Both the locations and the connections are being stored in arrays which are being initialized inside WorldData,
      and both the locations and the connections are being created inside WorldDatasetFactory.
        - To get the locations and the connections, for-loops are being used, which are O(n) operations. The location loop
          compares the IDs and return the right location, while the connection loop is comparing fromLocationIds and adds
          all to a list.
- Which operations appear frequent?
    - The comparision of location id and connection fromId.
- Which operation(s) do you predict may scale poorly?
    - Both searching of location and connection may scale poorly, especially the searching for connection, since
      we always have to loop through the whole list.
- What will you investigate first?
    - The first thing I will investigate is if there are more efficient ways to store the data, since this may lead me
      to which search operation will be most efficient.

## 2. Data representation

Describe important changes/retained choices, operations affected, and time/memory trade-offs.

- Changed Location storage from list to dictionary
    - By changing the data structure to a dictionary, finding the location will go from an O(n)
      operation to an O(1) operation.
    - After implementing this the baseline workload, elapsed time went down to 398ms.
- Changed Connection storage from list to dictionary with lists
    - As with the change of location storage, the operation goes from O(n) to O(1). By sorting all the connections
      into lists of FromLocationId I've removed the need of looping through all connections during lookup, instead
      there'll be a smaller amount of data to work with and potentially loop through.
    - Here the elapsed time to look up went down to 0ms.
        - One issue that can occur is if there's a lot of connections. The dictionary may become quite large,
          but since the world creation time only grew from 1 ms to 3 ms, this will be worth the cost of making the lookup
          instant instead.

## 3. Algorithms and adaptation

- The DijkstraRoutFinder
  - The algorithm starts with checking whether the start or goal has any connections inside the connection dictionary in 
    WorldData. If neither has any, we know that the goal cannot be reached from the starting location.
  - During the algorithm we're constantly checking whether the current calculated cost to each node is lower than the 
    previously calculated value. If the newer value is lower, we update the value inside the dictionary that stores the
    lowest costs.
  - Once we have gone through all the nodes connected to the current node, we move on to the next.
  - To know which node to check next we're using a MinPriorityQueue which ensures that we can access the node 
    with the lowest travel cost. Since we're looking for the cheapest path, it's better to always move on to the node 
    with the lowest currentCost first. This makes sure that in case we find the goal that way, we can discard all paths 
    that would create a more expensive route.
  - When the cheapest route has been found, the algorithm creates a list containing the nodes along the path to the goal.
    During the algorithm loop a dictionary containing all nodes and which node they descended from is being 
    updated. We then follow the road back from the goal to the start, and add all the nodes to a list, which we then 
    reverse.
- ReachabilityQuery
  - The ReachabilityQuery works similarly to Dijkstra, but since we're looking for all locations that we can reach
    without exceeding the given budget, we only update the lowestCost dictionary if the lowest cost is lower than the
    maxTravelCost. 
  - But the idea of having a starting node and then checking the cost of each connected node is still the same.

## 4. Complexity

| Operation | Your representation/algorithm | Complexity | Short explanation |
| --- | --- | --- | --- |
| Find location by ID |Dictionary/Instant lookup by index |Expected to be O(1) on average |At setup loops through the array of locations and then adds to a Dictionary. Requires more memory, but faster lookup. |
| Get outgoing connections |Dictionary with Lists/Instant lookup by index, followed by iteration |O(1) on average but in worst case O(n) |At setup loops through the array of connections and then adds to a Dictionary of Lists. Instead of looping through all connections, a shorter list needs to be looped through|
| Priority queue enqueue |List/Adds and rearrange if needed |Expected to be O(log n) |Adds element to the end of a List, and then sift it up until its priority is no longer smaller than its parents priority.|
| Priority queue dequeue-min |List/Removes top and then rearrange |Expected to be O(log n) |Removes the top item from the queue, and then replaces it with the item on the last index. The last index is removed and then the top is sifted down until its priority is no longer larger than its childrens priorities |
| Route finding |Dictionaries and MinPriorityQueue/ Dijkstra's algorithm |Expected to be O((V+E)log V) (V = vertices/E = edges) |Complexity depends on the amount of nodes and connections that need to be checked before finding the shortest path to the goal |

## 5. Correctness and testing
-Test SiftUp
Important tests/edge cases and what they demonstrate.
-MinPriorityQueue
    -Implemented the test PriorityQueue_EnqueueDequeue_ReturnsInRightOrder() to make sure that the algorithm's
     SiftDown method works, and that it always returns the lowest value even after removing items.
-ReachabiltyQuery and Dijkstra
    -Implemented a test to check if the algorithms could handle if the start location lacked connections and one to 
     check whether the goal location lacked connections (for Dijkstra). This is useful in case there's a location that 
     cannot always be reached or, like in my case, to make sure that the algorithms can handle locations without 
     connections.

## 6. Performance investigation
-Sorting of Locations and Connections:
 The original measurements were:
    - Baseline Workload:
      Locations: 2500
      Connections: 9800
      Iterations: 5000
      Elapsed world time: 1ms
      Elapsed lookup: 417ms
      Checksum: 6,287,100
      Allocation: 440,000
 
  To lower the elapsed time of the lookup of locations and connections I decided to remake them from Arrays to 
  Dictionaries. By doing so the lookup time should go down since the lookup should go from O(n) to O(1).
  After changing the Location storage into a dictionary the lookup time went down to 398ms, and after also changing 
  the Connection storage the lookup time went down to 0ms. The time for creating the world only went up from 1ms to 3ms.
  As suspected, I ended up with O(1) lookup complexity, and since the time to build the world only increased by 2ms this 
  change was worth it.
  The difference in allocated bytes goes from 440,000 to 0. This means that after the changes, the number of allocated 
  bytes doesn't increase during the query loop, but stays the same.


 Updated measurements:
    - Baseline Workload:
      Locations: 2500
      Connections: 2500
      Iterations: 5000
      Elapsed world time: 3ms
      Elapsed lookup: 0ms
      Checksum: 6,287,100
      Allocation: 0

## 7. Development reflection

-One bug I stumbled upon was in the ReachabilityQuery. In one of my tests, I checked whether the dictionary returned by 
 the method contains the right amount of elements but at first it added all existing nodes, even if they exceeded the 
 budget. The first thing I did was to redo my calculations of how many nodes that could be reached within the given 
 budget, but they were correct. The second thing I did was to investigate the code. After a long time of investigation I
 finally found that I had written the check for if an element should be added to the lowestCost Dictionary wrong. 
 Instead of writing:
 --if (currentBest <= maxTravelCost && (!lowestCosts.TryGetValue(locationId, out int known) || currentBest < known)), 
 I had written:
 --if (!lowestCosts.TryGetValue(locationId, out int known) || currentBest < known && currentBest <= maxTravelCost). 
 This made it so the elements could be added, even if they exceeded the maxTravelCost. The correct solution ensures that
 the currentBest has to be lower than the maxTravelCost to be able to be added, and at least one of the other
 conditions. I think a mistake like this is hard to find by testing. Sure, I tried to move some variables, most of the 
 things I considered doing, I knew wouldn't help.
 Overall I think most of the bugs I encounter are caused by simple errors like this. This project has taught me to
 be more aware of how I write the code, and also that it's okay to ask for a second opinion, especially when I'm staring
 myself blind at small writing mistakes. 

## 8. Tools and AI assistance

-I have used very little AI assistance, mostly to get feedback on my working code to see if there's better ways of
 writing it. Overall I think it's best not to use it during your education, rather than trying to solve things on your 
 own, or by asking fellow students for opinions. But I have also used it on some occasions when I've been unsure whether 
 my code works as intended. But I do realize that it's better to perform tests in those situations, like the one I did 
 for the ReachabilityQuery.

## 9. VG extension

Only if attempting VG: heuristic, traversal constraints, correctness, and Dijkstra/A* comparison.
