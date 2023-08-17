

using System.Diagnostics;
using System.IO;
using System.Resources;
using System.Windows.Forms;

namespace NetworkDrive
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            listView1.View = View.Details;
            listView1.Columns[0].Width = 100;
            listView1.Columns[1].Width = 100;

            listView1.FullRowSelect = true;
            List<DriveInfo> driveInfoList = NetworkDrive.GetNetworkDrives();
            foreach (DriveInfo drive in driveInfoList)
            {
                ListViewItem item = new ListViewItem();
                item.Text = drive.Name;
                item.SubItems.Add(drive.VolumeLabel);
                fileSystemWatcher1.Path = drive.Name;
                listView1.Items.Add(item);
                //   Debug.WriteLine(drive);
            }
            //fileSystemWatcher1.NotifyFilter = NotifyFilters.;
            //fileSystemWatcher1.EnableRaisingEvents = true;






        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            Debug.WriteLine(NetworkDrive.MapNetworkDrive("R", @"\\XOXO\mir1"));
            
            //var dirs1 = Directory.GetDirectories("R:");
            
            //NetworkDrive.DisconnectNetworkDrive("R", true);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Debug.WriteLine(NetworkDrive.DisconnectNetworkDrive("R", true));
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            //if the form is minimized
            //hide it from the task bar
            //and show the system tray icon (represented by the NotifyIcon control)
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                notifyIcon1.Visible = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void FSChanged(object sender, FileSystemEventArgs e)
        {
            MessageBox.Show(e.ChangeType.ToString());
        }
    }
}
