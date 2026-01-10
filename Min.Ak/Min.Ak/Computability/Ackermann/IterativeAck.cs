namespace Min.Ak.Computability.Ackermann;

internal static class IterativeAck
{
    public static int Ack(int n, int m)
    {
        Stack<Frame> stack = [];
        stack.Push(new Frame(n, m));
        int result = 0;
        while (stack.Count > 0)
        {
            Frame frame = stack.Pop();
            if (frame.N == 0)
            {
                result = frame.M + 1;
            }
            else if (frame.M == 0)
            {
                stack.Push(new Frame(frame.N - 1, 1));
            }
            else
            {
                stack.Push(new Frame(frame.N - 1, 0)); // Placeholder for the result of Ack(n, m - 1)
                stack.Push(new Frame(frame.N, frame.M - 1));
                continue;
            }
            // If we just computed Ack(n, m - 1), we need to update the previous frame
            if (stack.Count > 0 && stack.Peek().M == 0)
            {
                Frame prevFrame = stack.Pop();
                stack.Push(new Frame(prevFrame.N, result));
            }
        }
        return result;
    }

    private readonly record struct Frame(int N, int M);
}