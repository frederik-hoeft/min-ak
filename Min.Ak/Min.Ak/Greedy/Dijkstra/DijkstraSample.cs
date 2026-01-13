using Min.Ak.Model.GraphTheory;

namespace Min.Ak.Greedy.Dijkstra;

internal static class DijkstraSample
{
    public static void RunSample()
    {
        const int INF = int.MaxValue;
        DijkstraSolution<int>? result = DijkstraSolver.Solve
        (
            names:
            [
                "OS",  "HB",   "HH",   "LG",   "UE",   "BS",   "HI",   "GÖ",   "ELZ",  "MI",   "NI",   "VER",  "H"
            ],
            DistanceMatrix.Create(
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
            ], INF),
            start: "OS",
            target: "LG"
        );

        Console.WriteLine(result?.ToString() ?? "No path found.");
    }
}
