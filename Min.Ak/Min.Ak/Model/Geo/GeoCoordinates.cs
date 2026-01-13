namespace Min.Ak.Model.Geo;

internal readonly record struct GeoCoordinates(float Latitude, float Longitude)
{
    public static GeoCoordinates CreateChecked(double latitude, double longitude) =>
        CreateChecked((float)latitude, (float)longitude);

    public static GeoCoordinates CreateChecked(float latitude, float longitude)
    {
        if (latitude is < -90f or > 90f)
        {
            throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90 degrees.");
        }
        if (longitude is < -180f or > 180f)
        {
            throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180 degrees.");
        }
        return new GeoCoordinates(latitude, longitude);
    }

    // Copilot-generated
    public float DistanceTo(GeoCoordinates other)
    {
        // Haversine formula
        const float EARTH_RADIUS_KM = 6371f;
        float dLat = ToRadians(other.Latitude - Latitude);
        float dLon = ToRadians(other.Longitude - Longitude);
        float a = (MathF.Sin(dLat / 2) * MathF.Sin(dLat / 2)) +
                  (MathF.Cos(ToRadians(Latitude)) * MathF.Cos(ToRadians(other.Latitude)) *
                  MathF.Sin(dLon / 2) * MathF.Sin(dLon / 2));
        float c = 2 * MathF.Atan2(MathF.Sqrt(a), MathF.Sqrt(1 - a));
        return EARTH_RADIUS_KM * c;
    }

    private static float ToRadians(float degrees) => degrees * (MathF.PI / 180f);
}