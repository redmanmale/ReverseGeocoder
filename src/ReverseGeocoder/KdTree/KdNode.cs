namespace ReverseGeocoding.KdTree
{
    internal class KdNode<T> where T : IKdComparator<T>
    {
        public readonly KdNode<T> Left;
        public readonly T Location;
        public readonly KdNode<T> Right;

        public KdNode(KdNode<T> left, KdNode<T> right, T location)
        {
            Left = left;
            Right = right;
            Location = location;
        }
    }
}
