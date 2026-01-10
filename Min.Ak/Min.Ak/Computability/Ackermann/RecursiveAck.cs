using System.Runtime.CompilerServices;

namespace Min.Ak.Computability.Ackermann;

internal static class RecursiveAck
{
    public static int Ack(int n, int m)
    {
        RuntimeHelpers.EnsureSufficientExecutionStack();
        return (n, m) switch
        {
            (0, _) => m + 1,
            (_, 0) => Ack(n - 1, 1),
            _ => Ack(n - 1, Ack(n, m - 1))
        };
    }
}