﻿/*************************************************************************
 *
 * DO NOT REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * @Authors:
 *       markusm, timop
 *
 * Copyright 2004-2009 by OM International
 *
 * This file is part of OpenPetra.org.
 *
 * OpenPetra.org is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * OpenPetra.org is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with OpenPetra.org.  If not, see <http://www.gnu.org/licenses/>.
 *
 ************************************************************************/
using System;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace Ict.Petra.Client.CommonDialogs
{
    partial class TLoginForm
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TLoginForm));
            this.lblUserName = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.prbLogin = new System.Windows.Forms.ProgressBar();
            this.pnlLoginControls = new System.Windows.Forms.Panel();
            this.chkNewNavigation = new System.Windows.Forms.CheckBox();
            this.pnlLoginControls.SuspendLayout();
            this.SuspendLayout();

            //
            // lblUserName
            //
            this.lblUserName.Location = new System.Drawing.Point(8, 8);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(188, 21);
            this.lblUserName.TabIndex = 2;
            this.lblUserName.Text = "&User ID:";
            this.lblUserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            //
            // txtUserName
            //
            this.txtUserName.Font =
                new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserName.Location = new System.Drawing.Point(8, 30);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(188, 21);
            this.txtUserName.TabIndex = 3;
            this.txtUserName.Leave += new System.EventHandler(this.TxtUserName_Leave);

            //
            // lblPassword
            //
            this.lblPassword.Location = new System.Drawing.Point(8, 58);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(188, 23);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "&Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            //
            // txtPassword
            //
            this.txtPassword.AcceptsReturn = true;
            this.txtPassword.Font =
                new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(8, 80);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(188, 21);
            this.txtPassword.TabIndex = 5;
            this.txtPassword.WordWrap = false;

            //
            // btnLogin
            //
            this.btnLogin.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogin.Image = ((System.Drawing.Image)(resources.GetObject("btnLogin.Image")));
            this.btnLogin.Location = new System.Drawing.Point(4, 8);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 25);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Text = " &Login";
            this.btnLogin.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);

            //
            // btnCancel
            //
            this.btnCancel.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(4, 36);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = " &Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);

            //
            // lblDatabase
            //
            this.lblDatabase.Location = new System.Drawing.Point(8, 106);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(188, 32);
            this.lblDatabase.TabIndex = 8;
            this.lblDatabase.Text = "Database:";
            this.lblDatabase.Visible = false;

            //
            // prbLogin
            //
            this.prbLogin.Anchor =
                ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) |
                                                      System.Windows.Forms.AnchorStyles.Right)));
            this.prbLogin.Location = new System.Drawing.Point(4, 38);
            this.prbLogin.Name = "prbLogin";
            this.prbLogin.Size = new System.Drawing.Size(76, 18);
            this.prbLogin.TabIndex = 10;
            this.prbLogin.Visible = false;

            //
            // pnlLoginControls
            //
            this.pnlLoginControls.Controls.Add(this.btnCancel);
            this.pnlLoginControls.Controls.Add(this.prbLogin);
            this.pnlLoginControls.Controls.Add(this.btnLogin);
            this.pnlLoginControls.Location = new System.Drawing.Point(205, 76);
            this.pnlLoginControls.Name = "pnlLoginControls";
            this.pnlLoginControls.Size = new System.Drawing.Size(82, 66);
            this.pnlLoginControls.TabIndex = 11;

            //
            // chkNewNavigation
            //
            this.chkNewNavigation.Location = new System.Drawing.Point(8, 112);
            this.chkNewNavigation.Name = "chkNewNavigation";
            this.chkNewNavigation.Size = new System.Drawing.Size(188, 24);
            this.chkNewNavigation.TabIndex = 12;
            this.chkNewNavigation.Text = "Use New Navigation";
            this.chkNewNavigation.UseVisualStyleBackColor = true;

            //
            // TLoginForm
            //
            this.AcceptButton = this.btnLogin;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(293, 142);
            this.Controls.Add(this.chkNewNavigation);
            this.Controls.Add(this.pnlLoginControls);
            this.Controls.Add(this.lblDatabase);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.lblUserName);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TLoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Petra Login";
            this.TopMost = false;
            this.Load += new System.EventHandler(this.TLoginForm_Load);
            this.Shown += new System.EventHandler(this.TLoginForm_Shown);
            this.pnlLoginControls.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.CheckBox chkNewNavigation;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.ProgressBar prbLogin;
        private System.Windows.Forms.Panel pnlLoginControls;
    }
}