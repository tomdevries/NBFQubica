using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NBF.Qubica.Common;

namespace NBF.Qubica.Crypt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPassword.Text))
            {
                txtEncryptedPassword.Text = Common.Crypt.Encrypt(txtPassword.Text);
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtEncryptedPassword.Text))
            {
                txtDecrypted.Text = Common.Crypt.Decrypt(txtEncryptedPassword.Text);
            }
        }
    }
}
