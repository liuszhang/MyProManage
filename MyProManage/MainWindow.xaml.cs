using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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

namespace MyProManage
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public List<Projects> Projects { get; set; }
        public List<Projects> NewProjects = new List<Projects>();
        public List<Customer> Customers { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            //YourDataCollection=new List<Projects>();

            //string connectionString = "Data Source=mypro.db";
            //SQLiteConnection connection = new SQLiteConnection(connectionString);

            //connection.Open();

            //string createTableQuery = "CREATE TABLE IF NOT EXISTS Customers (Id INTEGER PRIMARY KEY, Name TEXT)";
            //string createTableQuery2 = "CREATE TABLE IF NOT EXISTS Projects (ID INTEGER PRIMARY KEY, Name TEXT, Customer TEXT, stutas TEXT, path TEXT)";
            //SQLiteCommand command = new SQLiteCommand(createTableQuery, connection);
            //command.ExecuteNonQuery();
            //command = new SQLiteCommand(createTableQuery2, connection);
            //command.ExecuteNonQuery();



            //SQLiteDatabaseHelper dbHelper = new SQLiteDatabaseHelper("mypro.db");
            ////dbHelper.CreateTable(tableName, columns);

            //string tableName = "Projects";
            //string[] columns = { "Name" ,"path"};
            //object[] values = { "testpro3" , "D://00_ProjectFiles" };

            //dbHelper.InsertData(tableName, columns, values);


            //var results=new SQLiteHelper("mypro.db").ExecuteQuery("SELECT * FROM Projects");
            //foreach (var dictionary in results)
            //{
            //    Projects myClass = new Projects();
            //    int id;
            //    int.TryParse(dictionary["ID"].ToString(),out id);
            //    myClass.ID = id;


            //    if (dictionary.ContainsKey("Name") && dictionary["Name"] != null)
            //    {
            //        myClass.Name = dictionary["Name"].ToString();
            //        //MessageBox.Show(dictionary["Name"].ToString());
            //    }
            //    if (dictionary.ContainsKey("Path") && dictionary["Path"] != null)
            //    {                    
            //        myClass.Path = dictionary["Path"].ToString();                    
            //    }
            //    if (dictionary.ContainsKey("Customer") && dictionary["Customer"] != null)
            //    {
            //        myClass.Customer = dictionary["Customer"].ToString();
            //    }
            //    if (dictionary.ContainsKey("Stutas") && dictionary["Stutas"] != null)
            //    {
            //        myClass.Path = dictionary["Stutas"].ToString();
            //    }
            //    // 其他属性的映射...

            //    YourDataCollection.Add(myClass);
            //}

            //YourDataCollection = MyTools.GetProjects();
            MyTools.GetProjects();
            //List<Customer> listC = new List<Customer>();
            //listC.Add(new Customer() { Id = 1, Name = "test3" });
            //listC.Add(new Customer() { Id = 2, Name = "test4" });
            //this.listCus.ItemsSource = listC;
            //this.listPro.ItemsSource = YourDataCollection;
            Projects= MyTools.Projects;
            Customers= MyTools.Customers;
            this.listCus.ItemsSource = Customers;
            this.listPro.ItemsSource = Projects;





        }

        private void listPro_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // 首先获取双击的ListBoxItem
            Projects item = (Projects)listPro.SelectedItem;

            // 根据需要进行处理，例如输出被双击的项的内容
            //MessageBox.Show(item.Name.ToString());
            var folderPath = item.Path.ToString();
            if (Directory.Exists(folderPath))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
                Process.Start(new ProcessStartInfo()
                {
                    FileName = directoryInfo.FullName,
                    UseShellExecute = true,
                    Verb = "open"
                });
            }
            else
            {
                Console.WriteLine("指定路径的文件夹不存在。");
            }
        }

        private void listCus_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            

        }

        private void listCus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            NewProjects = new List<Projects>();
            RefreshList();
            if (listCus.SelectedItems.Count == 0)
            {
                RefreshList();
                return;
            }

            foreach( var item in listCus.SelectedItems)
            {
                Customer item1 = (Customer)item;
                Console.WriteLine(item1.Name.ToString());
                
                
                foreach (Projects pro in Projects)
                {
                    Console.WriteLine(pro.Customer);
                    if (pro.Customer == item1.Name)
                    {
                        
                        NewProjects.Add(pro);
                    }
                }
                
            }
            this.listPro.ItemsSource = null;

            listPro.Items.Clear();

            this.listPro.ItemsSource = NewProjects;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new NewProject().ShowDialog();
            MyTools.GetProjects();
            
            listCus.ItemsSource = null;
            this.listPro.ItemsSource = null;
            listCus.Items.Clear();
            listPro.Items.Clear();
            Projects = MyTools.Projects;
            Customers = MyTools.Customers;
            this.listCus.ItemsSource = Customers;
            this.listPro.ItemsSource = Projects;
        }





        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            MyTools.RootPath=rootPath.Text.Trim();
            MyTools.GetProjects();
            listCus.ItemsSource = null;
            listPro.ItemsSource = null;
            listCus.Items.Clear();
            listPro.Items.Clear();
            Projects = MyTools.Projects;
            Customers = MyTools.Customers;
            this.listCus.ItemsSource = Customers;
            this.listPro.ItemsSource = Projects;

        }

        public void RefreshList()
        {
            Projects = new List<Projects>();
            
            if (isSS.IsChecked == true)
            {
                foreach (Projects pro in MyTools.Projects)
                {
                    //Console.WriteLine(pro.Stutas);
                    if (pro.Stutas == "实施")
                    {
                        Projects.Add(pro);
                    }
                }
            }
            //else if(isSS.IsChecked == false)
            //{

            //}
            if (isSQ.IsChecked == true)
            {
                foreach (Projects pro in MyTools.Projects)
                {
                    //Console.WriteLine(pro.Stutas);
                    if (pro.Stutas == "售前")
                    {
                        Projects.Add(pro);
                    }
                }
            }
            //else if (isSQ.IsChecked == false)
            //{

            //}
            if (isGD.IsChecked == true)
            {
                foreach (Projects pro in MyTools.Projects)
                {
                    //Console.WriteLine(pro.Stutas);
                    if (pro.Stutas == "归档")
                    {
                        Projects.Add(pro);
                    }
                }
            }
            //else if (!isSQ.IsChecked == false)
            //{

            //}
            if (isQT.IsChecked == true)
            {
                foreach (Projects pro in MyTools.Projects)
                {
                    //Console.WriteLine(pro.Stutas);
                    if (pro.Stutas == "其他")
                    {
                        Projects.Add(pro);
                    }
                }
            }
            //else if(!isSQ.IsChecked == false)
            //{

            //}


            this.listPro.ItemsSource = null;

            listPro.Items.Clear();

            this.listPro.ItemsSource = Projects;
        }

        private void isSS_Unchecked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("取消实施");
           
            //RefreshList();
        }

        private void isSS_Checked(object sender, RoutedEventArgs e)
        {
            //RefreshList();
        }

        private void isSQ_Checked(object sender, RoutedEventArgs e)
        {
            //RefreshList();
        }

        private void isSQ_Click(object sender, RoutedEventArgs e)
        {
            RefreshList();
        }

        private void isSS_Click(object sender, RoutedEventArgs e)
        {
            RefreshList();
        }

        private void isQT_Click(object sender, RoutedEventArgs e)
        {
            RefreshList();
        }

        private void isGD_Click(object sender, RoutedEventArgs e)
        {
            RefreshList();
        }
    }
}
