using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcessKillerV1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshProcessList();
        }

        private void btnProcessKill_Click(object sender, EventArgs e)
        {
            string selectedProcessName = cmbProcesses.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedProcessName))
            {
                DialogResult result = MessageBox.Show($"Are you sure you want to terminate process '{selectedProcessName}'?",
                    "Confirm Kill Process", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    Process[] processes = Process.GetProcessesByName(selectedProcessName);

                    if (processes.Length > 0)
                    {
                        foreach (Process process in processes)
                        {
                            try
                            {
                                process.Kill();
                                
                            }
                            catch (Exception ex)
                            {
                                
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Process '{selectedProcessName}' is not currently running.", "Process Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a process to kill.", "No Process Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void RefreshProcessList()
        {
            cmbProcesses.Items.Clear();

            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                cmbProcesses.Items.Add(process.ProcessName);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshProcessList();
        }


        private void cmbProcesses_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayProcessDetails();
        }

        private void DisplayProcessDetails()
        {
            txtProcessInfo.Clear();

            string selectedProcessName = cmbProcesses.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedProcessName))
            {
                Process[] processes = Process.GetProcessesByName(selectedProcessName);

                if (processes.Length > 0)
                {
                    foreach (Process process in processes)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"Process Name: {process.ProcessName}");
                        sb.AppendLine($"Process ID: {process.Id}");
                        sb.AppendLine($"Memory Usage: {process.PrivateMemorySize64 / 1024} KB");
                        sb.AppendLine($"Path: {process.MainModule.FileName}");
                        sb.AppendLine($"Company Name: {GetFileProperty(process.MainModule.FileName, "CompanyName")}");
                        sb.AppendLine($"Priority Class: {process.PriorityClass}");

                        //sb.AppendLine($"CPU Time: {process.TotalProcessorTime}");
                        //sb.AppendLine($"Start Time: {process.StartTime}");

                        txtProcessInfo.Text = sb.ToString();
                    }
                }
            }
        }

        private string GetFileProperty(string filePath, string propertyName)
        {
            try
            {
                FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(filePath);

                return fileVersionInfo.CompanyName;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private void btnSearchOnline_Click(object sender, EventArgs e)
        {
            string selectedProcessName = cmbProcesses.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedProcessName))
            {
                string searchQuery = $"https://www.google.com/search?q={Uri.EscapeUriString(selectedProcessName)}";

                try
                {
                    Process.Start(new ProcessStartInfo(searchQuery));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to perform the online search: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a process to search online.", "No Process Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
