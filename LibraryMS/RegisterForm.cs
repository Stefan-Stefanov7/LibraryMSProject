using LibraryMS;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace libraryMS
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            txtFullName = new TextBox();
            txtEmail = new TextBox();
            txtPassword = new TextBox();
            txtConfirmPassword = new TextBox();
            btnRegister = new Button();
            linkLogin = new LinkLabel();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 36F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(196, 22);
            label1.Name = "label1";
            label1.Size = new Size(429, 81);
            label1.TabIndex = 0;
            label1.Text = "REGISTRATION";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(133, 133);
            label2.Name = "label2";
            label2.Size = new Size(79, 20);
            label2.TabIndex = 1;
            label2.Text = "Full Name:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(133, 219);
            label3.Name = "label3";
            label3.Size = new Size(49, 20);
            label3.TabIndex = 2;
            label3.Text = "Email:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(504, 133);
            label4.Name = "label4";
            label4.Size = new Size(73, 20);
            label4.TabIndex = 3;
            label4.Text = "Password:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(504, 219);
            label5.Name = "label5";
            label5.Size = new Size(130, 20);
            label5.TabIndex = 4;
            label5.Text = "Confirm Password:";
            // 
            // txtFullName
            // 
            txtFullName.Location = new Point(133, 156);
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new Size(201, 27);
            txtFullName.TabIndex = 5;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(133, 242);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(201, 27);
            txtEmail.TabIndex = 6;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(504, 156);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(180, 27);
            txtPassword.TabIndex = 7;
            txtPassword.UseSystemPasswordChar = true;
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Location = new Point(504, 242);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Size = new Size(180, 27);
            txtConfirmPassword.TabIndex = 8;
            txtConfirmPassword.UseSystemPasswordChar = true;
            // 
            // btnRegister
            // 
            btnRegister.Location = new Point(347, 314);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(138, 51);
            btnRegister.TabIndex = 9;
            btnRegister.Text = "REGISTER";
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += btnRegister_Click;
            // 
            // linkLogin
            // 
            linkLogin.AutoSize = true;
            linkLogin.Location = new Point(347, 393);
            linkLogin.Name = "linkLogin";
            linkLogin.Size = new Size(138, 20);
            linkLogin.TabIndex = 10;
            linkLogin.TabStop = true;
            linkLogin.Text = "Already registered?";
            linkLogin.LinkClicked += linkLogin_LinkClicked;
            // 
            // RegisterForm
            // 
            ClientSize = new Size(800, 450);
            Controls.Add(linkLogin);
            Controls.Add(btnRegister);
            Controls.Add(txtConfirmPassword);
            Controls.Add(txtPassword);
            Controls.Add(txtEmail);
            Controls.Add(txtFullName);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "RegisterForm";
            Text = "Register";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox txtFullName;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private Button btnRegister;
        private LinkLabel linkLogin;

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrEmpty(fullName) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Моля, попълнете всички полета.");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Паролите не съвпадат.");
                return;
            }

            string hashedPassword = SecurityHelper.HashPassword(password);

            bool success = DatabaseHelper.RegisterUser(fullName, email, hashedPassword);

            if (success)
            {
                MessageBox.Show("Регистрацията беше успешна!");
                this.Close();
                LoginForm loginForm = new LoginForm();
                loginForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Потребител с този имейл вече съществува.");
            }
        }

        private void linkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }
    }
}
