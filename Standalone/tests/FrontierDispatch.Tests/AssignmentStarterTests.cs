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
        [Test]
        public void PriorityQueue_EnqueueAndDequeue_ReturnsLowestPriorityFirst()
        {
            var queue = new MinPriorityQueue<string>();
            queue.Enqueue("expensive", 9);
            queue.Enqueue("cheap", 2);
            queue.Enqueue("medium", 5);
            Assert.That(queue.Dequeue().Item, Is.EqualTo("cheap"));
        }

        [Test]
		public void PriorityQueue_EnqueueDequeue_SameValuesReturnsValue()
		{
			var queue = new MinPriorityQueue<string>();
			queue.Enqueue("A", 5);
			queue.Enqueue("B", 4);
			queue.Enqueue("C", 1);
			queue.Enqueue("D", 1);
			Assert.That(queue.Dequeue().Item, Is.EqualTo("C"));
		}

		[Test]
		public void PriorityQueue_EnqueueDequeue_ReturnsInRightOrder()
		{
			var queue = new MinPriorityQueue<string>();
			queue.Enqueue("MidHighest", 6);
			queue.Enqueue("MidLowest", 3);
			queue.Enqueue("Highest", 8);
			queue.Enqueue("Lowest", 2);
			queue.Enqueue("Mid", 4);
			Assert.That(queue.Dequeue().Item, Is.EqualTo("Lowest"));
			Assert.That(queue.Dequeue().Item, Is.EqualTo("MidLowest"));
			Assert.That(queue.Dequeue().Item, Is.EqualTo("Mid"));
			Assert.That(queue.Dequeue().Item, Is.EqualTo("MidHighest"));
			Assert.That(queue.Dequeue().Item, Is.EqualTo("Highest"));
		}
		
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

		[Test]
		public void Dijkstra_StartNotFoundOrNoConnections()
		{
			var finder = new DijkstraRouteFinder(SampleWorldFactory.CreateSmall());
			RouteResult result = finder.FindRoute(402, 101);
			Assert.That(result.Found, Is.False);
		}

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
			var query = new ReachabilityQuery(SampleWorldFactory.CreateSmall());
			var result = query.FindReachable(901, 8); 
			Assert.That(result.Count, Is.EqualTo(4));
		}

		[Test]
		public void Reachability_StartNotFoundOrNoConnections()
		{
			var query = new ReachabilityQuery(SampleWorldFactory.CreateSmall());
			var result = query.FindReachable(402, 100);
			Assert.That(result.ContainsKey(402));
		}
    }
}
