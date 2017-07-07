<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Fout.aspx.cs" Inherits="NBF.Qubica.Registration.Fout" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <title>Bowlscene - Registratie</title>
		<meta name="viewport" content="width=device-width, initial-scale=1" />
		<link rel="stylesheet" href="http://www.w3schools.com/lib/w3.css" />
		<link href='http://fonts.googleapis.com/css?family=Lily+Script+One|Fascinate+Inline|Fugaz+One' rel='stylesheet' type='text/css' />
        <link rel="stylesheet" href="styles.css" />
</head>
<body>
	<div class="w3-row-padding">
		<div class="w3-third">&nbsp;</div>
		<div class="w3-third">
			<table>
				<tbody>
					<tr>
						<td id="title">Account</td>
					</tr>
					<tr>
						<td class="label">Er is iets fout gegaan.</td>
					</tr>
					<tr>
						<td class="label" id="error" runat="server"></td>
					</tr>
					<tr>
						<td><input type="submit" id="submit" value="Ga Terug" onclick="javascript: window.history.back();"/></td>
					</tr>
				</tbody>
			</table>
		</div>
		<div class="w3-third">&nbsp;</div>
	</div>
</body>
</html>
