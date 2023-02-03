using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3_2023._02._01_Névgenerátor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int surnameCount = 0;
        public MainWindow()
        {
            InitializeComponent();
            sliGenerateName.Maximum = lbSurnameList.Items.Count;
            lblMaxSli.Content = lbSurnameList.Items.Count;
        }
        private void btnSurnameLoad_Click(object sender, RoutedEventArgs e)
        {
            if (lbSurnameList.Items.Count == 0)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if(openFileDialog.ShowDialog() == true)
                {
                    string[] sorok = File.ReadAllLines(openFileDialog.FileName); 
                    foreach(string sor in sorok) 
                    {
                        lbSurnameList.Items.Add(sor);
                    }
                }

                /* StreamReader sr = new StreamReader("Családnevek.txt");
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    lbSurnameList.Items.Add(line);
                    surnameCount++;
                }
                sr.Close(); */

                lblSurname.Content = "Családnevek   " + lbSurnameList.Items.Count;
                sliGenerateName.Maximum = lbSurnameList.Items.Count;
                lblMaxSli.Content = lbSurnameList.Items.Count;
            }
        }
        private void btnForenameLoad_Click(object sender, RoutedEventArgs e)
        {
            if (lbForenameList.Items.Count < 200)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    string[] sorok = File.ReadAllLines(openFileDialog.FileName);
                    foreach(string sor in sorok)
                    {
                        lbForenameList.Items.Add(sor);
                    }
                    
                }
                /* int forenameCount = 0;
                StreamReader sr = new StreamReader("Utónevek.txt");
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    lbForenameList.Items.Add(line);
                    forenameCount++;
                } 
                sr.Close(); */

                lblForename.Content = "Utónevek   " + lbForenameList.Items.Count;
                sliGenerateName.Maximum = lbSurnameList.Items.Count;
                lblMaxSli.Content = lbSurnameList.Items.Count;
            }
        }
        private void sliGenerateName_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            txtGenerateNumber.Text = ((int)sliGenerateName.Value).ToString();
        }
        private void generateName_Click(object sender, RoutedEventArgs e)
        {
            Random rnd = new Random();
            string fullName = null;
            int rndSurname;
            int rndForename;
            int rndForename2;
            if (rdoOneForename.IsChecked == true)
            {
                for (int i = 0; i < sliGenerateName.Value; i++)
                {
                    rndSurname = rnd.Next(lbSurnameList.Items.Count - 1);
                    rndForename = rnd.Next(lbForenameList.Items.Count - 1);
                    fullName = lbSurnameList.Items[rndSurname] + " " + Convert.ToString(lbForenameList.Items[rndForename]);
                    lbgeneratedNames.Items.Add(fullName);
                    lbSurnameList.Items.RemoveAt(rndSurname);
                    lbForenameList.Items.RemoveAt(rndForename);
                }
            }
            if (rdoTwoForename.IsChecked == true)
            {
                for (int i = 0; i < sliGenerateName.Value; i++)
                {
                    rndSurname = rnd.Next(lbSurnameList.Items.Count - 1);
                    rndForename = rnd.Next(lbForenameList.Items.Count - 1);
                    rndForename2 = rnd.Next(lbForenameList.Items.Count - 1);
                    fullName = lbSurnameList.Items[rndSurname] + " " + Convert.ToString(lbForenameList.Items[rndForename]) + " " + Convert.ToString(lbForenameList.Items[rndForename2]);
                    lbgeneratedNames.Items.Add(fullName);
                    lbSurnameList.Items.RemoveAt(rndSurname);
                    lbForenameList.Items.RemoveAt(rndForename);
                    lbForenameList.Items.RemoveAt(rndForename2);
                }
            }
            lblNumberGeneratedNames.Content = lbgeneratedNames.Items.Count;
            lblSurname.Content = "Családnevek   " + lbSurnameList.Items.Count;
            lblForename.Content = "Utónevek   " + lbForenameList.Items.Count;
            sliGenerateName.Maximum = lbSurnameList.Items.Count;
            lblMaxSli.Content = lbSurnameList.Items.Count;
        }
        private void deleteGeneratedNames_Click(object sender, RoutedEventArgs e)
        {
            List<string> generatedNamesList = new List<string>();

            for (int item = 0; item < lbgeneratedNames.Items.Count; item++)
            {
                string[] splittedName = Convert.ToString(lbgeneratedNames.Items[item]).Split(' ');
                for (int itemIndex = 0; itemIndex < splittedName.Length; itemIndex++)
                {
                    if (itemIndex == 0)
                    {
                        lbSurnameList.Items.Add(splittedName[itemIndex]);
                    }
                    else
                    {
                        lbForenameList.Items.Add(splittedName[itemIndex]);
                    }
                }
            }
            lbgeneratedNames.Items.Clear();
            lblSurname.Content = "Családnevek   " + lbSurnameList.Items.Count;
            lblForename.Content = " Utónevek   " + lbForenameList.Items.Count;
            lblNumberGeneratedNames.Content = lbgeneratedNames.Items.Count;
            sliGenerateName.Maximum = lbSurnameList.Items.Count;
            lblMaxSli.Content = lbSurnameList.Items.Count;
        }
        private void sortNames_Click(object sender, RoutedEventArgs e)
        {
            lbgeneratedNames.Items.SortDescriptions.Add(
                new SortDescription("", ListSortDirection.Ascending));
        }
        private void saveGeneratedNames_Click(object sender, RoutedEventArgs e)
        {
            string[] names = new string[lbgeneratedNames.Items.Count];
            for (int rowIndex = 0; rowIndex < lbgeneratedNames.Items.Count; rowIndex++)
            {
                names[rowIndex] += Convert.ToString(lbgeneratedNames.Items[rowIndex]);
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.txt)|*.txt|CSV file (*.csv)|*.csv|All Files (*.*)|*.*";
            saveFileDialog.InitialDirectory = @"C:\Users\LifebookE736\source\repos\WpfApp3_2023.02.01_Névgenerátor\WpfApp3_2023.02.01_Névgenerátor\bin\Debug\net6.0-windows";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllLines(saveFileDialog.FileName, names);
                MessageBox.Show("Successful file save!");
            }
        }

        private void txtGenerateNumber_TextInput(object sender, TextCompositionEventArgs e)
        {
            sliGenerateName.Value = Convert.ToInt32(txtGenerateNumber.Text);
        }
        private void txtGenerateNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                sliGenerateName.Value = Convert.ToInt32(txtGenerateNumber.Text);
            }
        }
        private void lbgeneratedNames_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string selectedItem = Convert.ToString(lbgeneratedNames.SelectedItem);
            string[] selectedItemList = selectedItem.Split(' ');
            for (int item = 0; item < selectedItemList.Length; item++)
            {
                if (item == 0)
                {
                    lbSurnameList.Items.Add(selectedItemList[item]);
                }
                else
                {
                    lbForenameList.Items.Add(selectedItemList[item]);
                }
            }
            lbgeneratedNames.Items.RemoveAt(lbgeneratedNames.SelectedIndex);
            lblSurname.Content = "Családnevek   " + lbSurnameList.Items.Count;
            lblForename.Content = "Utónevek   " + lbForenameList.Items.Count;
            lblNumberGeneratedNames.Content = lbgeneratedNames.Items.Count;
        }

        private void txtGenerateNumber_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            sliGenerateName.Value = Convert.ToInt32(txtGenerateNumber.Text);
        }

        private void txtGenerateNumber_MouseLeave(object sender, MouseEventArgs e)
        {
            sliGenerateName.Value = Convert.ToInt32(txtGenerateNumber.Text);
        }

        private void txtGenerateNumber_MouseLeave_1(object sender, MouseEventArgs e)
        {
            sliGenerateName.Value = Convert.ToInt32(txtGenerateNumber.Text);
        }
    }
}

