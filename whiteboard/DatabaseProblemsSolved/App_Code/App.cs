using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.Data;

/// <summary>
/// Summary description for App
/// </summary>
public class App
{
    public User user;
    public Database db;

    public App()
    {
        db = new DBHandler().getConnection();
        user = new User(db);

    }
}