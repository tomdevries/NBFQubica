<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="NBF.Qubica.BowlingScores.Profile" %>

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
    <!-- Modal Profile -->
    <div class="portfolio-modal" id="ProfileModal" role="dialog" aria-hidden="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <a href="default.aspx">
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
                                <!-- Profile Details Go Here -->
                                <h2>Profiel</h2>
                                <p class="item-intro text-muted">Werk je profiel bij en sla de aanpassing op.</p>
                                <form name="sentMessage" id="registerForm" novalidate runat="server">
                                    <div class="row">
                                       <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:TextBox type="text" class="form-control" placeholder="Je naam" id="profilenaam" runat="server" />
                                            </div>
                                            <div class="form-group">
                                                <asp:TextBox type="text" class="form-control" placeholder="Je adres" id="profileadres" runat="server" />
                                            </div>
                                            <div class="form-group">
                                                <asp:TextBox type="text" class="form-control" placeholder="Je postcode en plaats" id="profileplaats" runat="server" />
                                            </div>
                                            <div class="form-group">
                                                <asp:TextBox type="text" class="form-control" placeholder="Je e-mail adres *" id="profileemail" runat="server" />
                                                <asp:RequiredFieldValidator class="help-block text-danger" runat="server" ControlToValidate="profileemail" ErrorMessage="Geef je e-mail adres op."  Display="Dynamic"/>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:TextBox type="text" class="form-control" placeholder="Je ID-Naam *" id="profilefrequentbowlernaam" runat="server" style="text-transform: uppercase;" readonly/>
                                            </div>
                                            <div class="form-group">
                                                <asp:TextBox type="text" class="form-control" placeholder="Je ID-Nummer *" id="profilefrequentbowlernummmer" runat="server" readonly/>
                                            </div>
                                            <div class="form-group">
                                                <asp:TextBox type="password" class="form-control" placeholder="Je wachtwoord" id="profilewachtwoord" runat="server" />
                                            </div>
                                            <div class="form-group">
                                                <asp:TextBox type="password" class="form-control" placeholder="Bevestig je wachtwoord" id="profilecontrole" runat="server" />
                                            </div>
                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="col-lg-12 text-center">
                                            <div id="profileSuccess" runat="server"></div><br /><br />
                                            <asp:Button type="submit" class="btn btn-xl" Text="Sla op" runat="server" ID="buttonSubmitForm" OnClick="buttonSubmitForm_Click"/>
                                        </div>
                                        <div class="clearfix">&nbsp;</div>
                                        <div class="col-lg-12 text-center">
                                            Klik <asp:LinkButton Text="hier" runat="server" ID="btnRemoveAccount" OnClientClick="return confirm('Weet jezeker dat je jouw account wilt verwijderen?')" OnClick="btnRemoveAccount_Click"/> om je account te verwijderen.
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                    <hr />
                    <div class="row">
                        <%=_competitions%>
                    </div>
                    <hr />
                    <div class="row">
                        <%=_scores%>
                        <!--
                        <div class="col-lg-8 col-lg-offset-2">
                            <h2>Bowling Harderwijk</h2>
                            <div class="row">
                                <div class="col-lg-2">Datum</div>
                                <div class="col-lg-6"></div>
                                <div class="col-lg-2">Hi-Score</div>
                                <div class="col-lg-1">&nbsp;</div>
                                <div class="col-lg-1">&nbsp;</div>
                            </div>
                            <div class="row">
                                <div class="col-lg-2">01-01-2017</div>
                                <div class="col-lg-6">Baan 1</div>
                                <div class="col-lg-2">300</div>
                                <div class="col-lg-1">&nbsp;</div>
                                <div class="col-lg-1">&nbsp;</div>
                            </div>
                            <div class="row">
                                <div class="col-lg-1" style="border-width: 1px 0 0 1px; border-style: solid; border-color: black;">1</div>
                                <div class="col-lg-1" style="border-width: 1px 0 0 1px; border-style: solid; border-color: black;">2</div>
                                <div class="col-lg-1" style="border-width: 1px 0 0 1px; border-style: solid; border-color: black;">3</div>
                                <div class="col-lg-1" style="border-width: 1px 0 0 1px; border-style: solid; border-color: black;">4</div>
                                <div class="col-lg-1" style="border-width: 1px 0 0 1px; border-style: solid; border-color: black;">5</div>
                                <div class="col-lg-1" style="border-width: 1px 0 0 1px; border-style: solid; border-color: black;">6</div>
                                <div class="col-lg-1" style="border-width: 1px 0 0 1px; border-style: solid; border-color: black;">7</div>
                                <div class="col-lg-1" style="border-width: 1px 0 0 1px; border-style: solid; border-color: black;">8</div>
                                <div class="col-lg-1" style="border-width: 1px 0 0 1px; border-style: solid; border-color: black;">9</div>
                                <div class="col-lg-1" style="border-width: 1px 0 0 1px; border-style: solid; border-color: black;">10</div>
                                <div class="col-lg-1" style="border-width: 1px 1px 0 1px; border-style: solid; border-color: black;">Total</div>
                                <div class="col-lg-1" style="border-width: 0 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                            </div>
                            <div class="row">
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">X</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">20</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">8</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">/</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">39</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">9</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">/</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">57</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">8</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">-</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">65</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">X</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">94</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">X</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">114</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">9</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">/</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">133</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">9</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">/</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">153</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">X</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">182</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">X</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">9</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">202</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 1px 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 1px 1px 0; border-style: solid; border-color: black;">/</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">202</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 0 0; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <img src="img/Facebook-Share.jpg" width="50px"/>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">7</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">/</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">20</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">X</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">48</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">X</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">67</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">8</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">1</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">76</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">9</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">/</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">94</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">8</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">1</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">103</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">X</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">123</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">9</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">/</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">141</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">8</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">/</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">157</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">6</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">1</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">164</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 1px 0 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 1px 1px 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">164</div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-1" style="border-width: 0 0 1px 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">X</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">30</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 1px 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">X</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">60</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 1px 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">X</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">90</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 1px 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">X</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">120</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 1px 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">X</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">150</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 1px 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">X</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">180</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 1px 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">X</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">210</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 1px 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">X</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">240</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 1px 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">X</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">270</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 0 1px 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">X</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 1px 1px; border-style: solid; border-color: black;">X</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">300</div>
                                    </div>
                                </div>
                                <div class="col-lg-1" style="border-width: 0 1px 1px 1px; border-style: solid; border-color: black;">
                                    <div class="row">
                                        <div style="float:left;width:34%;border-width: 1px 1px 1px 0; border-style: solid; border-color: black;">X</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                        <div style="float:left;width:33%;border-width: 1px 0 0 0; border-style: solid; border-color: black;">&nbsp;</div>
                                    </div>
                                    <div class="row">
                                        <div style="border-width: 0 0 0 0; border-style: solid; border-color: black;font-size:24px">300</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        -->
                    </div>
                </div>
            </div>
        </div>
    </div>    
</body>
</html>
