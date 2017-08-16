using NBF.Qubica.Classes;
using NBF.Qubica.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NBF.Qubica.BowlingScores
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                bool auth = Convert.ToBoolean(Session["auth"]);

                if (!auth)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    long id = Convert.ToInt64(Session["uid"]);
                    S_User user = UserManager.GetUserById(id);

                    if (!string.IsNullOrEmpty(user.name)) profilenaam.Text = user.name;
                    if (!string.IsNullOrEmpty(user.address)) profileadres.Text = user.address;
                    if (!string.IsNullOrEmpty(user.city)) profileplaats.Text = user.city;
                    if (!string.IsNullOrEmpty(user.email)) profileemail.Text = user.email;
                    profilefrequentbowlernaam.Text = user.username;
                    profilefrequentbowlernummmer.Text = user.frequentbowlernumber.ToString();
                }
            }
        }

        protected void buttonSubmitForm_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                bool auth = Convert.ToBoolean(Session["auth"]);

                if (!auth)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    bool validUser = true;
                    string result = string.Empty;

                    if (profilewachtwoord.Text.CompareTo(profilecontrole.Text) != 0) { result = "Het wachtwoord is niet twee keer hetzelfde ingevuld!"; validUser = false; }

                    if (validUser)
                    {
                        long id = Convert.ToInt64(Session["uid"]);
                        S_User user = UserManager.GetUserById(id);

                        user.name = profilenaam.Text;
                        user.address = profileadres.Text;
                        user.city = profileplaats.Text;

                        if (!string.IsNullOrEmpty(profilewachtwoord.Text))
                            user.password = profilewachtwoord.Text;

                        UserManager.Update(user);

                        result = "De gegevens zijn opgeslagen!";
                    }
                    if (!validUser)
                        profileSuccess.Attributes.Add("style", "color:red");
                    else
                        profileSuccess.Attributes.Add("style", "color:black");

                    profileSuccess.InnerHtml = result;
                }
            }
        }
    }
}