using System.Collections;

namespace Wordle
{
    public partial class Form1 : Form
    {
        StreamReader sr1 = new StreamReader("dictionary.txt");
        StreamReader sr2 = new StreamReader("allowed.txt");
        ArrayList ar1 = new ArrayList();
        ArrayList ar2 = new ArrayList();
        Label[] labels;
        int pos = 0, count = 0;
        string word;

        public Form1()
        {
            while (sr1.Peek() != -1)
                ar1.Add(sr1.ReadLine().ToUpper());
            sr1.Close();

            while (sr2.Peek() != -1)
                ar2.Add(sr2.ReadLine().ToUpper());
            sr2.Close();

            InitializeComponent();
            this.KeyPreview = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Random random = new Random();
            word = ar1[random.Next(ar1.Count)].ToString();
            labels = new Label[25];
            for (int i = 0; i < labels.Length; i++)
            {
                labels[i] = new Label();
                labels[i].Text = "";
                labels[i].Width = 50;
                labels[i].Height = 50;
                labels[i].TextAlign = ContentAlignment.MiddleCenter;
                labels[i].Font = new Font("Arial", 14, FontStyle.Bold);
                labels[i].ForeColor = Color.White;
                labels[i].BackColor = Color.FromArgb(31, 32, 34);
            }
            int p = 0;
            for(int i = 0; i < 5; i++)
            {
                for(int j=0; j < 5; j++)
                {
                    labels[p].Left = 75*j + 35;
                    labels[p].Top = 75*i + 25;
                    p++;
                }
            }
            for (int i = 0; i < labels.Length; i++)
                this.Controls.Add(labels[i]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(count == 5)
            {
                string guess = "";
                for (int i = 5; i > 0; i--)
                    guess += labels[pos - i].Text;
                bool Found, InPlace;

                if (ar2.Contains(guess))
                {
                    count = 0;
                    for (int i = 0; i < 5; i++)
                    {
                        Found = false;
                        InPlace = false;
                        if (guess[i] == word[i])
                            InPlace = true;
                        else
                        {
                            for (int j = 0; j < 5; j++)
                                if (guess[i] == word[j])
                                {   Found = true;
                                    if (NbOccurences(guess, guess[i]) > NbOccurences(word, guess[i]) && i != guess.IndexOf(guess[i]))
                                        Found = false;
                                }
                        }
                        if (InPlace)
                        {
                            labels[pos - 5 + i].BackColor = Color.YellowGreen;
                        }
                        if (Found)
                            labels[pos - 5 + i].BackColor = Color.DarkGoldenrod;
                        if (!InPlace && !Found)
                            labels[pos - 5 + i].BackColor = Color.Gray;
                    }

                    if (guess == word.ToUpper())
                    {
                        MessageBox.Show("You won!");
                        this.Close();
                    }
                    if (pos == 25)
                    {
                        MessageBox.Show("Game over. The word was: " + word);
                        this.Close();
                    }
                }
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (count < 5)
            {
                if (Char.IsLetter(e.KeyChar) && pos < 25 && pos >= 0)
                {
                    labels[pos].Text = Char.ToUpper(e.KeyChar).ToString();
                    pos++;
                    count++;
                }
            }
            if (count > 0 && count <= 5)
            {
                if (e.KeyChar == '\b' && pos >= 1)
                {
                    labels[pos - 1].Text = "";
                    pos--;
                    count--;
                }
            }
        }

        private static int NbOccurences(string s,char c) {
            int NbOccurences = 0;
            foreach (char ch in s)
                if (ch == c)
                    NbOccurences++;
            return NbOccurences;
        }   
    }
}