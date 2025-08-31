namespace Backend.Application.Common;

public static class Geo
{
    private const double EarthRadiusKm = 6371.0;

    public static double HaversineKm(double lat1, double lon1, double lat2, double lon2)
    {
        double ToRad(double d) => d * Math.PI / 180.0;
        var dLat = ToRad(lat2 - lat1);
        var dLon = ToRad(lon2 - lon1);
        lat1 = ToRad(lat1); lat2 = ToRad(lat2);

        var a = Math.Pow(Math.Sin(dLat / 2), 2) +
                Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dLon / 2), 2);
        var c = 2 * Math.Asin(Math.Sqrt(a));
        return EarthRadiusKm * c;
    }
}
