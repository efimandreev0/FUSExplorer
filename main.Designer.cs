namespace LABO
{
    public partial class main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(main));
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            findToolStripMenuItem = new ToolStripMenuItem();
            treeView1 = new TreeView();
            tabPage2 = new TabPage();
            tableLayoutPanel2 = new TableLayoutPanel();
            menuStrip2 = new MenuStrip();
            fileToolStripMenuItem1 = new ToolStripMenuItem();
            openToolStripMenuItem1 = new ToolStripMenuItem();
            saveToolStripMenuItem1 = new ToolStripMenuItem();
            saveAsToolStripMenuItem1 = new ToolStripMenuItem();
            saveInsertToolStripMenuItem = new ToolStripMenuItem();
            findToolStripMenuItem1 = new ToolStripMenuItem();
            tableLayoutPanel3 = new TableLayoutPanel();
            tableLayoutPanel4 = new TableLayoutPanel();
            richTextBox1 = new RichTextBox();
            richTextBox2 = new RichTextBox();
            label1 = new Label();
            label2 = new Label();
            listBox1 = new ListBox();
            contextMenuStrip1 = new ContextMenuStrip(components);
            openAsTextToolStripMenuItem = new ToolStripMenuItem();
            textToolStripMenuItem = new ToolStripMenuItem();
            imageToolStripMenuItem = new ToolStripMenuItem();
            extractToolStripMenuItem = new ToolStripMenuItem();
            extractAsToolStripMenuItem = new ToolStripMenuItem();
            replaceToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripMenuItem();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            menuStrip1.SuspendLayout();
            tabPage2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            menuStrip2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 450);
            tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = SystemColors.ActiveCaption;
            tabPage1.Controls.Add(tableLayoutPanel1);
            tabPage1.Location = new Point(8, 46);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(784, 396);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Archive";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Controls.Add(menuStrip1, 0, 0);
            tableLayoutPanel1.Controls.Add(treeView1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 90F));
            tableLayoutPanel1.Size = new Size(778, 390);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.Dock = DockStyle.Fill;
            menuStrip1.ImageScalingSize = new Size(32, 32);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, findToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(778, 39);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.ItemClicked += menuStrip1_ItemClicked;
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(71, 35);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(244, 44);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(244, 44);
            saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(244, 44);
            saveAsToolStripMenuItem.Text = "Save As...";
            // 
            // findToolStripMenuItem
            // 
            findToolStripMenuItem.Name = "findToolStripMenuItem";
            findToolStripMenuItem.Size = new Size(80, 35);
            findToolStripMenuItem.Text = "Find";
            // 
            // treeView1
            // 
            treeView1.BackColor = SystemColors.InactiveCaption;
            treeView1.Dock = DockStyle.Fill;
            treeView1.ForeColor = Color.DimGray;
            treeView1.Location = new Point(3, 42);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(772, 345);
            treeView1.TabIndex = 1;
            treeView1.AfterSelect += treeView1_AfterSelect;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(tableLayoutPanel2);
            tabPage2.Location = new Point(8, 46);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(784, 396);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Text";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(menuStrip2, 0, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 41F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(778, 390);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // menuStrip2
            // 
            menuStrip2.Dock = DockStyle.Fill;
            menuStrip2.ImageScalingSize = new Size(32, 32);
            menuStrip2.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem1, findToolStripMenuItem1 });
            menuStrip2.Location = new Point(0, 0);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Size = new Size(778, 41);
            menuStrip2.TabIndex = 0;
            menuStrip2.Text = "menuStrip2";
            // 
            // fileToolStripMenuItem1
            // 
            fileToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem1, saveToolStripMenuItem1, saveAsToolStripMenuItem1, saveInsertToolStripMenuItem });
            fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            fileToolStripMenuItem1.Size = new Size(71, 37);
            fileToolStripMenuItem1.Text = "File";
            // 
            // openToolStripMenuItem1
            // 
            openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            openToolStripMenuItem1.Size = new Size(310, 44);
            openToolStripMenuItem1.Text = "Open";
            openToolStripMenuItem1.Click += openToolStripMenuItem1_Click;
            // 
            // saveToolStripMenuItem1
            // 
            saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
            saveToolStripMenuItem1.Size = new Size(310, 44);
            saveToolStripMenuItem1.Text = "Save";
            saveToolStripMenuItem1.Click += saveToolStripMenuItem1_Click;
            // 
            // saveAsToolStripMenuItem1
            // 
            saveAsToolStripMenuItem1.Name = "saveAsToolStripMenuItem1";
            saveAsToolStripMenuItem1.Size = new Size(310, 44);
            saveAsToolStripMenuItem1.Text = "Save As...";
            saveAsToolStripMenuItem1.Click += saveAsToolStripMenuItem1_Click;
            // 
            // saveInsertToolStripMenuItem
            // 
            saveInsertToolStripMenuItem.Name = "saveInsertToolStripMenuItem";
            saveInsertToolStripMenuItem.Size = new Size(310, 44);
            saveInsertToolStripMenuItem.Text = "Save and Insert";
            saveInsertToolStripMenuItem.Click += saveInsertToolStripMenuItem_Click;
            // 
            // findToolStripMenuItem1
            // 
            findToolStripMenuItem1.Name = "findToolStripMenuItem1";
            findToolStripMenuItem1.Size = new Size(80, 37);
            findToolStripMenuItem1.Text = "Find";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17.616581F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 82.38342F));
            tableLayoutPanel3.Controls.Add(tableLayoutPanel4, 1, 0);
            tableLayoutPanel3.Controls.Add(listBox1, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 44);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(772, 343);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(richTextBox1, 0, 0);
            tableLayoutPanel4.Controls.Add(richTextBox2, 1, 0);
            tableLayoutPanel4.Controls.Add(label1, 0, 1);
            tableLayoutPanel4.Controls.Add(label2, 1, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(139, 3);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 297F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Size = new Size(630, 337);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // richTextBox1
            // 
            richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.Location = new Point(3, 3);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(309, 291);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            richTextBox1.TextChanged += richTextBox1_TextChanged;
            // 
            // richTextBox2
            // 
            richTextBox2.Dock = DockStyle.Fill;
            richTextBox2.Location = new Point(318, 3);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.ReadOnly = true;
            richTextBox2.Size = new Size(309, 291);
            richTextBox2.TabIndex = 1;
            richTextBox2.Text = "";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 297);
            label1.Name = "label1";
            label1.Size = new Size(0, 32);
            label1.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(318, 297);
            label2.Name = "label2";
            label2.Size = new Size(309, 40);
            label2.TabIndex = 3;
            label2.Text = "Original";
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(3, 3);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(130, 337);
            listBox1.TabIndex = 1;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(32, 32);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { openAsTextToolStripMenuItem, extractToolStripMenuItem, extractAsToolStripMenuItem, replaceToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.RenderMode = ToolStripRenderMode.System;
            contextMenuStrip1.Size = new Size(206, 156);
            // 
            // openAsTextToolStripMenuItem
            // 
            openAsTextToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { textToolStripMenuItem, imageToolStripMenuItem });
            openAsTextToolStripMenuItem.Name = "openAsTextToolStripMenuItem";
            openAsTextToolStripMenuItem.Size = new Size(205, 38);
            openAsTextToolStripMenuItem.Text = "Open As...";
            openAsTextToolStripMenuItem.Click += openAsTextToolStripMenuItem_Click;
            // 
            // textToolStripMenuItem
            // 
            textToolStripMenuItem.Name = "textToolStripMenuItem";
            textToolStripMenuItem.Size = new Size(213, 44);
            textToolStripMenuItem.Text = "Text";
            textToolStripMenuItem.Click += textToolStripMenuItem_Click;
            // 
            // imageToolStripMenuItem
            // 
            imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            imageToolStripMenuItem.Size = new Size(213, 44);
            imageToolStripMenuItem.Text = "Image";
            // 
            // extractToolStripMenuItem
            // 
            extractToolStripMenuItem.Name = "extractToolStripMenuItem";
            extractToolStripMenuItem.Size = new Size(205, 38);
            extractToolStripMenuItem.Text = "Extract";
            extractToolStripMenuItem.Click += extractToolStripMenuItem_Click;
            // 
            // extractAsToolStripMenuItem
            // 
            extractAsToolStripMenuItem.Name = "extractAsToolStripMenuItem";
            extractAsToolStripMenuItem.Size = new Size(205, 38);
            extractAsToolStripMenuItem.Text = "Extract As...";
            extractAsToolStripMenuItem.Click += extractAsToolStripMenuItem_Click;
            // 
            // replaceToolStripMenuItem
            // 
            replaceToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem2 });
            replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            replaceToolStripMenuItem.Size = new Size(205, 38);
            replaceToolStripMenuItem.Text = "Replace";
            replaceToolStripMenuItem.Click += replaceToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(359, 44);
            toolStripMenuItem1.Text = "File";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(359, 44);
            toolStripMenuItem2.Text = "Folder";
            toolStripMenuItem2.Click += toolStripMenuItem2_Click;
            // 
            // main
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.SeaShell;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "main";
            Text = "LABO - Tool For edit TT-games";
            Load += main_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            menuStrip2.ResumeLayout(false);
            menuStrip2.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPage1;
        private TableLayoutPanel tableLayoutPanel1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem findToolStripMenuItem;
        private TabPage tabPage2;
        private TreeView treeView1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem openAsTextToolStripMenuItem;
        private ToolStripMenuItem extractToolStripMenuItem;
        private ToolStripMenuItem extractAsToolStripMenuItem;
        private ToolStripMenuItem replaceToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel2;
        private MenuStrip menuStrip2;
        private ToolStripMenuItem fileToolStripMenuItem1;
        private ToolStripMenuItem openToolStripMenuItem1;
        private ToolStripMenuItem saveToolStripMenuItem1;
        private ToolStripMenuItem findToolStripMenuItem1;
        private TableLayoutPanel tableLayoutPanel3;
        private ToolStripMenuItem saveAsToolStripMenuItem1;
        private ToolStripMenuItem saveInsertToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel4;
        private RichTextBox richTextBox1;
        private RichTextBox richTextBox2;
        private Label label1;
        private Label label2;
        private ListBox listBox1;
        private ToolStripMenuItem textToolStripMenuItem;
        private ToolStripMenuItem imageToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
    }
}
