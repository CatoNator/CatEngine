namespace Boner
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
            this.BoneTreeView = new System.Windows.Forms.TreeView();
            this.AddChildButton = new System.Windows.Forms.Button();
            this.RemoveBoneButton = new System.Windows.Forms.Button();
            this.BonePropBox = new System.Windows.Forms.GroupBox();
            this.BonePosZBox = new System.Windows.Forms.NumericUpDown();
            this.BonePosYBox = new System.Windows.Forms.NumericUpDown();
            this.BonePosXBox = new System.Windows.Forms.NumericUpDown();
            this.BoneOrigXBox = new System.Windows.Forms.NumericUpDown();
            this.BoneOrigYBox = new System.Windows.Forms.NumericUpDown();
            this.BoneSizeXBox = new System.Windows.Forms.NumericUpDown();
            this.BoneSizeYBox = new System.Windows.Forms.NumericUpDown();
            this.BoneTextureLabel = new System.Windows.Forms.Label();
            this.BoneTexBox = new System.Windows.Forms.TextBox();
            this.BonePosLabel = new System.Windows.Forms.Label();
            this.BoneOriginLabel = new System.Windows.Forms.Label();
            this.BoneNameBox = new System.Windows.Forms.TextBox();
            this.BoneSizeLabel = new System.Windows.Forms.Label();
            this.BoneNameLabel = new System.Windows.Forms.Label();
            this.BoneAnimBox = new System.Windows.Forms.GroupBox();
            this.AnimSpeedLabel = new System.Windows.Forms.Label();
            this.AnimSpeedBox = new System.Windows.Forms.NumericUpDown();
            this.ImageIndBox = new System.Windows.Forms.NumericUpDown();
            this.RotationBox = new System.Windows.Forms.NumericUpDown();
            this.CurrentFrameLabel = new System.Windows.Forms.Label();
            this.MaxFramesLabel = new System.Windows.Forms.Label();
            this.MaxFramesBox = new System.Windows.Forms.NumericUpDown();
            this.BoneAnimImgLabel = new System.Windows.Forms.Label();
            this.BoneAnimRotLabel = new System.Windows.Forms.Label();
            this.CurrentFrameSelect = new System.Windows.Forms.NumericUpDown();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadSkeletonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSkeletonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAnimationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FileMenuStrip = new System.Windows.Forms.MenuStrip();
            this.cAnimationPreview1 = new Boner.CAnimationPreview();
            this.LoadSkeletonDialog = new System.Windows.Forms.OpenFileDialog();
            this.LoadAnimationDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveSkeletonDialog = new System.Windows.Forms.SaveFileDialog();
            this.SaveAnimationDialog = new System.Windows.Forms.SaveFileDialog();
            this.BonePropBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BonePosZBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BonePosYBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BonePosXBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoneOrigXBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoneOrigYBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoneSizeXBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoneSizeYBox)).BeginInit();
            this.BoneAnimBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AnimSpeedBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageIndBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxFramesBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentFrameSelect)).BeginInit();
            this.FileMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // BoneTreeView
            // 
            this.BoneTreeView.Location = new System.Drawing.Point(12, 27);
            this.BoneTreeView.Name = "BoneTreeView";
            this.BoneTreeView.Size = new System.Drawing.Size(179, 396);
            this.BoneTreeView.TabIndex = 0;
            this.BoneTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.BoneTreeView_AfterSelect);
            // 
            // AddChildButton
            // 
            this.AddChildButton.Location = new System.Drawing.Point(12, 430);
            this.AddChildButton.Name = "AddChildButton";
            this.AddChildButton.Size = new System.Drawing.Size(87, 23);
            this.AddChildButton.TabIndex = 1;
            this.AddChildButton.Text = "Add Child";
            this.AddChildButton.UseVisualStyleBackColor = true;
            this.AddChildButton.Click += new System.EventHandler(this.AddChildButton_Click);
            // 
            // RemoveBoneButton
            // 
            this.RemoveBoneButton.Location = new System.Drawing.Point(105, 430);
            this.RemoveBoneButton.Name = "RemoveBoneButton";
            this.RemoveBoneButton.Size = new System.Drawing.Size(86, 23);
            this.RemoveBoneButton.TabIndex = 2;
            this.RemoveBoneButton.Text = "Remove Bone";
            this.RemoveBoneButton.UseVisualStyleBackColor = true;
            this.RemoveBoneButton.Click += new System.EventHandler(this.RemoveBoneButton_Click);
            // 
            // BonePropBox
            // 
            this.BonePropBox.Controls.Add(this.BonePosZBox);
            this.BonePropBox.Controls.Add(this.BonePosYBox);
            this.BonePropBox.Controls.Add(this.BonePosXBox);
            this.BonePropBox.Controls.Add(this.BoneOrigXBox);
            this.BonePropBox.Controls.Add(this.BoneOrigYBox);
            this.BonePropBox.Controls.Add(this.BoneSizeXBox);
            this.BonePropBox.Controls.Add(this.BoneSizeYBox);
            this.BonePropBox.Controls.Add(this.BoneTextureLabel);
            this.BonePropBox.Controls.Add(this.BoneTexBox);
            this.BonePropBox.Controls.Add(this.BonePosLabel);
            this.BonePropBox.Controls.Add(this.BoneOriginLabel);
            this.BonePropBox.Controls.Add(this.BoneNameBox);
            this.BonePropBox.Controls.Add(this.BoneSizeLabel);
            this.BonePropBox.Controls.Add(this.BoneNameLabel);
            this.BonePropBox.Location = new System.Drawing.Point(197, 299);
            this.BonePropBox.Name = "BonePropBox";
            this.BonePropBox.Size = new System.Drawing.Size(292, 154);
            this.BonePropBox.TabIndex = 3;
            this.BonePropBox.TabStop = false;
            this.BonePropBox.Text = "Bone Properties";
            // 
            // BonePosZBox
            // 
            this.BonePosZBox.DecimalPlaces = 3;
            this.BonePosZBox.Location = new System.Drawing.Point(180, 98);
            this.BonePosZBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.BonePosZBox.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.BonePosZBox.Name = "BonePosZBox";
            this.BonePosZBox.Size = new System.Drawing.Size(47, 20);
            this.BonePosZBox.TabIndex = 24;
            this.BonePosZBox.ValueChanged += new System.EventHandler(this.BonePosZBox_ValueChanged);
            // 
            // BonePosYBox
            // 
            this.BonePosYBox.DecimalPlaces = 3;
            this.BonePosYBox.Location = new System.Drawing.Point(127, 98);
            this.BonePosYBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.BonePosYBox.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.BonePosYBox.Name = "BonePosYBox";
            this.BonePosYBox.Size = new System.Drawing.Size(47, 20);
            this.BonePosYBox.TabIndex = 23;
            this.BonePosYBox.ValueChanged += new System.EventHandler(this.BonePosYBox_ValueChanged);
            // 
            // BonePosXBox
            // 
            this.BonePosXBox.DecimalPlaces = 3;
            this.BonePosXBox.Location = new System.Drawing.Point(74, 98);
            this.BonePosXBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.BonePosXBox.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.BonePosXBox.Name = "BonePosXBox";
            this.BonePosXBox.Size = new System.Drawing.Size(47, 20);
            this.BonePosXBox.TabIndex = 22;
            this.BonePosXBox.ValueChanged += new System.EventHandler(this.BonePosXBox_ValueChanged);
            // 
            // BoneOrigXBox
            // 
            this.BoneOrigXBox.Location = new System.Drawing.Point(74, 72);
            this.BoneOrigXBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.BoneOrigXBox.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.BoneOrigXBox.Name = "BoneOrigXBox";
            this.BoneOrigXBox.Size = new System.Drawing.Size(47, 20);
            this.BoneOrigXBox.TabIndex = 21;
            this.BoneOrigXBox.ValueChanged += new System.EventHandler(this.BoneOrigXBox_ValueChanged);
            // 
            // BoneOrigYBox
            // 
            this.BoneOrigYBox.Location = new System.Drawing.Point(127, 72);
            this.BoneOrigYBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.BoneOrigYBox.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.BoneOrigYBox.Name = "BoneOrigYBox";
            this.BoneOrigYBox.Size = new System.Drawing.Size(47, 20);
            this.BoneOrigYBox.TabIndex = 20;
            this.BoneOrigYBox.ValueChanged += new System.EventHandler(this.BoneOrigYBox_ValueChanged);
            // 
            // BoneSizeXBox
            // 
            this.BoneSizeXBox.Location = new System.Drawing.Point(75, 46);
            this.BoneSizeXBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.BoneSizeXBox.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.BoneSizeXBox.Name = "BoneSizeXBox";
            this.BoneSizeXBox.Size = new System.Drawing.Size(47, 20);
            this.BoneSizeXBox.TabIndex = 19;
            this.BoneSizeXBox.ValueChanged += new System.EventHandler(this.BoneSizeXBox_ValueChanged);
            // 
            // BoneSizeYBox
            // 
            this.BoneSizeYBox.Location = new System.Drawing.Point(128, 46);
            this.BoneSizeYBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.BoneSizeYBox.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.BoneSizeYBox.Name = "BoneSizeYBox";
            this.BoneSizeYBox.Size = new System.Drawing.Size(47, 20);
            this.BoneSizeYBox.TabIndex = 18;
            this.BoneSizeYBox.ValueChanged += new System.EventHandler(this.BoneSizeYBox_ValueChanged);
            // 
            // BoneTextureLabel
            // 
            this.BoneTextureLabel.AutoSize = true;
            this.BoneTextureLabel.Location = new System.Drawing.Point(6, 130);
            this.BoneTextureLabel.Name = "BoneTextureLabel";
            this.BoneTextureLabel.Size = new System.Drawing.Size(43, 13);
            this.BoneTextureLabel.TabIndex = 13;
            this.BoneTextureLabel.Text = "Texture";
            // 
            // BoneTexBox
            // 
            this.BoneTexBox.Location = new System.Drawing.Point(75, 126);
            this.BoneTexBox.Name = "BoneTexBox";
            this.BoneTexBox.Size = new System.Drawing.Size(100, 20);
            this.BoneTexBox.TabIndex = 13;
            this.BoneTexBox.TextChanged += new System.EventHandler(this.BoneTexBox_TextChanged);
            // 
            // BonePosLabel
            // 
            this.BonePosLabel.AutoSize = true;
            this.BonePosLabel.Location = new System.Drawing.Point(6, 103);
            this.BonePosLabel.Name = "BonePosLabel";
            this.BonePosLabel.Size = new System.Drawing.Size(63, 13);
            this.BonePosLabel.TabIndex = 12;
            this.BonePosLabel.Text = "Bone Offset";
            // 
            // BoneOriginLabel
            // 
            this.BoneOriginLabel.AutoSize = true;
            this.BoneOriginLabel.Location = new System.Drawing.Point(6, 77);
            this.BoneOriginLabel.Name = "BoneOriginLabel";
            this.BoneOriginLabel.Size = new System.Drawing.Size(62, 13);
            this.BoneOriginLabel.TabIndex = 7;
            this.BoneOriginLabel.Text = "Bone Origin";
            // 
            // BoneNameBox
            // 
            this.BoneNameBox.Location = new System.Drawing.Point(75, 20);
            this.BoneNameBox.Name = "BoneNameBox";
            this.BoneNameBox.Size = new System.Drawing.Size(100, 20);
            this.BoneNameBox.TabIndex = 2;
            this.BoneNameBox.TextChanged += new System.EventHandler(this.BoneNameBox_TextChanged);
            // 
            // BoneSizeLabel
            // 
            this.BoneSizeLabel.AutoSize = true;
            this.BoneSizeLabel.Location = new System.Drawing.Point(6, 51);
            this.BoneSizeLabel.Name = "BoneSizeLabel";
            this.BoneSizeLabel.Size = new System.Drawing.Size(55, 13);
            this.BoneSizeLabel.TabIndex = 1;
            this.BoneSizeLabel.Text = "Bone Size";
            // 
            // BoneNameLabel
            // 
            this.BoneNameLabel.AutoSize = true;
            this.BoneNameLabel.Location = new System.Drawing.Point(6, 23);
            this.BoneNameLabel.Name = "BoneNameLabel";
            this.BoneNameLabel.Size = new System.Drawing.Size(63, 13);
            this.BoneNameLabel.TabIndex = 0;
            this.BoneNameLabel.Text = "Bone Name";
            // 
            // BoneAnimBox
            // 
            this.BoneAnimBox.Controls.Add(this.AnimSpeedLabel);
            this.BoneAnimBox.Controls.Add(this.AnimSpeedBox);
            this.BoneAnimBox.Controls.Add(this.ImageIndBox);
            this.BoneAnimBox.Controls.Add(this.RotationBox);
            this.BoneAnimBox.Controls.Add(this.CurrentFrameLabel);
            this.BoneAnimBox.Controls.Add(this.MaxFramesLabel);
            this.BoneAnimBox.Controls.Add(this.MaxFramesBox);
            this.BoneAnimBox.Controls.Add(this.BoneAnimImgLabel);
            this.BoneAnimBox.Controls.Add(this.BoneAnimRotLabel);
            this.BoneAnimBox.Controls.Add(this.CurrentFrameSelect);
            this.BoneAnimBox.Location = new System.Drawing.Point(495, 299);
            this.BoneAnimBox.Name = "BoneAnimBox";
            this.BoneAnimBox.Size = new System.Drawing.Size(293, 154);
            this.BoneAnimBox.TabIndex = 4;
            this.BoneAnimBox.TabStop = false;
            this.BoneAnimBox.Text = "Bone Animation Properties";
            // 
            // AnimSpeedLabel
            // 
            this.AnimSpeedLabel.AutoSize = true;
            this.AnimSpeedLabel.Location = new System.Drawing.Point(154, 105);
            this.AnimSpeedLabel.Name = "AnimSpeedLabel";
            this.AnimSpeedLabel.Size = new System.Drawing.Size(87, 13);
            this.AnimSpeedLabel.TabIndex = 28;
            this.AnimSpeedLabel.Text = "Animation Speed";
            // 
            // AnimSpeedBox
            // 
            this.AnimSpeedBox.DecimalPlaces = 2;
            this.AnimSpeedBox.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.AnimSpeedBox.Location = new System.Drawing.Point(247, 102);
            this.AnimSpeedBox.Name = "AnimSpeedBox";
            this.AnimSpeedBox.Size = new System.Drawing.Size(40, 20);
            this.AnimSpeedBox.TabIndex = 27;
            this.AnimSpeedBox.ValueChanged += new System.EventHandler(this.AnimSpeedBox_ValueChanged);
            // 
            // ImageIndBox
            // 
            this.ImageIndBox.Location = new System.Drawing.Point(72, 46);
            this.ImageIndBox.Name = "ImageIndBox";
            this.ImageIndBox.Size = new System.Drawing.Size(100, 20);
            this.ImageIndBox.TabIndex = 26;
            this.ImageIndBox.ValueChanged += new System.EventHandler(this.ImageIndBox_ValueChanged);
            // 
            // RotationBox
            // 
            this.RotationBox.DecimalPlaces = 5;
            this.RotationBox.Location = new System.Drawing.Point(72, 19);
            this.RotationBox.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.RotationBox.Name = "RotationBox";
            this.RotationBox.Size = new System.Drawing.Size(100, 20);
            this.RotationBox.TabIndex = 25;
            this.RotationBox.ValueChanged += new System.EventHandler(this.RotationBox_ValueChanged);
            // 
            // CurrentFrameLabel
            // 
            this.CurrentFrameLabel.AutoSize = true;
            this.CurrentFrameLabel.Location = new System.Drawing.Point(168, 132);
            this.CurrentFrameLabel.Name = "CurrentFrameLabel";
            this.CurrentFrameLabel.Size = new System.Drawing.Size(73, 13);
            this.CurrentFrameLabel.TabIndex = 19;
            this.CurrentFrameLabel.Text = "Current Frame";
            // 
            // MaxFramesLabel
            // 
            this.MaxFramesLabel.AutoSize = true;
            this.MaxFramesLabel.Location = new System.Drawing.Point(60, 132);
            this.MaxFramesLabel.Name = "MaxFramesLabel";
            this.MaxFramesLabel.Size = new System.Drawing.Size(89, 13);
            this.MaxFramesLabel.TabIndex = 18;
            this.MaxFramesLabel.Text = "Animation Length";
            // 
            // MaxFramesBox
            // 
            this.MaxFramesBox.Location = new System.Drawing.Point(7, 128);
            this.MaxFramesBox.Name = "MaxFramesBox";
            this.MaxFramesBox.Size = new System.Drawing.Size(46, 20);
            this.MaxFramesBox.TabIndex = 17;
            this.MaxFramesBox.ValueChanged += new System.EventHandler(this.MaxFramesBox_ValueChanged);
            // 
            // BoneAnimImgLabel
            // 
            this.BoneAnimImgLabel.AutoSize = true;
            this.BoneAnimImgLabel.Location = new System.Drawing.Point(6, 51);
            this.BoneAnimImgLabel.Name = "BoneAnimImgLabel";
            this.BoneAnimImgLabel.Size = new System.Drawing.Size(64, 13);
            this.BoneAnimImgLabel.TabIndex = 16;
            this.BoneAnimImgLabel.Text = "Image index";
            // 
            // BoneAnimRotLabel
            // 
            this.BoneAnimRotLabel.AutoSize = true;
            this.BoneAnimRotLabel.Location = new System.Drawing.Point(6, 22);
            this.BoneAnimRotLabel.Name = "BoneAnimRotLabel";
            this.BoneAnimRotLabel.Size = new System.Drawing.Size(47, 13);
            this.BoneAnimRotLabel.TabIndex = 1;
            this.BoneAnimRotLabel.Text = "Rotation";
            // 
            // CurrentFrameSelect
            // 
            this.CurrentFrameSelect.DecimalPlaces = 2;
            this.CurrentFrameSelect.Location = new System.Drawing.Point(247, 128);
            this.CurrentFrameSelect.Name = "CurrentFrameSelect";
            this.CurrentFrameSelect.Size = new System.Drawing.Size(40, 20);
            this.CurrentFrameSelect.TabIndex = 0;
            this.CurrentFrameSelect.ValueChanged += new System.EventHandler(this.CurrentFrameSelect_ValueChanged);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadSkeletonToolStripMenuItem,
            this.loadAnimationToolStripMenuItem});
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.loadToolStripMenuItem.Text = "Load";
            // 
            // loadSkeletonToolStripMenuItem
            // 
            this.loadSkeletonToolStripMenuItem.Name = "loadSkeletonToolStripMenuItem";
            this.loadSkeletonToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.loadSkeletonToolStripMenuItem.Text = "Load Skeleton";
            this.loadSkeletonToolStripMenuItem.Click += new System.EventHandler(this.loadSkeletonToolStripMenuItem_Click);
            // 
            // loadAnimationToolStripMenuItem
            // 
            this.loadAnimationToolStripMenuItem.Name = "loadAnimationToolStripMenuItem";
            this.loadAnimationToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.loadAnimationToolStripMenuItem.Text = "Load Animation";
            this.loadAnimationToolStripMenuItem.Click += new System.EventHandler(this.loadAnimationToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveSkeletonToolStripMenuItem,
            this.saveAnimationToolStripMenuItem});
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // saveSkeletonToolStripMenuItem
            // 
            this.saveSkeletonToolStripMenuItem.Name = "saveSkeletonToolStripMenuItem";
            this.saveSkeletonToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.saveSkeletonToolStripMenuItem.Text = "Save Skeleton";
            this.saveSkeletonToolStripMenuItem.Click += new System.EventHandler(this.saveSkeletonToolStripMenuItem_Click);
            // 
            // saveAnimationToolStripMenuItem
            // 
            this.saveAnimationToolStripMenuItem.Name = "saveAnimationToolStripMenuItem";
            this.saveAnimationToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.saveAnimationToolStripMenuItem.Text = "Save Animation";
            this.saveAnimationToolStripMenuItem.Click += new System.EventHandler(this.saveAnimationToolStripMenuItem_Click);
            // 
            // FileMenuStrip
            // 
            this.FileMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.FileMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.FileMenuStrip.Name = "FileMenuStrip";
            this.FileMenuStrip.Size = new System.Drawing.Size(800, 24);
            this.FileMenuStrip.TabIndex = 6;
            this.FileMenuStrip.Text = "menuStrip1";
            // 
            // cAnimationPreview1
            // 
            this.cAnimationPreview1.Location = new System.Drawing.Point(206, 27);
            this.cAnimationPreview1.MouseHoverUpdatesOnly = false;
            this.cAnimationPreview1.Name = "cAnimationPreview1";
            this.cAnimationPreview1.Size = new System.Drawing.Size(582, 266);
            this.cAnimationPreview1.TabIndex = 5;
            this.cAnimationPreview1.Text = "cAnimationPreview1";
            // 
            // LoadSkeletonDialog
            // 
            this.LoadSkeletonDialog.Filter = "CatEngine Bone Structure files (*.ske)|*.ske|All files (*.*)|*.*";
            this.LoadSkeletonDialog.Title = "Load Bone Structure";
            // 
            // LoadAnimationDialog
            // 
            this.LoadAnimationDialog.Filter = "CatEngine Bone keyframe files (*.key)|*.key|All files (*.*)|*.*";
            this.LoadAnimationDialog.Title = "Load Animation";
            // 
            // SaveSkeletonDialog
            // 
            this.SaveSkeletonDialog.FileName = "new_skeleton.ske";
            this.SaveSkeletonDialog.Filter = "CatEngine Bone Structure files (*.ske)|*.ske|All files (*.*)|*.*";
            this.SaveSkeletonDialog.Title = "Save Bone Structure";
            // 
            // SaveAnimationDialog
            // 
            this.SaveAnimationDialog.FileName = "new_animation.key";
            this.SaveAnimationDialog.Filter = "CatEngine Bone animation files (*.key)|*.key|All files (*.*)|*.*";
            this.SaveAnimationDialog.Title = "Save Animation";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 466);
            this.Controls.Add(this.cAnimationPreview1);
            this.Controls.Add(this.BoneAnimBox);
            this.Controls.Add(this.BonePropBox);
            this.Controls.Add(this.RemoveBoneButton);
            this.Controls.Add(this.AddChildButton);
            this.Controls.Add(this.BoneTreeView);
            this.Controls.Add(this.FileMenuStrip);
            this.MainMenuStrip = this.FileMenuStrip;
            this.Name = "Form1";
            this.Text = "Boner (CatEngine Bone Animation Editor)";
            this.BonePropBox.ResumeLayout(false);
            this.BonePropBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BonePosZBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BonePosYBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BonePosXBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoneOrigXBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoneOrigYBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoneSizeXBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BoneSizeYBox)).EndInit();
            this.BoneAnimBox.ResumeLayout(false);
            this.BoneAnimBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AnimSpeedBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageIndBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotationBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MaxFramesBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentFrameSelect)).EndInit();
            this.FileMenuStrip.ResumeLayout(false);
            this.FileMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TreeView BoneTreeView;
        private System.Windows.Forms.Button AddChildButton;
        private System.Windows.Forms.Button RemoveBoneButton;
        private System.Windows.Forms.GroupBox BonePropBox;
        private System.Windows.Forms.Label BonePosLabel;
        private System.Windows.Forms.Label BoneOriginLabel;
        private System.Windows.Forms.TextBox BoneNameBox;
        private System.Windows.Forms.Label BoneSizeLabel;
        private System.Windows.Forms.Label BoneNameLabel;
        private System.Windows.Forms.GroupBox BoneAnimBox;
        private System.Windows.Forms.NumericUpDown CurrentFrameSelect;
        private System.Windows.Forms.Label BoneTextureLabel;
        private System.Windows.Forms.TextBox BoneTexBox;
        private System.Windows.Forms.Label CurrentFrameLabel;
        private System.Windows.Forms.Label MaxFramesLabel;
        private System.Windows.Forms.NumericUpDown MaxFramesBox;
        private System.Windows.Forms.Label BoneAnimImgLabel;
        private System.Windows.Forms.Label BoneAnimRotLabel;
        private System.Windows.Forms.NumericUpDown BonePosZBox;
        private System.Windows.Forms.NumericUpDown BonePosYBox;
        private System.Windows.Forms.NumericUpDown BonePosXBox;
        private System.Windows.Forms.NumericUpDown BoneOrigXBox;
        private System.Windows.Forms.NumericUpDown BoneOrigYBox;
        private System.Windows.Forms.NumericUpDown BoneSizeXBox;
        private System.Windows.Forms.NumericUpDown BoneSizeYBox;
        private System.Windows.Forms.NumericUpDown ImageIndBox;
        private System.Windows.Forms.NumericUpDown RotationBox;
        public CAnimationPreview cAnimationPreview1;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadSkeletonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadAnimationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSkeletonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAnimationToolStripMenuItem;
        private System.Windows.Forms.MenuStrip FileMenuStrip;
        private System.Windows.Forms.OpenFileDialog LoadSkeletonDialog;
        private System.Windows.Forms.OpenFileDialog LoadAnimationDialog;
        private System.Windows.Forms.SaveFileDialog SaveSkeletonDialog;
        private System.Windows.Forms.SaveFileDialog SaveAnimationDialog;
        private System.Windows.Forms.Label AnimSpeedLabel;
        private System.Windows.Forms.NumericUpDown AnimSpeedBox;
    }
}

