<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ConfirmationPage.aspx.cs" Inherits="finance_ConfirmationPage" %>

<!DOCTYPE html>

<html class="no-js" lang="">
    <head>
        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <title>Advanced Forms, Fields and Workflow</title>
        <meta name="description" content="">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <link rel="apple-touch-icon">

        <!-- Styles and Fonts -->
        <link rel="stylesheet" href="../style/screen.css">
        <link href='http://fonts.googleapis.com/css?family=Open+Sans:400,600,700,300' rel='stylesheet' type='text/css'>
        <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.2.0/css/font-awesome.min.css" rel='stylesheet' type='text/css'>
    </head>
    <body class="finance">

      <div class="demo">For demonstration purposes only.</div>

      <header>
        <div class="container-fixed">

        <nav class="navbar">
        <div class="navbar-mini">
            <ul>
            <li><a href="https://github.com/magicparadigm/AdvancedFormFieldsWorkflow2">Source Code</a></li>
            <li><a href="https://www.docusign.com/developer-center">DocuSign DevCenter</a></li>
            <li><a href="https://www.docusign.com/p/APIGuide/Content/Sending%20Group/Rules%20for%20CompositeTemplate%20Usage.htm">Field Transforms</a></li>
            </ul>
        </div>
        <div class="navbar-header">
            <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#collaps0r">
            <span class="sr-only">Toggle navigation</span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            </button>
            <a class="navbar-brand" href="default.aspx">Advanced Forms, Fields and Workflow <span>DocuSign DevCon</span></a>
        </div>
        </nav>
        </div>
      </header>
	  <div class="container-fixed formz-vertical">
		<form class="form-inline" runat="server" id="form">
		<div class="row">
            <div class="col-lg-1">
                <img class="center-block" src="../images/homebkgrd-financial.jpg" style="height: 573px; width: 1023px; margin-top: 70px" />
            </div>
			<div class="col-xs-12 text-center">
			<h1>Transaction created successfully</h1>

			</div>
		</div>
        </form>
    </div>
      

      <!-- Google Analytics -->
      <script>
        (function(b,o,i,l,e,r){b.GoogleAnalyticsObject=l;b[l]||(b[l]=
        function(){(b[l].q=b[l].q||[]).push(arguments)});b[l].l=+new Date;
        e=o.createElement(i);r=o.getElementsByTagName(i)[0];
        e.src='//www.google-analytics.com/analytics.js';
        r.parentNode.insertBefore(e,r)}(window,document,'script','ga'));
        ga('create','UA-XXXXX-X','auto');ga('send','pageview');
      </script>

      <!-- Scripts -->
      <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>
      <script src="../js/main.js"></script>

      <script type='text/javascript' id="__bs_script__">
        document.write("<script async src='//localhost:3000/browser-sync/browser-sync-client.1.9.0.js'><\/script>".replace(/HOST/g, location.hostname).replace(/PORT/g, location.port));
      </script>
    </body>
</html>

