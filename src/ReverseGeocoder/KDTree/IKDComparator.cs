using System.Collections.Generic;

namespace ReverseGeocoder.KDTree
{
    internal interface IKdComparator<in T>
    {
        // Return squared distance between one axis only
        double AxisSquaredDistance(T other, Axis axis);

        IComparer<T> Comparator(Axis axis);

        // Return squared distance between current and other
        double SquaredDistance(T other);
    }
}
