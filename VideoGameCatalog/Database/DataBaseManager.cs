using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoGameCatalog.Models;

namespace VideoGameCatalog.Database
{
    public class DatabaseManager
    {
        private readonly string _connectionString = "Data Source=videogames.db;Version=3;";

        public void InitializeDatabase()
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string createPlataformasTable = @"
                        CREATE TABLE IF NOT EXISTS Plataformas (
                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                            Nombre TEXT NOT NULL
                        );";

                    string createJuegosTable = @"
                    CREATE TABLE IF NOT EXISTS Juegos (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Titulo TEXT NOT NULL,
                        Genero TEXT NOT NULL,
                        Nota REAL,
                        PlataformaId INTEGER NOT NULL,
                        FOREIGN KEY (PlataformaId) REFERENCES Plataformas(Id)
                    );";


                    using (var cmd = new SQLiteCommand(createPlataformasTable, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd = new SQLiteCommand(createJuegosTable, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error inicializar BD: " + ex.Message);
            }
        }


        public List<Plataforma> ObtenerPlataformas()
        {
            var plataformas = new List<Plataforma>();
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT Id, Nombre FROM Plataformas;";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = (int)(long)reader["Id"]; 

                                plataformas.Add(new Plataforma
                                {
                                    Id = id,
                                    Nombre = reader["Nombre"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            return plataformas;
        }


        public void InsertarPlataforma(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("Nombre obligatorio");

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Plataformas (Nombre) VALUES (@nombre);";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        public void ActualizarPlataforma(int id, string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new Exception("Nombre obligatorio");

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Plataformas SET Nombre = @nombre WHERE Id = @id;";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        public void EliminarPlataforma(int id)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();

                    string deleteJuegos = "DELETE FROM Juegos WHERE PlataformaId = @id;";
                    using (var cmd = new SQLiteCommand(deleteJuegos, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }

                    string deletePlataforma = "DELETE FROM Plataformas WHERE Id = @id;";
                    using (var cmd = new SQLiteCommand(deletePlataforma, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }


        public List<Juego> ObtenerJuegosPorPlataforma(int plataformaId)
        {
            var juegos = new List<Juego>();
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string query = "SELECT Id, Titulo, Genero, Nota, PlataformaId FROM Juegos WHERE PlataformaId = @plataformaId;";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@plataformaId", plataformaId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = (int)(long)reader["Id"];
                                int plataforma = (int)(long)reader["PlataformaId"];

                                decimal nota = 0;
                                var notaObj = reader["Nota"];
                                if (notaObj != DBNull.Value)
                                {
                                    decimal.TryParse(notaObj.ToString(), out nota);
                                }

                                juegos.Add(new Juego
                                {
                                    Id = id,
                                    Titulo = reader["Titulo"].ToString(),
                                    Genero = reader["Genero"].ToString(),
                                    Nota = nota,
                                    PlataformaId = plataforma
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            return juegos;
        }


        public void InsertarJuego(string titulo, string genero, decimal nota, int plataformaId)
        {
            if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(genero))
                throw new Exception("Título y Género obligatorios");

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Juegos (Titulo, Genero, Nota, PlataformaId) VALUES (@titulo, @genero, @nota, @plataformaId);";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@titulo", titulo);
                        cmd.Parameters.AddWithValue("@genero", genero);
                        cmd.Parameters.AddWithValue("@nota", nota);
                        cmd.Parameters.AddWithValue("@plataformaId", plataformaId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        public void ActualizarJuego(int id, string titulo, string genero, decimal nota, int plataformaId)
        {
            if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(genero))
                throw new Exception("Título y Género obligatorios");

            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Juegos SET Titulo = @titulo, Genero = @genero, Nota = @nota, PlataformaId = @plataformaId WHERE Id = @id;";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@titulo", titulo);
                        cmd.Parameters.AddWithValue("@genero", genero);
                        cmd.Parameters.AddWithValue("@nota", nota);
                        cmd.Parameters.AddWithValue("@plataformaId", plataformaId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }

        public void EliminarJuego(int id)
        {
            try
            {
                using (var connection = new SQLiteConnection(_connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Juegos WHERE Id = @id;";

                    using (var cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
        }
    }
}
