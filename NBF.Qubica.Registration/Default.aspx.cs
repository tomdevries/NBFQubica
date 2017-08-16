using NBF.Qubica.Classes;
using NBF.Qubica.Managers;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NBF.Qubica.Registration
{
    public partial class Default : System.Web.UI.Page
    {
        static Logger logger = LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack)
            {
                writeResult(FormSubmit()); 
            }
        }

        protected Result FormSubmit()
        {
            Result result = new Result(true);
            bool inputValid = true;
            try
            {
                S_User user = new S_User();

                if (string.IsNullOrEmpty(address.Value)) { result = new Result("Vul een adres in!"); inputValid=false; }
                if (string.IsNullOrEmpty(city.Value)) { result = new Result("Vul een plaats in!"); inputValid=false; }
                if (string.IsNullOrEmpty(email.Value)) { result = new Result("Vul een e-mail adres in!"); inputValid=false; }
                if (string.IsNullOrEmpty(name.Value)) { result = new Result("Vul een naam in!"); inputValid=false; }
                if (string.IsNullOrEmpty(password.Value)) { result = new Result("Vul een wachtwoord in!"); inputValid=false; }
                if (string.IsNullOrEmpty(username.Value)) { result = new Result("Vul een ID-Naam in!"); inputValid = false; }

                //if (!string.IsNullOrEmpty(username.Value))
                //{
                //    if (UserManager.UserExistByUsername(username.Value))
                //    { result = new Result("Kies een andere ID-Naam, deze bestaat reeds!"); inputValid = false; }
                //    if (UserManager.UsernameHasGames(username.Value))
                //    { result = new Result("Kies een andere ID-Naam, deze bestaat reeds!"); inputValid = false; }
                //}

                if (password.Value.CompareTo(confirmpwd.Value) != 0) { result = new Result("Het wachtwoord is net twee keer herzelfde ingevuld!"); inputValid = false; }
                if (string.IsNullOrEmpty(frequentbowlernumber.Value)) {
                    result = new Result("Vul een frequent bowler nummer in!");
                    inputValid = false;
                }
                else
                {   
                    long n;
                    bool isNumeric = long.TryParse(frequentbowlernumber.Value, out n);
                    if (!isNumeric)
                    {
                        result = new Result("Vul een frequent bowler nummer in!");
                        inputValid = false;
                    }
                    else
                    {
                        if (UserManager.UserExistByFrequentBowlerNumber(n))
                        {
                            result = new Result("Kies een ander frequent bowler nummer, deze bestaat reeds!");
                            inputValid = false;
                        }
                    }
                }



                if (inputValid)
                {
                    user.address = address.Value;
                    user.city = city.Value;
                    user.email = email.Value;
                    user.isMember = false;
                    user.isRegistrationConfirmed = true;
                    user.name = name.Value;
                    user.password = password.Value;
                    user.roleid = Role.USER;
                    user.username = username.Value;
                    long n;
                    long.TryParse(frequentbowlernumber.Value, out n);
                    user.frequentbowlernumber = n;

                    UserManager.Insert(user);
                    result.stored = true;
                }
            }
            catch (Exception ex)
            {
                result.stored = false;
                result.message = ex.Message;
                logger.Error(ex.Message);
            }

            return result;
        }

        protected void writeResult(Result result)
        {
            if (result.stored == true)
            {
                Response.Redirect("Succes.aspx");
            }
            else
            {
                Response.Redirect("Fout.aspx?message="+result.message);
            }
        }
    }

    public class Result
    {
        public bool stored { get; set; }
        public string message { get; set; }

        public Result(bool s)
        {
            stored = s;
        }

        public Result(string m)
        {
            stored = false;
            message = m;
        }
    }
}