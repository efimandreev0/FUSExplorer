using LABO.FUSE;
using System.Windows.Forms;
namespace LABO
{
    public partial class main : Form
    {
        public FibArchive fib;
        public List<string> paths = [];
        public List<string> strings = [];
        public List<string> chStrings = [];
        public bool IsLoc;
        public string beenPath;
        public main()
        {
            //paths.Clear();
            InitializeComponent();
            treeView1.ContextMenuStrip = contextMenuStrip1;
        }
        private void AddPathToTreeView(TreeView treeView, string path)
        {
            // Разделяем путь по символу '/'
            string[] parts = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            TreeNode currentNode = null;

            for (int i = 0; i < parts.Length - 1; i++) // Обрабатываем только все элементы, кроме последнего
            {
                string part = parts[i];

                // Если это корень, начинаем с него
                if (currentNode == null)
                {
                    // Найдем или создадим корневой узел
                    currentNode = treeView.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Text.Equals(part, StringComparison.OrdinalIgnoreCase));
                    if (currentNode == null) // Не существует -> создаем новый корень
                    {
                        currentNode = new TreeNode(part);
                        treeView.Nodes.Add(currentNode);
                    }
                }
                else
                {
                    // Находим или создаем папку в текущем узле
                    TreeNode folderNode = currentNode.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Text.Equals(part, StringComparison.OrdinalIgnoreCase));
                    if (folderNode == null) // Если не существует, создаем новый узел
                    {
                        folderNode = new TreeNode(part);
                        currentNode.Nodes.Add(folderNode);
                    }

                    // Перемещаем указатель на текущий узел (папку)
                    currentNode = folderNode;
                }
            }

            // Добавляем последний элемент как файл
            string fileName = parts.Last(); // Получаем последний элемент
            TreeNode fileNode = new TreeNode(fileName);
            if (currentNode == null)
            {
                //treeView.
            }
            else
                currentNode.Nodes.Add(fileNode); // Добавляем файл в текущую папку
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            paths.Clear();
            OpenFileDialog openFileDialog = new();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fib = new(openFileDialog.FileName);
                for (int i = 0; i < fib.Files.Count; i++)
                {
                    paths.Add(new DirectoryInfo(fib.ArchiveFilePath.Replace(Path.GetExtension(fib.ArchiveFilePath), "")).Name + "/" + fib.Files[i].Path.Replace("\\", "/"));
                    AddPathToTreeView(treeView1, paths[i]);
                }

                MessageBox.Show($"File {fib.ArchiveFilePath} was succesfully loaded!");
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void openAsTextToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
            listBox1.SelectedIndex = -1;
            listBox1.Items.Clear();
            chStrings.Clear();
            strings.Clear();

            string fileName = treeView1.SelectedNode.FullPath.Replace("\\", "/");
            File.WriteAllBytes("tmp.bin", fib.ExtractFile(fib.Files[paths.IndexOf(fileName)], true));
            beenPath = fileName;
            if (fileName.EndsWith(".loc"))
                strings = LOCA.Read("tmp.bin").ToList();
            else
                strings = File.ReadAllLines("tmp.bin").ToList();
            for (int i = 0; i < strings.Count; i++)
                listBox1.Items.Add((i + 1).ToString("d4"));
            chStrings.AddRange(strings);
            richTextBox1.Text = chStrings[0];
            richTextBox2.Text = strings[0];
            //MessageBox.Show("Полный путь к файлу: " + fileName);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                richTextBox1.Text = chStrings[listBox1.SelectedIndex];
                richTextBox2.Text = strings[listBox1.SelectedIndex];
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
                chStrings[listBox1.SelectedIndex] = richTextBox1.Text;
        }
        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                string selectedPath = treeView1.SelectedNode.FullPath.Replace("\\", "/");
                if (!selectedPath.Contains("."))
                {
                    for (int i = 0; i < paths.Count; i++)
                    {
                        if (paths[i].Contains(selectedPath))
                            ExtractFile(paths[i]);
                    }

                }
                else if (selectedPath.Contains("."))
                    ExtractFile(selectedPath);
                else
                    MessageBox.Show("Selected item is not a file or directory.");
            }
        }
        private void ExtractFile(string filePath)
        {
            try
            {
                string dirPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
                byte[] fileData = fib.ExtractFile(fib.Files[paths.IndexOf(filePath)], true);
                File.WriteAllBytes(filePath, fileData);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error extracting file " + filePath + ": " + ex.Message);
            }
        }
        private void extractAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog o = new();
            if (o.ShowDialog() == DialogResult.OK)
            {
                string fileName = treeView1.SelectedNode.FullPath.Replace("\\", "/");
                File.WriteAllBytes(o.FileName, fib.ExtractFile(fib.Files[paths.IndexOf(fileName)], true));
            }
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            OpenFileDialog o = new();
            if (o.ShowDialog() == DialogResult.OK)
            {
                strings.Clear();
                listBox1.Items.Clear();
                chStrings.Clear();

                switch (o.FileName)
                {
                    case ".loc":
                        IsLoc = true;
                        strings.AddRange(LOCA.Read(o.FileName));
                        break;
                    default:
                        strings.AddRange(File.ReadAllLines(o.FileName));
                        break;
                }
                File.Copy(o.FileName, "tmp.bin");
                chStrings.AddRange(strings);
            }
        }

        private void saveInsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //File.WriteAllLines("ChangedFile.txt", chStrings);
            if (IsLoc)
            {
                LOCA.Write("tmp.bin", chStrings.ToArray());
            }
            fib.ReplaceFile(fib.Files[paths.IndexOf(beenPath)], "tmp.bin");

        }

        private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            File.WriteAllLines("ChangedFile.txt", chStrings);
        }

        private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new();
            if (o.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllLines(o.FileName, chStrings);
            }
        }

        private void main_Load(object sender, EventArgs e)
        {

        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new();
            if (o.ShowDialog() == DialogResult.OK)
            {
                string fileName = treeView1.SelectedNode.FullPath.Replace("\\", "/");
                fib.ReplaceFile(fib.Files[paths.IndexOf(fileName)], o.FileName);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog f = new();
            if (f.ShowDialog() == DialogResult.OK)
            {
                string dir = f.SelectedPath;
                string[] files = Directory.GetFiles(dir, "*.*");
                for (int i = 0; i < files.Length; i++)
                {
                    for (int a = 0; a < paths.Count; a++)
                    {
                        if (paths[a].Contains(Path.GetFileNameWithoutExtension(files[i])))
                        {
                            fib.ReplaceFile(fib.Files[a], files[i]);
                            break;
                        }
                    }
                }
            }
        }
    }
}
