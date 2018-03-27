using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using WebMatrix.Data;


/// <summary>
/// Summary description for User
/// </summary>
public class User
{
    private Database db;
    public User(Database db)
    {
        this.db = db;
    }
    public string RegisterUser(string firstName, string lastName, string email, string pass, int activated, int studieid)
    {
        try
        {
            EmailAddressAttribute evalidator = new EmailAddressAttribute();
            if (evalidator.IsValid(email))
            {

                string query1 = "SELECT email FROM users WHERE email=@0";
                dynamic result = db.Query(query1, email.ToLower());
                if (result.Count == 0)
                {

                    string query2 = "INSERT INTO users (firstname, lastname, email, password, status, studieID) VALUES (@0, @1, @2, @3, @4, @5)";
                    db.Execute(query2, firstName, lastName, email.ToLower(), BCrypt.Net.BCrypt.HashPassword(pass), activated, studieid);
                    return "Success!";
                }
                else
                {
                    return "E-mailadres is is ongeldig of bestaat al in onze database!";
                }
            }
            else
            {
                return "E-mailadres is is ongeldig of bestaat al in onze database!";
            }
        }
        catch (Exception e)
        {
            return e.ToString();
        }
    }
    public string RegisterAdmin(string firstName, string lastName, string email, string pass, int permission, int activated)
    {
            EmailAddressAttribute evalidator = new EmailAddressAttribute();
            if (evalidator.IsValid(email))
            {

                string query1 = "SELECT email FROM admins WHERE email=@0";
                dynamic result = db.Query(query1, email.ToLower());
                if (result.Count == 0)
                {

                    string query2 = "INSERT INTO admins (firstname, lastname, email, password, usertype, status) VALUES (@0, @1, @2, @3, @4, @5)";
                    db.Execute(query2, firstName, lastName, email.ToLower(), BCrypt.Net.BCrypt.HashPassword(pass), permission, activated);
                    return "Success!";
                }
                else
                {
                    return "E-mailadres is is ongeldig of bestaat al in onze database!";
                }
            }
            else
            {
                return "E-mailadres is is ongeldig of bestaat al in onze database!";
            }
        
    }
    public string LogIn(string email, string pass)
    {
        EmailAddressAttribute evalidator = new EmailAddressAttribute();
        if (evalidator.IsValid(email))
        {
            string query1;
            if (email.Split('@')[1] == "nhl.nl")
                query1 = "SELECT Id, email, password FROM admins WHERE email=@0";
            else
                query1 = "SELECT Id, email, password FROM users WHERE email=@0";
            dynamic result1 = db.Query(query1, email.ToLower());
            if (result1.Count == 1)
            {
                if (BCrypt.Net.BCrypt.Verify(pass, result1[0][2]))
                {
                    HttpContext.Current.Session["cuid"] = result1[0][0];
                    HttpContext.Current.Session["cuemail"] = email;
                    return "Succesvol!";
                }
                else
                {
                    return "E-mail/wachtwoord incorrect!";
                }
            }
            else
            {
                return "E-mail/wachtwoord incorrect!";
            }

        }
        else
        {
            return "Email is ongeldig!";
        }
    }
    public bool IsLoggedIn()
    {
        if (HttpContext.Current.Session["cuid"] != null)
        {
            if (HttpContext.Current.Session["cuemail"] != null)
            {
                EmailAddressAttribute evalidator = new EmailAddressAttribute();
                if (evalidator.IsValid(HttpContext.Current.Session["cuemail"]))
                {
                    string query1;
                    string email = HttpContext.Current.Session["cuemail"].ToString();
                    string id = HttpContext.Current.Session["cuid"].ToString();
                    if (email.Split('@')[1] == "nhl.nl")
                        query1 = "SELECT Id, email, password FROM admins WHERE Id=@0";
                    else
                        query1 = "SELECT Id, email FROM users WHERE Id=@0";
                    dynamic result1 = db.Query(query1, HttpContext.Current.Session["cuid"]);
                    if (result1.Count == 1)
                    {
                        return true;
                    }
                }
            }

        }
        return false;

    }
    public dynamic getUser()
    {
        if (IsLoggedIn()) {
            EmailAddressAttribute evalidator = new EmailAddressAttribute();
            string email = HttpContext.Current.Session["cuemail"].ToString();
            int uid = int.Parse(HttpContext.Current.Session["cuid"].ToString());
            if (evalidator.IsValid(email))
            {
                string query1;
                if (email.Split('@')[1] == "nhl.nl")
                    query1 = "SELECT Id, firstName, lastName, email, usertype, status FROM admins WHERE Id=@0";
                else
                    query1 = "SELECT Id, firstName, lastName, email, studieID, status FROM users WHERE Id=@0";
                return db.Query(query1, uid);
            }
            } 
       throw new ArgumentException("User is not logged in!");

    }
}

