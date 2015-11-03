<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

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

    <script>
        function updateWindowSize() {
            var width = window.innerWidth ||
                        document.documentElement.clientWidth ||
                        document.body.clientWidth;
            var height = window.innerHeight ||
                            document.documentElement.clientHeight ||
                            document.body.clientHeight;
            docusignFrame.height = height - 130;
            docusignFrame.width = width;

        }

        window.onload = updateWindowSize;
        window.onresize = updateWindowSize;
    </script>
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

    <div id="mainForm" runat="server" class="container-fixed formz-vertical">
        <br />
        <ul class="nav nav-pills" role="tablist">
            <li class="active"><a href="#">Templates</a></li>
            <li><a href="DynamicFields.aspx">Dynamic Fields</a></li>
            <li><a href="AnchorText.aspx">Anchor Text Fields</a></li>
            <li><a href="PDFFormFields.aspx">PDF Form Fields</a></li>
        </ul>
        <form class="form-inline" runat="server" id="form">
            <div class="row">
                <div class="col-xs-12">
                    <h1><a id="PrefillClick" runat="server" href="#">Templates Example</a></h1>

                </div>
            </div>
            <div class="row" id="primarySignerSection" runat="server">
                <div class="col-xs-12">
                    <h2>Primary Account Holder</h2>
                    <div class="form-group">
                        <label for="firstname">First Name</label>
                        <input type="text" runat="server" class="form-control" id="firstname" placeholder="">
                    </div>
                    <div class="form-group">
                        <label for="lastname">Last Name</label>
                        <input type="text" runat="server" class="form-control" id="lastname" placeholder="">
                    </div>
                    <br>
                    <div class="form-group">
                        <label for="email">Email Address</label>
                        <input type="email" runat="server" class="form-control" id="email" placeholder="">
                    </div>
                    <hr />
                </div>

            </div>
            <div class="row" id="jointSignerSection" runat="server">
                <div class="col-xs-12">
                    <h2>Joint Account Holder</h2>
                    <div class="form-group">
                        <label for="firstname">First Name</label>
                        <input type="text" runat="server" class="form-control" id="jointFirstname" placeholder="">
                    </div>
                    <div class="form-group">
                        <label for="lastname">Last Name</label>
                        <input type="text" runat="server" class="form-control" id="jointLastname" placeholder="">
                    </div>
                    <br>
                    <div class="form-group">
                        <label for="email">Email Address </label>
                        <input type="email" runat="server" class="form-control" id="jointEmail" placeholder="">
                    </div>
                    <hr />
                </div>
            </div>
            <div class="row" id="templates" runat="server">
                <div class="col-xs-12">
                    <h2>Template Information</h2>
                    <div class="form-group">
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                    </div>
                    <div class="form-group">
                        <button type="button" visible="true" id="uploadButton" runat="server" class="btn" style="color: #fff; padding: 10px 80px; font-size: 14px; margin: 40px auto; display: block;"></button>
                    </div>
                    <div class="form-group">
                        <label for="uploadFile">Upload File </label>
                        <input type="text" runat="server" class="form-control" id="uploadFile" placeholder="" readonly="readonly">
                    </div>
                    <br />
                    <label for="templatesListLabel" id="templatesListLabel">Templates List</label>
                    <asp:DropDownList ID="templatesList" runat="server" AutoPostBack="True" />
                </div>
            </div>
            <button type="button" visible="true" id="button" runat="server" class="btn" style="color: #fff; padding: 10px 80px; font-size: 14px; margin: 40px auto; display: block;"></button>
        </form>
    </div>

    <iframe runat="server" id="docusignFrame" />

    <iframe runat="server" id="docusignFrameIE" style="width: 100%; height: 768px" />

    <!-- Google Analytics -->
    <script>
        (function (b, o, i, l, e, r) {
            b.GoogleAnalyticsObject = l; b[l] || (b[l] =
            function () { (b[l].q = b[l].q || []).push(arguments) }); b[l].l = +new Date;
            e = o.createElement(i); r = o.getElementsByTagName(i)[0];
            e.src = '//www.google-analytics.com/analytics.js';
            r.parentNode.insertBefore(e, r)
        }(window, document, 'script', 'ga'));
        ga('create', 'UA-XXXXX-X', 'auto'); ga('send', 'pageview');
    </script>

    <!-- Scripts -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.2/jquery.min.js"></script>
    <script src="../js/main.js"></script>

    <script type='text/javascript' id="__bs_script__">
        document.write("<script async src='//localhost:3000/browser-sync/browser-sync-client.1.9.0.js'><\/script>".replace(/HOST/g, location.hostname).replace(/PORT/g, location.port));
    </script>
</body>
</html>
