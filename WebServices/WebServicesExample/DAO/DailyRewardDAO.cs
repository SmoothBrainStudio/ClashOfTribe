using MySql.Data.MySqlClient;

namespace WebServicesExample.DAO
{
    public class DailyRewardDAO
    {

        private MySqlConnection connection;
        public DailyRewardDAO()
        {
            string connectionString = "SERVER=127.0.0.1; DATABASE=clashoftribes_db; UID=root; PASSWORD=root";
            this.connection = new MySqlConnection(connectionString);
        }


        public bool AddDailyReward(DailyReward dailyReward)
        {
            try
            {
                this.connection.Open();
                MySqlCommand cmd = this.connection.CreateCommand();
                
                cmd.CommandText = "INSERT INTO dailyreward (idplayer, lasttimesincecollected) VALUES (@idplayer, @lasttimesincecollected)";
                cmd.Parameters.AddWithValue("@idplayer", dailyReward.IdPlayer);
                cmd.Parameters.AddWithValue("@lasttimesincecollected", dailyReward.LastTimeSinceCollected);

                cmd.ExecuteNonQuery();
                this.connection.Close();
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("fail to add a new dailyscore");
                return false;
            }
            return true;
        }

        public DailyReward GetDailyRewardByPlayerId(int playerId)
        {
            DailyReward result = new DailyReward();
            try
            {
                string query =
                "SELECT idplayer, lasttimesincecollected " +
                "FROM dailyreward " +
                "WHERE idplayer = @idplayer";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@idplayer", playerId);

                connection.Open();

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    try
                    {
                        while (reader.Read())
                        {
                            result.IdPlayer = reader.GetInt32(0);
                            result.LastTimeSinceCollected = reader.GetDateTime(1);
                        }
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine("fail to get the score with player id: "+ playerId + " from the DB");
                    }
                }

                this.connection.Close();
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("fail to create get score request for player id: " + playerId);
            }

            return result;
        }
        
        public bool UpdateDailyReward(DailyReward dailyReward)
        {
            try
            {
                this.connection.Open();
                MySqlCommand cmd = this.connection.CreateCommand();

                cmd.CommandText =
                    "UPDATE dailyreward " +
                    "SET lasttimesincecollected = @lasttimesincecollected " +
                    "WHERE playerId = @playerId";
                cmd.Parameters.AddWithValue("@lasttimesincecollected", dailyReward.LastTimeSinceCollected);
                cmd.Parameters.AddWithValue("@playerId", dailyReward.IdPlayer);

                cmd.ExecuteNonQuery();
                this.connection.Close();

                return true;
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("fail to update score with player id: " + dailyReward.IdPlayer);
                return false;
            }
        }

        public bool DeleteDailyRewardWithPlayerId(int playerId)
        {
            try
            {
                this.connection.Open();
                MySqlCommand cmd = this.connection.CreateCommand();

                cmd.CommandText =
                    "Delete From dailyreward " +
                    "WHERE playerId = @playerId";
                cmd.Parameters.AddWithValue("@playerId", playerId);

                cmd.ExecuteNonQuery();
                this.connection.Close();

                return true;
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("fail to delete score with player id: " + playerId);
                return false;
            }
        }
    }
}
