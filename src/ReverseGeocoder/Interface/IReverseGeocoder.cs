namespace ReverseGeocoding.Interface
{
    public interface IReverseGeocoder
    {
        IPlaceInfo GetNearestPlace(double latitude, double longitude);
        string GetNearestPlaceName(double latitude, double longitude);
    }
}
