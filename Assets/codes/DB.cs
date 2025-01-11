using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class SQLiteManager : MonoBehaviour
{
    private string dbPath;

    void Start()
    {
        dbPath = "URI=file:" + Application.streamingAssetsPath + "/MyDatabase.db";

        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();
            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Area (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        area_name TEXT
                    )";
                command.ExecuteNonQuery();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Punishment (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        code_name TEXT,
                        description TEXT
                    )";
                command.ExecuteNonQuery();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Bonus (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        code_name TEXT,
                        description TEXT
                    )";
                command.ExecuteNonQuery();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS News (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT,
                        icon_path TEXT,
                        description TEXT
                    )";
                command.ExecuteNonQuery();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Source_level (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        level INTEGER,
                        icon_path TEXT
                    )";
                command.ExecuteNonQuery();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Loot (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        code_name TEXT
                    )";
                command.ExecuteNonQuery();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Node (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        description TEXT,
                        source_level_id INTEGER,
                        area_id INTEGER,
                        loot_id INTEGER,
                        FOREIGN KEY (source_level_id) REFERENCES Source_level(id),
                        FOREIGN KEY (area_id) REFERENCES Area(id),
                        FOREIGN KEY (loot_id) REFERENCES Loot(id)
                    )";
                command.ExecuteNonQuery();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Edge (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        start_node_id INTEGER,
                        end_node_id INTEGER,
                        FOREIGN KEY (start_node_id) REFERENCES Node(id),
                        FOREIGN KEY (end_node_id) REFERENCES Node(id)
                    )";
                command.ExecuteNonQuery();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Linked_list_node (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        edge_id INTEGER,
                        FOREIGN KEY (edge_id) REFERENCES Edge(id)
                    )";
                command.ExecuteNonQuery();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Linked_list_edge (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        start_liked_list_node_id INTEGER,
                        end_liked_list_node_id INTEGER,
                        FOREIGN KEY (start_liked_list_node_id) REFERENCES Linked_list_node(id),
                        FOREIGN KEY (end_liked_list_node_id) REFERENCES Linked_list_node(id)
                    )";
                command.ExecuteNonQuery();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Path (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        start_node_id INTEGER,
                        FOREIGN KEY (start_node_id) REFERENCES Node(id)
                    )";
                command.ExecuteNonQuery();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Map (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        start_node_id INTEGER,
                        start_path_id INTEGER,
                        FOREIGN KEY (start_node_id) REFERENCES Node(id),
                        FOREIGN KEY (start_path_id) REFERENCES Path(id)
                    )";
                command.ExecuteNonQuery();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Event (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT,
                        description TEXT,
                        count_edges_to_appear INTEGER,
                        area_id INTEGER,
                        punishment_id INTEGER,
                        bonus_id INTEGER,
                        FOREIGN KEY (area_id) REFERENCES Area(id),
                        FOREIGN KEY (punishment_id) REFERENCES Punishment(id),
                        FOREIGN KEY (bonus_id) REFERENCES Bonus(id)
                    )";
                command.ExecuteNonQuery();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Sources (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        title TEXT,
                        description TEXT,
                        code_name TEXT
                    )";
                command.ExecuteNonQuery();
            }

            using (var command = connection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Studied_sources (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        source_id INTEGER,
                        FOREIGN KEY (source_id) REFERENCES Sources(id)
                    )";
                command.ExecuteNonQuery();
            }

            Debug.Log("All tables created successfully.");
        }
    }

    public Source GetSourceLevelById(int id)
    {
        Source source = null;

        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // SQL-запрос для извлечения записи по id
                command.CommandText = "SELECT * FROM Sources WHERE id = @id";
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read()) // Если запись найдена
                    {
                        Debug.Log($"source was found!!!!");
                        source = new Source
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Title = reader.GetString(reader.GetOrdinal("title")),
                            Description = reader.GetString(reader.GetOrdinal("description")),
                            CodeName = reader.GetString(reader.GetOrdinal("code_name"))
                        };
                    }
                }
            }
        }

        return source;
    }

    public void AddStudiedSource(int sourceId)
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();

            using (var command = connection.CreateCommand())
            {
                // SQL-запрос для добавления записи в таблицу Studied_sources
                command.CommandText = "INSERT INTO Studied_sources (source_id) VALUES (@source_id)";
                command.Parameters.AddWithValue("@source_id", sourceId);

                // Выполнение запроса
                command.ExecuteNonQuery();
            }
        }

        Debug.Log($"Record with source_id {sourceId} added to Studied_sources.");
    }
}

public class Source
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string CodeName { get; set; }
}