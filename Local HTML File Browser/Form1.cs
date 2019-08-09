using System;
using System.IO;
using System.Windows.Forms;

namespace Local_HTML_File_Browser {
    public partial class Main : Form {
        public Main() {
            InitializeComponent();
        }

        bool useCode = false;
        string selectedFilePath = "";

        private void OpenDirectoryToolStripMenuItem_Click(object sender, EventArgs e) {
            FolderBrowserDialog folderPicker = new FolderBrowserDialog();
            if (folderPicker.ShowDialog() == DialogResult.OK) {

                fileViewer.Items.Clear();

                string[] files = Directory.GetFiles(folderPicker.SelectedPath);
                foreach (string file in files) {

                    string fileName = Path.GetFileNameWithoutExtension(file);

                    if (file.Contains("htm")) {
                        ListViewItem item = new ListViewItem(fileName);
                        item.Tag = file;

                        fileViewer.Items.Add(item);
                    }
                }

            }
        }

        private void FileViewer_SelectedIndexChanged(object sender, EventArgs e) {
            if (fileViewer.SelectedItems.Count > 0) {

                ListViewItem selected = fileViewer.SelectedItems[0];
                string selectedFilePath = selected.Tag.ToString();
                this.selectedFilePath = selectedFilePath;

                load();
            }
        }

        private void load() {
            if (selectedFilePath.Length < 1)
                return;

            if (useCode) {
                srcCode.Text = "";

                var lines = File.ReadLines(selectedFilePath);

                foreach (var line in lines) {
                    srcCode.AppendText(line.ToString());
                    srcCode.AppendText(Environment.NewLine);
                }

                webBrowser.Visible = false;
                srcCode.Visible = true;
            } else {
                webBrowser.Url = new Uri("file:///" + selectedFilePath);
                webBrowser.Visible = true;
                srcCode.Visible = false;
            }
        }

        private void ToggleCodeBrowserToolStripMenuItem_Click(object sender, EventArgs e) {
            useCode = !useCode;
            load();
        }
    }
}
