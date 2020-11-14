using System;
using System.Collections.Generic;
using System.Linq;

namespace Trie
{
    public class Trie
    {

        // Alphabet size (# of symbols) 
        static readonly int ALPHABET_SIZE = 26;

        //private List<string> found;
        private Dictionary<string, int> found;

        // trie node 
        class TrieNode
        {
            //public TrieNode[] children = new TrieNode[ALPHABET_SIZE];
            public Dictionary<Char, TrieNode> children = new Dictionary<char, TrieNode>();
            // isEndOfWord is true if the node represents 
            // end of a word 
            public bool isEndOfWord;
            public int weight;
            public TrieNode()
            {
                isEndOfWord = false;
                weight = 0;
                //for (int i = 0; i < ALPHABET_SIZE; i++)
                    //children[i] = null;
            }
        };

        static TrieNode root;

        // If not present, inserts key into trie 
        // If the key is prefix of trie node,  
        // just marks leaf node 
        static void insert(String key)
        {
            int level;
            int length = key.Length;
            int index;

            TrieNode pCrawl = root;

            //for (level = 0; level < length; level++)
            //{
            //    index = key[level] - 'a';
            //    if (pCrawl.children[index] == null)
            //        pCrawl.children[index] = new TrieNode();

            //    pCrawl = pCrawl.children[index];
            //}

            for (level = 0; level < length; level++)
            {
                if (!pCrawl.children.ContainsKey(key[level]))
                    pCrawl.children[key[level]] = new TrieNode();

                pCrawl = pCrawl.children[key[level]];
            }


            // mark last node as leaf 
            pCrawl.isEndOfWord = true;
        }

        // Returns true if key  
        // presents in trie, else false 
        static bool search(String key)
        {
            int level;
            int length = key.Length;
            TrieNode pCrawl = root;

            //for (level = 0; level < length; level++)
            //{
            //    index = key[level] - 'a';

            //    if (pCrawl.children[index] == null)
            //        return false;

            //    pCrawl = pCrawl.children[index];
            //}


            for (level = 0; level < length; level++)
            {
                if (!pCrawl.children.ContainsKey(key[level]))
                    return false;

                pCrawl = pCrawl.children[key[level]];
            }

            return (pCrawl != null && pCrawl.isEndOfWord);
        }

        private void addWeight(String key)
        {
            int level;
            int length = key.Length;
            TrieNode pCrawl = root;

            for (level = 0; level < length; level++)
            {
                if (!pCrawl.children.ContainsKey(key[level]))
                    return;

                pCrawl = pCrawl.children[key[level]];
            }
            if (pCrawl != null && pCrawl.isEndOfWord)
            {
                pCrawl.weight++;
            }
        }

        // basically autocomplete
        private void searchAll(String key)
        {
            //List<string> ret = new List<string>();

            int level;
            int length = key.Length;
            TrieNode pCrawl = root;

            //for (level = 0; level < length; level++)
            //{
            //    index = key[level] - 'a';

            //    if (pCrawl.children[index] == null)
            //        return;

            //    pCrawl = pCrawl.children[index];
            //}

            for (level = 0; level < length; level++)
            {
                if (!pCrawl.children.ContainsKey(key[level]))
                    return;

                pCrawl = pCrawl.children[key[level]];
            }
            if (pCrawl.isEndOfWord)
            {
                found.Add(key, pCrawl.weight);
            }            
            searchNode(key, pCrawl);

            //return ret;
        }

        private void searchNode(String builder, TrieNode node)
        {
            //for (int i = 0; i < ALPHABET_SIZE; i++)
            //{
            //    if (node.children[i] != null)
            //    {

            //        if (node.children[i].isEndOfWord)
            //        {
            //            found.Add(builder + ((char)(i + 97)).ToString());
            //            searchNode(builder + ((char)(i + 97)).ToString(), node.children[i]);
            //        } else
            //        {
            //            searchNode(builder + ((char)(i + 97)).ToString(), node.children[i]);
            //        }
            //    }
            //}
            foreach (var item in node.children)
            {
                if (item.Value.isEndOfWord)
                {
                    found.Add(builder + item.Key.ToString(), item.Value.weight);
                    searchNode(builder + item.Key.ToString(), item.Value);
                } else
                {
                    searchNode(builder + item.Key.ToString(), item.Value);
                }
            }

            return;
        }

        public static void Main()
        {
            Trie trie = new Trie();
            trie.Go();
        }
        // Driver 
        public void Go()
        {
            // Input keys (use only 'a'  
            // through 'z' and lower case) 
            string rline;
            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader("words.txt");
            List<String> keys = new List<string>();
            while ((rline = file.ReadLine()) != null)
            {
                keys.Add(rline);
            }
            //using (System.IO.StreamWriter file2 = new System.IO.StreamWriter(@"C:\Users\Brett\Desktop\dict.txt"))
            //{
            //    while ((rline = file.ReadLine()) != null)
            //    {
            //        keys.Add(rline);               
            //        if (!rline.Contains("&"))
            //        {
            //            file2.WriteLine(rline.ToLower());
            //        }
            //    }
            //}

            root = new TrieNode();
            // Construct trie 
            int i;
            for (i = 0; i < keys.Count; i++)
                insert(keys[i]);

            //keys = null;
            string line = "";
            found = new Dictionary<string, int>();
            do
            {
                //read a complete line
                char c = Console.ReadKey().KeyChar;
                found.Clear();
                //check if line is empty or not
                if (c == '\r')
                {
                    addWeight(line);
                    Console.Clear();
                    Console.WriteLine("Searching: ");
                    line = "";
                } else if(Char.IsLetter(c))
                {
                    line += c.ToString();
                    searchAll(line);
                    var test = found;
                    Console.Clear();
                    Console.WriteLine("Searching: " + line);
                    Console.WriteLine(found.Count + " found");
                    int count = 0;                
                    foreach (var item in found.OrderByDescending(x => x.Value))
                    {
                        if (count == 20) break;
                        string ln = item.Key;
                        if (item.Value > 0)
                        {
                            ln += " (suggested)";
                        }
                        Console.WriteLine(ln);
                        count++;
                    }
                    //Console.Write("Line was = " + line);
                }
            } while (line != null);


        }
    }
}
