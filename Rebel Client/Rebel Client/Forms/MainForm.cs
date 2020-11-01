using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Net;
using Rebel_Client.Properties;
public partial class MainForm : MaterialSkin.Controls.MaterialForm
{
    public MainForm()
    {
        InitializeComponent();
    }
    private void MainForm_Load(object sender, EventArgs e)
    {
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
        Utils.serverPage = null;
        if (Utils.disconnectReason != "")
        {
            materialLabel5.Text = "Status: " + Utils.disconnectReason;
            Utils.disconnectReason = "";
        }
        Utils.loadAll();
        Settings.Default.Reload();
        materialSingleLineTextField1.Text = Settings.Default.ServerIP;
        materialSingleLineTextField2.Text = Settings.Default.ServerPort;
        materialSingleLineTextField3.Text = Settings.Default.Username;
        if (Settings.Default.PushToTalkKey == 0)
        {
            Settings.Default.PushToTalkKey = Keys.F1;
            Settings.Default.Save();
        }
        Utils.joined = false;
        materialSingleLineTextField1.KeyDown += new KeyEventHandler(automaticAccess);
        materialSingleLineTextField2.KeyDown += new KeyEventHandler(automaticAccess);
        materialSingleLineTextField3.KeyDown += new KeyEventHandler(automaticAccess);
    }
    private void materialRaisedButton1_Click(object sender, EventArgs e)
    {
        materialLabel5.Text = "Status: Connecting to server...";
        Settings.Default.ServerIP = materialSingleLineTextField1.Text;
        Settings.Default.ServerPort = materialSingleLineTextField2.Text;
        Settings.Default.Username = materialSingleLineTextField3.Text;
        Settings.Default.Save();
        string filtered = Utils.filterString(materialSingleLineTextField3.Text);
        if (filtered == "")
        {
            goto no_connect;
        }
        string ipString = "";
        if (Utils.ValidateIPv4(materialSingleLineTextField1.Text))
        {
            ipString = materialSingleLineTextField1.Text;
        }
        else
        {
            try
            {
                ipString = Dns.GetHostAddresses(materialSingleLineTextField1.Text)[0].ToString();
            }
            catch (Exception ex)
            {
                goto no_connect;
            }
        }
        try
        {
            if (!Utils.connect(ipString, int.Parse(materialSingleLineTextField2.Text), filtered, "127.0.0.1", 53))
            {
                goto no_connect;
            }
            else
            {
                Utils.sendString("9");
                Utils.serverPage = new ServerPage();
                Utils.serverPage.Show();
                materialLabel5.Text = "Status: Please, connect to a server.";
                Hide();
                Utils.sendString("9");
                return;
            }
        }
        catch (Exception ex)
        {
            goto no_connect;
        }
        no_connect: materialLabel5.Text = "Status: Failed to connect to server.";
        Utils.playAudioFile("failed_to_connect_to_server");
        return;
    }
    private void automaticAccess(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            materialRaisedButton1.PerformClick();
        }
    }
    private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        Process.GetCurrentProcess().Kill();
    }
}