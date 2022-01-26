using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LocalKeyVault
{
    public partial class LoginUI : Form
    {
        int triesCounter = 3;
        Utilities ut = new Utilities();
        public LoginUI()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var mainUI = new MainUI();
            
            if ("+JyFASUQK6G9vMJ/WJDvHg==" == ut.Encrypt(textBox1.Text))
            {
                mainUI.Show();
                Hide();
            }

            else
            {
                triesCounter--;
                var response = MessageBox.Show($"Remaining intents: {triesCounter}", "Incorrect password", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);

                if (response == DialogResult.Cancel || triesCounter==0)
                {
                    Close();
                }

            }
        }
    }
}
