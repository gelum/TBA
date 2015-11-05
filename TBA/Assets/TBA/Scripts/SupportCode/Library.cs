using System;
using System.Collections;
using System.Collections.Generic;

/*
 * 		Public Enumeration to deal with hex corners elegantly.
 * 
 * 						   < u ;; Up >
 *						       *
 *		  					  ***
 *	       < ul ;; Up Left > ***** < ur ;; Up Right >
 *		  					 ***** 
 *		 < dl ;; Down Left > *****	< dr ;; Down Right >
 *         					  ***
 * 		    				   *
 * 						   < d ;; Down > 
 * 
 * */
public enum CornerDirection { ur, u, ul, dl, d, dr };

/*
 * 		Public Tuple Class
 * 
 * 			Used to store a pair of generic types.
 * 			This code exists in C#, but not in Unity#, which is unfortunate.
 * 
 * */
public sealed class Tuple<T1, T2>
{
	private readonly T1 item1;
	private readonly T2 item2;
	
	/// <summary>
	/// Retyurns the first element of the tuple
	/// </summary>
	public T1 Item1
	{
		get { return item1; }
	}
	
	/// <summary>
	/// Returns the second element of the tuple
	/// </summary>
	public T2 Item2
	{
		get { return item2; }
	}
	
	/// <summary>
	/// Create a new tuple value
	/// </summary>
	/// <param name="item1">First element of the tuple</param>
	/// <param name="second">Second element of the tuple</param>
	public Tuple(T1 item1, T2 item2)
	{
		this.item1 = item1;
		this.item2 = item2;
	}
	
	public override string ToString()
	{
		return string.Format("Tuple({0}, {1})", Item1, Item2);
	}
	
	public override int GetHashCode()
	{
		int hash = 17;
		hash = hash * 23 + (item1 == null ? 0 : item1.GetHashCode());
		hash = hash * 23 + (item2 == null ? 0 : item2.GetHashCode());
		return hash;
	}
	
	public override bool Equals(object o)
	{
		if (!(o is Tuple<T1, T2>)) {
			return false;
		}
		
		var other = (Tuple<T1, T2>) o;
		
		return this == other;
	}
	
	public bool Equals(Tuple<T1, T2> other)
	{
		return this == other;
	}
	
	public static bool operator==(Tuple<T1, T2> a, Tuple<T1, T2> b)
	{
		if (object.ReferenceEquals(a, null)) {
			return object.ReferenceEquals(b, null);
		}
		if (a.item1 == null && b.item1 != null) return false;
		if (a.item2 == null && b.item2 != null) return false;
		return
			a.item1.Equals(b.item1) &&
				a.item2.Equals(b.item2);
	}
	
	public static bool operator!=(Tuple<T1, T2> a, Tuple<T1, T2> b)
	{
		return !(a == b);
	}
	
	public void Unpack(Action<T1, T2> unpackerDelegate)
	{
		unpackerDelegate(Item1, Item2);
	}
}

/*
 * 		Public Tuple Static Library
 * 
 * 			Contains methods required for proper use of the above Tuple Generic Class.
 * 			Again, this code is available in C# but not in Unity#. Go go Unity.
 * 
 * */
public static class Tuple
{
	/// <summary>
	/// Creates a new tuple value with the specified elements. The method
	/// can be used without specifying the generic parameters, because C#
	/// compiler can usually infer the actual types.
	/// </summary>
	/// <param name="item1">First element of the tuple</param>
	/// <param name="second">Second element of the tuple</param>
	/// <returns>A newly created tuple</returns>
	public static Tuple<T1, T2> Create<T1, T2>(T1 item1, T2 second)
	{
		return new Tuple<T1, T2>(item1, second);
	}
	
	/// <summary>
	/// Extension method that provides a concise utility for unpacking
	/// tuple components into specific out parameters.
	/// </summary>
	/// <param name="tuple">the tuple to unpack from</param>
	/// <param name="ref1">the out parameter that will be assigned tuple.Item1</param>
	/// <param name="ref2">the out parameter that will be assigned tuple.Item2</param>
	public static void Unpack<T1, T2>(this Tuple<T1, T2> tuple, out T1 ref1, out T2 ref2)
	{
		ref1 = tuple.Item1;
		ref2 = tuple.Item2;
	}
	
}

/* 
 * 		Public Priority Queue Generic
 * 
 * 			Items are dequeued with highest priority first.
 * 			Internally implemented as a queue of Tuple< T, int> with an int priority.
 * 			This avoids potential solution of maintaining multiple Queues, i.e. one for each priority.
 * 
 * */
public class PriorityQueue<T>
{
	
	private List<Tuple<T, int>> elements = new List<Tuple<T, int>>();
	
	public int Count
	{
		get { return elements.Count; }
	}
	
	public void Enqueue(T item, int priority)
	{
		elements.Add(Tuple.Create(item, priority));
	}
	
	public T Dequeue()
	{
		int bestIndex = 0;
		
		for (int i = 0; i < elements.Count; i++) {
			if (elements[i].Item2 < elements[bestIndex].Item2) {
				bestIndex = i;
			}
		}
		
		T bestItem = elements[bestIndex].Item1;
		elements.RemoveAt(bestIndex);
		return bestItem;
	}
}