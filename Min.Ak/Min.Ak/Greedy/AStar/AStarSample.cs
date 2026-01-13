using Min.Ak.Model.Geo;
using Min.Ak.Model.GraphTheory;

namespace Min.Ak.Greedy.AStar;

internal static class AStarSample
{
    public static void RunSample()
    {
        const int INF = int.MaxValue;

        // A* for train travel times between German cities
        // cities in order of indices:
        string[] names =
        [
            "OS",  "HB",   "HH",   "LG",   "UE",   "BS",   "HI",   "GÖ",   "ELZ",  "MI",   "NI",   "VER",  "H"
        ];
        // travel times in minutes by train between the cities (indices correspond to the names array, INF for no direct connection)
        DistanceMatrix<int> travelTimeMinMatrix = DistanceMatrix.Create(
        [
            [INF,    51,    INF,    INF,    INF,    INF,    INF,    INF,    INF,     39,     INF,    INF,    INF],
            [ 51,   INF,     52,    INF,    INF,    INF,    INF,    INF,    INF,    INF,     INF,     20,     59],
            [INF,    52,    INF,     28,    INF,    INF,    INF,    INF,    INF,    INF,     INF,     71,     75],
            [INF,   INF,     28,    INF,     14,    INF,    INF,    INF,    INF,    INF,     INF,    INF,    INF],
            [INF,   INF,    INF,     14,    INF,    110,    INF,    INF,    INF,    INF,     INF,    139,     41],
            [INF,   INF,    INF,    INF,    110,    INF,     25,    INF,    INF,    INF,     INF,    INF,     31],
            [INF,   INF,    INF,    INF,    INF,     25,    INF,     28,    INF,    INF,     INF,    INF,     25],
            [INF,   INF,    INF,    INF,    INF,    INF,     28,    INF,     50,    INF,     INF,    INF,     34],
            [INF,   INF,    INF,    INF,    INF,    INF,    INF,     50,    INF,    111,     INF,    INF,     21],
            [ 39,   INF,    INF,    INF,    INF,    INF,    INF,    INF,    111,    INF,      48,    INF,     32],
            [INF,   INF,    INF,    INF,    INF,    INF,    INF,    INF,    INF,     48,     INF,     14,     26],
            [INF,    20,     71,    INF,    139,    INF,    INF,    INF,    INF,    INF,      14,    INF,    INF],
            [INF,    59,     75,    INF,     41,     31,     25,     34,     21,     32,      26,    INF,    INF],
        ], INF);
        // geographic coordinates of the cities (latitude, longitude) in order of indices
        GeoCoordinates[] geoCoordinates =
        [
            GeoCoordinates.CreateChecked(52.2728, 8.0587),
            GeoCoordinates.CreateChecked(53.0831, 8.8112),
            GeoCoordinates.CreateChecked(53.5528, 10.0048),
            GeoCoordinates.CreateChecked(53.2391, 10.3875),
            GeoCoordinates.CreateChecked(52.9694, 10.5511),
            GeoCoordinates.CreateChecked(52.2525, 10.5360),
            GeoCoordinates.CreateChecked(52.1597, 9.9502),
            GeoCoordinates.CreateChecked(51.5366, 9.9246),
            GeoCoordinates.CreateChecked(52.1203, 9.7443),
            GeoCoordinates.CreateChecked(52.3061, 8.8250),
            GeoCoordinates.CreateChecked(52.6451, 9.2143),
            GeoCoordinates.CreateChecked(52.9208, 9.2357),
            GeoCoordinates.CreateChecked(52.3765, 9.7388),
        ];
        // precompute distances between all pairs of cities (we really only need the distances to the target, but this is simpler)
        float[,] distances = new float[names.Length, names.Length];
        for (int from = 0; from < names.Length; ++from)
        {
            for (int to = 0; to < names.Length; ++to)
            {
                distances[from, to] = geoCoordinates[from].DistanceTo(geoCoordinates[to]);
            }
        }
        // we are operating on travel times in minutes, but we only know distances in kilometers for the heuristic
        // to avoid missing a better path due to overestimated travel times, we need to find the maximum speed observed in the travel time matrix
        // we'll use that to estimate the minimum possible travel time between any two cities
        float maxSpeedKph = 0f;
        for (int from = 0; from < travelTimeMinMatrix.Size; ++from)
        {
            for (int to = 0; to < travelTimeMinMatrix.Size; ++to)
            {
                float timeMins = travelTimeMinMatrix[from, to];
                if (timeMins is INF or 0)
                {
                    continue;
                }
                float timeHours = timeMins / 60f;
                float distanceKm = distances[from, to];
                float speedKph = distanceKm / timeHours;
                if (speedKph > maxSpeedKph)
                {
                    maxSpeedKph = speedKph;
                }
            }
        }
        // heuristic function estimating travel time in minutes between two cities based on straight-line distance and maximum speed
        int travelTimeHeuristic(int fromIndex, int toIndex)
        {
            float distanceKm = distances[fromIndex, toIndex];
            float estimatedTimeHours = distanceKm / maxSpeedKph;
            int estimatedTimeMins = (int)Math.Ceiling(estimatedTimeHours * 60f);
            return estimatedTimeMins;
        }
        // should find the same path as Dijkstra from OS to LG, but with potentially fewer explored nodes
        AStarSolution<int>? result = AStarSolver.Solve
        (
            names,
            // distance in minutes of travel time by train
            travelTimeMinMatrix,
            start: "OS",
            target: "LG",
            travelTimeHeuristic
        );

        Console.WriteLine(result?.ToString() ?? "No path found.");
    }
}
