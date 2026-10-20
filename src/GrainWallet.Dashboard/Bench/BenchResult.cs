namespace GrainWallet.Dashboard.Bench;

/// <summary>Lifecycle of a single benchmark run, in order: Pending, Seeding, Warming, Running, then Completed or Failed.</summary>
public enum BenchStatus
{
    /// <summary>Queued but not started.</summary>
    Pending,
    /// <summary>Seeding the target services with starting data.</summary>
    Seeding,
    /// <summary>Warm-up load before measurement begins.</summary>
    Warming,
    /// <summary>Measured load is in progress.</summary>
    Running,
    /// <summary>Finished and the outcomes are available.</summary>
    Completed,
    /// <summary>Stopped early; see <see cref="BenchRun.Error"/>.</summary>
    Failed,
}

/// <summary>One project's result for one scenario. All latencies are in milliseconds; AvgRps is the achieved rate, not the requested one.</summary>
public sealed record ScenarioOutcome(
    string Project,
    string Scenario,
    long OkCount,
    long FailCount,
    double MeanMs,
    double P50Ms,
    double P95Ms,
    double P99Ms,
    double StdDevMs,
    double AvgRps);

public sealed class BenchRun
{
    public required string Id { get; init; }
    public required DateTimeOffset StartedAt { get; init; }
    public DateTimeOffset? FinishedAt { get; set; }
    public required string[] ProjectNames { get; init; }
    public required string Scenario { get; init; }
    public BenchStatus Status { get; set; } = BenchStatus.Pending;
    /// <summary>Human-readable note on the current status, such as which phase is running.</summary>
    public string? StatusDetail { get; set; }
    /// <summary>Failure message when <see cref="Status"/> is <see cref="BenchStatus.Failed"/>, otherwise null.</summary>
    public string? Error { get; set; }
    /// <summary>One <see cref="ScenarioOutcome"/> per project once the run completes.</summary>
    public List<ScenarioOutcome> Outcomes { get; set; } = [];
    public int WarmUpSeconds { get; init; }
    public int DurationSeconds { get; init; }
    public int RequestsPerSecond { get; init; }

    /// <summary>Absolute path to the per-run folder under the dashboard's reports root. Holds NBomber's HTML/CSV/MD/TXT reports plus summary.json. Set by <see cref="BenchRunner"/> on run start.</summary>
    public string? FolderPath { get; set; }
}
