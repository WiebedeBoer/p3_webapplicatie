using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.Data;

/// <summary>
/// Summary description for DBHandler
/// </summary>
public class DBHandler
{
    private Database db;
    public DBHandler()
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\whiteboard.mdf;Integrated Security=True";

        string provider = "System.Data.SqlClient";

        db = Database.OpenConnectionString(connectionString, provider);
    }
    public Database getConnection()
    {
        return db;
    }
}