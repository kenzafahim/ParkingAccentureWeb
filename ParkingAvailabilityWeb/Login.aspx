<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ParkingAvailabilityWeb.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Sign In</title>
    <link href="Content/Site.css" rel="stylesheet" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <style>

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <section class="cover">
            <div>
                <asp:Image ID="Image1" ImageUrl="~/Images/Accenture-Job.jpg" runat="server" />
                <div class="elements">
                    <div class="intro">
                        <asp:Label ID="Label1" runat="server" Text="Sign In"></asp:Label>
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="txt1 txtstyle" placeholder="UserName"></asp:TextBox>
                        <asp:TextBox ID="TextBox2" runat="server" CssClass="txt2 txtstyle" placeholder="Password" TextMode="Password"></asp:TextBox>
                    <div>
                        <asp:Button ID="Button1" CssClass="btn1 btnstyle" runat="server" Text="Login"></asp:Button>
                        <asp:Button ID="Button2" CssClass="btn2 btnstyle" runat="server" Text="Cancel"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </form>
</body>
</html>
