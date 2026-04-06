using Microsoft.Data.Sqlite;

namespace MLOpsRoadmap.Api;

public record ProgressRecord(string ModuleId, bool Completed, string UpdatedAt);

public class ProgressService
{
    private readonly string _connectionString;

    public ProgressService(IConfiguration configuration)
    {
        var dbPath = configuration["Database:Path"] ?? "progress.db";
        _connectionString = $"Data Source={dbPath}";
    }

    public void Initialize()
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        conn.Execute(@"
            CREATE TABLE IF NOT EXISTS progress (
                module_id TEXT PRIMARY KEY,
                completed INTEGER NOT NULL DEFAULT 0,
                updated_at TEXT NOT NULL
            )");
    }

    public List<ProgressRecord> GetAll()
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT module_id, completed, updated_at FROM progress";
        using var reader = cmd.ExecuteReader();
        var results = new List<ProgressRecord>();
        while (reader.Read())
        {
            results.Add(new ProgressRecord(
                reader.GetString(0),
                reader.GetInt32(1) == 1,
                reader.GetString(2)
            ));
        }
        return results;
    }

    public void Upsert(string moduleId, bool completed)
    {
        using var conn = new SqliteConnection(_connectionString);
        conn.Open();
        using var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            INSERT INTO progress (module_id, completed, updated_at)
            VALUES ($moduleId, $completed, $updatedAt)
            ON CONFLICT(module_id) DO UPDATE SET
                completed = excluded.completed,
                updated_at = excluded.updated_at";
        cmd.Parameters.AddWithValue("$moduleId", moduleId);
        cmd.Parameters.AddWithValue("$completed", completed ? 1 : 0);
        cmd.Parameters.AddWithValue("$updatedAt", DateTime.UtcNow.ToString("o"));
        cmd.ExecuteNonQuery();
    }
}

internal static class SqliteConnectionExtensions
{
    public static void Execute(this SqliteConnection conn, string sql)
    {
        using var cmd = conn.CreateCommand();
        cmd.CommandText = sql;
        cmd.ExecuteNonQuery();
    }
}
