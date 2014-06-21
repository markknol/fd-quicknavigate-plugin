using ASCompletion.Context;
using ASCompletion.Model;
using PluginCore;
using QuickNavigatePlugin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace QuickNavigatePlugin
{
    public partial class OpenResourceForm : BaseForm
    {
        private readonly List<string> projectFiles = new List<string>();
        private readonly List<string> openedFiles = new List<string>();

        public OpenResourceForm(Settings settings):base(settings)
        {
            InitializeComponent();
            if (settings.ResourceFormSize.Width > MinimumSize.Width) Size = settings.ResourceFormSize;
            Init(tree);
        }

        protected override void InitBasics()
        {
            refreshButton.Image = PluginBase.MainForm.FindImage("66");
            new ToolTip().SetToolTip(refreshButton, "Ctrl+R");
            LoadFileList();
        }

        protected override void FillTree()
        {
            List<string> matches;
            string searchText = input.Text.Trim();
            if (string.IsNullOrEmpty(searchText)) matches = openedFiles;
            else 
            {
                bool wholeWord = settings.ResourceFormWholeWord;
                bool matchCase = settings.ResourceFormMatchCase;
                matches = SearchUtil.Matches(openedFiles, searchText, "\\", 0, wholeWord, matchCase);
                if (matches.Capacity > 0) matches.Add(ITEM_SPACER);
                matches.AddRange(SearchUtil.Matches(projectFiles, searchText, "\\", MAX_ITEMS, wholeWord, matchCase));
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
            string file = node.Text;
            PluginBase.MainForm.OpenEditableDocument(file);
            base.Navigate(new TreeNode(Path.GetFileNameWithoutExtension(file)) { Tag = node.Tag });
        }

        private void LoadFileList()
        {
            openedFiles.Clear();
            projectFiles.Clear();
            ShowMessage("Reading project files...");
            worker.RunWorkerAsync();
        }

        private void RebuildJob()
        {
            IProject project = PluginBase.CurrentProject;
            foreach (string file in GetProjectFiles())
            {
                if (IsFileHidden(file)) continue;
                if (SearchUtil.IsFileOpened(file)) openedFiles.Add(project.GetAbsolutePath(file));
                else projectFiles.Add(project.GetAbsolutePath(file));
            }
        }

        private bool IsFileHidden(string file)
        {
            //TODO slavara: move to settings
            string path = Path.GetDirectoryName(file);
            string name = Path.GetFileName(file);
            return path.Contains(".svn") || path.Contains(".cvs") || path.Contains(".git") || name.Substring(0, 1) == ".";
        }

        private void ShowMessage(string text)
        {
            tree.Nodes.Clear();
            tree.Nodes.Add(new TreeNode(text));
        }

        private List<string> GetProjectFiles()
        {
            if (!settings.ResourcesCaching || projectFiles.Count == 0)
            {
                projectFiles.Clear();
                foreach (string folder in GetProjectFolders())
                    projectFiles.AddRange(Directory.GetFiles(folder, "*.*", SearchOption.AllDirectories));
            }
            return projectFiles;
        }

        private List<string> GetProjectFolders()
        {
            List<string> folders = new List<string>();
            IProject project = PluginBase.CurrentProject;
            if (project == null) return folders;
            string projectFolder = Path.GetDirectoryName(project.ProjectPath);
            folders.Add(projectFolder);
            if (!settings.SearchExternalClassPath) return folders;
            IASContext context = ASContext.GetLanguageContext(PluginBase.CurrentProject.Language);
            if (context == null) return folders;
            foreach (PathModel pathModel in context.Classpath)
            {
                string absolute = project.GetAbsolutePath(pathModel.Path);
                if (Directory.Exists(absolute)) folders.Add(absolute);
            }
            return folders;
        }

        #region Event Handlers

        protected override void Input_TextChanged(object sender, EventArgs e)
        {
            if (!worker.IsBusy) base.Input_TextChanged(sender, e);
        }

        protected override void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R && e.Control && !worker.IsBusy) LoadFileList();
            else base.Form_KeyDown(sender, e);
        }

        protected override void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            projectFiles.Clear();
            openedFiles.Clear();
            settings.ResourceFormSize = Size;
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            if (!worker.IsBusy) LoadFileList();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            RebuildJob();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FillTree();
        }

        #endregion
    }
}