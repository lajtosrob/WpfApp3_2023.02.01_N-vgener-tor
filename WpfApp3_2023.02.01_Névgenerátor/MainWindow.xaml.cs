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
        string fullName = null;
        int rndSurname;
        int rndForename;
        public MainWindow()
        {
            InitializeComponent();

            // sliGenerateName.Maximum = lbSurnameList.Items.Count;
            // lblMaxSli.Content = lbSurnameList.Items.Count;
        }
        private void btnSurnameLoad_Click(object sender, RoutedEventArgs e)
        {
            if (lbSurnameList.Items.Count == 0)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == true)
                {
                    string[] sorok = File.ReadAllLines(openFileDialog.FileName);
                    foreach (string sor in sorok)
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
                    foreach (string sor in sorok)
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
            int rndForename2;
            if (rdoOneForename.IsChecked == true)
            {
                for (int nameCount = 0; nameCount < ((int)sliGenerateName.Value); nameCount++)
                {
                    rndSurname = rnd.Next(lbSurnameList.Items.Count - 1);
                    rndForename = rnd.Next(lbForenameList.Items.Count - 1);
                    fullName = lbSurnameList.Items[rndSurname] + " " + Convert.ToString(lbForenameList.Items[rndForename]);
                    AddAndDeleteListItem();
                }
            }
            if (rdoTwoForename.IsChecked == true)
            {
                for (int nameCount2 = 0; nameCount2 < ((int)sliGenerateName.Value); nameCount2++)
                {
                    rndSurname = rnd.Next(lbSurnameList.Items.Count - 1);
                    rndForename = rnd.Next(lbForenameList.Items.Count - 1);
                    rndForename2 = rnd.Next(lbForenameList.Items.Count - 1);
                    fullName = lbSurnameList.Items[rndSurname] + " " + Convert.ToString(lbForenameList.Items[rndForename]) + " " + Convert.ToString(lbForenameList.Items[rndForename2]);
                    AddAndDeleteListItem();
                    if (rndForename < rndForename2)
                    {
                        lbForenameList.Items.RemoveAt(rndForename2 - 1);
                    }
                    else
                    {
                        lbForenameList.Items.RemoveAt(rndForename2);
                    }
                }
            }
            ResetCounters();
            stbOrderedList.Content = "";
            JumpToEndOfGenList();
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
            ResetCounters();
            JumpToEndOfNamesList();
        }
        private void sortNames_Click(object sender, RoutedEventArgs e)
        {
            lbgeneratedNames.Items.SortDescriptions.Add(
                new SortDescription("", ListSortDirection.Ascending));
            stbOrderedList.Content = "Rendezett lista";
        }
        private void saveGeneratedNames_Click(object sender, RoutedEventArgs e)
        {
            string[] names = new string[lbgeneratedNames.Items.Count];
            for (int rowIndex = 0; rowIndex < lbgeneratedNames.Items.Count; rowIndex++)
            {
                names[rowIndex] += Convert.ToString(lbgeneratedNames.Items[rowIndex]);
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.Filter = "Text file (*.txt)|*.txt|CSV file (*.csv)|*.csv|All Files (*.*)|*.*";
            saveFileDialog.Title = "Enter the name of the file!";
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
                try
                {
                    sliGenerateName.Value = Convert.ToInt32(txtGenerateNumber.Text);
                }
                catch (Exception)
                {
                    txtGenerateNumber.Text = "";
                    txtGenerateNumber.Focus();
                }
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
            ResetCounters();
            lbgeneratedNames.Items.RemoveAt(lbgeneratedNames.SelectedIndex);
            JumpToEndOfNamesList();

        }

        private void txtGenerateNumber_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                sliGenerateName.Value = Convert.ToInt32(txtGenerateNumber.Text);
            }
            catch (Exception)
            {
                txtGenerateNumber.Text = "";
                txtGenerateNumber.Focus();
            }
        }

        private void txtGenerateNumber_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                sliGenerateName.Value = Convert.ToInt32(txtGenerateNumber.Text);
            }
            catch (Exception)
            {
                txtGenerateNumber.Text = "";
                txtGenerateNumber.Focus();
            }
        }

        private void txtGenerateNumber_MouseLeave_1(object sender, MouseEventArgs e)
        {
            try
            {
                sliGenerateName.Value = Convert.ToInt32(txtGenerateNumber.Text);
            }
            catch (Exception)
            {
                txtGenerateNumber.Text = "";
                txtGenerateNumber.Focus();
            }
        }

        private void JumpToEndOfGenList()
        {
            lbgeneratedNames.Items.MoveCurrentToLast();
            lbgeneratedNames.ScrollIntoView(lbgeneratedNames.Items.CurrentItem);
        }

        private void JumpToEndOfNamesList()
        {
            lbSurnameList.Items.MoveCurrentToLast();
            lbSurnameList.ScrollIntoView(lbSurnameList.Items.CurrentItem);
            lbForenameList.Items.MoveCurrentToLast();
            lbForenameList.ScrollIntoView(lbForenameList.Items.CurrentItem);
        }

        private void AddAndDeleteListItem()
        {
            lbgeneratedNames.Items.Add(fullName);
            lbSurnameList.Items.RemoveAt(rndSurname);
            lbForenameList.Items.RemoveAt(rndForename);
        }
        private void ResetCounters()
        {
            tbNumberOfGeneratedNames.Text = lbgeneratedNames.Items.Count.ToString();
            lblSurname.Content = "Családnevek   " + lbSurnameList.Items.Count;
            lblForename.Content = "Utónevek   " + lbForenameList.Items.Count;
            sliGenerateName.Maximum = lbSurnameList.Items.Count;
            lblMaxSli.Content = lbSurnameList.Items.Count;
        }
    }
}

