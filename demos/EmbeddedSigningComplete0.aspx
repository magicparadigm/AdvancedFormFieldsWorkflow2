<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EmbeddedSigningComplete0.aspx.cs" Inherits="EmbeddedSigningComplete0" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script>
        var parser = document.createElement('a');
        parser.href = top.location;
// document.write('parser.href = ' + parser.href);
        var text = parser.href;
// document.write('text = ' + text);
        text = text.substring(0, text.lastIndexOf('/'));
// document.write('text2 = ' + text);
        text = text + '/ConfirmationPage.aspx';
        parent.location.href = text; 
   </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>