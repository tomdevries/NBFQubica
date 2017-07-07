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
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                List<S_Text> texts = TextManager.GetTexts();

                foreach (S_Text st in texts)
                {
                    switch (st.label)
                    {
                        case "Web Site Titel":
                            web_site_title.Text = st.text;
                            break;
                        case "Home":
                            home.Text = st.text;
                            break;
                    }
                }

            }
        }
    }
}