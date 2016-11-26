using System;

namespace GeoSharp.KDTree
{
    public class KDTree<T>
        where T : IKDComparator<T>
    {
        private KDNode<T> Root;

        public KDTree(T[] items)
        {
            Root = CreateTree(items, 0, items.Length, 0);
        }

        public T FindNearest(T search)
        {
            return FindNearest(Root, search, 0).Location;
        }

        // Only ever goes to log2(items.length) depth so lack of tail recursion is a non-issue
        private KDNode<T> CreateTree(T[] items, int start, int end, int depth)
        {
            if (start >= end)
                return null;

            Array.Sort(items, start, end - start, items[0].Comparator((Axis)(depth % 3)));
            int Index = start + ((end - start) / 2);
            return new KDNode<T>(CreateTree(items, start, Index, depth + 1), CreateTree(items, Index + 1, end, depth + 1), items[Index]);
        }

        private KDNode<T> FindNearest(KDNode<T> node, T dearch, int depth)
        {
            int Direction = dearch.Comparator((Axis)(depth % 3)).Compare(dearch, node.Location);
            KDNode<T> Next = (Direction < 0) ? node.Left : node.Right;
            KDNode<T> Other = (Direction < 0) ? node.Right : node.Left;
            KDNode<T> Best = (Next == null) ? node : FindNearest(Next, dearch, depth + 1);
            if (node.Location.SquaredDistance(dearch) < Best.Location.SquaredDistance(dearch))
            {
                Best = node;
            }

            if (Other != null)
            {
                if (Other.Location.AxisSquaredDistance(dearch, (Axis)(depth % 3)) < Best.Location.SquaredDistance(dearch))
                {
                    KDNode<T> PossibleBest = FindNearest(Other, dearch, depth + 1);
                    if (PossibleBest.Location.SquaredDistance(dearch) < Best.Location.SquaredDistance(dearch))
                    {
                        Best = PossibleBest;
                    }
                }
            }

            return Best;
        }
    }
}