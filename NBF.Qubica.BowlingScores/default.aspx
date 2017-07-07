<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="NBF.Qubica.BowlingScores._default" %>

<!DOCTYPE html>
<html lang="en">

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

    <!-- HTML5 Shim IE8 support of HTML5 elements and media queries -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/libs/html5shiv/3.7.0/html5shiv.js"    integrity="sha384-0s5Pv64cNZJieYFkXYOTId2HMA2Lfb6q2nAcx2n0RTLUnCAoTTsS0nKEO27XyKcY" crossorigin="anonymous"></script>
        <script src="https://oss.maxcdn.com/libs/respond.js/1.4.2/respond.min.js" integrity="sha384-ZoaMbDF+4LeFxg6WdScQ9nnR1QC2MIRxA1O9KWEXQwns1G8UNyIEZIQidzb0T1fo" crossorigin="anonymous"></script>
    <![endif]-->

</head>

<body id="page-top" class="index">

    <!-- Navigation -->
    <nav id="mainNav" class="navbar navbar-default navbar-custom navbar-fixed-top">
        <div class="container">
            <!-- Brand and toggle get grouped for better mobile display -->
            <div class="navbar-header page-scroll">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
                    <span class="sr-only">Toggle navigation</span> Menu <i class="fa fa-bars"></i>
                </button>
                <a class="navbar-brand page-scroll" href="#page-top"><asp:Literal ID="home" runat="server" /></a>
            </div>

            <!-- Collect the nav links, forms, and other content for toggling -->
            <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                <ul class="nav navbar-nav navbar-right">
                    <li class="hidden">
                        <a href="#page-top"></a>
                    </li>
                    <li>
                        <a class="page-scroll" href="#doemee">Doe mee</a>
                    </li>
                    <li>
                        <a class="page-scroll" href="#bowlingcentra">Bowling Centra</a>
                    </li>
                    <li>
                        <a class="page-scroll" href="#competities">Competities</a>
                    </li>
                    <li>
                        <a class="page-scroll" href="#aanmelden">Meld je aan</a>
                    </li>
                    <li>
                        <a href="#ContactModal" class="portfolio-link" data-toggle="modal">Contact</a>
                    </li>
                </ul>
            </div>
            <!-- /.navbar-collapse -->
        </div>
        <!-- /.container-fluid -->
    </nav>

    <!-- Header -->
    <header>
        <div class="container">
            <div class="intro-text">
                <div class="intro-lead-in">Bowlen, Scores bekijken, Winnen!</div>
                <div class="intro-heading">Doe mee en win prijzen</div>
                <a href="#doemee" class="page-scroll btn btn-xl">Hoe werkt het</a>
            </div>
        </div>
    </header>

    <!-- Doe Mee Section -->
    <section id="doemee">
        <div class="container">
            <div class="row">
                <div class="col-lg-12 text-center">
                    <h2 class="section-heading">Doe mee</h2>
                    <h3 class="section-subheading text-muted">In drie simple stappen kun je mee doen</h3>
                </div>
            </div>
            <div class="row text-center">
                <div class="col-md-4">
                    <span class="fa-stack fa-4x">
                        <i class="fa fa-circle fa-stack-2x text-primary"></i>
                        <i class="fa fa-arrow-circle-o-down fa-stack-1x fa-inverse"></i>
                    </span>
                    <h4 class="service-heading">Installeer de App</h4>
                    <p class="text-muted">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Minima maxime quam architecto quo inventore harum ex magni, dicta impedit.</p>
                </div>
                <div class="col-md-4">
                    <span class="fa-stack fa-4x">
                        <i class="fa fa-circle fa-stack-2x text-primary"></i>
                        <i class="fa fa-laptop fa-stack-1x fa-inverse"></i>
                    </span>
                    <h4 class="service-heading">Meld je aan</h4>
                    <p class="text-muted">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Minima maxime quam architecto quo inventore harum ex magni, dicta impedit. <a class="page-scroll" href="#aanmelden">Meld je aan</a></p>
                </div>
                <div class="col-md-4">
                    <span class="fa-stack fa-4x">
                        <i class="fa fa-circle fa-stack-2x text-primary"></i>
                        <i class="fa fa-play fa-stack-1x fa-inverse"></i>
                    </span>
                    <h4 class="service-heading">Ga bowlen</h4>
                    <p class="text-muted">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Minima maxime quam architecto quo inventore harum ex magni, dicta impedit.</p>
                </div>
            </div>
        </div>
    </section>

    <!-- Bowling Centra Section -->
    <section id="bowlingcentra" class="bg-light-gray">
        <div class="container">
            <div class="row">
                <div class="col-lg-12 text-center">
                    <h2 class="section-heading">Bowling Centra</h2>
                    <h3 class="section-subheading text-muted">De volgende bowlingcentra zijn aangesloten</h3>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4 col-sm-6 portfolio-item">
                    <a href="#BowlingCentraModal1" class="portfolio-link" data-toggle="modal">
                        <div class="portfolio-hover">
                            <div class="portfolio-hover-content">
                                <i class="fa fa-play fa-3x"></i>
                            </div>
                        </div>
                        <img src="img/bowling/harderwijk.png" class="img-responsive" alt="">
                    </a>
                    <div class="portfolio-caption">
                        <h4>Bowling Centrum</h4>
                        <p class="text-muted">Harderwijk</p>
                    </div>
                </div>
                <div class="col-md-4 col-sm-6 portfolio-item">
                    <a href="#BowlingCentraModal2" class="portfolio-link" data-toggle="modal">
                        <div class="portfolio-hover">
                            <div class="portfolio-hover-content">
                                <i class="fa fa-play fa-3x"></i>
                            </div>
                        </div>
                        <img src="img/bowling/veenendaal.png" class="img-responsive" alt="">
                    </a>
                    <div class="portfolio-caption">
                        <h4>Olround</h4>
                        <p class="text-muted">Veenendaal</p>
                    </div>
                </div>
                <div class="col-md-4 col-sm-6 portfolio-item">
                    <a href="#BowlingCentraModal3" class="portfolio-link" data-toggle="modal">
                        <div class="portfolio-hover">
                            <div class="portfolio-hover-content">
                                <i class="fa fa-play fa-3x"></i>
                            </div>
                        </div>
                        <img src="img/bowling/amsterdam.png" class="img-responsive" alt="">
                    </a>
                    <div class="portfolio-caption">
                        <h4>Borchland</h4>
                        <p class="text-muted">Amsterdam</p>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- Competities Section -->
    <section id="competities">
        <div class="container">
            <div class="row">
                <div class="col-lg-12 text-center">
                    <h2 class="section-heading">Competities</h2>
                    <h3 class="section-subheading text-muted">Lorem ipsum dolor sit amet consectetur.</h3>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <ul class="timeline">
                        <li>
                            <div class="timeline-image">
                                <img class="img-circle img-responsive" src="img/competities/1.png" alt="">
                            </div>
                            <div class="timeline-panel">
                                <div class="timeline-heading">
                                    <h4>September 2018</h4>
                                    <h4 class="subheading">Gooi de meeste strikes</h4>
                                </div>
                                <div class="timeline-body">
                                    <p class="text-muted">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Sunt ut voluptatum eius sapiente, totam reiciendis temporibus qui quibusdam, recusandae sit vero unde, sed, incidunt et ea quo dolore laudantium consectetur! <a href="#AanmeldenModal" class="portfolio-link" data-toggle="modal">Doe mee!</a></p>
                                </div>
                            </div>
                        </li>
                        <li class="timeline-inverted">
                            <div class="timeline-image">
                                <img class="img-circle img-responsive" src="img/competities/1.png" alt="">
                            </div>
                            <div class="timeline-panel">
                                <div class="timeline-heading">
                                    <h4>Januari 2018</h4>
                                    <h4 class="subheading">Hoogste score</h4>
                                </div>
                                <div class="timeline-body">
                                    <p class="text-muted">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Sunt ut voluptatum eius sapiente, totam reiciendis temporibus qui quibusdam, recusandae sit vero unde, sed, incidunt et ea quo dolore laudantium consectetur! <a href="#AanmeldenModal" class="portfolio-link" data-toggle="modal">Doe mee!</a></p>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="timeline-image">
                                <img class="img-circle img-responsive" src="img/competities/1.png" alt="">
                            </div>
                            <div class="timeline-panel">
                                <div class="timeline-heading">
                                    <h4>December 2017</h4>
                                    <h4 class="subheading">Meeste headpins </h4>
                                </div>
                                <div class="timeline-body">
                                    <p class="text-muted">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Sunt ut voluptatum eius sapiente, totam reiciendis temporibus qui quibusdam, recusandae sit vero unde, sed, incidunt et ea quo dolore laudantium consectetur! <a href="#AanmeldenModal" class="portfolio-link" data-toggle="modal">Doe mee!</a></p>
                                </div>
                            </div>
                        </li>
                        <li class="timeline-inverted">
                            <div class="timeline-image">
                                <img class="img-circle img-responsive" src="img/competities/1.png" alt="">
                            </div>
                            <div class="timeline-panel">
                                <div class="timeline-heading">
                                    <h4>September 2017</h4>
                                    <h4 class="subheading">Meeste splits</h4>
                                </div>
                                <div class="timeline-body">
                                    <p class="text-muted">Lorem ipsum dolor sit amet, consectetur adipisicing elit. Sunt ut voluptatum eius sapiente, totam reiciendis temporibus qui quibusdam, recusandae sit vero unde, sed, incidunt et ea quo dolore laudantium consectetur! <a href="#AanmeldenModal" class="portfolio-link" data-toggle="modal">Doe mee!</a></p>
                                </div>
                            </div>
                        </li>
                        <li class="timeline-inverted">
                            <div class="timeline-image">
                                <h4>Doe mee<br>en<br>Win!</h4>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </section>

    <!-- Logo's Aside -->
    <aside class="clients">
        <div class="container">
            <div class="row">
                <div class="col-md-4 col-sm-6">
                    <a href="#" target="_new">
                        <img src="" class="img-responsive img-centered" alt="">
                    </a>
                </div>
                <div class="col-md-4 col-sm-6">
                    <a href="http://www.bowlingnbf.nl/" target="_new">
                        <img src="img/logos/nbf.jpg" class="img-responsive img-centered" alt="">
                    </a>
                </div>
                <div class="col-md-4 col-sm-6">
                    <a href="#" target="_new">
                        <img src="" class="img-responsive img-centered" alt="">
                    </a>
                </div>
            </div>
        </div>
    </aside>

    <!-- Aanmelden Section -->
    <section id="aanmelden">
        <div class="container">
            <div class="row">
                <div class="col-lg-12 text-center">
                    <h2 class="section-heading">Meld je aan</h2>
                    <h3 class="section-subheading text-muted">Lorem ipsum dolor sit amet consectetur.</h3>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <form name="sentMessage" id="contactForm" novalidate>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <input type="text" class="form-control" placeholder="Je naam *" id="name" required data-validation-required-message="Geef je naam op.">
                                    <p class="help-block text-danger"></p>
                                </div>
                                <div class="form-group">
                                    <input type="text" class="form-control" placeholder="Je adres *" id="adres" required data-validation-required-message="Geef je adres op.">
                                    <p class="help-block text-danger"></p>
                                </div>
                                <div class="form-group">
                                    <input type="text" class="form-control" placeholder="Je postcode *" id="postcode" required data-validation-required-message="Geef je postcode op.">
                                    <p class="help-block text-danger"></p>
                                </div>
                                <div class="form-group">
                                    <input type="text" class="form-control" placeholder="Je woonplaats *" id="woonplaats" required data-validation-required-message="Geef je woonplaats op.">
                                    <p class="help-block text-danger"></p>
                                </div>
                                <div class="form-group">
                                    <input type="text" class="form-control" placeholder="Je telefoonnummer *" id="telefoonnummer" required data-validation-required-message="Geef je telefoonnummer op.">
                                    <p class="help-block text-danger"></p>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <input type="email" class="form-control" placeholder="Je Email *" id="email" required data-validation-required-message="Geef je e-mail adres op.">
                                    <p class="help-block text-danger"></p>
                                </div>
                                <div class="form-group">
                                    <input type="text" class="form-control" placeholder="Je bowlingnaam *" id="bowlingnaam" required data-validation-required-message="Geef je bowlingnaam op.">
                                    <p class="help-block text-danger"></p>
                                </div>
                                <div class="form-group">
                                    <input type="text" class="form-control" placeholder="Je Frequent Bolwernummer *" id="frequentbowlernummer" required data-validation-required-message="Geef je frequent bowlernummer op.">
                                    <p class="help-block text-danger"></p>
                                </div>
                                <div class="form-group">
                                    <input type="password" class="form-control" placeholder="Je wachtwoord *" id="wachtwoord" required data-validation-required-message="Geef je wachtwoord op.">
                                    <p class="help-block text-danger"></p>
                                </div>
                                <div class="form-group">
                                    <input type="password" class="form-control" placeholder="Je wachtwoord *" id="controle" required data-validation-required-message="Geef je wachtwoord nogmaals op.">
                                    <p class="help-block text-danger"></p>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="col-lg-12 text-center">
                                <div id="success"></div>
                                <button type="submit" class="btn btn-xl">Verstuur</button>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </section>

    <footer>
        <div class="container">
            <div class="row">
                <div class="col-md-4">
                    <span class="copyright">Copyright &copy; Nederlandse Bowling Federatie 2017</span>
                </div>
                <div class="col-md-4">
                    <ul class="list-inline social-buttons">
                        <li><a href="https://twitter.com/NBF_Bowlen" target="_new"><i class="fa fa-twitter"></i></a></li>
                        <li><a href="https://www.facebook.com/Nederlandse-Bowling-Federatie-112897208786219/" target="_new"><i class="fa fa-facebook"></i></a></li>
                        <li><a href="https://www.linkedin.com/company/nederlandse-bowling-federatie?trk=top_nav_home" target="_new"><i class="fa fa-linkedin"></i></a></li>
                        <li><a href="https://www.youtube.com/user/dutchbowlingtv" target="_new"><i class="fa fa-youtube"></i></a></li>
                        <li><a href="https://www.instagram.com/nbfbowlen/" target="_new"><i class="fa fa-instagram"></i></a></li>
                    </ul>
                </div>
                <div class="col-md-4">
                    <ul class="list-inline quicklinks">
                        <li><a href="#PrivacyStatementModal" class="portfolio-link" data-toggle="modal">Privacy Policy</a></li>
                        <li><a href="#GebruikersvoorwaardenModal" class="portfolio-link" data-toggle="modal">Disclamer</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </footer>

    <!-- Modals Bowling Centra-->
    <div class="portfolio-modal modal fade" id="BowlingCentraModal1" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="close-modal" data-dismiss="modal">
                    <div class="lr">
                        <div class="rl">
                        </div>
                    </div>
                </div>
                <div class="container">
                    <div class="row">
                        <div class="col-lg-8 col-lg-offset-2">
                            <div class="modal-body">
                                <!-- Bowling Centra Details Go Here -->
                                <h2>Bowling Centrum Harderwijk</h2>
                                <p class="item-intro text-muted">Lorem ipsum dolor sit amet consectetur.</p>
                                <img class="img-responsive img-centered" src="img/bowling/harderwijk-full.png" alt="">
                                <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Est blanditiis dolorem culpa incidunt minus dignissimos deserunt repellat aperiam quasi sunt officia expedita beatae cupiditate, maiores repudiandae, nostrum, reiciendis facere nemo!</p>

                                <button type="button" class="btn btn-primary" data-dismiss="modal"><i class="fa fa-times"></i> Sluiten</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="portfolio-modal modal fade" id="BowlingCentraModal2" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="close-modal" data-dismiss="modal">
                    <div class="lr">
                        <div class="rl">
                        </div>
                    </div>
                </div>
                <div class="container">
                    <div class="row">
                        <div class="col-lg-8 col-lg-offset-2">
                            <div class="modal-body">
                                <!-- Bowling Centra Details Go Here -->
                                <h2>Olround Veenendaal</h2>
                                <p class="item-intro text-muted">Lorem ipsum dolor sit amet consectetur.</p>
                                <img class="img-responsive img-centered" src="img/bowling/veenendaal.png" alt="">
                                <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Est blanditiis dolorem culpa incidunt minus dignissimos deserunt repellat aperiam quasi sunt officia expedita beatae cupiditate, maiores repudiandae, nostrum, reiciendis facere nemo!</p>
                                <button type="button" class="btn btn-primary" data-dismiss="modal"><i class="fa fa-times"></i> Sluiten</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="portfolio-modal modal fade" id="BowlingCentraModal3" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="close-modal" data-dismiss="modal">
                    <div class="lr">
                        <div class="rl">
                        </div>
                    </div>
                </div>
                <div class="container">
                    <div class="row">
                        <div class="col-lg-8 col-lg-offset-2">
                            <div class="modal-body">
                                <!-- Bowling Centra Details Go Here -->
                                <h2>Borchland Amsterdam</h2>
                                <p class="item-intro text-muted">Lorem ipsum dolor sit amet consectetur.</p>
                                <img class="img-responsive img-centered" src="img/bowling/amsterdam.png" alt="">
                                <p>Lorem ipsum dolor sit amet, consectetur adipisicing elit. Est blanditiis dolorem culpa incidunt minus dignissimos deserunt repellat aperiam quasi sunt officia expedita beatae cupiditate, maiores repudiandae, nostrum, reiciendis facere nemo!</p>
                                <button type="button" class="btn btn-primary" data-dismiss="modal"><i class="fa fa-times"></i> Sluiten</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Privacy Statement -->
    <div class="portfolio-modal modal fade" id="PrivacyStatementModal" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="close-modal" data-dismiss="modal">
                    <div class="lr">
                        <div class="rl">
                        </div>
                    </div>
                </div>
                <div class="container">
                    <div class="row">
                        <div class="col-lg-8 col-lg-offset-2">
                            <div class="modal-body">
                                <!-- Privacy Statement Details Go Here -->
                                <h2>Privacy Statement voor Bezoekers</h2>
                                <p class="item-intro text-muted">De Privacy Policy voor Bezoekers geldt voor iedereen die de website www.bowlingscores.nl bezoekt.</p>
                                <div style="text-align:justify">
                                    <p>
                                        <strong>1. Beheer</strong><br />
                                        De website www.bowlingscores.nl staat onder beheer van de Nederlandse Bowling Federatie (hierna te noemen: "NBF"). Via de voorgenoemde website kan een gebruiker zich aanmelden voor deelname aan verschillende competities uitgeschreven door de NBF in samenwerking met aangesloten bowling centra. Via het contact formulier op de website kan de gebruiker van de website contact op nemen m et de NBF.
                                    </p>

                                    <p>
                                        <strong>2. Gegevens van bezoekers</strong><br />
                                        2a Sommige gegevens die voortkomen uit één of meer bezoeken aan www.bowlingscores.nl worden permanent bewaard, maar wel anoniem. De gegevens zullen dus nooit te herleiden zijn naar een persoon of organisatie.
                                        2b De NBF zorgt voor een goede beveiliging van de opgeslagen gegevens.
                                    </p>
                                    <p>
                                        <strong>3. Cookies</strong><br />
                                        3a De NBF maakt gebruik van functionele cookies om de functionaliteit van bepaalde pagina's van de website te optimaliseren. Cookies zijn kleine tekstbestanden die door een pagina van de website op de computer van de bezoeker worden geplaatst. In zo'n cookie wordt informatie opgeslagen zoals bepaalde voorkeuren van de bezoeker. Daardoor kan De NBF de bezoeker bij een volgend bezoek nog beter van dienst zijn, door bijvoorbeeld de domeinnamen die de bezoeker gecheckt en bewaard heeft. Je hoeft dan ook niet telkens opnieuw dezelfde informatie in te vullen. Hierdoor wordt de site veel gebruiksvriendelijker.
                                        3b De bezoeker kan zelf bepalen hoe er met cookies omgegaan moet worden. Hij of zij kan zijn browser zo instellen dat die het gebruik van functionele cookies toestaat, niet toestaat of gedeeltelijk toestaat. In dit laatste geval kan worden ingesteld welke websites functionele cookies mogen plaatsen. Bij alle overige websites wordt het dan verboden. Deze mogelijkheid wordt door de meestgebruikte moderne browsers geboden.
                                        3c Cookies kunnen altijd van een computer worden verwijderd, ook weer via de browser.
                                        3d De NBF maakt nu nog geen gebruik van marketingcookies om de communicatie van de NBF zowel op haar eigen site als op haar partnersites persoonlijker te maken en beter af te stemmen op de wensen van de individuele klant.
                                        3e De NBF maakt nu nog geen gebruik van Analytics cookies waarmee niet het surfgedrag van individuen maar van grote aantallen bezoekers - geanonimiseerd - worden verwerkt tot grafieken en patronen die ons helpen om onze websites te verbeteren en te optimaliseren.
                                    </p>
                                    <p>
                                        <strong>4. Vragen</strong><br />
                                        Bezoekers kunnen met hun vragen over deze Privacy Policy terecht bij de NBF. hoe de gebruiker contact kan opnemen staat wordt benoemd in lid 1 van deze Privacy Policy.
                                    </p>
                                    <p>
                                        <strong>5. Disclaimer</strong><br />
                                        De NBF is gerechtigd de inhoud van de Privacy Policy te wijzigen zonder dat de bezoeker daarvan op de hoogte wordt gesteld. Het doorvoeren van de wijziging op de website is daarvoor afdoende.
                                    </p>
                                </div>

                                <button type="button" class="btn btn-primary" data-dismiss="modal"><i class="fa fa-times"></i> Sluiten</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <!-- Modal Gebruikersvoorwaarden -->
    <div class="portfolio-modal modal fade" id="GebruikersvoorwaardenModal" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="close-modal" data-dismiss="modal">
                    <div class="lr">
                        <div class="rl">
                        </div>
                    </div>
                </div>
                <div class="container">
                    <div class="row">
                        <div class="col-lg-8 col-lg-offset-2">
                            <div class="modal-body">
                                <!-- Gebruikersvoorwaarden Details Go Here -->
                                <h2>Disclaimer</h2>
                                <p class="item-intro text-muted">Gebruiksvoorwaarden</p>
                                <div style="text-align:justify">
                                    <p><strong>De volgende gebruiksvoorwaarden gelden voor de website die door de Nederlandse Bowling Federatie (hierna te noemen: "NBF") beheerd wordt.</strong></p>
                                    <p>Indien u niet akkoord bent met deze gebruiksvoorwaarden, dan dient u de website niet te gebruiken.</p>

                                    <p>
                                        <strong>Aansprakelijkheid NBF</strong><br />
                                        De NBF gaat uiterst zorgvuldig te werk en spant zich in om de informatie die zij aanbiedt op de website zo volledig en nauwkeurig mogelijk te laten zijn, maar geeft ter zake geen garanties. De NBF aanvaardt geen enkele aansprakelijkheid voor directe of indirecte schade ontstaan uit het bezoeken van de website, uit de onbereikbaarheid van de website, uit bijdragen die door derden op de website zijn geplaatst of uit het afgaan op informatie die op de website wordt verstrekt. De NBF is niet aansprakelijk voor verwijzingen naar sites van derden en informatie die op deze site te vinden is en voor de inhoud van door derden aangeboden bijdragen zoals foto’s en videomateriaal.
                                    </p>

                                    <p>
                                        <strong>Intellectuele eigendomsrechten</strong><br />
                                        Alle eigendomsrechten (inclusief de intellectuele eigendomsrechten) op al het materiaal inclusief maar niet beperkt tot alle teksten, foto’s, beelden, geluiden, merken en programmatuur van de website zijn voorbehouden aan de NBF en/of haar licentiegevers. De inhoud van de website is bestemd voor persoonlijk, niet-commercieel, gebruik. Voor elk ander gebruik is vooraf schriftelijke toestemming van de NBF vereist.
                                    </p>
                                    <p>
                                        <strong>Rechtskeuze</strong><br />
                                        Op deze gebruiksvoorwaarden is Nederlands recht van toepassing. Alle eventuele geschillen worden voorgelegd aan de daartoe bevoegde rechter te Apeldoorn. Deze gebruiksvoorwaarden kunnen te allen tijde door de NBF aangepast worden. Wij adviseren daarom de gebruiksvoorwaarden regelmatig te bekijken.
                                </div>
                                </p>
                                <button type="button" class="btn btn-primary" data-dismiss="modal"><i class="fa fa-times"></i> Sluiten</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Aanmelden Competitie -->
    <div class="portfolio-modal modal fade" id="AanmeldenModal" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="close-modal" data-dismiss="modal">
                    <div class="lr">
                        <div class="rl">
                        </div>
                    </div>
                </div>
                <div class="container">
                    <div class="row">
                        <div class="col-lg-8 col-lg-offset-2">
                            <div class="modal-body">
                                <!-- Aanmelden Competitie Details Go Here -->
                                <h2>Doe mee met een Competitie</h2>
                                <p class="item-intro text-muted">Selecteer een competitie en schrijf je in met je bowlingnaam.<br />Nog geen bowlingnaam? Meld je eerst aan!</a></p>
                                <form name="sentMessage" id="contactForm" novalidate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <select class="form-control">
                                                    <option>-- Selecteer een competitie --</option>
                                                    <option>September 2017 : Meeste splits</option>
                                                    <option>December 2017 : Meeste headpins</option>
                                                    <option>Januari 2018 : Hoogste score</option>
                                                    <option>September 2018 : Gooi de meeste strikes</option>
                                                </select>
                                                <p class="help-block text-danger"></p>
                                            </div>
                                            <div class="form-group">
                                                <input type="email" class="form-control" placeholder="Je Email *" id="email" required data-validation-required-message="Geef je e-mail adres op.">
                                                <p class="help-block text-danger"></p>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <input type="text" class="form-control" placeholder="Je bowlingnaam *" id="bowlingnaam" required data-validation-required-message="Geef je bowlingnaam op.">
                                                <p class="help-block text-danger"></p>
                                            </div>
                                            <div class="form-group">
                                                <input type="password" class="form-control" placeholder="Je wachtwoord *" id="wachtwoord" required data-validation-required-message="Geef je wachtwoord op.">
                                                <p class="help-block text-danger"></p>
                                            </div>
                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="col-lg-12 text-center">
                                            <div id="success"></div>
                                            <button type="submit" class="btn btn-xl">Verstuur</button>
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

    <!-- Modals Contact formulier -->
    <div class="portfolio-modal modal fade" id="ContactModal" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="close-modal" data-dismiss="modal">
                    <div class="lr">
                        <div class="rl">
                        </div>
                    </div>
                </div>
                <div class="container">
                    <div class="row">
                        <div class="col-lg-8 col-lg-offset-2">
                            <div class="modal-body">
                                <!-- Project Details Go Here -->
                                <h2>Contact</h2>
                                <p class="item-intro text-muted">Stel uw vraag door het formulier in te vullen.</a></p>
                                <form name="sentMessage" id="contactForm" novalidate>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <input type="text" class="form-control" placeholder="Je Naam *" id="naam" required data-validation-required-message="Geef je naam op.">
                                                <p class="help-block text-danger"></p>
                                            </div>
                                            <div class="form-group">
                                                <input type="email" class="form-control" placeholder="Je Email *" id="email" required data-validation-required-message="Geef je e-mail adres op.">
                                                <p class="help-block text-danger"></p>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <textarea rows="10" class="form-control" placeholder="Your Message *" id="message" required data-validation-required-message="Please enter a message."></textarea>
                                                <p class="help-block text-danger"></p>
                                            </div>
                                        </div>
                                        <div class="clearfix"></div>
                                        <div class="col-lg-12 text-center">
                                            <div id="success"></div>
                                            <button type="submit" class="btn btn-xl">Verstuur</button>
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

    <!-- jQuery -->
    <script src="vendor/jquery/jquery.min.js"></script>

    <!-- Bootstrap Core JavaScript -->
    <script src="vendor/bootstrap/js/bootstrap.min.js"></script>

    <!-- Plugin JavaScript -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-easing/1.3/jquery.easing.min.js" integrity="sha384-mE6eXfrb8jxl0rzJDBRanYqgBxtJ6Unn4/1F7q4xRRyIw7Vdg9jP4ycT7x1iVsgb" crossorigin="anonymous"></script>

    <!-- Aanmelden Form JavaScript -->
    <script src="js/jqBootstrapValidation.js"></script>

    <!-- Theme JavaScript -->
    <script src="js/bowling.min.js"></script>

</body>

</html>
