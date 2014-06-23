using ASCompletion.Context;
using ASCompletion.Model;
using PluginCore;
using QuickNavigatePlugin.Controls;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace QuickNavigatePlugin
{
    public partial class OpenTypeForm : BaseForm
    {
        private readonly List<string> projectTypes = new List<string>();
        private readonly List<string> openedTypes = new List<string>();
        private readonly Dictionary<string, FileModel> name2model = new Dictionary<string, FileModel>();

        public OpenTypeForm(Settings settings) : base(settings)
        {
            InitializeComponent();
            if (settings.TypeFormSize.Width > MinimumSize.Width) Size = settings.TypeFormSize;
            Init(tree);
        }

        protected override void InitBasics()
        {
            IASContext context = ASContext.GetLanguageContext(PluginBase.CurrentProject.Language);
            if (context == null) return;
            foreach (PathModel path in context.Classpath) path.ForeachFile(FileModelDelegate);
        }

        protected override void FillTree()
        {
            List<string> matches;
            string searchText = input.Text.Trim();
            if (string.IsNullOrEmpty(searchText)) matches = openedTypes;
            else
            {
                bool wholeWord = settings.TypeFormWholeWord;
                bool matchCase = settings.TypeFormMatchCase;
                matches = SearchUtil.Matches(openedTypes, searchText, ".", 0, wholeWord, matchCase);
                if (matches.Capacity > 0) matches.Add(ITEM_SPACER);
                matches.AddRange(SearchUtil.Matches(projectTypes, searchText, ".", MAX_ITEMS, wholeWord, matchCase));
            }
            foreach (string text in matches)
            {
                TreeNode node = new TreeNode(text);
                if (text != ITEM_SPACER) node.Tag = "class";
                tree.Nodes.Add(node);
            }
            if (tree.Nodes.Count > 0) tree.SelectedNode = tree.Nodes[0];
        }

        protected override void Navigate(TreeNode node)
        {
            string file = name2model[node.Text].FileName;
            PluginBase.MainForm.OpenEditableDocument(file);
            base.Navigate(new TreeNode(Path.GetFileNameWithoutExtension(file)) { Tag = node.Tag });
        }

        private bool FileModelDelegate(FileModel model)
        {
            foreach (ClassModel classModel in model.Classes)
            {
                string name = classModel.QualifiedName;
                if (name.Contains("<") || openedTypes.Contains(name) || projectTypes.Contains(name)) continue;
                if (SearchUtil.IsFileOpened(classModel.InFile.FileName)) openedTypes.Add(name);
                else projectTypes.Add(name);
                name2model.Add(name, model);
            }
            return true;
        }
        
        #region Event Handlers

        protected override void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            settings.TypeFormSize = Size;
        }

        #endregion
    }
}