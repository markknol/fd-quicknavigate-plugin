using PluginCore;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuickNavigatePlugin.Controls
{
    public class BaseForm : Form
    {
        protected const int MAX_ITEMS = 100;
        protected const string ITEM_SPACER = "-----------------";
        public readonly Settings settings;
        public Brush SelectedNodeBrush { get; private set; }
        public Brush DefaultNodeBrush { get; private set; }
        private TreeView tree;

        public BaseForm(Settings settings)
        {
            Font = PluginBase.Settings.ConsoleFont;
            this.settings = settings;
        }

        protected void Init(TreeView tree)
        {
            (PluginBase.MainForm as FlashDevelop.MainForm).ThemeControls(this);
            this.tree = tree;
            SelectedNodeBrush = new SolidBrush(SystemColors.ControlDarkDark);
            DefaultNodeBrush = new SolidBrush(tree.BackColor);
            InitBasics();
            RefreshTree();
        }

        protected void RefreshTree()
        {
            tree.BeginUpdate();
            tree.Nodes.Clear();
            FillTree();
            tree.EndUpdate();
        }

        protected virtual void InitBasics()
        {
        }

        protected virtual void FillTree()
        {
        }

        protected virtual void Navigate(TreeNode node)
        {
            ASCompletion.Context.ASContext.Context.OnSelectOutlineNode(node);
            Close();
        }

        private bool GetCanNavigate(TreeNode node)
        {
            return node != null && node.Text != ITEM_SPACER; 
        }

        #region Event Handlers

        protected void Tree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;
            if (GetCanNavigate(node)) Navigate(node);
        }

        protected void Input_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control || e.Shift || tree.SelectedNode == null) return;
            TreeNode node;
            int visibleCount = tree.VisibleCount - 1;
            switch (e.KeyCode)
            {
                case Keys.Down:
                    if (tree.SelectedNode.NextVisibleNode != null) tree.SelectedNode = tree.SelectedNode.NextVisibleNode;
                    break;
                case Keys.Up:
                    if (tree.SelectedNode.PrevVisibleNode != null) tree.SelectedNode = tree.SelectedNode.PrevVisibleNode;
                    break;
                case Keys.Home:
                    tree.SelectedNode = tree.Nodes[0];
                    break;
                case Keys.End:
                    node = tree.SelectedNode;
                    while (node.NextVisibleNode != null) node = node.NextVisibleNode;
                    tree.SelectedNode = node;
                    break;
                case Keys.PageUp:
                    node = tree.SelectedNode;
                    for (int i = 0; i < visibleCount; i++)
                    {
                        if (node.PrevVisibleNode == null) break;
                        node = node.PrevVisibleNode;
                    }
                    tree.SelectedNode = node;
                    break;
                case Keys.PageDown:
                    node = tree.SelectedNode;
                    for (int i = 0; i < visibleCount; i++)
                    {
                        if (node.NextVisibleNode == null) break;
                        node = node.NextVisibleNode;
                    }
                    tree.SelectedNode = node;
                    break;
                default: return;
            }
            e.Handled = true;
        }

        protected void Tree_DrawNode(object sender, System.Windows.Forms.DrawTreeNodeEventArgs e)
        {
            if ((e.State & TreeNodeStates.Selected) > 0)
            {
                e.Graphics.FillRectangle(SelectedNodeBrush, e.Bounds);
                e.Graphics.DrawString(e.Node.Text, tree.Font, Brushes.White, e.Bounds.Left, e.Bounds.Top, StringFormat.GenericDefault);
            }
            else
            {
                e.Graphics.FillRectangle(DefaultNodeBrush, e.Bounds);
                e.Graphics.DrawString(e.Node.Text, tree.Font, Brushes.Black, e.Bounds.Left, e.Bounds.Top, StringFormat.GenericDefault);
            }
        }

        protected virtual void Input_TextChanged(object sender, EventArgs e)
        {
            RefreshTree();
        }

        protected virtual void Form_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Close();
                    break;
                case Keys.Enter:
                    e.Handled = true;
                    TreeNode node = tree.SelectedNode;
                    if (GetCanNavigate(node)) Navigate(node);
                    break;
            }
        }

        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        #endregion
    }
}