<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="bongTranslator.aspx.cs" Inherits="CSCELearning.Task_1_Part_B.bongTranslator" %>

<!DOCTYPE html>

<script src="https://code.jquery.com/jquery-1.10.2.js"></script>
<script>
    $(window).load(function () {
        $('#translateBtn').click(function (evt) {
            var inputText = $('#MyTextBox').val();
            var authToken = $('#txtAuthToken').val();
            var data = {
                appId: 'Bearer ' + authToken,
                from: 'en',
                to: 'zh-CHS',
                contentType: 'text/plain',
                text: inputText
            };
            $.ajax({
                url: "http://api.microsofttranslator.com/V2/Ajax.svc/Translate",
                dataType: 'jsonp',
                data: data,
                jsonp: 'oncomplete'
            })
            .done(function (result, textStatus, errorThrown) {
                $('#output').html("<span style='font-weight: bold'>Your Translation is: </span>"+result);
            })
            .fail(function (result, textStatus, errorThrown) {
                $('#output').html("We are experiencing technical difficulties<br/><span style='color: red; font-style: italic'>Sorry for the inconvenience caused!</span>");
            });
        });
    });

</script>

<style>
        #translateHead {
            font-size: 30px;
            font-weight: bold;
        }

        #txtAuthToken{
            display: none;
        }
</style>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form runat="server">

        <p id="translateHead">My Translator</p>
        Enter your text in English:
        <br/><br/>
        <!-- Used to store authentication token-->
        <asp:TextBox ID="txtAuthToken" runat="server"></asp:TextBox>
        
        <input id="MyTextBox" type="text"/>
        <br/><br/>

        <input type="button" id="translateBtn" value="Translate"/>
        <br/>
        
        <p>Here is your translation:</p>
        <label id="output"></label>
    </form>
</body>
</html>
