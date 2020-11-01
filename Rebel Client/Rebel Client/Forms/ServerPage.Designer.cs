    partial class ServerPage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerPage));
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.currentServerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.disconnectFromServerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.quitFromAppToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.yourProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.muteYourMicrophoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.muteYourHeadphonesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hearYourVoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitFromCurrentChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.materialSingleLineTextField1 = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.antiSpam = new System.Windows.Forms.Timer(this.components);
            this.joinConfirm = new System.Windows.Forms.Timer(this.components);
            this.ClearBuffer = new System.Windows.Forms.Timer(this.components);
            this.keepAlive = new System.Windows.Forms.Timer(this.components);
            this.keepAliveSend = new System.Windows.Forms.Timer(this.components);
            this.menuStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip2
            // 
            this.menuStrip2.BackColor = System.Drawing.Color.White;
            this.menuStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.currentServerToolStripMenuItem1,
            this.yourProfileToolStripMenuItem,
            this.toolsSettingsToolStripMenuItem});
            this.menuStrip2.Location = new System.Drawing.Point(2, 69);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(297, 24);
            this.menuStrip2.TabIndex = 11;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // currentServerToolStripMenuItem1
            // 
            this.currentServerToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.disconnectFromServerToolStripMenuItem1,
            this.quitFromAppToolStripMenuItem1});
            this.currentServerToolStripMenuItem1.Name = "currentServerToolStripMenuItem1";
            this.currentServerToolStripMenuItem1.Size = new System.Drawing.Size(93, 20);
            this.currentServerToolStripMenuItem1.Text = "Current server";
            // 
            // disconnectFromServerToolStripMenuItem1
            // 
            this.disconnectFromServerToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("disconnectFromServerToolStripMenuItem1.Image")));
            this.disconnectFromServerToolStripMenuItem1.Name = "disconnectFromServerToolStripMenuItem1";
            this.disconnectFromServerToolStripMenuItem1.Size = new System.Drawing.Size(196, 22);
            this.disconnectFromServerToolStripMenuItem1.Text = "Disconnect from server";
            this.disconnectFromServerToolStripMenuItem1.Click += new System.EventHandler(this.disconnectFromServerToolStripMenuItem1_Click);
            // 
            // quitFromAppToolStripMenuItem1
            // 
            this.quitFromAppToolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("quitFromAppToolStripMenuItem1.Image")));
            this.quitFromAppToolStripMenuItem1.Name = "quitFromAppToolStripMenuItem1";
            this.quitFromAppToolStripMenuItem1.Size = new System.Drawing.Size(196, 22);
            this.quitFromAppToolStripMenuItem1.Text = "Quit from app";
            this.quitFromAppToolStripMenuItem1.Click += new System.EventHandler(this.quitFromAppToolStripMenuItem1_Click);
            // 
            // yourProfileToolStripMenuItem
            // 
            this.yourProfileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.muteYourMicrophoneToolStripMenuItem,
            this.muteYourHeadphonesToolStripMenuItem,
            this.hearYourVoiceToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitFromCurrentChannelToolStripMenuItem});
            this.yourProfileToolStripMenuItem.Name = "yourProfileToolStripMenuItem";
            this.yourProfileToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.yourProfileToolStripMenuItem.Text = "Your profile";
            // 
            // muteYourMicrophoneToolStripMenuItem
            // 
            this.muteYourMicrophoneToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("muteYourMicrophoneToolStripMenuItem.Image")));
            this.muteYourMicrophoneToolStripMenuItem.Name = "muteYourMicrophoneToolStripMenuItem";
            this.muteYourMicrophoneToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.muteYourMicrophoneToolStripMenuItem.Text = "Mute your microphone";
            this.muteYourMicrophoneToolStripMenuItem.Click += new System.EventHandler(this.muteYourMicrophoneToolStripMenuItem_Click);
            // 
            // muteYourHeadphonesToolStripMenuItem
            // 
            this.muteYourHeadphonesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("muteYourHeadphonesToolStripMenuItem.Image")));
            this.muteYourHeadphonesToolStripMenuItem.Name = "muteYourHeadphonesToolStripMenuItem";
            this.muteYourHeadphonesToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.muteYourHeadphonesToolStripMenuItem.Text = "Mute your headphones";
            this.muteYourHeadphonesToolStripMenuItem.Click += new System.EventHandler(this.muteYourHeadphonesToolStripMenuItem_Click);
            // 
            // hearYourVoiceToolStripMenuItem
            // 
            this.hearYourVoiceToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("hearYourVoiceToolStripMenuItem.Image")));
            this.hearYourVoiceToolStripMenuItem.Name = "hearYourVoiceToolStripMenuItem";
            this.hearYourVoiceToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.hearYourVoiceToolStripMenuItem.Text = "Hear your voice";
            this.hearYourVoiceToolStripMenuItem.Click += new System.EventHandler(this.hearYourVoiceToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(205, 6);
            // 
            // exitFromCurrentChannelToolStripMenuItem
            // 
            this.exitFromCurrentChannelToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("exitFromCurrentChannelToolStripMenuItem.Image")));
            this.exitFromCurrentChannelToolStripMenuItem.Name = "exitFromCurrentChannelToolStripMenuItem";
            this.exitFromCurrentChannelToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.exitFromCurrentChannelToolStripMenuItem.Text = "Exit from current channel";
            this.exitFromCurrentChannelToolStripMenuItem.Click += new System.EventHandler(this.exitFromCurrentChannelToolStripMenuItem_Click);
            // 
            // toolsSettingsToolStripMenuItem
            // 
            this.toolsSettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem});
            this.toolsSettingsToolStripMenuItem.Name = "toolsSettingsToolStripMenuItem";
            this.toolsSettingsToolStripMenuItem.Size = new System.Drawing.Size(116, 20);
            this.toolsSettingsToolStripMenuItem.Text = "Tools And Settings";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("settingsToolStripMenuItem.Image")));
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.panel1.Location = new System.Drawing.Point(102, 98);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(568, 24);
            this.panel1.TabIndex = 13;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton4,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(2, 98);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(104, 25);
            this.toolStrip1.TabIndex = 14;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Mute your microphone";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Mute your headphones";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "Hear your voice";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "Exit from current channel";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.listBox1);
            this.panel2.Location = new System.Drawing.Point(12, 128);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(646, 329);
            this.panel2.TabIndex = 18;
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 18;
            this.listBox1.Location = new System.Drawing.Point(0, 2);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(645, 324);
            this.listBox1.TabIndex = 0;
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            // 
            // materialSingleLineTextField1
            // 
            this.materialSingleLineTextField1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.materialSingleLineTextField1.Depth = 0;
            this.materialSingleLineTextField1.Hint = "";
            this.materialSingleLineTextField1.Location = new System.Drawing.Point(12, 787);
            this.materialSingleLineTextField1.MaxLength = 2000;
            this.materialSingleLineTextField1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialSingleLineTextField1.Name = "materialSingleLineTextField1";
            this.materialSingleLineTextField1.PasswordChar = '\0';
            this.materialSingleLineTextField1.SelectedText = "";
            this.materialSingleLineTextField1.SelectionLength = 0;
            this.materialSingleLineTextField1.SelectionStart = 0;
            this.materialSingleLineTextField1.Size = new System.Drawing.Size(646, 23);
            this.materialSingleLineTextField1.TabIndex = 19;
            this.materialSingleLineTextField1.TabStop = false;
            this.materialSingleLineTextField1.UseSystemPasswordChar = false;
            this.materialSingleLineTextField1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.materialSingleLineTextField1_KeyDown);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 463);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(646, 318);
            this.tabControl1.TabIndex = 20;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 27);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(638, 287);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Global chat";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(3, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(632, 281);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.richTextBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 27);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(638, 287);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Channel chat";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBox2
            // 
            this.richTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox2.BackColor = System.Drawing.Color.White;
            this.richTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox2.Location = new System.Drawing.Point(3, 3);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(632, 281);
            this.richTextBox2.TabIndex = 1;
            this.richTextBox2.Text = "";
            // 
            // antiSpam
            // 
            this.antiSpam.Interval = 4000;
            this.antiSpam.Tick += new System.EventHandler(this.antiSpam_Tick);
            // 
            // joinConfirm
            // 
            this.joinConfirm.Interval = 1250;
            this.joinConfirm.Tick += new System.EventHandler(this.joinConfirm_Tick);
            // 
            // ClearBuffer
            // 
            this.ClearBuffer.Interval = 10000;
            this.ClearBuffer.Tick += new System.EventHandler(this.ClearBuffer_Tick);
            // 
            // keepAlive
            // 
            this.keepAlive.Interval = 6000;
            this.keepAlive.Tick += new System.EventHandler(this.keepAlive_Tick);
            // 
            // keepAliveSend
            // 
            this.keepAliveSend.Interval = 500;
            this.keepAliveSend.Tick += new System.EventHandler(this.keepAliveSend_Tick);
            // 
            // ServerPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(670, 823);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.materialSingleLineTextField1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip2);
            this.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ServerPage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rebel Client - The server";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerPage_FormClosing);
            this.Load += new System.EventHandler(this.ServerPage_Load);
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

    #endregion
    private System.Windows.Forms.ToolStripContainer toolStripContainer1;
    private System.Windows.Forms.MenuStrip menuStrip2;
    private System.Windows.Forms.ToolStripMenuItem currentServerToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem disconnectFromServerToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem quitFromAppToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem yourProfileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem toolsSettingsToolStripMenuItem;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton toolStripButton1;
    private System.Windows.Forms.ToolStripButton toolStripButton2;
    private System.Windows.Forms.ToolStripButton toolStripButton3;
    private System.Windows.Forms.ToolStripMenuItem muteYourMicrophoneToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem muteYourHeadphonesToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem exitFromCurrentChannelToolStripMenuItem;
    private System.Windows.Forms.Panel panel2;
    private MaterialSkin.Controls.MaterialSingleLineTextField materialSingleLineTextField1;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.RichTextBox richTextBox1;
    private System.Windows.Forms.RichTextBox richTextBox2;
    private System.Windows.Forms.Timer antiSpam;
    private System.Windows.Forms.ListBox listBox1;
    private System.Windows.Forms.Timer joinConfirm;
    private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
    private System.Windows.Forms.ToolStripButton toolStripButton4;
    private System.Windows.Forms.ToolStripMenuItem hearYourVoiceToolStripMenuItem;
    private System.Windows.Forms.Timer ClearBuffer;
    private System.Windows.Forms.Timer keepAlive;
    private System.Windows.Forms.Timer keepAliveSend;
}