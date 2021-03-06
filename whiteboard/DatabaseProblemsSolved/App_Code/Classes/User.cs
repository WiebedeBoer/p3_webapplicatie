﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using WebMatrix.Data;


/// <summary>
/// User class aanmaken
/// </summary>
public class User
{
    /// <summary>
    /// Database
    /// </summary>
    private Database db;
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="db">De database connectie</param>
    public User(Database db)
    {
        this.db = db;
    }
    /// <summary>
    /// Registreer een gebruiker
    /// </summary>
    /// <param name="firstName">Voornaam</param>
    /// <param name="lastName">Achternaam</param>
    /// <param name="email">E-mail</param>
    /// <param name="pass">Wachtwoord</param>
    /// <param name="activated">Geactiveerd</param>
    /// <param name="studieid">StudieID</param>
    /// <returns>Error of succesvol</returns>
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
                    int userid = int.Parse(db.QueryValue(query2, firstName, lastName, email.ToLower(), BCrypt.Net.BCrypt.HashPassword(pass), activated, studieid));
                    db.Query("INSERT INTO enrolled (userType, CourseID, UserID, status) SELECT @0, Id, @1, @2 FROM courses WHERE studieID=@3", 0, userid, 0, studieid);
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
    public string GetUserType(int type)
    {
        switch (type)
        {
            case 0:
                return "Student";
            case 1:
                return "Docent";
            case 2:
                return "Admin";
        }
        return "";
    }
    public string RegisterAdmin(string firstName, string lastName, string email, string pass, int permission, int activated, int studieID)
    {
        EmailAddressAttribute evalidator = new EmailAddressAttribute();
        if (evalidator.IsValid(email))
        {

            string query1 = "SELECT email FROM admins WHERE email=@0";
            dynamic result = db.Query(query1, email.ToLower());
            if (result.Count == 0)
            {
                if (studieID == 0)
                {
                    string query2 = "INSERT INTO admins (firstname, lastname, email, password, usertype, status) VALUES (@0, @1, @2, @3, @4, @5)";
                    db.Execute(query2, firstName, lastName, email.ToLower(), BCrypt.Net.BCrypt.HashPassword(pass), permission, activated);
                }
                else
                {
                    string query2 = "INSERT INTO admins (firstname, lastname, email, password, usertype, studieID, status) OUTPUT Inserted.Id VALUES (@0, @1, @2, @3, @4, @5, @6)";
                    int userid = int.Parse(db.QueryValue(query2, firstName, lastName, email.ToLower(), BCrypt.Net.BCrypt.HashPassword(pass), permission, studieID, activated).ToString());
                    db.Query("INSERT INTO enrolled (userType, CourseID, UserID, status) SELECT @0, Id, @1, @2 FROM courses WHERE studieID=@3", permission, userid, 0, studieID);
                }

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
                    query1 = "SELECT admins.Id AS Id, firstName, lastName, email, usertype, studieID, status, studies.naam AS studyName, usertype FROM admins INNER JOIN studies ON admins.studieID=studies.Id WHERE admins.Id=@0";
                else
                    query1 = "SELECT users.Id AS Id, firstName, lastName, email, studieID, status, studies.naam AS studyName, 0 AS usertype FROM users INNER JOIN studies ON users.studieID=studies.Id WHERE users.Id=@0";
                dynamic result = db.Query(query1, uid);
                if (result.Count == 0)
                {
                    if (email.Split('@')[1] == "nhl.nl")
                        query1 = "SELECT Id, firstName, lastName, email, usertype, studieID, status, usertype FROM admins WHERE users.Id=@0";
                    else
                        query1 = "SELECT Id, firstName, lastName, email, studieID, status AS studyName, 0 AS usertype FROM users WHERE users.Id=@0";
                }
                return result[0];
            }
        }
        return "Failed!";
    }
}

