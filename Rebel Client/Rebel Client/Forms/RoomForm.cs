using Rebel_Client.Properties;
using System;
using System.Windows.Forms;
public partial class RoomForm : MaterialSkin.Controls.MaterialForm
{
    public RoomForm()
    {
        InitializeComponent();
    }
    private void RoomForm_Load(object sender, EventArgs e)
    {
        Checker.Start();
        for (int i = 0; i < Screen.AllScreens.Length; i++)
        {
            comboBox1.Items.Add("Screen " + (i + 1).ToString());
        }
        foreach (string user in Utils.screenshareUsers)
        {
            listBox1.Items.Add(user);
        }
        if (Utils.inScreenShare)
        {
            materialRaisedButton3.Enabled = false;
            materialRaisedButton4.Enabled = true;
        }
        Settings.Default.Reload();
        try
        {
            if (comboBox1.Items[Settings.Default.CurrentScreen].ToString() == Settings.Default.CurrentScreenName)
            {
                comboBox1.SelectedIndex = Settings.Default.CurrentScreen;
            }
            else
            {
                comboBox1.SelectedIndex = 0;
            }
        }
        catch (System.Exception ex)
        {
            comboBox1.SelectedIndex = 0;
        }
    }
    private void Checker_Tick(object sender, EventArgs e)
    {
        if (!Utils.roomFormOpened)
        {
            Close();
        }
    }
    private void RoomForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        Settings.Default.Save();
        Utils.roomFormOpened = false;
    }
    private void materialRaisedButton3_Click(object sender, EventArgs e)
    {
        Utils.sendString("A");
        Utils.inScreenShare = true;
        materialRaisedButton3.Enabled = false;
        materialRaisedButton4.Enabled = true;
        Utils.screenShareThread = new System.Threading.Thread(new System.Threading.ThreadStart(Utils.screenShare));
        Utils.screenShareThread.Start();
    }
    private void materialRaisedButton4_Click(object sender, EventArgs e)
    {
        Utils.sendString("B");
        Utils.inScreenShare = false;
        materialRaisedButton4.Enabled = false;
        materialRaisedButton3.Enabled = true;
        pictureBox1.BackgroundImage = null;
        Utils.screenShareThread.Abort();
        Utils.screenShareThread = null;
        GC.Collect();
    }
    private void materialRaisedButton1_Click(object sender, EventArgs e)
    {
        if (listBox1.SelectedItem != null)
        {
            Utils.currentScreenshares.Add(listBox1.SelectedItem.ToString());
        }
        updateIt();
    }
    private void materialRaisedButton2_Click(object sender, EventArgs e)
    {
        if (listBox1.SelectedItem != null)
        {
            if (Utils.currentScreenshares.Contains(listBox1.SelectedItem.ToString()))
            {
                Utils.currentScreenshares.Remove(listBox1.SelectedItem.ToString());
            }
        }
        updateIt();
    }
    public void updateIt()
    {
        bool c = false;
        if (listBox1.SelectedItem != null)
        {
                if (listBox1.SelectedItem.ToString() != Utils.savedUsername)
                {
                    if (Utils.currentScreenshares.Contains(listBox1.SelectedItem.ToString()))
                    {
                        c = true;
                        Utils.sendString("C" + listBox1.SelectedItem.ToString());
                    }
                }
        }
        if (!c)
        {
            Utils.sendString("C");
        }
    }
    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (listBox1.SelectedItem != null)
        {
                if (listBox1.SelectedItem.ToString() != Utils.savedUsername)
                {
                    if (Utils.currentScreenshares.Contains(listBox1.SelectedItem.ToString()))
                    {
                        materialRaisedButton1.Enabled = true;
                        materialRaisedButton2.Enabled = false;
                    }
                    else
                    {
                        materialRaisedButton2.Enabled = true;
                        materialRaisedButton1.Enabled = false;
                    }
                }
                else
                {
                    materialRaisedButton1.Enabled = false;
                    materialRaisedButton2.Enabled = false;
                }
        }
        updateIt();
    }
    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Settings.Default.CurrentScreen = comboBox1.SelectedIndex;
        Settings.Default.CurrentScreenName = comboBox1.SelectedItem.ToString();
        Settings.Default.Save();
    }
}