namespace CatEd
{
    partial class CatEdMainForm
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
            this.cLevelView1 = new CatEd.CLevelView();
            this.EditorTabControl = new System.Windows.Forms.TabControl();
            this.LevelTabPage = new System.Windows.Forms.TabPage();
            this.TileTabPage = new System.Windows.Forms.TabPage();
            this.EntityTab = new System.Windows.Forms.TabPage();
            this.EntityListBox = new System.Windows.Forms.ListBox();
            this.RoomEntityListBox = new System.Windows.Forms.CheckedListBox();
            this.EditorTabControl.SuspendLayout();
            this.EntityTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // cLevelView1
            // 
            this.cLevelView1.Location = new System.Drawing.Point(-1, 0);
            this.cLevelView1.MouseHoverUpdatesOnly = false;
            this.cLevelView1.Name = "cLevelView1";
            this.cLevelView1.Size = new System.Drawing.Size(867, 670);
            this.cLevelView1.TabIndex = 0;
            this.cLevelView1.Text = "cLevelView1";
            // 
            // EditorTabControl
            // 
            this.EditorTabControl.Controls.Add(this.LevelTabPage);
            this.EditorTabControl.Controls.Add(this.TileTabPage);
            this.EditorTabControl.Controls.Add(this.EntityTab);
            this.EditorTabControl.Location = new System.Drawing.Point(872, 12);
            this.EditorTabControl.Name = "EditorTabControl";
            this.EditorTabControl.SelectedIndex = 0;
            this.EditorTabControl.Size = new System.Drawing.Size(263, 647);
            this.EditorTabControl.TabIndex = 1;
            // 
            // LevelTabPage
            // 
            this.LevelTabPage.Location = new System.Drawing.Point(4, 22);
            this.LevelTabPage.Name = "LevelTabPage";
            this.LevelTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.LevelTabPage.Size = new System.Drawing.Size(255, 621);
            this.LevelTabPage.TabIndex = 0;
            this.LevelTabPage.Text = "Level";
            this.LevelTabPage.UseVisualStyleBackColor = true;
            // 
            // TileTabPage
            // 
            this.TileTabPage.Location = new System.Drawing.Point(4, 22);
            this.TileTabPage.Name = "TileTabPage";
            this.TileTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.TileTabPage.Size = new System.Drawing.Size(255, 621);
            this.TileTabPage.TabIndex = 1;
            this.TileTabPage.Text = "Tiles";
            this.TileTabPage.UseVisualStyleBackColor = true;
            // 
            // EntityTab
            // 
            this.EntityTab.Controls.Add(this.RoomEntityListBox);
            this.EntityTab.Controls.Add(this.EntityListBox);
            this.EntityTab.Location = new System.Drawing.Point(4, 22);
            this.EntityTab.Name = "EntityTab";
            this.EntityTab.Padding = new System.Windows.Forms.Padding(3);
            this.EntityTab.Size = new System.Drawing.Size(255, 621);
            this.EntityTab.TabIndex = 2;
            this.EntityTab.Text = "Entities";
            this.EntityTab.UseVisualStyleBackColor = true;
            // 
            // EntityListBox
            // 
            this.EntityListBox.FormattingEnabled = true;
            this.EntityListBox.Location = new System.Drawing.Point(6, 338);
            this.EntityListBox.Name = "EntityListBox";
            this.EntityListBox.Size = new System.Drawing.Size(243, 277);
            this.EntityListBox.TabIndex = 0;
            // 
            // RoomEntityListBox
            // 
            this.RoomEntityListBox.FormattingEnabled = true;
            this.RoomEntityListBox.Location = new System.Drawing.Point(6, 6);
            this.RoomEntityListBox.Name = "RoomEntityListBox";
            this.RoomEntityListBox.Size = new System.Drawing.Size(243, 319);
            this.RoomEntityListBox.TabIndex = 1;
            // 
            // CatEdMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1147, 671);
            this.Controls.Add(this.EditorTabControl);
            this.Controls.Add(this.cLevelView1);
            this.Name = "CatEdMainForm";
            this.Text = "CatEd";
            this.EditorTabControl.ResumeLayout(false);
            this.EntityTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CLevelView cLevelView1;
        private System.Windows.Forms.TabControl EditorTabControl;
        private System.Windows.Forms.TabPage LevelTabPage;
        private System.Windows.Forms.TabPage TileTabPage;
        private System.Windows.Forms.TabPage EntityTab;
        private System.Windows.Forms.CheckedListBox RoomEntityListBox;
        private System.Windows.Forms.ListBox EntityListBox;
    }
}

