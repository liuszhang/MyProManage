using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows;

namespace MyProManage
{
    

    public enum ProStatus
    {
        售前,
        实施,
        归档,
        其他
    }


    public class MyTools
    {
        public static string RootPath = "D://00_ProjectFiles";
        public static List<Projects> Projects = new List<Projects>();
        public static List<Customer> Customers = new List<Customer>();
        


        //获取所有项目清单
        public static void GetProjects()
        {
            Projects.Clear();
            Customers.Clear();
            try
            {
                // 使用 Directory 类的 GetDirectories 方法获取文件夹清单
                string[] subDirectories = Directory.GetDirectories(RootPath);
                string[] tmp = { };

                if (subDirectories.Length > 0)
                {
                    //Console.WriteLine("子文件夹清单：");
                    foreach (string directory in subDirectories)
                    {
                        Console.WriteLine("now:"+directory);
                        if (IsProPath(directory)!=null)
                        {
                            Projects project= new Projects();
                            //project.Name = directory.Split('-').Reverse().First();
                            project.Name = "【"+ IsProPath(directory) + "】"+directory.Split('\\').Reverse().First();
                            project.Path = directory;
                            project.Customer= directory.Split('\\')[1].Split('-')[0];
                            project.Stutas = IsProPath(directory);

                            Projects.Add(project);


                            Console.WriteLine("客户名称：");
                            Console.WriteLine(directory.Split('\\').Reverse().First().Split('-')[0]);
                            //tmp.Append(directory.Split('\\')[1].Split('-')[0]);

                            Customer customer = new Customer();
                            customer.Name= directory.Split('\\').Reverse().First().Split('-')[0];
                            Customers.Add(customer);
                            //Customers.Distinct();
                        }
                        
                    }

                    Customers= new HashSet<Customer>(Customers).ToList();
                    Console.WriteLine(Customers.Count.ToString());

                }
                else
                {
                    Console.WriteLine("文件夹中没有子文件夹");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取文件夹清单失败：" + ex.Message);
            }


            //return new List<Projects>();
        }

        //判定是否为项目文件夹
        public static string IsProPath(string path)
        {
            if(File.Exists(Path.Combine(path,"[tag].售前"))) return "售前";
            else if(File.Exists(Path.Combine(path, "[tag].实施"))) return "实施";
            else if(File.Exists(Path.Combine(path, "[tag].归档"))) return "归档";
            else if(File.Exists(Path.Combine(path, "[tag].其他"))) return "其他";
            else return null;
        }



        //获取项目类型
        public static int GetProStatus(string proPath)
        {
            if (File.Exists(Path.Combine(proPath, "[tag].实施")))
            {
                return 1;
            }
            else if(File.Exists(Path.Combine(proPath, "[tag].售前")))
            {
                return 2;
            }
            else if (File.Exists(Path.Combine(proPath, "[tag].归档")))
            {
                return 3;
            }
            else if (File.Exists(Path.Combine(proPath, "[tag].其他")))
            {
                return 4;
            }
            else { return 0; }

        }


        //创建文件夹
        public static void CreateProPath(string cus,string dep,string status, string proName)
        {
            string folderPath;
            if (dep == "")
            {
                // 指定要创建的文件夹路径
                folderPath = Path.Combine(RootPath, cus + "-" + proName);
            }
            else
            {
                // 指定要创建的文件夹路径
                folderPath = Path.Combine(RootPath, cus + "-" + dep + "-" + proName);
            }

            if (Directory.Exists(folderPath))
            {
                MessageBox.Show("文件夹已经存在");
                return;
            }

            try
            {
                // 使用 Directory 类的 CreateDirectory 方法创建文件夹
                Directory.CreateDirectory(folderPath);
                //MessageBox.Show("文件夹创建成功");


                string statusFile=Path.Combine(folderPath, "[tag]."+status);
                // 使用 File 类的 Create 方法创建文件
                using (FileStream fs = File.Create(statusFile))
                {
                    MessageBox.Show("文件夹创建成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("文件夹创建失败：" + ex.Message);
            }
        }
    }

    



    public class SQLiteHelper
    {
        private string connectionString;

        public SQLiteHelper(string databasePath)
        {
            connectionString = $"Data Source={databasePath}";
        }

        public SQLiteHelper()
        {
        }

        public void ExecuteNonQuery(string query)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Dictionary<string, object>> ExecuteQuery(string query)
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string columnName = reader.GetName(i);
                                object value = reader.GetValue(i);

                                row.Add(columnName, value);
                            }

                            results.Add(row);
                        }
                    }
                }
            }

            return results;
        }

        public void UpdateData(string tableName, Dictionary<string, object> data, string whereClause)
        {
            string updateQuery = $"UPDATE {tableName} SET ";

            foreach (var entry in data)
            {
                updateQuery += $"{entry.Key} = @{entry.Key}, ";
            }

            updateQuery = updateQuery.TrimEnd(',', ' ');

            if (!string.IsNullOrEmpty(whereClause))
            {
                updateQuery += $" WHERE {whereClause}";
            }

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                {
                    foreach (var entry in data)
                    {
                        command.Parameters.AddWithValue($"@{entry.Key}", entry.Value);
                    }

                    command.ExecuteNonQuery();
                }
            }
        }




        public void CreateTable(string tableName, string[] columns)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string createTableQuery = $"CREATE TABLE IF NOT EXISTS {tableName} ({string.Join(", ", columns)})";
                using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertData(string tableName, string[] columns, object[] values)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string insertQuery = $"INSERT INTO {tableName} ({string.Join(", ", columns)}) VALUES ({GetFormattedValues(values)})";
                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                {
                    AddParameters(command, values);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void UpdateData(string tableName, string[] columns, object[] values, string condition)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string updateQuery = $"UPDATE {tableName} SET {GetFormattedUpdateValues(columns)} WHERE {condition}";
                using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
                {
                    AddParameters(command, values);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteData(string tableName, string condition)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string deleteQuery = $"DELETE FROM {tableName} WHERE {condition}";
                using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private string GetFormattedValues(object[] values)
        {
            return string.Join(", ", values.Select((value, index) => $"@param{index}"));
        }

        private string GetFormattedUpdateValues(string[] columns)
        {
            return string.Join(", ", columns.Select((column, index) => $"{column} = @param{index}"));
        }

        private void AddParameters(SQLiteCommand command, object[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                command.Parameters.AddWithValue($"@param{i}", values[i]);
            }
        }

    }

}
