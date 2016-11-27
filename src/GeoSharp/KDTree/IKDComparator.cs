using System.Collections.Generic;

namespace GeoSharp.KDTree
{
    public enum Axis
    {
        X,
        Y,
        Z
    }

    public interface IKDComparator<T>
    {
        // Return squared distance between one axis only
        double AxisSquaredDistance(T other, Axis axis);

        IComparer<T> Comparator(Axis axis);

        // Return squared distance between current and other
        double SquaredDistance(T other);
    }
}