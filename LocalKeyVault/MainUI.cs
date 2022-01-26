using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LocalKeyVault
{
    public partial class MainUI : Form
    {
        #region Variables
        Utilities ut = new Utilities();
        #endregion

        public MainUI()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Reload();
        }

        private void Reload()
        {
            listBox1.DataSource = null;
            listBox1.DataSource = ut.ListSecrets();
            listBox1.DisplayMember = "name";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = (ut.Encrypt(textBox1.Text));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = (ut.Decrypt(textBox2.Text));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(textBox1.Text);
                //    label4.Text = "Copied!";
                //    Thread.Sleep(1500);
                //    label4.Text = "";
            }
            catch (Exception)
            {
                MessageBox.Show("The secret is not valid or is null.", "Error with the secret",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
                ut.AddSecret(textBox3.Text, ut.Encrypt(textBox1.Text));

            Reload();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.DataSource != null)
            {
                var item = (SecretsModel)listBox1.SelectedItem;
                textBox2.Text = item.secret;
                textBox1.Text = (ut.Decrypt(textBox2.Text));
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
                ut.EditSecret(textBox3.Text, ut.Encrypt(textBox1.Text));

            Reload();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
                ut.RemoveSecret(textBox3.Text, ut.Encrypt(textBox1.Text));

            Reload();
        }
    }
}