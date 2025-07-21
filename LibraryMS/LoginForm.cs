using libraryMS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibraryMS
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                lblMessage.Text = "Please enter both email and password.";
                return;
            }

            string hashedPassword = SecurityHelper.HashPassword(password);
            var user = DatabaseHelper.GetUserByEmail(email);

            if (user != null && user.Password == hashedPassword)
            {
                lblMessage.Text = "Login successful!";

            }
            else
            {
                lblMessage.Text = "Invalid email or password.";
            }
        }

        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide(); 

            using (RegisterForm registerForm = new RegisterForm())
            {
                registerForm.ShowDialog(); 
            }

            this.Show(); 
        }
    }
}
