using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Electronic_Diary.Forms
{
    public partial class FormAuth : Form
    {
        Thread th;
        public FormAuth()
        {
            InitializeComponent();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void tbLogin_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if(tbLogin.Text=="")
                {
                    tbLogin.Text = "Введите логин";
                    return;
                }
                tbLogin.ForeColor = Color.White;
                pnlInvalidName.Visible = false;
            }
            catch { }
        }

        private void tbPass_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (tbPass.Text == "")
                {
                    return;
                }
                tbPass.ForeColor = Color.White;
                tbPass.PasswordChar = '*';
                pnlInvalidPass.Visible = false;
            }
            catch { }
        }

        private void tbLogin_Click(object sender, EventArgs e)
        {
            tbLogin.SelectAll();
        }

        private void tbPass_Click(object sender, EventArgs e)
        {
            tbPass.SelectAll();
        }

        private void btnLogin_MouseEnter(object sender, EventArgs e)
        {
            btnLogin.ForeColor = Color.Black;
        }

        private void btnLogin_MouseLeave(object sender, EventArgs e)
        {
            btnLogin.ForeColor = Color.Lime;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (tbLogin.Text == "Введите логин")
            {
                pnlInvalidName.Visible = true;
                tbLogin.Focus();
                return;
            }
            if (tbPass.Text == "Введите пароль")
            {
                pnlInvalidPass.Visible = true;
                tbPass.Focus();
                return;
            }

            String loginUser = tbLogin.Text;
            String passUser = tbPass.Text;

            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `login` = @uL AND `pass` = @uP", db.getConnection());

            command.Parameters.Add("@uL",MySqlDbType.VarChar).Value = loginUser;
            command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = passUser;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                this.Close();
                th = new Thread(open);
                th.SetApartmentState(ApartmentState.STA);
                th.Start();
            }
            else
            {
                pnlInvalidName.Visible = true;
                pnlInvalidPass.Visible = true;
                return;
            }
        }

        public void open(object obj)
        {
            Application.Run(new Form1());
        }

        private void btnCloseApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panelTitle_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
