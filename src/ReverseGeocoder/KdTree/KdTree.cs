using System;

namespace ReverseGeocoding.KdTree
{
    internal class KdTree<T> where T : IKdComparator<T>
    {
        private readonly KdNode<T> _root;

        public KdTree(T[] items)
        {
            _root = CreateTree(items, 0, items.Length, 0);
        }

        public T FindNearest(T search)
        {
            return FindNearest(_root, search, 0).Location;
        }

        // Only ever goes to log2(items.length) depth so lack of tail recursion is a non-issue
        private KdNode<T> CreateTree(T[] items, int start, int end, int depth)
        {
            if (start >= end)
                return null;

            Array.Sort(items, start, end - start, items[0].Comparator((Axis)(depth % 3)));
            var index = start + (end - start) / 2;
            return new KdNode<T>(CreateTree(items, start, index, depth + 1), CreateTree(items, index + 1, end, depth + 1), items[index]);
        }

        private KdNode<T> FindNearest(KdNode<T> node, T dearch, int depth)
        {
            var direction = dearch.Comparator((Axis)(depth % 3)).Compare(dearch, node.Location);
            var next = direction < 0 ? node.Left : node.Right;
            var other = direction < 0 ? node.Right : node.Left;
            var best = next == null ? node : FindNearest(next, dearch, depth + 1);
            if (node.Location.SquaredDistance(dearch) < best.Location.SquaredDistance(dearch))
            {
                best = node;
            }

            if (other != null)
            {
                if (other.Location.AxisSquaredDistance(dearch, (Axis)(depth % 3)) < best.Location.SquaredDistance(dearch))
                {
                    var possibleBest = FindNearest(other, dearch, depth + 1);
                    if (possibleBest.Location.SquaredDistance(dearch) < best.Location.SquaredDistance(dearch))
                    {
                        best = possibleBest;
                    }
                }
            }

            return best;
        }
    }
}
