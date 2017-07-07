<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NBF.Qubica.Registration.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <title>Bowlscene - registratie</title>
		<meta name="viewport" content="width=device-width, initial-scale=1" />
		<link rel="stylesheet" href="http://www.w3schools.com/lib/w3.css" />
		<link href='http://fonts.googleapis.com/css?family=Lily+Script+One|Fascinate+Inline|Fugaz+One' rel='stylesheet' type='text/css' />
        <link rel="stylesheet" href="styles.css" />
</head>
<body>
    <form id="form1" runat="server">
		<div class="w3-row-padding">
			<div class="w3-third">&nbsp;</div>
			<div class="w3-third">
				<table>
					<tbody>
						<tr>
							<td id="title">Account</td>
						</tr>
						<tr>
							<td class="label">Naam</td>
						</tr>
						<tr>
							<td><input id="name" type="text" placeholder="naam" runat="server"/></td>
						</tr>
						<tr>
							<td class="label">Adres</td>
						</tr>
						<tr>
							<td><input id="address" type="text" placeholder="adres" runat="server" /></td>
						</tr>
						<tr>
							<td class="label">Postcode</td>
						</tr>
						<tr>
							<td><input type="text" placeholder="1234AA" runat="server" /></td>
						</tr>
						<tr>
							<td class="label">Woonplaats</td>
						</tr>
						<tr>
							<td><input id="city" type="text" placeholder="woonplaats" runat="server" /></td>
						</tr>
						<tr>
							<td class="label">E-mailadres</td>
						</tr>
						<tr>
							<td><input id="email" type="text" placeholder="emailadres@provider.nl" runat="server" /></td>
						</tr>
						<tr>
							<td>&nbsp;</td>
						</tr>
						<tr>
							<td class="label">Gebruiker</td>
						</tr>
						<tr>
							<td><input id="username" type="text" placeholder="gebruikersnaam" runat="server" style="text-transform:uppercase;" /></td>
						</tr>
						<tr>
							<td class="label">Wachtwoord</td>
						</tr>
						<tr>
							<td><input id="password" type="password" runat="server" /></td>
						</tr>
                        <tr>
							<td class="label">Bevestig</td>
						</tr>
						<tr>
							<td><input id="confirmpwd" type="password" runat="server" /></td>
						</tr>
						<tr>
							<td class="label">Frequent bowler nummer</td>
						</tr>
						<tr>
							<td><input id="frequentbowlernumber" type="text" placeholder="1234567890" runat="server" /></td>
						</tr>
                        <tr>
							<td>&nbsp;</td>
						</tr>
						<tr>
							<td><input type="submit" id="submit" value="REGISTREER"/></td>
						</tr>
					</tbody>
				</table>
			</div>
			<div class="w3-third">&nbsp;</div>
		</div>
    </form>
</body>
</html>

