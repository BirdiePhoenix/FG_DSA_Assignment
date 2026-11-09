using System;
using NUnit.Framework;
using FrontierDispatch.Core;
using System.Collections.Generic;

namespace FrontierDispatch.Tests
{
    /// <summary>
    /// Small contract examples for later assignment phases.
    ///
    /// These are not an exhaustive assignment test suite.
    /// Students are expected to enable the relevant tests as they reach each
    /// phase and add meaningful tests of their own, including edge cases.
    /// </summary>
    public sealed class AssignmentStarterTests
    {
        // These tests document later assignment contracts without making the
        // untouched baseline red. Remove Ignore when you reach the named phase.

        //[Test, Ignore("Phase 3: enable when implementing the binary min-heap priority queue.")]
        [Test]
        public void PriorityQueue_EnqueueAndDequeue_ReturnsLowestPriorityFirst()
        {
            var queue = new MinPriorityQueue<string>();
            queue.Enqueue("expensive", 9);
            queue.Enqueue("cheap", 2);
            queue.Enqueue("medium", 5);
            Assert.That(queue.Dequeue(), Is.EqualTo("cheap"));
        }

        [Test]
        public void PriorityQueue_EnqueueAndDequeue_SiftUpCorrect()
        {
            var queue = new MinPriorityQueue<string>();
            queue.Enqueue("expensive", 8);
            queue.Enqueue("cheap", 3);
            queue.Enqueue("medium", 5);
            queue.Enqueue("mid-cheap", 4);

            var priorities = new List<string>();
            while (queue.Count > 0)
            {
                priorities.Add(queue.Dequeue());
            }
            
            Assert.That(priorities, Is.EqualTo(new[] { "cheap", "mid-cheap", "medium", "expensive" }));
        }
        
        [Test]
        public void PriorityQueue_EnqueueAndDequeue_SiftDownCorrect()
        {
            var queue = new MinPriorityQueue<string>();
            queue.Enqueue("expensive", 15);
            queue.Enqueue("medium", 10);
            queue.Enqueue("cheap", 6);
            queue.Enqueue("mid-expensive", 12);
            queue.Enqueue("mid-cheap", 7);

            queue.Dequeue();

            var priorities = new List<string>();
            while (queue.Count > 0)
            {
                priorities.Add(queue.Dequeue());
            }
            
            Assert.That(priorities, Is.EqualTo(new[] { "mid-cheap", "mid-expensive", "medium", "expensive" }));
        }

        [Test, Ignore("Phase 4: enable when implementing Dijkstra route finding.")]
        public void Dijkstra_StartEqualsGoal_ReturnsZeroCostRoute()
        {
            var finder = new DijkstraRouteFinder(WorldDatasetFactory.CreateSmall());
            RouteResult result = finder.FindRoute(0, 0);
            Assert.That(result.Found, Is.True);
            Assert.That(result.TotalCost, Is.EqualTo(0));
            Assert.That(result.LocationIds, Is.EqualTo(new[] { 0 }));
        }

        [Test, Ignore("Phase 5: enable when implementing bounded reachability.")]
        public void Reachability_ZeroBudget_IncludesStartAtZeroCost()
        {
            var query = new ReachabilityQuery(WorldDatasetFactory.CreateSmall());
            var result = query.FindReachable(0, 0);
            Assert.That(result.ContainsKey(0), Is.True);
            Assert.That(result[0], Is.EqualTo(0));
        }
    }
}
