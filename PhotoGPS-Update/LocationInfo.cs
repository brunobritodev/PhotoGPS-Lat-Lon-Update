using System.Diagnostics;

[DebuggerDisplay("{latitude},{longitude}")]
public class LocationInfo
{
    public int latitudeE7 { get; set; }
    public int longitudeE7 { get; set; }
    public double latitude => latitudeE7 / 1e7;
    public double longitude => longitudeE7 / 1e7;
    public int accuracy { get; set; }
    public Activity[] activity { get; set; }
    public string source { get; set; }
    public int deviceTag { get; set; }
    public DateTime timestamp { get; set; }
    public DateTime? deviceTimestamp { get; set; }

    public DateTime ObterHorario(TimeSpan localTimeOffset) => deviceTimestamp.HasValue ? deviceTimestamp.Value.Add(localTimeOffset) : timestamp.Add(localTimeOffset);
}

public class Activity
{
    public Activity[] activity { get; set; }
    public DateTime timestamp { get; set; }
    public string type { get; set; }
    public int confidence { get; set; }
}