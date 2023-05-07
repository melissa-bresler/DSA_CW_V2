using System;
using static System.Collections.Specialized.BitVector32;


namespace Tube_Walking_Guide
{
    internal class PriorityQueue
    {
        private Station[] heap;
        public int Size { get; set; }

        public PriorityQueue()
        {
            heap = new Station[5];
        }
        public void Enqueue(Station vertex, int[] distances)
        {
            int distanceTravelled = distances[vertex.StationID];
            if (Size == heap.Length)
            {
                heap = Utils.ExtendStationArray(heap);
            }
            heap[Size] = vertex;
            int i = Size;
            Size++;
            while (i > 0)
            {
                int j = (i - 1) / 2;
                if (distances[heap[j].StationID].CompareTo(distanceTravelled) <= 0)
                {
                    break;
                }
                Swap(i, j);
                i = j;
            }
        }

        public Station Dequeue(int[] distances)
        {
            Station shortestRoute = heap[0];
            Size--;
            if (Size > 0)
            {
                heap[0] = heap[Size];
                int i = 0;
                while (true)
                {
                    int left = 2 * i + 1;
                    if (left >= Size)
                    {
                        break;
                    }
                    int right = left + 1;
                    int minChild = left;
                    if (right < Size && distances[heap[right].StationID].CompareTo(distances[heap[left].StationID]) < 0)
                    {
                        minChild = right;
                    }
                    if (distances[heap[i].StationID].CompareTo(distances[heap[minChild].StationID]) <= 0)
                    {
                        break;
                    }
                    Swap(i, minChild);
                    i = minChild;
                }
            }
            return shortestRoute;
        }

        private void Swap(int i, int j)
        {
            Station temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }
        public bool IsEmpty()
        {
            if (Size == 0)
            {
                return true;
            }
            return false;
        }
    }

}

