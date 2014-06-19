using ASCompletion.Context;
using ASCompletion.Model;
using PluginCore;
using QuickNavigatePlugin.Controls;
using System.Collections.Generic;
using System.Windows.Forms;

namespace QuickNavigatePlugin
{
    public partial class OpenTypeForm : BaseForm
    {
        private readonly List<string> projectTypes = new List<string>();
        private readonly List<string> openedTypes = new List<string>();

        public OpenTypeForm(Settings settings) : base(settings)
        {
            InitializeComponent();
            if (settings.TypeFormSize.Width > MinimumSize.Width) Size = settings.TypeFormSize;
            Init(tree);
        }

        protected override void InitBasics()
        {
            projectTypes.Clear();
            openedTypes.Clear();
            IASContext context = ASContext.GetLanguageContext(PluginBase.CurrentProject.Language);
            if (context == null) return;
            foreach (PathModel path in context.Classpath) path.ForeachFile(FileModelDelegate);
        }

        private bool FileModelDelegate(FileModel model)
        {
            foreach (ClassModel classModel in model.Classes)
            {
                string name = classModel.QualifiedName;
                if (name.Contains("<") || openedTypes.Contains(name) || projectTypes.Contains(name)) continue;
                if (SearchUtil.IsFileOpened(classModel.InFile.FileName)) openedTypes.Add(name);
                else projectTypes.Add(name);
            }
            return true;
        }

        protected override void FillTree()
        {
            List<string> matchedItems;
            string searchText = input.Text.Trim();
            if (string.IsNullOrEmpty(searchText)) matchedItems = openedTypes;
            else
            {
                bool wholeWord = settings.TypeFormWholeWord;
                bool matchCase = settings.TypeFormMatchCase;
                matchedItems = SearchUtil.GetMatchedItems(openedTypes, searchText, ".", 0, wholeWord, matchCase);
                if (matchedItems.Capacity > 0) matchedItems.Add(ITEM_SPACER);
                matchedItems.AddRange(SearchUtil.GetMatchedItems(projectTypes, searchText, ".", MAX_ITEMS, wholeWord, matchCase));
            }
            foreach (string text in matchedItems) tree.Nodes.Add(new TreeNode(text) { Tag = "import" });
            if (tree.Nodes.Count > 0) tree.SelectedNode = tree.Nodes[0];
        }

        #region Event Handlers

        protected override void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            settings.TypeFormSize = Size;
        }

        #endregion
    }
}