namespace PropsForThat
{
    partial class Form1
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DebugLabel = new System.Windows.Forms.Label();
            this.HPBox = new System.Windows.Forms.TextBox();
            this.ColHBox = new System.Windows.Forms.TextBox();
            this.ColWBox = new System.Windows.Forms.TextBox();
            this.SpriteNameBox = new System.Windows.Forms.TextBox();
            this.PropNameBox = new System.Windows.Forms.TextBox();
            this.HPLabel = new System.Windows.Forms.Label();
            this.ColHLabel = new System.Windows.Forms.Label();
            this.ColWLabel = new System.Windows.Forms.Label();
            this.SpriteNameLabel = new System.Windows.Forms.Label();
            this.PropNameLabel = new System.Windows.Forms.Label();
            this.AddButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Location = new System.Drawing.Point(12, 397);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(504, 304);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DebugLabel);
            this.groupBox1.Controls.Add(this.HPBox);
            this.groupBox1.Controls.Add(this.ColHBox);
            this.groupBox1.Controls.Add(this.ColWBox);
            this.groupBox1.Controls.Add(this.SpriteNameBox);
            this.groupBox1.Controls.Add(this.PropNameBox);
            this.groupBox1.Controls.Add(this.HPLabel);
            this.groupBox1.Controls.Add(this.ColHLabel);
            this.groupBox1.Controls.Add(this.ColWLabel);
            this.groupBox1.Controls.Add(this.SpriteNameLabel);
            this.groupBox1.Controls.Add(this.PropNameLabel);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(504, 379);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Prop Data";
            // 
            // DebugLabel
            // 
            this.DebugLabel.AutoSize = true;
            this.DebugLabel.Location = new System.Drawing.Point(180, 313);
            this.DebugLabel.Name = "DebugLabel";
            this.DebugLabel.Size = new System.Drawing.Size(79, 20);
            this.DebugLabel.TabIndex = 5;
            this.DebugLabel.Text = "debug ind";
            // 
            // HPBox
            // 
            this.HPBox.Location = new System.Drawing.Point(122, 244);
            this.HPBox.Name = "HPBox";
            this.HPBox.Size = new System.Drawing.Size(376, 26);
            this.HPBox.TabIndex = 9;
            this.HPBox.TextChanged += new System.EventHandler(this.HPBox_TextChanged);
            // 
            // ColHBox
            // 
            this.ColHBox.Location = new System.Drawing.Point(122, 198);
            this.ColHBox.Name = "ColHBox";
            this.ColHBox.Size = new System.Drawing.Size(376, 26);
            this.ColHBox.TabIndex = 8;
            this.ColHBox.TextChanged += new System.EventHandler(this.ColHBox_TextChanged);
            // 
            // ColWBox
            // 
            this.ColWBox.Location = new System.Drawing.Point(122, 151);
            this.ColWBox.Name = "ColWBox";
            this.ColWBox.Size = new System.Drawing.Size(376, 26);
            this.ColWBox.TabIndex = 7;
            this.ColWBox.TextChanged += new System.EventHandler(this.ColWBox_TextChanged);
            // 
            // SpriteNameBox
            // 
            this.SpriteNameBox.Location = new System.Drawing.Point(122, 105);
            this.SpriteNameBox.Name = "SpriteNameBox";
            this.SpriteNameBox.Size = new System.Drawing.Size(376, 26);
            this.SpriteNameBox.TabIndex = 6;
            this.SpriteNameBox.TextChanged += new System.EventHandler(this.SpriteNameBox_TextChanged);
            // 
            // PropNameBox
            // 
            this.PropNameBox.Location = new System.Drawing.Point(122, 60);
            this.PropNameBox.Name = "PropNameBox";
            this.PropNameBox.Size = new System.Drawing.Size(376, 26);
            this.PropNameBox.TabIndex = 5;
            this.PropNameBox.TextChanged += new System.EventHandler(this.PropNameBox_TextChanged);
            // 
            // HPLabel
            // 
            this.HPLabel.AutoSize = true;
            this.HPLabel.Location = new System.Drawing.Point(6, 244);
            this.HPLabel.Name = "HPLabel";
            this.HPLabel.Size = new System.Drawing.Size(56, 20);
            this.HPLabel.TabIndex = 4;
            this.HPLabel.Text = "Health";
            // 
            // ColHLabel
            // 
            this.ColHLabel.AutoSize = true;
            this.ColHLabel.Location = new System.Drawing.Point(6, 198);
            this.ColHLabel.Name = "ColHLabel";
            this.ColHLabel.Size = new System.Drawing.Size(83, 20);
            this.ColHLabel.TabIndex = 3;
            this.ColHLabel.Text = "Col Height";
            // 
            // ColWLabel
            // 
            this.ColWLabel.AutoSize = true;
            this.ColWLabel.Location = new System.Drawing.Point(6, 151);
            this.ColWLabel.Name = "ColWLabel";
            this.ColWLabel.Size = new System.Drawing.Size(77, 20);
            this.ColWLabel.TabIndex = 2;
            this.ColWLabel.Text = "Col Width";
            // 
            // SpriteNameLabel
            // 
            this.SpriteNameLabel.AutoSize = true;
            this.SpriteNameLabel.Location = new System.Drawing.Point(6, 105);
            this.SpriteNameLabel.Name = "SpriteNameLabel";
            this.SpriteNameLabel.Size = new System.Drawing.Size(97, 20);
            this.SpriteNameLabel.TabIndex = 1;
            this.SpriteNameLabel.Text = "Sprite Name";
            // 
            // PropNameLabel
            // 
            this.PropNameLabel.AutoSize = true;
            this.PropNameLabel.Location = new System.Drawing.Point(6, 60);
            this.PropNameLabel.Name = "PropNameLabel";
            this.PropNameLabel.Size = new System.Drawing.Size(88, 20);
            this.PropNameLabel.TabIndex = 0;
            this.PropNameLabel.Text = "Prop Name";
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(12, 714);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(109, 51);
            this.AddButton.TabIndex = 2;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(134, 714);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(109, 51);
            this.RemoveButton.TabIndex = 3;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(407, 714);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(109, 51);
            this.SaveButton.TabIndex = 4;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 777);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listBox1);
            this.Name = "Form1";
            this.Text = "PropsForThat";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox HPBox;
        private System.Windows.Forms.TextBox ColHBox;
        private System.Windows.Forms.TextBox ColWBox;
        private System.Windows.Forms.TextBox SpriteNameBox;
        private System.Windows.Forms.TextBox PropNameBox;
        private System.Windows.Forms.Label HPLabel;
        private System.Windows.Forms.Label ColHLabel;
        private System.Windows.Forms.Label ColWLabel;
        private System.Windows.Forms.Label SpriteNameLabel;
        private System.Windows.Forms.Label PropNameLabel;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Label DebugLabel;
    }
}

