using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System.Collections.Generic;
using System.Drawing;
using WebServicesExample.Models;

namespace WebServicesExample.DAO
{
    public class PlayersDAO
    {

        private MySqlConnection connection;
        public PlayersDAO()
        {
            //127.0.0.1 = localhost , c'est à dire ladresse IP de votre serveur BDD, ou votre nom de domaine, 
            //dans notre cas c'est notre machine
            string connectionString = "SERVER=127.0.0.1; DATABASE=clashoftribes_db; UID=root; PASSWORD=root";
            this.connection = new MySqlConnection(connectionString);
        }


        public bool AddPlayer(Player player)
        {
            try
            {
                this.connection.Open();
                MySqlCommand cmd = this.connection.CreateCommand();
                // le nom dans la liste de champs doit être identique au nom dans du champ dans la table
                // le nom du paramètre déclaré @paramètre doit être le même que celui qu'on ajoutera avec le AddWithValue.
                cmd.CommandText = "INSERT INTO player (nickname, golds, magecount, soldiercount, sothiefcount) VALUES (@nickname, @golds, @magecount, @soldiercount, @sothiefcount)";
                cmd.Parameters.AddWithValue("@nickname", player.Nickname);
                cmd.Parameters.AddWithValue("@golds", player.Golds);
                cmd.Parameters.AddWithValue("@magecount", player.MageCount);
                cmd.Parameters.AddWithValue("@soldiercount", player.SoldierCount);
                cmd.Parameters.AddWithValue("@sothiefcount", player.SothiefCount);
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

        public List<Player> GetAllPlayers()
        {
            List<Player> players = new List<Player>();
            string query = @"
            SELECT idplayer, golds, magecount, soldiercount, sothiefcount, nickname 
            FROM player";

            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                connection.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) // Loop through all rows
                    {
                        Player player = new Player
                        {
                            Id = reader.GetInt32("idplayer"),
                            Golds = reader.GetInt32("golds"),
                            MageCount = reader.GetInt32("magecount"),
                            SoldierCount = reader.GetInt32("soldiercount"),
                            SothiefCount = reader.GetInt32("sothiefcount"),
                            Nickname = reader.GetString("nickname")
                        };
                        players.Add(player);
                    }
                }

                connection.Close();
            }
            return players;
        }

        public Player GetPlayerByPlayerId(int playerId)
        {
            Player result = null;
            try
            {
                string query =
                "Select idplayer, golds, magecount, soldiercount, sothiefcount, nickname " +
                "from player " +
                "where idplayer = @idplayer";

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
                            result = new Player();
                            result.Id = reader.GetInt32(0);
                            result.Golds = reader.GetInt32(1);
                            result.MageCount = reader.GetInt32(2);
                            result.SoldierCount = reader.GetInt32(3);
                            result.SothiefCount = reader.GetInt32(4);
                            result.Nickname = reader.GetString(5);
                        }
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine("fail to get the player with id: "+ playerId + " from the DB");
                    }
                }
                this.connection.Close();
            }

            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("fail to create get player request for player id: " + playerId);
            }
            return result;
        }


        public bool UpdatePlayer(Player player)
        {
            try
            {
                this.connection.Open();
                MySqlCommand cmd = this.connection.CreateCommand();
                cmd.CommandText = 
                    "UPDATE player " +
                    "SET golds = @golds , magecount = @magecount , soldiercount = @soldiercount , sothiefcount = @sothiefcount " +
                    "WHERE idplayer = @idplayer";

                cmd.Parameters.AddWithValue("@idplayer", player.Id);
                cmd.Parameters.AddWithValue("@golds", player.Golds);
                cmd.Parameters.AddWithValue("@magecount", player.MageCount);
                cmd.Parameters.AddWithValue("@soldiercount", player.SoldierCount);
                cmd.Parameters.AddWithValue("@sothiefcount", player.SothiefCount);
                cmd.ExecuteNonQuery();
                this.connection.Close();
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("fail to update player with id: " + player.Id);
                return false;
            }
        }

        public bool DeletePlayerWithId(int playerId)
        {
            try
            {
                this.connection.Open();
                MySqlCommand cmd = this.connection.CreateCommand();
                cmd.CommandText =
                    "Delete From player " +
                    "WHERE idplayer = @playerId";

                cmd.Parameters.AddWithValue("@playerId", playerId);
                cmd.ExecuteNonQuery();
                this.connection.Close();
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("fail to delete player with id: " + playerId);
                return false;
            }
        }
    }
}
