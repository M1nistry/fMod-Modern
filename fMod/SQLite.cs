using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fMod
{
    internal class SQLite
    {
        private static readonly string _dbPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\fMod\ModsDB.db3";

        private static SQLiteConnection Connection { get; set; }

        static SQLite()
        {
            //var constring = $@"Data Source={_dbPath};Version=3;";
            var constring = new SQLiteConnectionStringBuilder
            {
                DataSource = _dbPath,
                Version = 3,
            };
            Connection = new SQLiteConnection(constring.ToString()) {ParseViaFramework = true};
            //
        }

        private void SetupDb()
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\fMod"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\fMod");
                if (!File.Exists(_dbPath)) SQLiteConnection.CreateFile(_dbPath);
            }
            try
            {
                using (var connection = new SQLiteConnection(Connection).OpenAndReturn())
                {
                    using (var cmd = new SQLiteCommand(connection))
                    {
                        cmd.CommandText = @"CREATE TABLE IF NOT EXISTS `mods` (`id` INTEGER PRIMARY KEY AUTOINCREMENT, `url` TEXT, `author` TEXT, `contact` TEXT, `title` TEXT, `name` TEXT, `description` TEXT, `homepage` TEXT);";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = @"CREATE TABLE IF NOT EXISTS `categories` (`id` INTEGER PRIMARY KEY AUTOINCREMENT, `title` TEXT, `name` TEXT);";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = @"CREATE TABLE IF NOT EXISTS `mod_categories` (`idmod` INTEGER, `idcategory` INTEGER);";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = @"CREATE TABLE IF NOT EXISTS `releases` (`id` INTEGER PRIMARY KEY AUTOINCREMENT, `idmod` INTEGER, `version` TEXT, `released` DATETIME);";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText =@"CREATE TABLE IF NOT EXISTS `release_versions` (`idrelease` INTEGER, `idmod` INTEGER);";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = @"CREATE TABLE IF NOT EXISTS `game_versions` (`id` INTEGER PRIMARY KEY AUTOINCREMENT, `version` TEXT)";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = @"CREATE TABLE IF NOT EXISTS `files` (`id` INTEGER PRIMARY KEY AUTOINCREMENT, `idmod` INTEGER, `name` TEXT, `mirror` TEXT, `url` TEXT);";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = @"CREATE TABLE IF NOT EXISTS `log` (`id` INTEGER PRIMARY KEY AUTOINCREMENT, `title` TEXT, `time` DATETIME, `description` TEXT)";
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                
            }
        }

        internal static void InsertLog(Log newLog)
        {
            using (var connection = new SQLiteConnection(Connection).OpenAndReturn())
            {
                const string insertLog = @"INSERT INTO `log` (title, description, time) VALUES (@title, @desc, @time);";
                using (var cmd = new SQLiteCommand(insertLog, connection))
                {
                    cmd.Parameters.AddWithValue("@title", newLog.Title);
                    cmd.Parameters.AddWithValue("@desc", newLog.Description);
                    cmd.Parameters.AddWithValue("@time", newLog.LogTime);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
