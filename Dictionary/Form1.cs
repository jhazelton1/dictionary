using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dictionary
{
    public partial class Form1 : Form
    {
        Dictionary<string, List<string>> words;
        List<string> allWords;

        public Form1()
        {
            InitializeComponent();
        }

        private void LoadDictionaryButton_Click(object sender, EventArgs e)
        {
            if (openDictionaryDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = openDictionaryDialog.FileName;

                string[] lines = System.IO.File.ReadAllLines(fileName);

                allWords = lines.ToList();

                words = lines.Aggregate(new Dictionary<string, List<string>>(), (acc, c) =>
                {

                    string firstLetter = c.Substring(0, 1);

                    if (acc.ContainsKey(firstLetter))
                    {
                        acc[firstLetter].Add(c);
                    }
                    else
                    {
                        acc[firstLetter] = new List<string>();
                        acc[firstLetter].Add(c);
                    }

                    return acc;
                });


                List<string> wordKeys = words.Keys.ToList();
                int i = 1;
                foreach (string key in wordKeys)
                {
                    Populate(key, i);
                    i++;
                }

                //typeof(Form1).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).ToList().ForEach(f => System.Diagnostics.Debug.WriteLine(f.Name));

                this.contentPanel.BringToFront();

            }
        }

        private void Populate(string key, int i)
        {
            Type type = typeof(Form1);

            FieldInfo richTextBoxField = type.GetField("richTextBox" + i, BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo labelField = type.GetField("label" + i, BindingFlags.NonPublic | BindingFlags.Instance);

            int numberOfWords = words[key].Count;
            int averageLengthOfWords = words[key].Aggregate(0, (acc, c) => acc += c.Length) / numberOfWords;
            string longestWord = words[key].Aggregate("", (acc, c) => c.Length > acc.Length ? acc = c : acc);
            string shortestWord = words[key].Aggregate(words[key][0], (acc, c) => c.Length < acc.Length ? acc = c : acc);
            int numberOfWordsEnd = allWords.Aggregate(0, (acc, c) => c.Substring(c.Length - 1, 1) == key ? acc += 1 : acc);

            string content = "Number of words: " + numberOfWords + "\n"
                + "Average Length: " + averageLengthOfWords + "\n"
                + "Longest Word: " + longestWord + "\n"
                + "Shortest Word: " + shortestWord + "\n"
                + "Words Ending With: " + numberOfWordsEnd;


            ((RichTextBox)richTextBoxField.GetValue(this)).Text = content;
            ((Label)labelField.GetValue(this)).Text = key.ToUpper();

        }

        private void HomeButton_Click(object sender, EventArgs e)
        {
            this.homePanel.BringToFront();
        }

    }
}
