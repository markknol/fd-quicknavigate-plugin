using ASCompletion;
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
        private const int MAX_ITEMS = 100;
        private const string ITEM_SPACER = "-----------------";
        private readonly List<string> projectTypes = new List<string>();
        private readonly List<string> openedTypes = new List<string>();
        private readonly Dictionary<string, ClassModel> dictionary = new Dictionary<string, ClassModel>();

        public OpenTypeForm(Settings settings) : base(settings)
        {
            Font = PluginBase.Settings.ConsoleFont;
            InitializeComponent();
            if (settings.TypeFormSize.Width > MinimumSize.Width) Size = settings.TypeFormSize;
            CreateItemsList();
            Init(tree);
        }

        private void CreateItemsList()
        {
            projectTypes.Clear();
            openedTypes.Clear();
            dictionary.Clear();
            IASContext context = ASContext.GetLanguageContext(PluginBase.CurrentProject.Language);
            if (context == null) return;
            foreach (PathModel path in context.Classpath) path.ForeachFile(FileModelDelegate);
        }

        private bool FileModelDelegate(FileModel model)
        {
            foreach (ClassModel classModel in model.Classes)
            {
                string name = classModel.QualifiedName;
                if (name.Contains("<") || dictionary.ContainsKey(name)) continue;
                if (SearchUtil.IsFileOpened(classModel.InFile.FileName)) openedTypes.Add(name);
                else projectTypes.Add(name);
                dictionary.Add(name, classModel);
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
            foreach (string text in matchedItems) tree.Nodes.Add(new TreeNode(text));
            if (tree.Nodes.Count > 0) tree.SelectedNode = tree.Nodes[0];
        }

        protected override void Navigate()
        {
            if (tree.SelectedNode == null) return;
            string selectedItem = tree.SelectedNode.Text;
            if (selectedItem == ITEM_SPACER) return;
            ClassModel classModel = dictionary[selectedItem];
            FileModel model = ModelsExplorer.Instance.OpenFile(classModel.InFile.FileName);
            if (model != null)
            {
                ClassModel theClass = model.GetClassByName(classModel.Name);
                if (!theClass.IsVoid())
                {
                    int line = theClass.LineFrom;
                    ScintillaNet.ScintillaControl sci = PluginBase.MainForm.CurrentDocument.SciControl;
                    if (sci != null && line > 0 && line < sci.LineCount)
                        sci.GotoLine(line);
                }
            }
            Close();
        }

        #region Event Handlers

        protected override void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            settings.TypeFormSize = Size;
        }

        #endregion
    }
}