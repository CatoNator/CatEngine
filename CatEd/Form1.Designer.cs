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
            this.LevelInfoBox = new System.Windows.Forms.GroupBox();
            this.PlayerInfoBox = new System.Windows.Forms.GroupBox();
            this.PXLocLabel = new System.Windows.Forms.Label();
            this.PDirBox = new System.Windows.Forms.TextBox();
            this.PDirLabel = new System.Windows.Forms.Label();
            this.PXLocBox = new System.Windows.Forms.TextBox();
            this.PYLocLabel = new System.Windows.Forms.Label();
            this.PYLocBox = new System.Windows.Forms.TextBox();
            this.WallsTab = new System.Windows.Forms.TabPage();
            this.WallDataBox = new System.Windows.Forms.GroupBox();
            this.WallYScaleBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.WallXScaleBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.WallXLocBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.WallYLocBox = new System.Windows.Forms.TextBox();
            this.WallRemoveButton = new System.Windows.Forms.Button();
            this.WallAddButton = new System.Windows.Forms.Button();
            this.WallListBox = new System.Windows.Forms.CheckedListBox();
            this.EnemyTab = new System.Windows.Forms.TabPage();
            this.EnemyRemoveButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.EnemyTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.EDirBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.EXLocbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.EYLocBox = new System.Windows.Forms.TextBox();
            this.EnemyAddButton = new System.Windows.Forms.Button();
            this.EnemyListBox = new System.Windows.Forms.CheckedListBox();
            this.ItemsTab = new System.Windows.Forms.TabPage();
            this.ItemDataBox = new System.Windows.Forms.GroupBox();
            this.ItemTypeComboBox = new System.Windows.Forms.ComboBox();
            this.ItemTypeLabel = new System.Windows.Forms.Label();
            this.ItemXLocLabel = new System.Windows.Forms.Label();
            this.ItemXLocBox = new System.Windows.Forms.TextBox();
            this.ItemYLocLabel = new System.Windows.Forms.Label();
            this.ItemYLocBox = new System.Windows.Forms.TextBox();
            this.ItemRemoveButton = new System.Windows.Forms.Button();
            this.ItemListBox = new System.Windows.Forms.CheckedListBox();
            this.ItemAddButton = new System.Windows.Forms.Button();
            this.PropsTab = new System.Windows.Forms.TabPage();
            this.PropDataBox = new System.Windows.Forms.GroupBox();
            this.PropTypeComboBox = new System.Windows.Forms.ComboBox();
            this.PropTypeLabel = new System.Windows.Forms.Label();
            this.PropXLocLabel = new System.Windows.Forms.Label();
            this.PropDirBox = new System.Windows.Forms.TextBox();
            this.PropDirLabel = new System.Windows.Forms.Label();
            this.PropXLocBox = new System.Windows.Forms.TextBox();
            this.PropYLocLabel = new System.Windows.Forms.Label();
            this.PropYLocBox = new System.Windows.Forms.TextBox();
            this.PropListBox = new System.Windows.Forms.CheckedListBox();
            this.PropRemoveButton = new System.Windows.Forms.Button();
            this.PropAddButton = new System.Windows.Forms.Button();
            this.EditorTabControl.SuspendLayout();
            this.LevelTabPage.SuspendLayout();
            this.PlayerInfoBox.SuspendLayout();
            this.WallsTab.SuspendLayout();
            this.WallDataBox.SuspendLayout();
            this.EnemyTab.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.ItemsTab.SuspendLayout();
            this.ItemDataBox.SuspendLayout();
            this.PropsTab.SuspendLayout();
            this.PropDataBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // cLevelView1
            // 
            this.cLevelView1.Location = new System.Drawing.Point(12, 12);
            this.cLevelView1.MouseHoverUpdatesOnly = false;
            this.cLevelView1.Name = "cLevelView1";
            this.cLevelView1.Size = new System.Drawing.Size(854, 643);
            this.cLevelView1.TabIndex = 0;
            this.cLevelView1.Text = "cLevelView1";
            // 
            // EditorTabControl
            // 
            this.EditorTabControl.Controls.Add(this.LevelTabPage);
            this.EditorTabControl.Controls.Add(this.WallsTab);
            this.EditorTabControl.Controls.Add(this.EnemyTab);
            this.EditorTabControl.Controls.Add(this.ItemsTab);
            this.EditorTabControl.Controls.Add(this.PropsTab);
            this.EditorTabControl.Location = new System.Drawing.Point(872, 12);
            this.EditorTabControl.Name = "EditorTabControl";
            this.EditorTabControl.SelectedIndex = 0;
            this.EditorTabControl.Size = new System.Drawing.Size(302, 647);
            this.EditorTabControl.TabIndex = 1;
            // 
            // LevelTabPage
            // 
            this.LevelTabPage.Controls.Add(this.LevelInfoBox);
            this.LevelTabPage.Controls.Add(this.PlayerInfoBox);
            this.LevelTabPage.Location = new System.Drawing.Point(4, 22);
            this.LevelTabPage.Name = "LevelTabPage";
            this.LevelTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.LevelTabPage.Size = new System.Drawing.Size(294, 621);
            this.LevelTabPage.TabIndex = 0;
            this.LevelTabPage.Text = "Level";
            this.LevelTabPage.UseVisualStyleBackColor = true;
            // 
            // LevelInfoBox
            // 
            this.LevelInfoBox.Location = new System.Drawing.Point(6, 6);
            this.LevelInfoBox.Name = "LevelInfoBox";
            this.LevelInfoBox.Size = new System.Drawing.Size(282, 499);
            this.LevelInfoBox.TabIndex = 1;
            this.LevelInfoBox.TabStop = false;
            this.LevelInfoBox.Text = "Level Data";
            // 
            // PlayerInfoBox
            // 
            this.PlayerInfoBox.Controls.Add(this.PXLocLabel);
            this.PlayerInfoBox.Controls.Add(this.PDirBox);
            this.PlayerInfoBox.Controls.Add(this.PDirLabel);
            this.PlayerInfoBox.Controls.Add(this.PXLocBox);
            this.PlayerInfoBox.Controls.Add(this.PYLocLabel);
            this.PlayerInfoBox.Controls.Add(this.PYLocBox);
            this.PlayerInfoBox.Location = new System.Drawing.Point(6, 511);
            this.PlayerInfoBox.Name = "PlayerInfoBox";
            this.PlayerInfoBox.Size = new System.Drawing.Size(282, 104);
            this.PlayerInfoBox.TabIndex = 0;
            this.PlayerInfoBox.TabStop = false;
            this.PlayerInfoBox.Text = "Player Spawn";
            // 
            // PXLocLabel
            // 
            this.PXLocLabel.AutoSize = true;
            this.PXLocLabel.Location = new System.Drawing.Point(6, 22);
            this.PXLocLabel.Name = "PXLocLabel";
            this.PXLocLabel.Size = new System.Drawing.Size(54, 13);
            this.PXLocLabel.TabIndex = 0;
            this.PXLocLabel.Text = "X location";
            // 
            // PDirBox
            // 
            this.PDirBox.Location = new System.Drawing.Point(66, 71);
            this.PDirBox.Name = "PDirBox";
            this.PDirBox.Size = new System.Drawing.Size(209, 20);
            this.PDirBox.TabIndex = 5;
            // 
            // PDirLabel
            // 
            this.PDirLabel.AutoSize = true;
            this.PDirLabel.Location = new System.Drawing.Point(3, 74);
            this.PDirLabel.Name = "PDirLabel";
            this.PDirLabel.Size = new System.Drawing.Size(49, 13);
            this.PDirLabel.TabIndex = 2;
            this.PDirLabel.Text = "Direction";
            // 
            // PXLocBox
            // 
            this.PXLocBox.Location = new System.Drawing.Point(66, 19);
            this.PXLocBox.Name = "PXLocBox";
            this.PXLocBox.Size = new System.Drawing.Size(209, 20);
            this.PXLocBox.TabIndex = 3;
            // 
            // PYLocLabel
            // 
            this.PYLocLabel.AutoSize = true;
            this.PYLocLabel.Location = new System.Drawing.Point(6, 48);
            this.PYLocLabel.Name = "PYLocLabel";
            this.PYLocLabel.Size = new System.Drawing.Size(54, 13);
            this.PYLocLabel.TabIndex = 1;
            this.PYLocLabel.Text = "Y location";
            // 
            // PYLocBox
            // 
            this.PYLocBox.Location = new System.Drawing.Point(66, 45);
            this.PYLocBox.Name = "PYLocBox";
            this.PYLocBox.Size = new System.Drawing.Size(209, 20);
            this.PYLocBox.TabIndex = 4;
            // 
            // WallsTab
            // 
            this.WallsTab.Controls.Add(this.WallDataBox);
            this.WallsTab.Controls.Add(this.WallRemoveButton);
            this.WallsTab.Controls.Add(this.WallAddButton);
            this.WallsTab.Controls.Add(this.WallListBox);
            this.WallsTab.Location = new System.Drawing.Point(4, 22);
            this.WallsTab.Name = "WallsTab";
            this.WallsTab.Padding = new System.Windows.Forms.Padding(3);
            this.WallsTab.Size = new System.Drawing.Size(294, 621);
            this.WallsTab.TabIndex = 2;
            this.WallsTab.Text = "Walls";
            this.WallsTab.UseVisualStyleBackColor = true;
            // 
            // WallDataBox
            // 
            this.WallDataBox.Controls.Add(this.WallYScaleBox);
            this.WallDataBox.Controls.Add(this.label5);
            this.WallDataBox.Controls.Add(this.label6);
            this.WallDataBox.Controls.Add(this.WallXScaleBox);
            this.WallDataBox.Controls.Add(this.label7);
            this.WallDataBox.Controls.Add(this.WallXLocBox);
            this.WallDataBox.Controls.Add(this.label8);
            this.WallDataBox.Controls.Add(this.WallYLocBox);
            this.WallDataBox.Location = new System.Drawing.Point(6, 451);
            this.WallDataBox.Name = "WallDataBox";
            this.WallDataBox.Size = new System.Drawing.Size(282, 135);
            this.WallDataBox.TabIndex = 4;
            this.WallDataBox.TabStop = false;
            this.WallDataBox.Text = "Wall Data";
            // 
            // WallYScaleBox
            // 
            this.WallYScaleBox.Location = new System.Drawing.Point(67, 102);
            this.WallYScaleBox.Name = "WallYScaleBox";
            this.WallYScaleBox.Size = new System.Drawing.Size(209, 20);
            this.WallYScaleBox.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Y Scale";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "X location";
            // 
            // WallXScaleBox
            // 
            this.WallXScaleBox.Location = new System.Drawing.Point(67, 76);
            this.WallXScaleBox.Name = "WallXScaleBox";
            this.WallXScaleBox.Size = new System.Drawing.Size(209, 20);
            this.WallXScaleBox.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "X Scale";
            // 
            // WallXLocBox
            // 
            this.WallXLocBox.Location = new System.Drawing.Point(67, 24);
            this.WallXLocBox.Name = "WallXLocBox";
            this.WallXLocBox.Size = new System.Drawing.Size(209, 20);
            this.WallXLocBox.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Y location";
            // 
            // WallYLocBox
            // 
            this.WallYLocBox.Location = new System.Drawing.Point(67, 50);
            this.WallYLocBox.Name = "WallYLocBox";
            this.WallYLocBox.Size = new System.Drawing.Size(209, 20);
            this.WallYLocBox.TabIndex = 4;
            // 
            // WallRemoveButton
            // 
            this.WallRemoveButton.Location = new System.Drawing.Point(213, 592);
            this.WallRemoveButton.Name = "WallRemoveButton";
            this.WallRemoveButton.Size = new System.Drawing.Size(75, 23);
            this.WallRemoveButton.TabIndex = 3;
            this.WallRemoveButton.Text = "Remove";
            this.WallRemoveButton.UseVisualStyleBackColor = true;
            // 
            // WallAddButton
            // 
            this.WallAddButton.Location = new System.Drawing.Point(6, 592);
            this.WallAddButton.Name = "WallAddButton";
            this.WallAddButton.Size = new System.Drawing.Size(75, 23);
            this.WallAddButton.TabIndex = 2;
            this.WallAddButton.Text = "Add";
            this.WallAddButton.UseVisualStyleBackColor = true;
            // 
            // WallListBox
            // 
            this.WallListBox.FormattingEnabled = true;
            this.WallListBox.Location = new System.Drawing.Point(6, 6);
            this.WallListBox.Name = "WallListBox";
            this.WallListBox.Size = new System.Drawing.Size(282, 439);
            this.WallListBox.TabIndex = 1;
            // 
            // EnemyTab
            // 
            this.EnemyTab.Controls.Add(this.EnemyRemoveButton);
            this.EnemyTab.Controls.Add(this.groupBox1);
            this.EnemyTab.Controls.Add(this.EnemyAddButton);
            this.EnemyTab.Controls.Add(this.EnemyListBox);
            this.EnemyTab.Location = new System.Drawing.Point(4, 22);
            this.EnemyTab.Name = "EnemyTab";
            this.EnemyTab.Padding = new System.Windows.Forms.Padding(3);
            this.EnemyTab.Size = new System.Drawing.Size(294, 621);
            this.EnemyTab.TabIndex = 3;
            this.EnemyTab.Text = "Enemies";
            this.EnemyTab.UseVisualStyleBackColor = true;
            this.EnemyTab.Click += new System.EventHandler(this.EnemyTab_Click);
            // 
            // EnemyRemoveButton
            // 
            this.EnemyRemoveButton.Location = new System.Drawing.Point(213, 592);
            this.EnemyRemoveButton.Name = "EnemyRemoveButton";
            this.EnemyRemoveButton.Size = new System.Drawing.Size(75, 23);
            this.EnemyRemoveButton.TabIndex = 7;
            this.EnemyRemoveButton.Text = "Remove";
            this.EnemyRemoveButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.EnemyTypeComboBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.EDirBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.EXLocbox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.EYLocBox);
            this.groupBox1.Location = new System.Drawing.Point(6, 451);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 135);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Enemy Data";
            this.groupBox1.Enter += new System.EventHandler(this.GroupBox1_Enter);
            // 
            // EnemyTypeComboBox
            // 
            this.EnemyTypeComboBox.FormattingEnabled = true;
            this.EnemyTypeComboBox.Location = new System.Drawing.Point(67, 23);
            this.EnemyTypeComboBox.Name = "EnemyTypeComboBox";
            this.EnemyTypeComboBox.Size = new System.Drawing.Size(209, 21);
            this.EnemyTypeComboBox.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Type";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "X location";
            this.label1.Click += new System.EventHandler(this.Label1_Click);
            // 
            // EDirBox
            // 
            this.EDirBox.Location = new System.Drawing.Point(67, 102);
            this.EDirBox.Name = "EDirBox";
            this.EDirBox.Size = new System.Drawing.Size(209, 20);
            this.EDirBox.TabIndex = 5;
            this.EDirBox.TextChanged += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Direction";
            this.label2.Click += new System.EventHandler(this.Label2_Click);
            // 
            // EXLocbox
            // 
            this.EXLocbox.Location = new System.Drawing.Point(67, 50);
            this.EXLocbox.Name = "EXLocbox";
            this.EXLocbox.Size = new System.Drawing.Size(209, 20);
            this.EXLocbox.TabIndex = 3;
            this.EXLocbox.TextChanged += new System.EventHandler(this.TextBox2_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Y location";
            this.label3.Click += new System.EventHandler(this.Label3_Click);
            // 
            // EYLocBox
            // 
            this.EYLocBox.Location = new System.Drawing.Point(67, 76);
            this.EYLocBox.Name = "EYLocBox";
            this.EYLocBox.Size = new System.Drawing.Size(209, 20);
            this.EYLocBox.TabIndex = 4;
            this.EYLocBox.TextChanged += new System.EventHandler(this.TextBox3_TextChanged);
            // 
            // EnemyAddButton
            // 
            this.EnemyAddButton.Location = new System.Drawing.Point(6, 592);
            this.EnemyAddButton.Name = "EnemyAddButton";
            this.EnemyAddButton.Size = new System.Drawing.Size(75, 23);
            this.EnemyAddButton.TabIndex = 6;
            this.EnemyAddButton.Text = "Add";
            this.EnemyAddButton.UseVisualStyleBackColor = true;
            // 
            // EnemyListBox
            // 
            this.EnemyListBox.FormattingEnabled = true;
            this.EnemyListBox.Location = new System.Drawing.Point(6, 6);
            this.EnemyListBox.Name = "EnemyListBox";
            this.EnemyListBox.Size = new System.Drawing.Size(282, 439);
            this.EnemyListBox.TabIndex = 2;
            // 
            // ItemsTab
            // 
            this.ItemsTab.Controls.Add(this.ItemDataBox);
            this.ItemsTab.Controls.Add(this.ItemRemoveButton);
            this.ItemsTab.Controls.Add(this.ItemListBox);
            this.ItemsTab.Controls.Add(this.ItemAddButton);
            this.ItemsTab.Location = new System.Drawing.Point(4, 22);
            this.ItemsTab.Name = "ItemsTab";
            this.ItemsTab.Padding = new System.Windows.Forms.Padding(3);
            this.ItemsTab.Size = new System.Drawing.Size(294, 621);
            this.ItemsTab.TabIndex = 4;
            this.ItemsTab.Text = "Items";
            this.ItemsTab.UseVisualStyleBackColor = true;
            // 
            // ItemDataBox
            // 
            this.ItemDataBox.Controls.Add(this.ItemTypeComboBox);
            this.ItemDataBox.Controls.Add(this.ItemTypeLabel);
            this.ItemDataBox.Controls.Add(this.ItemXLocLabel);
            this.ItemDataBox.Controls.Add(this.ItemXLocBox);
            this.ItemDataBox.Controls.Add(this.ItemYLocLabel);
            this.ItemDataBox.Controls.Add(this.ItemYLocBox);
            this.ItemDataBox.Location = new System.Drawing.Point(6, 483);
            this.ItemDataBox.Name = "ItemDataBox";
            this.ItemDataBox.Size = new System.Drawing.Size(279, 103);
            this.ItemDataBox.TabIndex = 9;
            this.ItemDataBox.TabStop = false;
            this.ItemDataBox.Text = "Item Data";
            // 
            // ItemTypeComboBox
            // 
            this.ItemTypeComboBox.FormattingEnabled = true;
            this.ItemTypeComboBox.Location = new System.Drawing.Point(67, 18);
            this.ItemTypeComboBox.Name = "ItemTypeComboBox";
            this.ItemTypeComboBox.Size = new System.Drawing.Size(206, 21);
            this.ItemTypeComboBox.TabIndex = 8;
            // 
            // ItemTypeLabel
            // 
            this.ItemTypeLabel.AutoSize = true;
            this.ItemTypeLabel.Location = new System.Drawing.Point(7, 21);
            this.ItemTypeLabel.Name = "ItemTypeLabel";
            this.ItemTypeLabel.Size = new System.Drawing.Size(31, 13);
            this.ItemTypeLabel.TabIndex = 6;
            this.ItemTypeLabel.Text = "Type";
            // 
            // ItemXLocLabel
            // 
            this.ItemXLocLabel.AutoSize = true;
            this.ItemXLocLabel.Location = new System.Drawing.Point(7, 48);
            this.ItemXLocLabel.Name = "ItemXLocLabel";
            this.ItemXLocLabel.Size = new System.Drawing.Size(54, 13);
            this.ItemXLocLabel.TabIndex = 0;
            this.ItemXLocLabel.Text = "X location";
            // 
            // ItemXLocBox
            // 
            this.ItemXLocBox.Location = new System.Drawing.Point(67, 45);
            this.ItemXLocBox.Name = "ItemXLocBox";
            this.ItemXLocBox.Size = new System.Drawing.Size(206, 20);
            this.ItemXLocBox.TabIndex = 3;
            // 
            // ItemYLocLabel
            // 
            this.ItemYLocLabel.AutoSize = true;
            this.ItemYLocLabel.Location = new System.Drawing.Point(7, 74);
            this.ItemYLocLabel.Name = "ItemYLocLabel";
            this.ItemYLocLabel.Size = new System.Drawing.Size(54, 13);
            this.ItemYLocLabel.TabIndex = 1;
            this.ItemYLocLabel.Text = "Y location";
            // 
            // ItemYLocBox
            // 
            this.ItemYLocBox.Location = new System.Drawing.Point(67, 71);
            this.ItemYLocBox.Name = "ItemYLocBox";
            this.ItemYLocBox.Size = new System.Drawing.Size(206, 20);
            this.ItemYLocBox.TabIndex = 4;
            // 
            // ItemRemoveButton
            // 
            this.ItemRemoveButton.Location = new System.Drawing.Point(213, 592);
            this.ItemRemoveButton.Name = "ItemRemoveButton";
            this.ItemRemoveButton.Size = new System.Drawing.Size(75, 23);
            this.ItemRemoveButton.TabIndex = 5;
            this.ItemRemoveButton.Text = "Remove";
            this.ItemRemoveButton.UseVisualStyleBackColor = true;
            // 
            // ItemListBox
            // 
            this.ItemListBox.FormattingEnabled = true;
            this.ItemListBox.Location = new System.Drawing.Point(6, 6);
            this.ItemListBox.Name = "ItemListBox";
            this.ItemListBox.Size = new System.Drawing.Size(282, 469);
            this.ItemListBox.TabIndex = 2;
            // 
            // ItemAddButton
            // 
            this.ItemAddButton.Location = new System.Drawing.Point(6, 592);
            this.ItemAddButton.Name = "ItemAddButton";
            this.ItemAddButton.Size = new System.Drawing.Size(75, 23);
            this.ItemAddButton.TabIndex = 4;
            this.ItemAddButton.Text = "Add";
            this.ItemAddButton.UseVisualStyleBackColor = true;
            // 
            // PropsTab
            // 
            this.PropsTab.Controls.Add(this.PropDataBox);
            this.PropsTab.Controls.Add(this.PropListBox);
            this.PropsTab.Controls.Add(this.PropRemoveButton);
            this.PropsTab.Controls.Add(this.PropAddButton);
            this.PropsTab.Location = new System.Drawing.Point(4, 22);
            this.PropsTab.Name = "PropsTab";
            this.PropsTab.Padding = new System.Windows.Forms.Padding(3);
            this.PropsTab.Size = new System.Drawing.Size(294, 621);
            this.PropsTab.TabIndex = 5;
            this.PropsTab.Text = "Props";
            this.PropsTab.UseVisualStyleBackColor = true;
            // 
            // PropDataBox
            // 
            this.PropDataBox.Controls.Add(this.PropTypeComboBox);
            this.PropDataBox.Controls.Add(this.PropTypeLabel);
            this.PropDataBox.Controls.Add(this.PropXLocLabel);
            this.PropDataBox.Controls.Add(this.PropDirBox);
            this.PropDataBox.Controls.Add(this.PropDirLabel);
            this.PropDataBox.Controls.Add(this.PropXLocBox);
            this.PropDataBox.Controls.Add(this.PropYLocLabel);
            this.PropDataBox.Controls.Add(this.PropYLocBox);
            this.PropDataBox.Location = new System.Drawing.Point(6, 455);
            this.PropDataBox.Name = "PropDataBox";
            this.PropDataBox.Size = new System.Drawing.Size(282, 131);
            this.PropDataBox.TabIndex = 3;
            this.PropDataBox.TabStop = false;
            this.PropDataBox.Text = "Prop Data";
            // 
            // PropTypeComboBox
            // 
            this.PropTypeComboBox.FormattingEnabled = true;
            this.PropTypeComboBox.Location = new System.Drawing.Point(67, 18);
            this.PropTypeComboBox.Name = "PropTypeComboBox";
            this.PropTypeComboBox.Size = new System.Drawing.Size(209, 21);
            this.PropTypeComboBox.TabIndex = 8;
            // 
            // PropTypeLabel
            // 
            this.PropTypeLabel.AutoSize = true;
            this.PropTypeLabel.Location = new System.Drawing.Point(7, 21);
            this.PropTypeLabel.Name = "PropTypeLabel";
            this.PropTypeLabel.Size = new System.Drawing.Size(31, 13);
            this.PropTypeLabel.TabIndex = 6;
            this.PropTypeLabel.Text = "Type";
            // 
            // PropXLocLabel
            // 
            this.PropXLocLabel.AutoSize = true;
            this.PropXLocLabel.Location = new System.Drawing.Point(7, 48);
            this.PropXLocLabel.Name = "PropXLocLabel";
            this.PropXLocLabel.Size = new System.Drawing.Size(54, 13);
            this.PropXLocLabel.TabIndex = 0;
            this.PropXLocLabel.Text = "X location";
            // 
            // PropDirBox
            // 
            this.PropDirBox.Location = new System.Drawing.Point(67, 97);
            this.PropDirBox.Name = "PropDirBox";
            this.PropDirBox.Size = new System.Drawing.Size(209, 20);
            this.PropDirBox.TabIndex = 5;
            // 
            // PropDirLabel
            // 
            this.PropDirLabel.AutoSize = true;
            this.PropDirLabel.Location = new System.Drawing.Point(7, 100);
            this.PropDirLabel.Name = "PropDirLabel";
            this.PropDirLabel.Size = new System.Drawing.Size(49, 13);
            this.PropDirLabel.TabIndex = 2;
            this.PropDirLabel.Text = "Direction";
            // 
            // PropXLocBox
            // 
            this.PropXLocBox.Location = new System.Drawing.Point(67, 45);
            this.PropXLocBox.Name = "PropXLocBox";
            this.PropXLocBox.Size = new System.Drawing.Size(209, 20);
            this.PropXLocBox.TabIndex = 3;
            // 
            // PropYLocLabel
            // 
            this.PropYLocLabel.AutoSize = true;
            this.PropYLocLabel.Location = new System.Drawing.Point(7, 74);
            this.PropYLocLabel.Name = "PropYLocLabel";
            this.PropYLocLabel.Size = new System.Drawing.Size(54, 13);
            this.PropYLocLabel.TabIndex = 1;
            this.PropYLocLabel.Text = "Y location";
            // 
            // PropYLocBox
            // 
            this.PropYLocBox.Location = new System.Drawing.Point(67, 71);
            this.PropYLocBox.Name = "PropYLocBox";
            this.PropYLocBox.Size = new System.Drawing.Size(209, 20);
            this.PropYLocBox.TabIndex = 4;
            // 
            // PropListBox
            // 
            this.PropListBox.FormattingEnabled = true;
            this.PropListBox.Location = new System.Drawing.Point(6, 6);
            this.PropListBox.Name = "PropListBox";
            this.PropListBox.Size = new System.Drawing.Size(282, 439);
            this.PropListBox.TabIndex = 10;
            // 
            // PropRemoveButton
            // 
            this.PropRemoveButton.Location = new System.Drawing.Point(213, 592);
            this.PropRemoveButton.Name = "PropRemoveButton";
            this.PropRemoveButton.Size = new System.Drawing.Size(75, 23);
            this.PropRemoveButton.TabIndex = 13;
            this.PropRemoveButton.Text = "Remove";
            this.PropRemoveButton.UseVisualStyleBackColor = true;
            // 
            // PropAddButton
            // 
            this.PropAddButton.Location = new System.Drawing.Point(6, 592);
            this.PropAddButton.Name = "PropAddButton";
            this.PropAddButton.Size = new System.Drawing.Size(75, 23);
            this.PropAddButton.TabIndex = 12;
            this.PropAddButton.Text = "Add";
            this.PropAddButton.UseVisualStyleBackColor = true;
            // 
            // CatEdMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1186, 671);
            this.Controls.Add(this.EditorTabControl);
            this.Controls.Add(this.cLevelView1);
            this.Name = "CatEdMainForm";
            this.Text = "CatEd";
            this.EditorTabControl.ResumeLayout(false);
            this.LevelTabPage.ResumeLayout(false);
            this.PlayerInfoBox.ResumeLayout(false);
            this.PlayerInfoBox.PerformLayout();
            this.WallsTab.ResumeLayout(false);
            this.WallDataBox.ResumeLayout(false);
            this.WallDataBox.PerformLayout();
            this.EnemyTab.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ItemsTab.ResumeLayout(false);
            this.ItemDataBox.ResumeLayout(false);
            this.ItemDataBox.PerformLayout();
            this.PropsTab.ResumeLayout(false);
            this.PropDataBox.ResumeLayout(false);
            this.PropDataBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CLevelView cLevelView1;
        private System.Windows.Forms.TabControl EditorTabControl;
        private System.Windows.Forms.TabPage LevelTabPage;
        private System.Windows.Forms.TabPage WallsTab;
        private System.Windows.Forms.CheckedListBox WallListBox;
        private System.Windows.Forms.TabPage EnemyTab;
        private System.Windows.Forms.TabPage ItemsTab;
        private System.Windows.Forms.TabPage PropsTab;
        private System.Windows.Forms.GroupBox PlayerInfoBox;
        private System.Windows.Forms.Label PDirLabel;
        private System.Windows.Forms.Label PYLocLabel;
        private System.Windows.Forms.Label PXLocLabel;
        private System.Windows.Forms.GroupBox LevelInfoBox;
        private System.Windows.Forms.TextBox PDirBox;
        private System.Windows.Forms.TextBox PXLocBox;
        private System.Windows.Forms.TextBox PYLocBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox EDirBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox EXLocbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox EYLocBox;
        private System.Windows.Forms.CheckedListBox EnemyListBox;
        private System.Windows.Forms.ComboBox EnemyTypeComboBox;
        private System.Windows.Forms.GroupBox WallDataBox;
        private System.Windows.Forms.TextBox WallYScaleBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox WallXScaleBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox WallXLocBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox WallYLocBox;
        private System.Windows.Forms.Button WallRemoveButton;
        private System.Windows.Forms.Button WallAddButton;
        private System.Windows.Forms.Button EnemyRemoveButton;
        private System.Windows.Forms.Button EnemyAddButton;
        private System.Windows.Forms.GroupBox ItemDataBox;
        private System.Windows.Forms.ComboBox ItemTypeComboBox;
        private System.Windows.Forms.Label ItemTypeLabel;
        private System.Windows.Forms.Label ItemXLocLabel;
        private System.Windows.Forms.TextBox ItemXLocBox;
        private System.Windows.Forms.Label ItemYLocLabel;
        private System.Windows.Forms.TextBox ItemYLocBox;
        private System.Windows.Forms.Button ItemRemoveButton;
        private System.Windows.Forms.CheckedListBox ItemListBox;
        private System.Windows.Forms.Button ItemAddButton;
        private System.Windows.Forms.GroupBox PropDataBox;
        private System.Windows.Forms.ComboBox PropTypeComboBox;
        private System.Windows.Forms.Label PropTypeLabel;
        private System.Windows.Forms.Label PropXLocLabel;
        private System.Windows.Forms.TextBox PropDirBox;
        private System.Windows.Forms.Label PropDirLabel;
        private System.Windows.Forms.TextBox PropXLocBox;
        private System.Windows.Forms.Label PropYLocLabel;
        private System.Windows.Forms.TextBox PropYLocBox;
        private System.Windows.Forms.CheckedListBox PropListBox;
        private System.Windows.Forms.Button PropRemoveButton;
        private System.Windows.Forms.Button PropAddButton;
    }
}

