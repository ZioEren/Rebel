using Rebel_Client.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using NAudio.Wave;
using System.Runtime.InteropServices;
public partial class ServerPage : MaterialSkin.Controls.MaterialForm
{
    private Thread childThread;
    private bool disconnect, lastMute, canSend = true;
    [DllImport("User32.dll")]
    public static extern short GetAsyncKeyState(Keys ArrowKeys);
    public ServerPage()
    {
        InitializeComponent();
    }
    private void ServerPage_FormClosing(object sender, FormClosingEventArgs e)
    {
        close();
    }
    private void ServerPage_Load(object sender, EventArgs e)
    {
        Utils.sendString("9");
        childThread = new Thread(new ThreadStart(Utils.receiveAll));
        childThread.Start();
        Settings.Default.Reload();
        updateAudioStatus();
        joinConfirm.Start();
        keepAlive.Start();
        keepAliveSend.Start();
        Utils.sendString("9");
    }
    void waveIn_DataAvailable(object sender, WaveInEventArgs e)
    {
        if (Settings.Default.PushToTalk) if (BitConverter.GetBytes(GetAsyncKeyState(Settings.Default.PushToTalkKey))[1] != 0x80) return;
        if (Utils.waveIn != null)
        {
            if (Utils.waveProvider != null)
            {
                if (!Settings.Default.MicMuted && !Settings.Default.AudioMuted && Utils.currentChannel != "" && Utils.currentChannel != null)
                {
                    Utils.sendString("8" + Convert.ToBase64String(Utils.voiceCompression ? Utils.Compress(e.Buffer) : e.Buffer));
                }
            }    
        }
    }
    private void disconnectFromServerToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        disconnectAll();
    }
    private void quitFromAppToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        close();
    }
    public void close()
    {
        Utils.playAudioFile("server_disconnect");
        Utils.sendString("2");
        Utils.disconnect();
        childThread.Abort();
        if (!disconnect) Process.GetCurrentProcess().Kill();
    }
    private void toolStripButton1_Click(object sender, EventArgs e)
    {
        Settings.Default.MicMuted = !Settings.Default.MicMuted;
        lastMute = Settings.Default.MicMuted;
        if (Settings.Default.AudioMuted)
        {
            if (!lastMute)
            {
                Settings.Default.AudioMuted = false;
                Settings.Default.MicMuted = false;
            }
        }
        updateAudioStatus();
    }
    private void toolStripButton2_Click(object sender, EventArgs e)
    {
        Settings.Default.AudioMuted = !Settings.Default.AudioMuted;
        if (!lastMute) Settings.Default.MicMuted = false;
        if (Settings.Default.AudioMuted) Settings.Default.MicMuted = true;
        if (lastMute && !Settings.Default.AudioMuted)
        {
            Settings.Default.MicMuted = lastMute;
            Settings.Default.AudioMuted = false;
            Settings.Default.MicMuted = true;
        }
        updateAudioStatus();
    }
    public void updateAudioStatus()
    {
        if (Settings.Default.MicMuted) toolStripButton1.BackColor = Color.DarkGray;
        else toolStripButton1.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        if (Settings.Default.AudioMuted) toolStripButton2.BackColor = Color.DarkGray;
        else toolStripButton2.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        if (Settings.Default.HearVoice) toolStripButton4.BackColor = Color.DarkGray;
        else toolStripButton4.BackColor = ColorTranslator.FromHtml("#F0F0F0");
        Settings.Default.Save();
        Utils.sendString("3" + (Settings.Default.MicMuted ? "1" : "0") + (Settings.Default.AudioMuted ? "1" : "0") + (Settings.Default.HearVoice ? "1" : "0"));
    }
    private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        if (!(listBox1.SelectedIndex < 0))
        {
            if (listBox1.SelectedItem != null && listBox1.SelectedItem.ToString() != "")
            {
                bool can = true;
                if (Utils.currentChannel != "" && Utils.currentChannel != null)
                {
                    if (listBox1.SelectedItem.ToString().EndsWith(Utils.currentChannel))
                    {
                        can = false;
                        if (!Utils.roomFormOpened)
                        {
                            Utils.roomFormOpened = true;
                            RoomForm roomForm = new RoomForm();
                            roomForm.Text = "Rebel Client - " + Utils.currentChannel;
                            Utils.roomForm = roomForm;
                            roomForm.Show();
                        }
                    }
                }
                if (can) Utils.sendString("5" + listBox1.SelectedItem.ToString().Substring(1));
            }
        }
    }
    private void toolStripButton3_Click(object sender, EventArgs e)
    {
        if (Utils.currentChannel != "") Utils.sendString("6");
    }
    private void exitFromCurrentChannelToolStripMenuItem_Click(object sender, EventArgs e)
    {
        toolStripButton3.PerformClick();
    }
    private void joinConfirm_Tick(object sender, EventArgs e)
    {
        joinConfirm.Stop();
        Utils.sendString("9");
        if (!Utils.joined)
        {
            Utils.disconnectReason = "Port is opened but failed to connect.";
            disconnectAll();
        }
        if (Utils.joined)
        {
            Utils.sendString("9");
            Utils.playAudioFile("server_join");
            if (Settings.Default.InputDevice == 0)
            {
                Utils.waveIn = new WaveIn(Handle);
            }
            else
            {
                Utils.waveIn = new WaveIn();
                Utils.waveIn.DeviceNumber = Settings.Default.InputDevice - 1;
            }
            Utils.waveIn.WaveFormat = new WaveFormat(Utils.forcedFrequency ? Utils.forcedFrequencyValue : Settings.Default.FrequencyRate, Utils.forcedBits ? Utils.forcedBitsValue : Settings.Default.AudioBits, Utils.forcedChannels ? Utils.forcedChannelsValue : (Settings.Default.StereoAudio ? 2 : 1));
            Utils.waveIn.BufferMilliseconds = 25;
            Utils.waveIn.DataAvailable += waveIn_DataAvailable;
            Utils.waveProvider = new BufferedWaveProvider(Utils.waveIn.WaveFormat);
            Utils.waveProvider.DiscardOnBufferOverflow = true;
            Utils.waveOut = new WaveOut();
            if (Settings.Default.OutputDevice != 0) Utils.waveOut.DeviceNumber = Settings.Default.OutputDevice - 1;
            Utils.waveOut.DesiredLatency = 100;
            Utils.waveOut.Init(Utils.waveProvider);
            Utils.waveIn.StartRecording();
            Utils.waveOut.Play();
            ClearBuffer.Start();
            Utils.sendString("9");
        }
        Utils.sendString("9");
    }
    private void antiSpam_Tick(object sender, EventArgs e)
    {
        canSend = true;
        antiSpam.Stop();
    }
    private void muteYourMicrophoneToolStripMenuItem_Click(object sender, EventArgs e)
    {
        toolStripButton1.PerformClick();
    }
    private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (!Utils.settingsOpened) new RebelSettings().Show();
    }
    private void toolStripButton4_Click(object sender, EventArgs e)
    {
        Settings.Default.HearVoice = !Settings.Default.HearVoice;
        updateAudioStatus();
    }
    private void hearYourVoiceToolStripMenuItem_Click(object sender, EventArgs e)
    {
        toolStripButton4.PerformClick();
    }
    private void ClearBuffer_Tick(object sender, EventArgs e)
    {
        if (Utils.waveProvider != null) Utils.waveProvider.ClearBuffer();
    }
    private void keepAlive_Tick(object sender, EventArgs e)
    {
        if (!Utils.keepAlived)
        {
            Utils.disconnectReason = "Connection timed out.";
            Utils.roomFormOpened = false;
            disconnectAll();
        }
        Utils.keepAlived = false;
    }
    public void disconnectAll()
    {
        disconnect = true;
        Close();
        new MainForm().Show();
    }
    private void keepAliveSend_Tick(object sender, EventArgs e)
    {
        Utils.sendString("9");
    }
    private void muteYourHeadphonesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        toolStripButton2.PerformClick();
    }
    private void materialSingleLineTextField1_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            if (!canSend && Utils.antiSpam) return;
            if (!(materialSingleLineTextField1.Text.Replace(" ", "") == ""))
            {
                string message = materialSingleLineTextField1.Text;
                if (tabControl1.SelectedIndex == 0)
                {
                    antiSpammer();
                    Utils.sendString("4" + message);
                }
                else
                {
                    if (Utils.currentChannel != "")
                    {
                        antiSpammer();
                        Utils.sendString("7" + message);
                    }
                }
            }
        }
    }
    public void antiSpammer()
    {
        materialSingleLineTextField1.Text = "";
        if (Utils.antiSpam)
        {
            canSend = false;
            antiSpam.Interval = Utils.antiSpamInterval;
            antiSpam.Start();
        }
    }
}