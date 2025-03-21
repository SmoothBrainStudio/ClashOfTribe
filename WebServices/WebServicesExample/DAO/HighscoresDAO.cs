using MySql.Data.MySqlClient;
using WebServicesExample.Models;

namespace WebServicesExample.DAO
{
    public class HighscoresDAO
    {

        private MySqlConnection connection;
        public HighscoresDAO()
        {
            //127.0.0.1 = localhost , c'est à dire ladresse IP de votre serveur BDD, ou votre nom de domaine, 
            //dans notre cas c'est notre machine
            string connectionString = "SERVER=127.0.0.1; DATABASE=clashoftribes_db; UID=root; PASSWORD=root";
            this.connection = new MySqlConnection(connectionString);
        }


        public bool AddScore(Score score)
        {
            try
            {
                this.connection.Open();
                MySqlCommand cmd = this.connection.CreateCommand();
                // le nom dans la liste de champs doit être identique au nom dans du champ dans la table
                // le nom du paramètre déclaré @paramètre doit être le même que celui qu'on ajoutera avec le AddWithValue.
                cmd.CommandText = "INSERT INTO score (idplayer, value) VALUES ( @idplayer, @value )";
                cmd.Parameters.AddWithValue("@idplayer", score.PlayerId);
                cmd.Parameters.AddWithValue("@value", score.Value);
                cmd.ExecuteNonQuery();
                this.connection.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("fail to add a new player");
                return false;
            }
            return true;
        }

        public Score GetScoreByPlayerId(int playerId)
        {
            Score result = null;
            try
            {
                string query =
                "Select score.value, player.nickname, player.idplayer " +
                "from score " +
                "inner join player on score.idplayer = player.idplayer " +
                "where score.idplayer = @idplayer";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@idplayer", playerId);
                //Ici on ouvre la connexion mais la requête n'est pas encore executée
                connection.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            //Attention, ici nous avons un seul résultat attendu donc la boucle ne devrait normalement
                            //se faire qu'une seule fois
                            result = new Score();
                            //les index suivants, 0,1,2 correspondant à l'ordre demandé dans la requête
                            result.Value = reader.GetInt32(0);
                            result.Name = reader.GetString(1);
                            result.PlayerId = reader.GetInt32(2);
                        }
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine("fail to get the score with player id: " + playerId + " from the DB");
                    }
                }
                this.connection.Close();
            }

            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("fail to create get score request for player id: " + playerId);
            }
            return result;
        }

        public List<Score> GetLeaderboard()
        {
            int size = 8;
            List<Score> result = new List<Score>();
            try
            {
                string query =
                "Select score.value, player.nickname, player.idplayer " +
                "from score " +
                "inner join player on score.idplayer = player.idplayer " +
                "Order by score.value desc " +
                "limit @size";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@size", size);
                //Ici on ouvre la connexion mais la requête n'est pas encore executée
                connection.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    try
                    {
                        int currentRank = 1;
                        while (reader.Read())
                        {
                            //Attention, ici nous avons un seul résultat attendu donc la boucle ne devrait normalement
                            //se faire qu'une seule fois
                            Score tempScore = new Score();
                            //les index suivants, 0,1,2 correspondant à l'ordre demandé dans la requête
                            tempScore.Value = reader.GetInt32(0);
                            tempScore.Name = reader.GetString(1);
                            tempScore.PlayerId = reader.GetInt32(2);
                            tempScore.Rang = currentRank;
                            result.Add(tempScore);
                            currentRank++;
                        }
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine("fail to get the leaderboard from the DB");
                    }
                }
                this.connection.Close();
            }

            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("fail to create get leaderboard request");
            }
            return result;
        }



        public bool UpdateScore(Score score)
        {
            try
            {
                this.connection.Open();
                MySqlCommand cmd = this.connection.CreateCommand();
                cmd.CommandText =
                    "UPDATE score " +
                    "SET value = @value " +
                    "WHERE idplayer = @idplayer";

                cmd.Parameters.AddWithValue("@idplayer", score.PlayerId);
                cmd.Parameters.AddWithValue("@value", score.Value);
                cmd.ExecuteNonQuery();
                this.connection.Close();
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("fail to update score with player id: " + score.PlayerId);
                return false;
            }
        }

        public bool DeleteScoreWithPlayerId(int playerId)
        {
            try
            {
                this.connection.Open();
                MySqlCommand cmd = this.connection.CreateCommand();
                cmd.CommandText =
                    "DELETE FROM score " +
                    "WHERE idplayer = @idplayer";

                cmd.Parameters.AddWithValue("@idplayer", playerId);
                cmd.ExecuteNonQuery();
                this.connection.Close();
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("fail to delete score with player id: " + playerId);
                return false;
            }
        }
    }
}
