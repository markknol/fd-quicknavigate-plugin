using ASCompletion;
using ASCompletion.Context;
using ASCompletion.Model;
using QuickNavigatePlugin.Controls;
using System.Drawing;
using System.Windows.Forms;

namespace QuickNavigatePlugin
{
    public partial class QuickOutlineForm : BaseForm
    {
        public QuickOutlineForm(Settings settings):base(settings)
        {
            InitializeComponent();
            if (settings.OutlineFormSize.Width > MinimumSize.Width) Size = settings.OutlineFormSize;
            Init(tree);
        }

        protected override void InitBasics()
        {
            ImageList treeIcons = new ImageList();
            treeIcons.TransparentColor = Color.Transparent;
            treeIcons.Images.AddRange(new Bitmap[] {
                new Bitmap(PluginUI.GetStream("FilePlain.png")),
                new Bitmap(PluginUI.GetStream("FolderClosed.png")),
                new Bitmap(PluginUI.GetStream("FolderOpen.png")),
                new Bitmap(PluginUI.GetStream("CheckAS.png")),
                new Bitmap(PluginUI.GetStream("QuickBuild.png")),
                new Bitmap(PluginUI.GetStream("Package.png")),
                new Bitmap(PluginUI.GetStream("Interface.png")),
                new Bitmap(PluginUI.GetStream("Intrinsic.png")),
                new Bitmap(PluginUI.GetStream("Class.png")),
                new Bitmap(PluginUI.GetStream("Variable.png")),
                new Bitmap(PluginUI.GetStream("VariableProtected.png")),
                new Bitmap(PluginUI.GetStream("VariablePrivate.png")),
                new Bitmap(PluginUI.GetStream("VariableStatic.png")),
                new Bitmap(PluginUI.GetStream("VariableStaticProtected.png")),
                new Bitmap(PluginUI.GetStream("VariableStaticPrivate.png")),
                new Bitmap(PluginUI.GetStream("Const.png")),
                new Bitmap(PluginUI.GetStream("ConstProtected.png")),
                new Bitmap(PluginUI.GetStream("ConstPrivate.png")),
                new Bitmap(PluginUI.GetStream("Const.png")),
                new Bitmap(PluginUI.GetStream("ConstProtected.png")),
                new Bitmap(PluginUI.GetStream("ConstPrivate.png")),
                new Bitmap(PluginUI.GetStream("Method.png")),
                new Bitmap(PluginUI.GetStream("MethodProtected.png")),
                new Bitmap(PluginUI.GetStream("MethodPrivate.png")),
                new Bitmap(PluginUI.GetStream("MethodStatic.png")),
                new Bitmap(PluginUI.GetStream("MethodStaticProtected.png")),
                new Bitmap(PluginUI.GetStream("MethodStaticPrivate.png")),
                new Bitmap(PluginUI.GetStream("Property.png")),
                new Bitmap(PluginUI.GetStream("PropertyProtected.png")),
                new Bitmap(PluginUI.GetStream("PropertyPrivate.png")),
                new Bitmap(PluginUI.GetStream("PropertyStatic.png")),
                new Bitmap(PluginUI.GetStream("PropertyStaticProtected.png")),
                new Bitmap(PluginUI.GetStream("PropertyStaticPrivate.png")),
                new Bitmap(PluginUI.GetStream("Template.png")),
                new Bitmap(PluginUI.GetStream("Declaration.png"))
            });
            tree.ImageList = treeIcons;
        }

        protected override void FillTree()
        {
            FileModel model = ASContext.Context.CurrentModel;
            if (model == FileModel.Ignore) return;
            if (model.Members.Count > 0) AddMembers(tree.Nodes, model.Members);
            foreach (ClassModel classModel in model.Classes)
            {
                int imageNum = PluginUI.GetIcon(classModel.Flags, classModel.Access);
                TreeNode node = new TreeNode(classModel.Name, imageNum, imageNum) { Tag = "class" };
                tree.Nodes.Add(node);
                AddMembers(node.Nodes, classModel.Members);
                node.Expand();
            }
        }

        private void AddMembers(TreeNodeCollection nodes, MemberList members)
        {
            bool wholeWord = settings.OutlineFormWholeWord;
            bool matchCase = settings.OutlineFormMatchCase;
            string searchedText = matchCase ? input.Text.Trim() : input.Text.ToLower().Trim();
            bool searchedTextIsNotEmpty = !string.IsNullOrEmpty(searchedText);
            foreach (MemberModel member in members)
            {
                string memberToString = member.ToString().Trim();
                string memberText = matchCase ? memberToString : memberToString.ToLower();
                if (searchedTextIsNotEmpty && (!wholeWord && memberText.IndexOf(searchedText) == -1 || wholeWord && !memberText.StartsWith(searchedText)))
                    continue;
                int imageIndex = PluginUI.GetIcon(member.Flags, member.Access);
                TreeNode node = new TreeNode(memberToString, imageIndex, imageIndex);
                node.Tag = member.Name + "@" + member.LineFrom;
                node.BackColor = Color.Black;
                nodes.Add(node);
                if (tree.SelectedNode == null) tree.SelectedNode = node;
            }
        }

        #region Event Handlers

        protected override void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            settings.OutlineFormSize = Size;
        }

        #endregion
    }
}