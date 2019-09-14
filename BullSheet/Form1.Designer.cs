namespace BullSheet
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
            this.SpriteListBox = new System.Windows.Forms.ListBox();
            this.SpriteDataGrpBox = new System.Windows.Forms.GroupBox();
            this.SpriteTexBox = new System.Windows.Forms.ComboBox();
            this.SpriteHeightBox = new System.Windows.Forms.TextBox();
            this.SpriteLeftBox = new System.Windows.Forms.TextBox();
            this.SpriteImgBox = new System.Windows.Forms.TextBox();
            this.SpriteWidthBox = new System.Windows.Forms.TextBox();
            this.SpriteTopBox = new System.Windows.Forms.TextBox();
            this.SpriteXorigBox = new System.Windows.Forms.TextBox();
            this.SpriteNameBox = new System.Windows.Forms.TextBox();
            this.SpriteYorigBox = new System.Windows.Forms.TextBox();
            this.SpriteYorigLabel = new System.Windows.Forms.Label();
            this.SpriteXorigLabel = new System.Windows.Forms.Label();
            this.SpriteImagesLabel = new System.Windows.Forms.Label();
            this.SpriteHeightLabel = new System.Windows.Forms.Label();
            this.SpriteWidthLabel = new System.Windows.Forms.Label();
            this.SpriteTopLabel = new System.Windows.Forms.Label();
            this.SpriteLeftLabel = new System.Windows.Forms.Label();
            this.SpriteTexLabel = new System.Windows.Forms.Label();
            this.SpriteNameLable = new System.Windows.Forms.Label();
            this.AddButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.PrevImgButton = new System.Windows.Forms.Button();
            this.NextImgButton = new System.Windows.Forms.Button();
            this.ImgLabel = new System.Windows.Forms.Label();
            this.SpritePreview = new BullSheet.SpritePreview();
            this.TextureSheetPreview = new BullSheet.TextureSheetPreview();
            this.SpriteDataGrpBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SpriteListBox
            // 
            this.SpriteListBox.FormattingEnabled = true;
            this.SpriteListBox.Location = new System.Drawing.Point(8, 284);
            this.SpriteListBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SpriteListBox.Name = "SpriteListBox";
            this.SpriteListBox.Size = new System.Drawing.Size(306, 186);
            this.SpriteListBox.TabIndex = 0;
            this.SpriteListBox.SelectedIndexChanged += new System.EventHandler(this.SpriteListBox_SelectedIndexChanged);
            // 
            // SpriteDataGrpBox
            // 
            this.SpriteDataGrpBox.Controls.Add(this.SpriteTexBox);
            this.SpriteDataGrpBox.Controls.Add(this.SpriteHeightBox);
            this.SpriteDataGrpBox.Controls.Add(this.SpriteLeftBox);
            this.SpriteDataGrpBox.Controls.Add(this.SpriteImgBox);
            this.SpriteDataGrpBox.Controls.Add(this.SpriteWidthBox);
            this.SpriteDataGrpBox.Controls.Add(this.SpriteTopBox);
            this.SpriteDataGrpBox.Controls.Add(this.SpriteXorigBox);
            this.SpriteDataGrpBox.Controls.Add(this.SpriteNameBox);
            this.SpriteDataGrpBox.Controls.Add(this.SpriteYorigBox);
            this.SpriteDataGrpBox.Controls.Add(this.SpriteYorigLabel);
            this.SpriteDataGrpBox.Controls.Add(this.SpriteXorigLabel);
            this.SpriteDataGrpBox.Controls.Add(this.SpriteImagesLabel);
            this.SpriteDataGrpBox.Controls.Add(this.SpriteHeightLabel);
            this.SpriteDataGrpBox.Controls.Add(this.SpriteWidthLabel);
            this.SpriteDataGrpBox.Controls.Add(this.SpriteTopLabel);
            this.SpriteDataGrpBox.Controls.Add(this.SpriteLeftLabel);
            this.SpriteDataGrpBox.Controls.Add(this.SpriteTexLabel);
            this.SpriteDataGrpBox.Controls.Add(this.SpriteNameLable);
            this.SpriteDataGrpBox.Location = new System.Drawing.Point(8, 8);
            this.SpriteDataGrpBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SpriteDataGrpBox.Name = "SpriteDataGrpBox";
            this.SpriteDataGrpBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SpriteDataGrpBox.Size = new System.Drawing.Size(305, 272);
            this.SpriteDataGrpBox.TabIndex = 1;
            this.SpriteDataGrpBox.TabStop = false;
            this.SpriteDataGrpBox.Text = "Sprite Data";
            // 
            // SpriteTexBox
            // 
            this.SpriteTexBox.FormattingEnabled = true;
            this.SpriteTexBox.Location = new System.Drawing.Point(45, 45);
            this.SpriteTexBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SpriteTexBox.Name = "SpriteTexBox";
            this.SpriteTexBox.Size = new System.Drawing.Size(257, 21);
            this.SpriteTexBox.TabIndex = 18;
            this.SpriteTexBox.SelectedIndexChanged += new System.EventHandler(this.SpriteTexBox_SelectedIndexChanged);
            // 
            // SpriteHeightBox
            // 
            this.SpriteHeightBox.Location = new System.Drawing.Point(45, 160);
            this.SpriteHeightBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SpriteHeightBox.Name = "SpriteHeightBox";
            this.SpriteHeightBox.Size = new System.Drawing.Size(257, 20);
            this.SpriteHeightBox.TabIndex = 16;
            this.SpriteHeightBox.TextChanged += new System.EventHandler(this.SpriteHeightBox_TextChanged);
            // 
            // SpriteLeftBox
            // 
            this.SpriteLeftBox.Location = new System.Drawing.Point(45, 73);
            this.SpriteLeftBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SpriteLeftBox.Name = "SpriteLeftBox";
            this.SpriteLeftBox.Size = new System.Drawing.Size(257, 20);
            this.SpriteLeftBox.TabIndex = 15;
            this.SpriteLeftBox.TextChanged += new System.EventHandler(this.SpriteLeftBox_TextChanged);
            // 
            // SpriteImgBox
            // 
            this.SpriteImgBox.Location = new System.Drawing.Point(45, 191);
            this.SpriteImgBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SpriteImgBox.Name = "SpriteImgBox";
            this.SpriteImgBox.Size = new System.Drawing.Size(257, 20);
            this.SpriteImgBox.TabIndex = 14;
            this.SpriteImgBox.TextChanged += new System.EventHandler(this.SpriteImgBox_TextChanged);
            // 
            // SpriteWidthBox
            // 
            this.SpriteWidthBox.Location = new System.Drawing.Point(45, 131);
            this.SpriteWidthBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SpriteWidthBox.Name = "SpriteWidthBox";
            this.SpriteWidthBox.Size = new System.Drawing.Size(257, 20);
            this.SpriteWidthBox.TabIndex = 13;
            this.SpriteWidthBox.TextChanged += new System.EventHandler(this.SpriteWidthBox_TextChanged);
            // 
            // SpriteTopBox
            // 
            this.SpriteTopBox.Location = new System.Drawing.Point(45, 102);
            this.SpriteTopBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SpriteTopBox.Name = "SpriteTopBox";
            this.SpriteTopBox.Size = new System.Drawing.Size(257, 20);
            this.SpriteTopBox.TabIndex = 12;
            this.SpriteTopBox.TextChanged += new System.EventHandler(this.SpriteTopBox_TextChanged);
            // 
            // SpriteXorigBox
            // 
            this.SpriteXorigBox.Location = new System.Drawing.Point(45, 219);
            this.SpriteXorigBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SpriteXorigBox.Name = "SpriteXorigBox";
            this.SpriteXorigBox.Size = new System.Drawing.Size(257, 20);
            this.SpriteXorigBox.TabIndex = 11;
            this.SpriteXorigBox.TextChanged += new System.EventHandler(this.SpriteXorigBox_TextChanged);
            // 
            // SpriteNameBox
            // 
            this.SpriteNameBox.Location = new System.Drawing.Point(45, 21);
            this.SpriteNameBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SpriteNameBox.Name = "SpriteNameBox";
            this.SpriteNameBox.Size = new System.Drawing.Size(257, 20);
            this.SpriteNameBox.TabIndex = 10;
            this.SpriteNameBox.TextChanged += new System.EventHandler(this.SpriteNameBox_TextChanged);
            // 
            // SpriteYorigBox
            // 
            this.SpriteYorigBox.Location = new System.Drawing.Point(45, 249);
            this.SpriteYorigBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SpriteYorigBox.Name = "SpriteYorigBox";
            this.SpriteYorigBox.Size = new System.Drawing.Size(257, 20);
            this.SpriteYorigBox.TabIndex = 9;
            this.SpriteYorigBox.TextChanged += new System.EventHandler(this.SpriteYorigBox_TextChanged);
            // 
            // SpriteYorigLabel
            // 
            this.SpriteYorigLabel.AutoSize = true;
            this.SpriteYorigLabel.Location = new System.Drawing.Point(0, 249);
            this.SpriteYorigLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SpriteYorigLabel.Name = "SpriteYorigLabel";
            this.SpriteYorigLabel.Size = new System.Drawing.Size(31, 13);
            this.SpriteYorigLabel.TabIndex = 8;
            this.SpriteYorigLabel.Text = "Yorig";
            // 
            // SpriteXorigLabel
            // 
            this.SpriteXorigLabel.AutoSize = true;
            this.SpriteXorigLabel.Location = new System.Drawing.Point(0, 219);
            this.SpriteXorigLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SpriteXorigLabel.Name = "SpriteXorigLabel";
            this.SpriteXorigLabel.Size = new System.Drawing.Size(31, 13);
            this.SpriteXorigLabel.TabIndex = 7;
            this.SpriteXorigLabel.Text = "Xorig";
            // 
            // SpriteImagesLabel
            // 
            this.SpriteImagesLabel.AutoSize = true;
            this.SpriteImagesLabel.Location = new System.Drawing.Point(0, 191);
            this.SpriteImagesLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SpriteImagesLabel.Name = "SpriteImagesLabel";
            this.SpriteImagesLabel.Size = new System.Drawing.Size(41, 13);
            this.SpriteImagesLabel.TabIndex = 6;
            this.SpriteImagesLabel.Text = "Images";
            // 
            // SpriteHeightLabel
            // 
            this.SpriteHeightLabel.AutoSize = true;
            this.SpriteHeightLabel.Location = new System.Drawing.Point(0, 160);
            this.SpriteHeightLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SpriteHeightLabel.Name = "SpriteHeightLabel";
            this.SpriteHeightLabel.Size = new System.Drawing.Size(38, 13);
            this.SpriteHeightLabel.TabIndex = 5;
            this.SpriteHeightLabel.Text = "Height";
            // 
            // SpriteWidthLabel
            // 
            this.SpriteWidthLabel.AutoSize = true;
            this.SpriteWidthLabel.Location = new System.Drawing.Point(0, 131);
            this.SpriteWidthLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SpriteWidthLabel.Name = "SpriteWidthLabel";
            this.SpriteWidthLabel.Size = new System.Drawing.Size(35, 13);
            this.SpriteWidthLabel.TabIndex = 4;
            this.SpriteWidthLabel.Text = "Width";
            // 
            // SpriteTopLabel
            // 
            this.SpriteTopLabel.AutoSize = true;
            this.SpriteTopLabel.Location = new System.Drawing.Point(0, 102);
            this.SpriteTopLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SpriteTopLabel.Name = "SpriteTopLabel";
            this.SpriteTopLabel.Size = new System.Drawing.Size(26, 13);
            this.SpriteTopLabel.TabIndex = 3;
            this.SpriteTopLabel.Text = "Top";
            // 
            // SpriteLeftLabel
            // 
            this.SpriteLeftLabel.AutoSize = true;
            this.SpriteLeftLabel.Location = new System.Drawing.Point(0, 73);
            this.SpriteLeftLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SpriteLeftLabel.Name = "SpriteLeftLabel";
            this.SpriteLeftLabel.Size = new System.Drawing.Size(25, 13);
            this.SpriteLeftLabel.TabIndex = 2;
            this.SpriteLeftLabel.Text = "Left";
            // 
            // SpriteTexLabel
            // 
            this.SpriteTexLabel.AutoSize = true;
            this.SpriteTexLabel.Location = new System.Drawing.Point(0, 47);
            this.SpriteTexLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SpriteTexLabel.Name = "SpriteTexLabel";
            this.SpriteTexLabel.Size = new System.Drawing.Size(43, 13);
            this.SpriteTexLabel.TabIndex = 1;
            this.SpriteTexLabel.Text = "Texture";
            // 
            // SpriteNameLable
            // 
            this.SpriteNameLable.AutoSize = true;
            this.SpriteNameLable.Location = new System.Drawing.Point(0, 21);
            this.SpriteNameLable.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SpriteNameLable.Name = "SpriteNameLable";
            this.SpriteNameLable.Size = new System.Drawing.Size(35, 13);
            this.SpriteNameLable.TabIndex = 0;
            this.SpriteNameLable.Text = "Name";
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(8, 480);
            this.AddButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(93, 29);
            this.AddButton.TabIndex = 2;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(220, 480);
            this.RemoveButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(93, 29);
            this.RemoveButton.TabIndex = 3;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(573, 480);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(93, 29);
            this.SaveButton.TabIndex = 4;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // PrevImgButton
            // 
            this.PrevImgButton.Location = new System.Drawing.Point(317, 482);
            this.PrevImgButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PrevImgButton.Name = "PrevImgButton";
            this.PrevImgButton.Size = new System.Drawing.Size(93, 29);
            this.PrevImgButton.TabIndex = 7;
            this.PrevImgButton.Text = "Prev Img";
            this.PrevImgButton.UseVisualStyleBackColor = true;
            this.PrevImgButton.Click += new System.EventHandler(this.PrevImgButton_Click);
            // 
            // NextImgButton
            // 
            this.NextImgButton.Location = new System.Drawing.Point(413, 482);
            this.NextImgButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.NextImgButton.Name = "NextImgButton";
            this.NextImgButton.Size = new System.Drawing.Size(93, 29);
            this.NextImgButton.TabIndex = 8;
            this.NextImgButton.Text = "Next Img";
            this.NextImgButton.UseVisualStyleBackColor = true;
            this.NextImgButton.Click += new System.EventHandler(this.NextImgButton_Click);
            // 
            // ImgLabel
            // 
            this.ImgLabel.AutoSize = true;
            this.ImgLabel.Location = new System.Drawing.Point(510, 489);
            this.ImgLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.ImgLabel.Name = "ImgLabel";
            this.ImgLabel.Size = new System.Drawing.Size(35, 13);
            this.ImgLabel.TabIndex = 9;
            this.ImgLabel.Text = "label1";
            // 
            // SpritePreview
            // 
            this.SpritePreview.Location = new System.Drawing.Point(317, 284);
            this.SpritePreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SpritePreview.MouseHoverUpdatesOnly = false;
            this.SpritePreview.Name = "SpritePreview";
            this.SpritePreview.Size = new System.Drawing.Size(348, 185);
            this.SpritePreview.TabIndex = 11;
            this.SpritePreview.Text = "spritePreview1";
            // 
            // TextureSheetPreview
            // 
            this.TextureSheetPreview.Location = new System.Drawing.Point(317, 8);
            this.TextureSheetPreview.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.TextureSheetPreview.MouseHoverUpdatesOnly = false;
            this.TextureSheetPreview.Name = "TextureSheetPreview";
            this.TextureSheetPreview.Size = new System.Drawing.Size(348, 272);
            this.TextureSheetPreview.TabIndex = 10;
            this.TextureSheetPreview.Text = "textureSheetPreview1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 518);
            this.Controls.Add(this.SpritePreview);
            this.Controls.Add(this.TextureSheetPreview);
            this.Controls.Add(this.ImgLabel);
            this.Controls.Add(this.NextImgButton);
            this.Controls.Add(this.PrevImgButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.SpriteDataGrpBox);
            this.Controls.Add(this.SpriteListBox);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "BullSheet";
            this.SpriteDataGrpBox.ResumeLayout(false);
            this.SpriteDataGrpBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox SpriteListBox;
        private System.Windows.Forms.GroupBox SpriteDataGrpBox;
        private System.Windows.Forms.TextBox SpriteHeightBox;
        private System.Windows.Forms.TextBox SpriteLeftBox;
        private System.Windows.Forms.TextBox SpriteImgBox;
        private System.Windows.Forms.TextBox SpriteWidthBox;
        private System.Windows.Forms.TextBox SpriteTopBox;
        private System.Windows.Forms.TextBox SpriteXorigBox;
        private System.Windows.Forms.TextBox SpriteNameBox;
        private System.Windows.Forms.TextBox SpriteYorigBox;
        private System.Windows.Forms.Label SpriteYorigLabel;
        private System.Windows.Forms.Label SpriteXorigLabel;
        private System.Windows.Forms.Label SpriteImagesLabel;
        private System.Windows.Forms.Label SpriteHeightLabel;
        private System.Windows.Forms.Label SpriteWidthLabel;
        private System.Windows.Forms.Label SpriteTopLabel;
        private System.Windows.Forms.Label SpriteLeftLabel;
        private System.Windows.Forms.Label SpriteTexLabel;
        private System.Windows.Forms.Label SpriteNameLable;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button PrevImgButton;
        private System.Windows.Forms.Button NextImgButton;
        private System.Windows.Forms.Label ImgLabel;
        private TextureSheetPreview TextureSheetPreview;
        private SpritePreview SpritePreview;
        private System.Windows.Forms.ComboBox SpriteTexBox;
    }
}

