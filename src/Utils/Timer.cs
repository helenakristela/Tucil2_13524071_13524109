using System.Diagnostics;

public class Timer
{
    private Stopwatch stopwatch;

    public Timer()
    {
        stopwatch = new Stopwatch();
    }

    public void Start()
    {
        stopwatch.Restart();
    }

    public void Stop()
    {
        stopwatch.Stop();
    }

    public long ElapsedMilliseconds()
    {
        return stopwatch.ElapsedMilliseconds;
    }

    public double ElapsedSeconds()
    {
        return stopwatch.Elapsed.TotalSeconds;
    }
}