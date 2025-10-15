using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToDoList
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string saveTasks = "ToDoList.txt";

        private void Form1_Load(object sender, EventArgs e)
        {
            toDoListView.Columns.Add("Задача", "Задача");
            toDoListView.Columns.Add("Описание", "Описание");
            toDoListView.Columns.Add("Время", "Время добавления");

            DataGridViewCheckBoxColumn checkCol = new DataGridViewCheckBoxColumn();
            checkCol.HeaderText = "Состояние";
            toDoListView.Columns.Add(checkCol);

            LoadData();
            this.FormClosing += (s, args) => SaveData();
        }

        private void deletebutton_Click(object sender, EventArgs e)
        {
            toDoListView.Rows.Remove(toDoListView.CurrentRow);
            SaveData();
        }

        private void savebutton_Click(object sender, EventArgs e)
        {
            toDoListView.Rows.Add(titleTextBox.Text, descriptTextBox.Text, DateTime.Now, false);
            titleTextBox.Text = "";
            descriptTextBox.Text = "";
            SaveData();
        }

        private void SaveData()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(saveTasks))
                {
                    foreach (DataGridViewRow row in toDoListView.Rows)
                    {
                        if (row.IsNewRow) continue; //пустые строки убирает

                        string task = row.Cells[0].Value?.ToString() ?? ""; //если стоят пустые данные 
                        string desc = row.Cells[1].Value?.ToString() ?? "";
                        string time = row.Cells[2].Value?.ToString() ?? "";
                        string state = (row.Cells[3].Value?.ToString() ?? "False");

                        writer.WriteLine($"{task}|{desc}|{time}|{state}");
                    }
                }
            }
            catch (Exception) { }
        }

        private void LoadData()
        {
            try
            {
                foreach (string line in File.ReadAllLines(saveTasks)) //читает данные из файла
                {
                    string[] parts = line.Split('|');
                    toDoListView.Rows.Add(parts[0], parts[1], parts[2], bool.Parse(parts[3]));
                }
            }
            catch (Exception) { }
        }
    }
}