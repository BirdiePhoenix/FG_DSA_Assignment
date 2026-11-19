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
		public void PriorityQueue_EnqueueDequeue_SameValuesReturnsValue()
		{
			var queue = new MinPriorityQueue<string>();
			queue.Enqueue("A", 5);
			queue.Enqueue("B", 4);
			queue.Enqueue("C", 1);
			queue.Enqueue("D", 1);
			Assert.That(queue.Dequeue(), Is.EqualTo("C"));
		}

		[Test]
		public void PriorityQueue_EnqueueDequeue_ReturnsNegativePriority()
		{
			var queue = new MinPriorityQueue<string>();
			queue.Enqueue("Positive1", 6);
			queue.Enqueue("Positive2", 3);
			queue.Enqueue("LowestNegative", -8);
			queue.Enqueue("HighestNegative", -3);
			queue.Enqueue("Positive3", 4);
			Assert.That(queue.Dequeue(), Is.EqualTo("LowestNegative"));
		}


        //[Test, Ignore("Phase 4: enable when implementing Dijkstra route finding.")]
        [Test]
        public void Dijkstra_StartEqualsGoal_ReturnsZeroCostRoute()
        {
            var finder = new DijkstraRouteFinder(WorldDatasetFactory.CreateSmall());
            RouteResult result = finder.FindRoute(0, 0);
            Assert.That(result.Found, Is.True);
            Assert.That(result.TotalCost, Is.EqualTo(0));
            Assert.That(result.LocationIds, Is.EqualTo(new[] { 0 }));
        }

		[Test]
		public void Dijkstra_FromFirstToLast_FindShortestPath()
		{
			var finder = new DijkstraRouteFinder(SampleWorldFactory.CreateSmall());
            RouteResult result = finder.FindRoute(101, 901);
            Assert.That(result.Found, Is.True);
			Assert.That(result.TotalCost, Is.EqualTo(9));
            Assert.That(result.LocationIds, Is.EqualTo(new[] { 101, 205, 901 }));
		}

		[Test]
		public void Dijkstra_UnreachableLocation()
		{
			var finder = new DijkstraRouteFinder(SampleWorldFactory.CreateSmall());
			RouteResult result = finder.FindRoute(101, 405);
            Assert.That(result.Found, Is.False);
		}

        //[Test, Ignore("Phase 5: enable when implementing bounded reachability.")]
		[Test] 
        public void Reachability_ZeroBudget_IncludesStartAtZeroCost() 
        {
            var query = new ReachabilityQuery(WorldDatasetFactory.CreateSmall());
            var result = query.FindReachable(0, 0);
            Assert.That(result.ContainsKey(0), Is.True);
            Assert.That(result[0], Is.EqualTo(0));
        }

		[Test]
		public void Reachability_IncludesRightValues()
		{
			var query = new ReachabilityQuery(WorldDatasetFactory.CreateSmall());
			var result = query.FindReachable(310, 10); 
			Assert.That(result, Is.EqualTo(new[] { {310, 0}, {} }));
		}
    }
}
