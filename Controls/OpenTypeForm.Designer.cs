﻿namespace QuickNavigatePlugin
{
    partial class OpenTypeForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.input = new System.Windows.Forms.TextBox();
            this.tree = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(271, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search string: (UPPERCASE for search by abbreviation)";
            // 
            // input
            // 
            this.input.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.input.BackColor = System.Drawing.SystemColors.Control;
            this.input.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.input.Location = new System.Drawing.Point(12, 26);
            this.input.Name = "input";
            this.input.Size = new System.Drawing.Size(365, 20);
            this.input.TabIndex = 1;
            this.input.TextChanged += new System.EventHandler(this.Input_TextChanged);
            this.input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Input_KeyDown);
            // 
            // listBox
            // 
            this.tree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tree.BackColor = System.Drawing.SystemColors.Control;
            this.tree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tree.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.tree.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tree.HideSelection = false;
            this.tree.ItemHeight = 16;
            this.tree.Location = new System.Drawing.Point(12, 53);
            this.tree.Name = "tree";
            this.tree.ShowLines = false;
            this.tree.ShowPlusMinus = false;
            this.tree.ShowRootLines = false;
            this.tree.Size = new System.Drawing.Size(365, 194);
            this.tree.TabIndex = 2;
            this.tree.NodeMouseDoubleClick += Tree_NodeMouseDoubleClick;
            this.tree.DrawNode += Tree_DrawNode;
            // 
            // OpenTypeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 255);
            this.Controls.Add(this.tree);
            this.Controls.Add(this.input);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(320, 200);
            this.Name = "OpenTypeForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Open Type";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OpenTypeForm_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OpenTypeForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox input;
        private System.Windows.Forms.TreeView tree;
    }
}