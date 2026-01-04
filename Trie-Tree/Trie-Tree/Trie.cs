using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace Trie_Tree
{
    internal class Trie
    {
        private TrieNode root;
        private int count = 0;
        public Trie()
        {
            root = new TrieNode();
        }
        public int Count => count;
        public bool KiemTra(string word)
        {
            return KiemTra(root, word.ToLower().Trim(), 0);
        }
        private bool KiemTra(TrieNode node, string word, int index)
        {
            if (index == word.Length) {return node.IsEnd;}
            char c = word[index];
            if (!node.children.ContainsKey(c))
            {
                return false;
            }
            bool loop = KiemTra(node.children[c], word, index + 1);
            if (loop) {return true;}
            return false; 
        }
        public bool Insert(string word, string meaning)
        {
            if (KiemTra(word))
            {
                return false;
            }
            if (string.IsNullOrEmpty(word)) return false;
            if (string.IsNullOrEmpty(meaning)) return false;
            var node = root;
            foreach (char c in word)
            {
                if (!node.children.ContainsKey(c))
                {
                    node.children[c] = new TrieNode();
                }
                node = node.children[c];
            }
            node.Meaning = meaning;
            node.IsEnd = true;
            count++;
            return true;
        }
        public bool Delete(string word)
        {
            return Delete(root,word.ToLower().Trim(),0);
        }
        private bool Delete(TrieNode node,string word, int index)
        {
            if (index == word.Length)
            {
                if (!node.IsEnd)
                {
                    return false; 
                }
                node.IsEnd = false; 
                count--;
                return node.children.Count == 0;
            }
            char c = word[index];
            if (!node.children.ContainsKey(c))
            {
                return false;
            }
            bool delete = Delete(node.children[c], word, index + 1);
            if (delete)
            {
                node.children.Remove(c);
                return !node.IsEnd && node.children.Count == 0; 
            }
            return false;
        }
        public bool Search ( string word)
        {
            TrieNode node = root;
            foreach (char c in word)
            {
                if (!node.children.ContainsKey(c))
                {
                    return false;
                }
                node = node.children[c];
            }
            return node.IsEnd;

        }
        public void print(ListBox lstbox)
        {
           printallofword(root, "",lstbox);
        }
        private void printallofword(TrieNode node, string word, ListBox lstbox)
        {
            if (node == null)
            {
                return; // rong thi quay ve thoi 
            }
            if (node.IsEnd) { 
                string display = $"{word} - {node.Meaning}";
                lstbox.Items.Add(display); 
            }
            foreach (var c in node.children.OrderBy(c => c.Key)) // them orderby để lúc in ra thì theo thứ tự a-z
            {
                printallofword(c.Value, word+c.Key,lstbox);
            }
        }
        public void LoadFromFile(string path)
        {
            if (!File.Exists(path)) return;

            foreach (var line in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split('|');
                if (parts.Length < 2) continue; // bỏ qua dòng không có nghĩa

                var word = parts[0].Trim().ToLower();
                var meaning = parts[1].Trim();

                if (string.IsNullOrWhiteSpace(word)) continue;
                if (string.IsNullOrWhiteSpace(meaning)) continue;

                Insert(word, meaning); // luôn kèm nghĩa
            }
        }

        public void SaveToFile(string path)
        {
            var words = new List<string>();
            CollectAllWords(root, "", words);
            File.WriteAllLines(path, words); // ghi mỗi string thành 1 dòng 
        }

        private void CollectAllWords(TrieNode node, string word, List<string> output)
        {
            if (node == null) return;

            if (node.IsEnd) output.Add(word);

            foreach (var c in node.children)
                CollectAllWords(c.Value, word + c.Key, output);
        }
        /// autôcm
        public List<string> AutoComplete(string prefix, int max = 10)
        {
            prefix = (prefix ?? "").Trim().ToLower();
            var result = new List<string>();
            if (string.IsNullOrWhiteSpace(prefix)) return result;

            TrieNode node = root;
            foreach (char c in prefix)
            {
                if (!node.children.TryGetValue(c, out TrieNode next)) {
                    result.Add(" ");
                    result.Add(" ");
                    result.Add(" ");
                    result.Add(" ");
                    result.Add(" ");
                    result.Add(" ");
                    result.Add(" ");
                    result.Add("                    Chưa có từ này!!!");
                    result.Add(" Hãy thêm vào từ điển của bạn để có nhiều ");
                    result.Add("                 từ vựng phong phú hơn");
                    return result; 
                }

                    node = next;
            }

            CollectLimited(node, prefix, result, max);
            if (result.Count == 0) { 
                result.Add("                         Chưa có từ này!!!");
                result.Add(" Hãy thêm vào từ điển của bạn để có tìm thấy!!");
            }
            return result;
        }

        private void CollectLimited(TrieNode node, string current, List<string> result, int max)
        {
            if (result.Count >= max) return;
           
            if (node.IsEnd)
            {
                string display = $"{current} - {node.Meaning}";
                result.Add(display);
            }
            foreach (var kv in node.children.OrderBy(kv => kv.Key))
            {
                if (result.Count >= max) break;
                CollectLimited(kv.Value, current + kv.Key, result, max);
            }
        }

       
    }
}
