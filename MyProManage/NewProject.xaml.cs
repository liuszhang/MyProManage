using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace MyProManage
{
    /// <summary>
    /// NewProject.xaml 的交互逻辑
    /// </summary>
    public partial class NewProject : Window
    {
        public NewProject()
        {
            InitializeComponent();

            //加载客户列表
            cusBox.ItemsSource = MyTools.Customers;

            //加载部门列表

            //加载状态列表
            status.ItemsSource= Enum.GetNames(typeof(ProStatus));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //确认是否新建客户


            //确认是否新建部门


            //创建文件夹
            MyTools.CreateProPath(cusBox.Text.Trim(), depBox.Text.Trim(),status.Text.Trim(),nameBox.Text.Trim());


            //文件夹标签属性


            //写入数据库



            this.Close();
        }
    }
}
