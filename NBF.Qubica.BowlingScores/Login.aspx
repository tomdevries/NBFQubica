﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="NBF.Qubica.BowlingScores.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>

    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <meta name="description" content="">
    <meta name="author" content="">

    <title><asp:Literal ID="web_site_title" runat="server"/></title>

    <!-- Bootstrap Core CSS -->
    <link href="vendor/bootstrap/css/bootstrap.min.css" rel="stylesheet">

    <!-- Custom Fonts -->
    <link href="vendor/font-awesome/css/font-awesome.min.css" rel="stylesheet" type="text/css">
    <link href="https://fonts.googleapis.com/css?family=Montserrat:400,700" rel="stylesheet" type="text/css">
    <link href='https://fonts.googleapis.com/css?family=Kaushan+Script' rel='stylesheet' type='text/css'>
    <link href='https://fonts.googleapis.com/css?family=Droid+Serif:400,700,400italic,700italic' rel='stylesheet' type='text/css'>
    <link href='https://fonts.googleapis.com/css?family=Roboto+Slab:400,100,300,700' rel='stylesheet' type='text/css'>

    <!-- Theme CSS -->
    <link href="css/bowling.min.css" rel="stylesheet">

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    
    <!-- HTML5 Shim IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"    integrity="sha384-0s5Pv64cNZJieYFkXYOTId2HMA2Lfb6q2nAcx2n0RTLUnCAoTTsS0nKEO27XyKcY" crossorigin="anonymous"></script>
        <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js" integrity="sha384-ZoaMbDF+4LeFxg6WdScQ9nnR1QC2MIRxA1O9KWEXQwns1G8UNyIEZIQidzb0T1fo" crossorigin="anonymous"></script>
    <![endif]-->

</head>
<body>
    <!-- Modal inloggen -->
    <div class="portfolio-modal" id="AanmeldenModal" role="dialog" aria-hidden="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <a href="default.aspx#competities">
                <div class="close-modal" data-dismiss="modal">
                    <div class="lr">
                        <div class="rl">
                        </div>
                    </div>
                </div>
                </a>
                <div class="container">
                    <div class="row">
                        <div class="col-lg-8 col-lg-offset-2">
                            <div class="modal-body">
                                <!-- Inlog Details Go Here -->
                                <h2>Log in</h2>
                                <p class="item-intro text-muted">Log in, bekijk je profiel, pas het aan en leg het vast.</p>
                                <form name="sentMessage" id="registerForm" novalidate runat="server">
                                    <div class="row">
                                        <div class="col-lg-12">
                                            <div class="form-group center-input">
                                                <asp:TextBox type="text" class="form-control" placeholder="Je ID-Naam *" id="loginFrequentBowlerNaam" runat="server" style="text-transform: uppercase;"/>
                                                <asp:RequiredFieldValidator class="help-block text-danger" runat="server" ControlToValidate="loginFrequentBowlerNaam" ErrorMessage="Geef je ID-Naam op." />
                                            </div>
                                            <div class="form-group center-input">
                                                <asp:TextBox type="number" class="form-control" placeholder="Je ID-nummer *" id="loginFrequentBowlerNummer" runat="server" />
                                                <asp:RequiredFieldValidator class="help-block text-danger" runat="server" ControlToValidate="loginFrequentBowlerNummer" ErrorMessage="Geef je ID-nummer op." />
                                            </div>
                                            <div class="form-group center-input">
                                                <asp:TextBox type="password" class="form-control" placeholder="Je wachtwoord *" id="loginWachtwoord" runat="server" />
                                                <asp:RequiredFieldValidator class="help-block text-danger" runat="server" ControlToValidate="loginWachtwoord" ErrorMessage="Geef je wachtwoord op." />
                                            </div>
                                            <div class="form-group center-input">
                                                <asp:LinkButton ID="forgotten" runat="server" Text="Wachtwoord vergeten?" onclick="forgotten_Click" CausesValidation="False"/>
                                            </div>
                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="col-lg-12 text-center">
                                            <div id="meldSuccess" runat="server"></div>
                                            <asp:Button type="submit" class="btn btn-xl" Text="Verstuur" runat="server" ID="buttonSubmitForm" OnClick="buttonSubmitForm_Click"/>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>    
</body>
</html>

