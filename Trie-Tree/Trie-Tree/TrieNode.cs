using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trie_Tree
{
    internal class TrieNode
    {
        public Dictionary<char, TrieNode> children= new Dictionary<char, TrieNode>();
        public bool IsEnd = false;
        public string Meaning;
    }
}
