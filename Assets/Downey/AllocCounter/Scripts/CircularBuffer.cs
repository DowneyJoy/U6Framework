using System;
using UnityEngine;

public class CircularBuffer<T>
{
    readonly T[] buffer;
    private int head, tail;
    public int Count { get; private set; }
    public int Capacity => buffer.Length;

    public CircularBuffer(int capacity)
    {
        if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        buffer = new T[capacity];
    }

    public void Enqueue(T item)
    {
        buffer[head] = item;
        head = (head + 1) % Capacity;
        if(Count == Capacity) tail = (tail + 1) % Capacity;
        else Count++;
    }

    public T Dequeue()
    {
        if(Count == 0) throw new InvalidOperationException("Buffer is empty");
        var item = buffer[tail];
        tail = (tail + 1) % Capacity;
        Count--;
        return item;
    }
    
    // Access elements by logical index (0 = oldest,Count-1 = newest)
    public T this[int index]
    {
        get
        {
            if((uint)index >= (uint)Count) throw new ArgumentOutOfRangeException(nameof(index));
            if(Capacity == 0 || buffer == null) throw new InvalidOperationException("Buffer is empty");
            return buffer[(tail + index)%Capacity];
        }
    }
}
