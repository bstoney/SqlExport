namespace SqlExport.Ui
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using SqlExport.Common.Data;
    using SqlExport.Data;
    using SqlExport.Properties;

    public partial class ObjectView : UserControl, IContextMenuProvider
    {
        /// <summary>
        /// The drag drop nodes.
        /// </summary>
        private List<TreeNode> dragDropNodes;

        public ObjectView()
        {
            InitializeComponent();

            lstObjectTree.ImageList = new ImageList();
            lstObjectTree.ImageList.ColorDepth = ColorDepth.Depth32Bit;
            lstObjectTree.ImageList.Images.AddRange(new Image[] {
                Resources.database,
                Resources.folder,
                Resources.database_table,
                Resources.table_find,
                Resources.bullet_black, 
                Resources.script_gear,
                Resources.sum
            });
            lstObjectTree.ItemHeight = Resources.database.Height + 2;

            lstObjectTree.KeyDown += new KeyEventHandler(OnKeyDown);
            lstObjectTree.DoubleClick += new EventHandler(OnDoubleClick);
            lstObjectTree.MouseDown += new MouseEventHandler(OnMouseDown);
            lstObjectTree.MouseUp += new MouseEventHandler(OnMouseUp);

            mnuActions.Opening += new CancelEventHandler(OnActionMenuOpening);
            mnuLoad.Click += new EventHandler(OnLoadClick);
            mnuGetList.Click += new EventHandler(OnGetListClick);
            mnuGetSource.Click += new EventHandler(OnGetSourceClick);
            mnuSortByDefault.Click += new EventHandler(OnSortByDefaultClick);
            mnuSortAlphabetically.Click += new EventHandler(OnSortAlphabeticallyClick);
            mnuRemove.Click += new EventHandler(OnRemoveClick);

            tmrDragDrop.Tick += new EventHandler(OnDragDropTimerTick);

            ContextMenuStrip = GetContextMenu();
        }

        public void AddConnection(DatabaseDetails db)
        {
            if (db != null)
            {
                TreeNode nodeParent = lstObjectTree.Nodes.Add(db.Name);
                nodeParent.ImageIndex = nodeParent.SelectedImageIndex = 0;
                nodeParent.Tag = new ObjectNodeData(lstObjectTree.Nodes.Count, db, null, db.Name, string.Empty);
                LoadDatabase(nodeParent);
                LoadItemTree(nodeParent.FirstNode);
                nodeParent.Expand();
                nodeParent.FirstNode.Expand();
            }
        }

        public void ClearConnections()
        {
            lstObjectTree.Nodes.Clear();
        }

        private void RemoveSelectedNodes()
        {
            foreach (TreeNode node in lstObjectTree.SelectedNodes)
            {
                node.Remove();
            }
        }

        void OnMouseUp(object sender, MouseEventArgs e)
        {
            tmrDragDrop.Stop();
        }

        /// <summary>
        /// Handles the mouse down event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.dragDropNodes = new List<TreeNode>(this.lstObjectTree.SelectedNodes);
                this.tmrDragDrop.Start();
            }
        }

        /// <summary>
        /// Handles the drag drop timer tick event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnDragDropTimerTick(object sender, EventArgs e)
        {
            if (this.dragDropNodes != null && this.dragDropNodes.Any(n => n.Tag != null))
            {
                this.tmrDragDrop.Stop();
                this.Cursor = Cursors.No;

                // Get a list of the selected items and their appropriate delimiters
                var dragTextQuery = from d in this.dragDropNodes.Select(n => n.Tag).Cast<ObjectNodeData>()
                                    select new
                                        {
                                            d.SqlText,
                                            Delimiter = d.Item.SchemaItemType == SchemaItemType.Column ? ", " : Environment.NewLine
                                        };

                // Join all the items using the delimiter provided. 
                var dragText = dragTextQuery.Aggregate(
                    (string)null, (a, i) => a == null ? i.SqlText : a + i.Delimiter + i.SqlText);

                this.DoDragDrop(dragText, DragDropEffects.Copy);
                this.Cursor = Cursors.Default;
            }
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            tmrDragDrop.Stop();
            if (lstObjectTree.SelectedNode != null && lstObjectTree.SelectedNode.GetNodeCount(false) == 0)
            {
                LoadItemTree(lstObjectTree.SelectedNode);
                lstObjectTree.SelectedNode.Expand();
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                RemoveSelectedNodes();
            }
        }

        private void OnLoadClick(object sender, EventArgs e)
        {
            LoadItemTree(lstObjectTree.SelectedNode);
        }

        private void OnGetListClick(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(Sublist(lstObjectTree.SelectedNode));
        }

        private void OnGetSourceClick(object sender, EventArgs e)
        {
            string script = GetSource(lstObjectTree.SelectedNode);
            if (script != null)
            {
                Clipboard.SetDataObject(script);
            }
        }

        private void OnSortByDefaultClick(object sender, EventArgs e)
        {
            SortNodes(new Comparison<TreeNode>(delegate(TreeNode x, TreeNode y)
            {
                return ((ObjectNodeData)x.Tag).DefaultIndex.CompareTo(((ObjectNodeData)y.Tag).DefaultIndex);
            }));
        }

        private void OnSortAlphabeticallyClick(object sender, EventArgs e)
        {
            SortNodes(new Comparison<TreeNode>(delegate(TreeNode x, TreeNode y)
            {
                return string.Compare(x.Text, y.Text, true);
            }));
        }

        private void OnRemoveClick(object sender, EventArgs e)
        {
            RemoveSelectedNodes();
        }

        private void SortNodes(Comparison<TreeNode> comparison)
        {
            if (lstObjectTree.SelectedNode != null && lstObjectTree.SelectedNode.Nodes.Count > 0)
            {
                TreeNode[] tns = new TreeNode[lstObjectTree.SelectedNode.Nodes.Count];
                lstObjectTree.SelectedNode.Nodes.CopyTo(tns, 0);
                Array.Sort(tns, comparison);
                lstObjectTree.SelectedNode.Nodes.Clear();
                lstObjectTree.SelectedNode.Nodes.AddRange(tns);
            }
        }

        private void OnActionMenuOpening(object sender, CancelEventArgs e)
        {
            if (lstObjectTree.SelectedNode != null)
            {
                ObjectNodeData ond = lstObjectTree.SelectedNode.Tag as ObjectNodeData;
                if (ond != null && ond.Item != null)
                {
                    switch (ond.Item.SchemaItemType)
                    {
                        case SchemaItemType.Folder:
                            mnuLoad.Visible = true;
                            mnuGetList.Visible = true;
                            mnuGetSource.Visible = false;
                            mnuSortByDefault.Visible = false;
                            mnuSortAlphabetically.Visible = false;
                            mnuSeperator.Visible = false;
                            mnuAddDatabase.Visible = false;
                            break;
                        case SchemaItemType.Table:
                        case SchemaItemType.View:
                        case SchemaItemType.Procedure:
                        case SchemaItemType.Function:
                            mnuLoad.Visible = true;
                            mnuGetList.Visible = true;
                            mnuGetSource.Visible = true;
                            mnuSortByDefault.Visible = true;
                            mnuSortAlphabetically.Visible = true;
                            mnuSeperator.Visible = false;
                            mnuAddDatabase.Visible = false;
                            break;
                        case SchemaItemType.Column:
                            mnuLoad.Visible = false;
                            mnuGetList.Visible = false;
                            mnuGetSource.Visible = false;
                            mnuSortByDefault.Visible = false;
                            mnuSortAlphabetically.Visible = false;
                            mnuSeperator.Visible = false;
                            mnuAddDatabase.Visible = false;
                            break;
                        default: // Database, unknown
                            mnuLoad.Visible = false;
                            mnuGetList.Visible = false;
                            mnuGetSource.Visible = false;
                            mnuSortByDefault.Visible = false;
                            mnuSortAlphabetically.Visible = false;
                            mnuSeperator.Visible = true;
                            mnuAddDatabase.Visible = true;
                            break;
                    }
                }
            }

            ContextMenuHelper.AddHierachicalContextMenu(this, GetContextMenu());
        }

        private void LoadItemTree(TreeNode nodeParent)
        {
            try
            {
                nodeParent.Nodes.Clear();
                using (ISchemaAdapter schema = ((ObjectNodeData)nodeParent.Tag).Database.GetSchemaAdapter())
                {
                    SchemaItem[] items = schema.PopulateFromPath(GetPathFromNode(nodeParent));
                    if (items != null)
                    {
                        for (int i = 0; i < items.Length; i++)
                        {
                            SchemaItem item = items[i];
                            TreeNode node = nodeParent.Nodes.Add(item.DisplayName);
                            switch (item.SchemaItemType)
                            {
                                case SchemaItemType.Table:
                                    node.ImageIndex = node.SelectedImageIndex = 2;
                                    break;
                                case SchemaItemType.View:
                                    node.ImageIndex = node.SelectedImageIndex = 3;
                                    break;
                                case SchemaItemType.Column:
                                    node.ImageIndex = node.SelectedImageIndex = 4;
                                    break;
                                case SchemaItemType.Procedure:
                                    node.ImageIndex = node.SelectedImageIndex = 5;
                                    break;
                                case SchemaItemType.Function:
                                    node.ImageIndex = node.SelectedImageIndex = 6;
                                    break;
                                case SchemaItemType.Folder:
                                    node.ImageIndex = node.SelectedImageIndex = 1;
                                    break;
                            }
                            node.Tag = new ObjectNodeData(i, ((ObjectNodeData)nodeParent.Tag).Database, item,
                                item.DisplayName, item.QueryText);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                ErrorDialogLogic.AddError(exp);
                ErrorDialogLogic.ShowForm();
            }
        }

        private string[] GetPathFromNode(TreeNode node)
        {
            List<string> path = new List<string>();
            while (node.Parent != null)
            {
                path.Add(((ObjectNodeData)node.Tag).DisplayText);
                node = node.Parent;
            }
            path.Reverse();
            return path.ToArray();
        }

        private void LoadDatabase(TreeNode nodeParent)
        {
            try
            {
                nodeParent.Nodes.Clear();
                using (ISchemaAdapter schema = ((ObjectNodeData)nodeParent.Tag).Database.GetSchemaAdapter())
                {
                    string[] sections = schema.GetSections();
                    for (int i = 0; i < sections.Length; i++)
                    {
                        string section = sections[i];
                        TreeNode node = nodeParent.Nodes.Add(section);
                        node.ImageIndex = node.SelectedImageIndex = 1;
                        node.Tag = new ObjectNodeData(i, ((ObjectNodeData)nodeParent.Tag).Database, new SchemaItem(section, SchemaItemType.Folder), section, string.Empty);
                    }
                }
            }
            catch (Exception exp)
            {
                ErrorDialogLogic.AddError(exp);
                ErrorDialogLogic.ShowForm();
            }
        }

        private string Sublist(TreeNode node)
        {
            if (node.Nodes.Count <= 0)
            {
                LoadItemTree(node);
            }
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                ObjectNodeData nd = (ObjectNodeData)node.Nodes[i].Tag;
                if (i == 0)
                {
                    sb.Append(nd.SqlText);
                }
                else
                {
                    sb.AppendFormat(", {0}", nd.SqlText);
                }
            }
            return sb.ToString();
        }

        private string GetSource(TreeNode node)
        {
            ObjectNodeData nodeData = (ObjectNodeData)node.Tag;
            string script = null;
            try
            {
                using (ISchemaAdapter schema = nodeData.Database.GetSchemaAdapter())
                {
                    script = schema.GetSchemaItemScript(GetPathFromNode(node));
                }
            }
            catch (Exception exp)
            {
                ErrorDialogLogic.AddError(exp);
                ErrorDialogLogic.ShowForm();
            }
            return script;
        }

        #region IContextMenuProvider Members

        public string MenuTitle
        {
            get { return "Database Item"; }
        }

        public bool HasContextMenu
        {
            get { return lstObjectTree.SelectedNode != null; }
        }

        public ContextMenuStrip GetContextMenu()
        {
            return mnuActions;
        }

        #endregion
    }
}
