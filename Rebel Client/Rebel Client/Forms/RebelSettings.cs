using System;
using NAudio.Wave;
using Rebel_Client.Properties;
using System.Runtime.InteropServices;
using System.Windows.Forms;
public partial class RebelSettings : MaterialSkin.Controls.MaterialForm
{
    private bool insertKey;
    [DllImport("winmm.dll")]
    public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);

    [DllImport("winmm.dll")]
    public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);
    public RebelSettings()
    {
        InitializeComponent();
    }
    private void RebelSettings_Load(object sender, EventArgs e)
    {
        Utils.settingsOpened = true;
        Settings.Default.Reload();
        if (Settings.Default.PushToTalk)
        {
            radioButton4.Checked = true;
        }
        if (!Settings.Default.StereoAudio)
        {
            radioButton1.Checked = true;
        }
        numericUpDown1.Value = Settings.Default.FrequencyRate;
        numericUpDown2.Value = Settings.Default.AudioBits;
        comboBox1.Items.Add("Default");
        comboBox2.Items.Add("Default");
        for (int waveInDevice = 0; waveInDevice < WaveIn.DeviceCount; waveInDevice++)
        {
            comboBox1.Items.Add(WaveIn.GetCapabilities(waveInDevice).ProductName);
        }
        for (int waveOutDevice = 0; waveOutDevice < WaveOut.DeviceCount; waveOutDevice++)
        {
            comboBox2.Items.Add(WaveOut.GetCapabilities(waveOutDevice).ProductName);
        }
        try
        {
            if (comboBox1.Items[Settings.Default.InputDevice].ToString() == Settings.Default.InputDeviceName)
            {
                comboBox1.SelectedIndex = Settings.Default.InputDevice;
            }
            else
            {
                comboBox1.SelectedIndex = 0;
            }
        }
        catch (Exception ex) 
        {
            comboBox1.SelectedIndex = 0;
        }
        try
        {
            if (comboBox2.Items[Settings.Default.OutputDevice].ToString() == Settings.Default.OutputDeviceName)
            {
                comboBox2.SelectedIndex = Settings.Default.OutputDevice;
            }
            else
            {
                comboBox2.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            comboBox2.SelectedIndex = 0;
        }
        uint CurrVol = 0;
        waveOutGetVolume(IntPtr.Zero, out CurrVol);
        trackBar1.Value = (ushort)(CurrVol & 0x0000ffff) / (ushort.MaxValue / 100);
        materialLabel6.Text = "Application volume (" + trackBar1.Value.ToString() + "/100):";
        try
        {
            textBox1.Text = ((new KeysConverter()).ConvertTo(Settings.Default.PushToTalkKey, typeof(string))).ToString().Replace("None", ",");
        }
        catch (Exception ex)
        {
        }
    }
    private void RebelSettings_FormClosing(object sender, FormClosingEventArgs e)
    {
        Settings.Default.Save();
        Utils.settingsOpened = false;
    }
    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        Settings.Default.InputDevice = comboBox1.SelectedIndex;
        Settings.Default.InputDeviceName = comboBox1.Items[comboBox1.SelectedIndex].ToString();
    }
    private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
    {
        Settings.Default.OutputDevice = comboBox2.SelectedIndex;
        Settings.Default.OutputDeviceName = comboBox2.Items[comboBox2.SelectedIndex].ToString();
    }
    private void numericUpDown1_ValueChanged(object sender, EventArgs e)
    {
        Settings.Default.FrequencyRate = Convert.ToInt32(numericUpDown1.Value);
    }
    private void numericUpDown2_ValueChanged(object sender, EventArgs e)
    {
        Settings.Default.AudioBits = Convert.ToInt32(numericUpDown2.Value);
    }
    private void radioButton1_CheckedChanged(object sender, EventArgs e)
    {
        Settings.Default.StereoAudio = false;
    }
    private void radioButton2_CheckedChanged(object sender, EventArgs e)
    {
        Settings.Default.StereoAudio = true;
    }
    private void trackBar1_Scroll(object sender, EventArgs e)
    {
        int NewVolume = ((ushort.MaxValue / 100) * trackBar1.Value);
        waveOutSetVolume(IntPtr.Zero, ((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
        materialLabel6.Text = "Application volume (" + trackBar1.Value.ToString() + "/100):";
    }
    private void textBox1_MouseClick(object sender, MouseEventArgs e)
    {
        insertKey = true;
    }
    private void textBox1_KeyDown(object sender, KeyEventArgs e)
    {
        if (insertKey)
        {
            insertKey = false;
            textBox1.Text = ((new KeysConverter()).ConvertTo(e.KeyCode, typeof(string))).ToString().Replace("None", ",");
            Settings.Default.PushToTalkKey = e.KeyCode;
        }
    }
    private void radioButton3_CheckedChanged(object sender, EventArgs e)
    {
        Settings.Default.PushToTalk = false;
    }
    private void radioButton4_CheckedChanged(object sender, EventArgs e)
    {
        Settings.Default.PushToTalk = true;
    }
}