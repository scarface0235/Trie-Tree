using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trie_Tree
{
    public partial class Form1 : Form
    {
        private Trie trie = new Trie();
        public Form1()
        {
            InitializeComponent();
        }

        public void upDateListBox()
        {
            lstBox.Items.Clear();
            trie.print(lstBox);
        }
        private void upCount()
        {
            txtCount.Text = trie.Count.ToString();
        }

        private void btn_insert_Click(object sender, EventArgs e)
        {
            string word = txtData.Text.ToLower().Trim();
            string meaning = txtMeaning.Text.ToLower().Trim();
            if (string.IsNullOrEmpty(word) || string.IsNullOrEmpty(meaning)) 
            {
                MessageBox.Show("Phải điền đầy đủ cả từ và nghĩa", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (trie.Insert(word, meaning))
            {
                MessageBox.Show("Đã thêm", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                upDateListBox();
                upCount();
            }
            else
            {
                MessageBox.Show("Có rồi mà", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            string word = txtData.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(word))
            {
                MessageBox.Show(" Phải nhập từ muốn xóa ", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DialogResult ketQua = MessageBox.Show("Bạn chắc muốn xóa từ này", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (ketQua == DialogResult.No)
                {
                    return;
                }
                bool ketQuaXoa = trie.Delete(txtData.Text);
                MessageBox.Show("Xóa thành công ", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtData.Clear();
                txtMeaning.Clear();
                upDateListBox();
                upCount();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnsearch_Click(object sender, EventArgs e)
        {
            string word = txtData.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(word))
            {
                MessageBox.Show("Phải nhập từ muốn tìm", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                bool ketquatim = trie.Search(word);
                if (ketquatim == true)
                {
                    MessageBox.Show("Có từ này nha", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Từ không tồn tại", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            trie.LoadFromFile("words-1000.txt");
            upDateListBox(); 
            upCount();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult ketqua = MessageBox.Show("Có muốn lưu thay đổi","Sắp đóng file đó nha",MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (ketqua == DialogResult.Cancel) 
            {
                e.Cancel = true;
                return;
            }
            if (ketqua == DialogResult.Yes)
            {
                trie.SaveToFile("words-1000.txt");
            }
        }

        private void txtData_TextChanged(object sender, EventArgs e)
        {
            if (isSelectingFromList) return; 

            string prefix = txtData.Text.Trim().ToLower();

            lstBox.Items.Clear();

            if (string.IsNullOrWhiteSpace(prefix))
            {
                trie.print(lstBox); 
                return;
            }

            foreach (var w in trie.AutoComplete(prefix, 10))
                lstBox.Items.Add(w);
        }

        private bool isSelectingFromList = false;
        private void lstBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (lstBox.SelectedItem == null) return;

            string selected = lstBox.SelectedItem.ToString();
            if (!selected.Contains(" - ")) return;

            var parts = selected.Split(new[] { " - " }, 2, StringSplitOptions.None);

            if (parts.Length >= 2)
            {
                isSelectingFromList = true;
                txtData.Text = parts[0].Trim();
                txtMeaning.Text = parts[1].Trim();
                isSelectingFromList = false;
            }
        }
    }
}
