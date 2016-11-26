namespace GeoSharp.KDTree
{
    public class KDNode<T>
        where T : IKDComparator<T>
    {
        public KDNode<T> Left;
        public T Location;
        public KDNode<T> Right;

        public KDNode(KDNode<T> left, KDNode<T> right, T location)
        {
            this.Left = left;
            this.Right = right;
            this.Location = location;
        }
    }
}