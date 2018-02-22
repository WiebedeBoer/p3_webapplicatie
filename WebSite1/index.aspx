@{
var db = Database.Open("C:\Users\Fam. de Boer\source\repos\WebSite1\App_Data\Database.mdf"); 
var selectQueryString = "SELECT * FROM Courses ORDER BY Naam"; 
}
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
    <table> 
<tr>
<th>Id</th> 
<th>Naam</th> 
<th>Vakcode</th> 
</tr>
@foreach(var row in db.Query(selectQueryString))
{
<tr> 
<td>@row.Id</td> 
<td>@row.Naam</td> 
<td>@row.Vakcode</td> 
</tr> 
}
</table>
</body>
</html>
