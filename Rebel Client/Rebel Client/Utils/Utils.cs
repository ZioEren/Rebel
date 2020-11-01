using System;
using System.Collections.Generic;
using System.Management;
using NAudio.Wave;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.IO.Compression;
using Rebel_Client.Properties;
using System.Linq;
using MaterialSkin.Controls;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Diagnostics;
public class Utils
{
    public static string cpuId;
    public static ServerPage serverPage;
    public static RoomForm roomForm;
    public static bool joined;
    public static WaveIn waveIn = null;
    public static BufferedWaveProvider waveProvider = null;
    public static WaveOut waveOut = null;
    public static bool settingsOpened = false;
    public static bool roomFormOpened = false;
    public static string currentChannel = "";
    public static bool antiSpam = true;
    public static int antiSpamInterval = 4000;
    public static List<string> userList;
    public static bool voiceCompression;
    public static string savedUsername;
    private static UdpClient udpClient;
    private static UdpClient receivingUdpClient;
    private static IPEndPoint remoteIpEndPoint;
    public static bool forcedBits;
    public static bool forcedFrequency;
    public static bool forcedChannels;
    public static int forcedBitsValue = 24;
    public static int forcedFrequencyValue = 48000;
    public static int forcedChannelsValue = 2;
    public static bool keepAlived;
    public static string disconnectReason = "";
    public static List<string> screenshareUsers;
    public static List<string> currentScreenshares;
    public static bool inScreenShare;
    public static Thread screenShareThread;
    public static bool screenShareCompression;
    public static bool clearRamThreadStarted;
    public static Thread clearRamThread;
    [DllImport("psapi.dll")]
    static extern int EmptyWorkingSet(IntPtr hwProc);
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetProcessWorkingSetSize(IntPtr process, UIntPtr minimumWorkingSetSize, UIntPtr maximumWorkingSetSize);
    public static void clearRam()
    {
        while (true)
        {
            Thread.Sleep(1000);
            EmptyWorkingSet(Process.GetCurrentProcess().Handle);
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect(GC.MaxGeneration);
            GC.WaitForPendingFinalizers();
            SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, (UIntPtr)0xFFFFFFFF, (UIntPtr)0xFFFFFFFF);
        }
    }
    public static bool ValidateIPv4(string ipString)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(ipString)) return false;
            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4) return false;
            byte tempForParsing;
            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public static void playAudioFile(string name)
    {
        if (File.Exists(Application.StartupPath + "\\data\\mp3\\" + name + ".mp3"))
        {
            IWavePlayer waveOutDevice = new WaveOut();
            AudioFileReader audioFileReader = new AudioFileReader(Application.StartupPath + "\\data\\mp3\\" + name + ".mp3");
            waveOutDevice.Init(audioFileReader);
            waveOutDevice.Play();
        }
    }
    public static void screenShare()
    {
        while (true)
        {
            Rectangle totalSize = Rectangle.Empty;
            if (Settings.Default.CurrentScreen == 0)
            {
                foreach (Screen s in Screen.AllScreens)
                {
                    totalSize = Rectangle.Union(totalSize, s.Bounds);
                }
            }
            else
            {
                for (int i = 0; i < Screen.AllScreens.Length; i++)
                {
                    if (Settings.Default.CurrentScreen == i + 1)
                    {
                        totalSize = Rectangle.Union(totalSize, Screen.AllScreens[i].Bounds);
                    }
                }
            }
            Bitmap screenShotBMP = new Bitmap(totalSize.Width, totalSize.Height, PixelFormat.Format32bppArgb);
            Graphics screenShotGraphics = Graphics.FromImage(screenShotBMP);
            screenShotGraphics.CopyFromScreen(totalSize.X, totalSize.Y, 0, 0, totalSize.Size, CopyPixelOperation.SourceCopy);
            screenShotGraphics.Dispose();
            screenShotGraphics = null;
            using (var stream = new MemoryStream())
            {
                screenShotBMP.Save(stream, ImageFormat.Jpeg);
                sendString("D" + Convert.ToBase64String(screenShareCompression ? Compress(stream.ToArray()) : stream.ToArray()));
            }
            if (roomFormOpened)
            {
                roomForm.Invoke(new MethodInvoker(() =>
                {
                    foreach (Control control in roomForm.Controls)
                    {
                        if (control.Name == "materialTabControl1")
                        {
                            ListBox theListBox = null;
                            PictureBox thePictureBox = null;
                            foreach (TabPage tabPage in ((MaterialTabControl)control).TabPages)
                            {
                                foreach (Control control1 in tabPage.Controls)
                                {
                                    if (control1.Name == "listBox1")
                                    {
                                        theListBox = (ListBox)control1;
                                    }
                                    if (control1.Name == "pictureBox1")
                                    {
                                        thePictureBox = (PictureBox)control1;
                                    }
                                }
                            }
                            if (theListBox.SelectedItem != null)
                            {
                                if (theListBox.SelectedItem.ToString() == savedUsername)
                                {
                                    thePictureBox.BackgroundImage = screenShotBMP;
                                }
                            }
                        }
                    }
                }));
            }
            GC.Collect();
        }
    }
    public static void loadAll()
    {
        if (!clearRamThreadStarted)
        {
            clearRamThread = new Thread(new ThreadStart(clearRam));
            clearRamThread.Start();
            clearRamThreadStarted = true;
        }
        ManagementObjectCollection managCollec = new ManagementClass("win32_processor").GetInstances();
        foreach (ManagementObject managObj in managCollec)
        {
            cpuId = managObj.Properties["processorID"].Value.ToString();
            break;
        }
        if (waveIn != null)
        {
            waveIn.Dispose();
            waveIn = null;
        }
        if (waveOut != null)
        {
            waveOut.Dispose();
            waveOut = null;
        } 
        waveProvider = null;
        userList = new List<string>();
        screenshareUsers = new List<string>();
        currentScreenshares = new List<string>();
        inScreenShare = false;
        roomFormOpened = false;
        try
        {
            screenShareThread.Abort();
        }catch (Exception ex) { }
        screenShareThread = null;
        GC.Collect();
    }
    public static string filterString(string toFilter)
    {
        string finalString = "";
        char[] letters = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'};
        foreach (char c in toFilter.ToCharArray())
        {
            bool valid = false;
            foreach (char c1 in letters)
            {
                if (c == c1 || c.ToString().ToUpper() == c1.ToString().ToUpper())
                {
                    valid = true;
                    break;
                }
            }
            if (valid)
            {
                finalString += c.ToString();
            }
        }
        return finalString;
    }
    public static bool connect(string ip, int port, string username, string yourIp, int yourPort)
    {
        try
        {
            savedUsername = username;
            udpClient = new UdpClient();
            receivingUdpClient = new UdpClient(yourPort);
            //remoteIpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            udpClient.Connect(IPAddress.Parse(ip), port);
            sendString("0" + cpuId + " " + username);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public static void sendBytes(byte[] command)
    {
        try
        {
            udpClient.Send(command, command.Length);
        }
        catch (Exception ex)
        {
        }
    }
    public static void sendString(string command)
    {
        sendBytes(System.Text.Encoding.Unicode.GetBytes(command));
    }
    public static void disconnect()
    {
        udpClient.Close();
        receivingUdpClient.Close();
        udpClient.Dispose();
        receivingUdpClient.Dispose();
        remoteIpEndPoint = null;
        udpClient = null;
        receivingUdpClient = null;
        GC.Collect();
    }
    public static void receiveAll()
    {
        while (true)
        {
            try
            {
                string returnData = System.Text.Encoding.Unicode.GetString(receivingUdpClient.Receive(ref remoteIpEndPoint));
                if (returnData.StartsWith("0"))
                {
                    joined = true;
                    returnData = returnData.Substring(1);
                    string[] splitter = returnData.Split(new string[] { "---" }, StringSplitOptions.None);
                    serverPage.Invoke(new MethodInvoker(() => {serverPage.Text = "Rebel Client - " + splitter[0];}));
                    string channels = splitter[1];
                    splitter = channels.Split(new string[] { "|||" }, StringSplitOptions.None);
                    serverPage.Invoke(new MethodInvoker(() =>
                    {
                        foreach (string line in splitter)
                        {
                            foreach (Control control in serverPage.Controls)
                            {
                                if (control.Name == "panel2")
                                {
                                    foreach (Control control1 in ((Panel)control).Controls)
                                    {
                                        if (control1.Name == "listBox1")
                                        {
                                            ((ListBox)control1).Items.Add("  " + line);
                                        }
                                    }
                                }
                            }
                        }
                    }));
                }
                else if (returnData.StartsWith("1"))
                {
                    returnData = returnData.Substring(1);
                    if (!returnData.StartsWith(savedUsername + " > ")) playAudioFile("received_message");
                    returnData = "[" + DateTime.Now.ToLongTimeString() + "] " + returnData;
                    serverPage.Invoke(new MethodInvoker(() =>
                    {
                        foreach (Control control in serverPage.Controls)
                        {
                            if (control.Name == "tabControl1")
                            {
                                foreach (TabPage tabPage in ((TabControl)control).TabPages)
                                {
                                    foreach (Control control1 in tabPage.Controls)
                                    {
                                        if (control1.Name == "richTextBox1")
                                        {
                                            if (control1.Text == "") control1.Text = returnData;
                                            else control1.Text += Environment.NewLine + returnData;
                                        }
                                    }
                                }
                            }
                        }
                    }));
                }
                else if (returnData.StartsWith("2"))
                {
                    returnData = returnData.Substring(1);
                    string[] splitter = returnData.Split(new string[] { "---" }, StringSplitOptions.None);
                    if (screenshareUsers.Contains(splitter[0]))
                    {
                        screenshareUsers.Remove(splitter[0]);
                    }
                    if (currentScreenshares.Contains(splitter[0]))
                    {
                        currentScreenshares.Remove(splitter[0]);
                    }
                    serverPage.Invoke(new MethodInvoker(() =>
                    {
                        foreach (Control control in serverPage.Controls)
                        {
                            if (control.Name == "panel2")
                            {
                                foreach (Control control1 in ((Panel)control).Controls)
                                {
                                    if (control1.Name == "listBox1")
                                    {
                                        ListBox listBox = (ListBox)control1;
                                        List<string> newItems = new List<string>();
                                        string selectedItem = listBox.SelectedItem.ToString();
                                        foreach (string item in listBox.Items)
                                        {
                                            if (!(item == "    - " + splitter[0]))
                                            {
                                                newItems.Add(item);
                                                if (item == "  " + splitter[1]) newItems.Add("    - " + splitter[0]);
                                            }
                                        }
                                        listBox.Items.Clear();
                                        foreach (string item in newItems)
                                        {
                                            listBox.Items.Add(item);
                                        }
                                        for (int i = 0; i < listBox.Items.Count - 1; i++)
                                        {
                                            if (listBox.Items[i].ToString() == selectedItem)
                                            {
                                                listBox.SelectedIndex = i;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }));
                    if (splitter[0] == savedUsername)
                    {
                        currentChannel = splitter[1];
                        roomFormOpened = false;
                        playAudioFile("channel_join");
                        screenshareUsers.Clear();
                        currentScreenshares.Clear();
                        inScreenShare = false;
                        try
                        {
                            screenShareThread.Abort();
                        }
                        catch (Exception ex) { }
                        screenShareThread = null;
                        GC.Collect();
                        serverPage.Invoke(new MethodInvoker(() =>
                        {
                            foreach (Control control in serverPage.Controls)
                            {
                                if (control.Name == "tabControl1")
                                {
                                    foreach (TabPage tabPage in ((TabControl)control).TabPages)
                                    {
                                        foreach (Control control1 in tabPage.Controls)
                                        {
                                            if (control1.Name == "richTextBox2") control1.Text = "";
                                        }
                                    }
                                }
                            }
                        }));
                    }
                }
                else if (returnData.StartsWith("3"))
                {
                    returnData = returnData.Substring(1);
                    if (screenshareUsers.Contains(returnData))
                    {
                        screenshareUsers.Remove(returnData);
                        if (currentScreenshares.Contains(returnData))
                        {
                            currentScreenshares.Remove(returnData);
                        }
                    }
                    serverPage.Invoke(new MethodInvoker(() =>
                    {
                        foreach (Control control in serverPage.Controls)
                        {
                            if (control.Name == "panel2")
                            {
                                foreach (Control control1 in ((Panel)control).Controls)
                                {
                                    if (control1.Name == "listBox1")
                                    {
                                        ListBox listBox = (ListBox)control1;
                                        List<string> newItems = new List<string>();
                                        string selectedItem = listBox.SelectedItem.ToString();
                                        foreach (string item in listBox.Items)
                                        {
                                            if (!(item == "    - " + returnData)) newItems.Add(item);
                                        }
                                        listBox.Items.Clear();
                                        foreach (string item in newItems) listBox.Items.Add(item);
                                        for (int i = 0; i < listBox.Items.Count - 1; i++)
                                        {
                                            if (listBox.Items[i].ToString() == selectedItem)
                                            {
                                                listBox.SelectedIndex = i;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }));
                    if (returnData == savedUsername)
                    {
                        userList.Clear();
                        currentChannel = "";
                        screenshareUsers.Clear();
                        currentScreenshares.Clear();
                        inScreenShare = false;
                        try
                        {
                            screenShareThread.Abort();
                        }
                        catch (Exception ex) { }
                        screenShareThread = null;
                        GC.Collect();
                        playAudioFile("channel_exit");
                        serverPage.Invoke(new MethodInvoker(() =>
                        {
                            foreach (Control control in serverPage.Controls)
                            {
                                if (control.Name == "tabControl1")
                                {
                                    foreach (TabPage tabPage in ((TabControl)control).TabPages)
                                    {
                                        foreach (Control control1 in tabPage.Controls)
                                        {
                                            if (control1.Name == "richTextBox2") control1.Text = "";
                                        }
                                    }
                                }
                            }
                        }));
                    }
                }
                else if (returnData.StartsWith("4"))
                {
                    if (!(returnData.Substring(1).StartsWith(savedUsername + " > "))) playAudioFile("received_message");
                    returnData = "[" + DateTime.Now.ToLongTimeString() + "] " + returnData.Substring(1);
                    serverPage.Invoke(new MethodInvoker(() =>
                    {
                        foreach (Control control in serverPage.Controls)
                        {
                            if (control.Name == "tabControl1")
                            {
                                foreach (TabPage tabPage in ((TabControl)control).TabPages)
                                {
                                    foreach (Control control1 in tabPage.Controls)
                                    {
                                        if (control1.Name == "richTextBox2")
                                        {
                                            if (control1.Text == "") control1.Text = returnData;
                                            else control1.Text += Environment.NewLine + returnData;
                                        }
                                    }
                                }
                            }
                        }
                    }));
                }
                else if (returnData.StartsWith("5") && !Settings.Default.AudioMuted)
                {
                    if (waveOut != null)
                    {
                        if (waveProvider != null)
                        {
                            returnData = returnData.Substring(1);
                            byte[] buffer = voiceCompression ? Decompress(Convert.FromBase64String(returnData)) : Convert.FromBase64String(returnData);
                            waveProvider.AddSamples(buffer, 0, buffer.Length);
                        }
                    }
                }
                else if (returnData.StartsWith("6"))
                {
                    Thread.Sleep(2000);
                    returnData = returnData.Substring(1);
                    string[] splitter = null;
                    if (returnData.Contains("|||")) splitter = returnData.Split(new string[] { "|||" }, StringSplitOptions.None);
                    else splitter = new string[] { returnData };
                    serverPage.Invoke(new MethodInvoker(() =>
                    {
                        foreach (Control control in serverPage.Controls)
                        {
                            if (control.Name == "panel2")
                            {
                                foreach (Control control1 in ((Panel)control).Controls)
                                {
                                    if (control1.Name == "listBox1")
                                    {
                                        foreach (string channel in splitter)
                                        {
                                            if (channel.Contains("---"))
                                            {
                                                string[] splitter1 = channel.Split(new string[] { "---" }, StringSplitOptions.None);
                                                ListBox listBox = (ListBox)control1;
                                                List<string> newItems = new List<string>();
                                                foreach (string item in listBox.Items)
                                                {
                                                    newItems.Add(item);
                                                    if (item == "  " + splitter[0])
                                                    {
                                                        for (int i = 0; i < splitter1.Length; i++)
                                                        {
                                                            if (i != 0)
                                                            {
                                                                try
                                                                {
                                                                    newItems.Add(splitter1[i]);
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                listBox.Items.Clear();
                                                foreach (string item in newItems) listBox.Items.Add(item);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }));
                }
                else if (returnData.StartsWith("7"))
                {
                    returnData = returnData.Substring(1);
                    string[] splitter = returnData.Split(new char[0]);
                    antiSpam = bool.Parse(splitter[0]);
                    antiSpamInterval = int.Parse(splitter[1]);
                    voiceCompression = bool.Parse(splitter[2]);
                    forcedBits = bool.Parse(splitter[3]);
                    forcedBitsValue = int.Parse(splitter[4]);
                    forcedChannels = bool.Parse(splitter[5]);
                    forcedChannelsValue = int.Parse(splitter[6]);
                    forcedFrequency = bool.Parse(splitter[7]);
                    forcedFrequencyValue = int.Parse(splitter[8]);
                    screenShareCompression = bool.Parse(splitter[9]);
                }
                else if (returnData.StartsWith("8"))
                {
                    returnData = returnData.Substring(1);
                    userList.Clear();
                    if (returnData.Contains("|||"))
                    {
                        string[] splitter = returnData.Split(new string[] { "|||" }, StringSplitOptions.None);
                        foreach (string userino in splitter)
                        {
                            userList.Add(userino);
                        }
                    }
                    else
                    {
                        userList.Add(returnData);
                    }
                }
                else if (returnData.Equals("9"))
                {
                    keepAlived = true;
                }
                else if (returnData.StartsWith("A"))
                {
                    returnData = returnData.Substring(1);
                    screenshareUsers.Add(returnData);
                    if (roomFormOpened)
                    {
                        roomForm.Invoke(new MethodInvoker(() =>
                        {
                            foreach (Control control in roomForm.Controls)
                            {
                                if (control.Name == "materialTabControl1")
                                {
                                    foreach (TabPage tabPage in ((MaterialTabControl)control).TabPages)
                                    {
                                        foreach (Control control1 in tabPage.Controls)
                                        {
                                            if (control1.Name == "listBox1")
                                            {
                                                ((ListBox)control1).Items.Add(returnData);
                                            }
                                        }
                                    }
                                }
                            }
                        }));
                    }
                }
                else if (returnData.StartsWith("B"))
                {
                    returnData = returnData.Substring(1);
                    if (screenshareUsers.Contains(returnData))
                    {
                        screenshareUsers.Remove(returnData);
                    }
                    if (currentScreenshares.Contains(returnData))
                    {
                        currentScreenshares.Remove(returnData);
                    }
                    if (roomFormOpened)
                    {
                        roomForm.Invoke(new MethodInvoker(() =>
                        {
                            foreach (Control control in roomForm.Controls)
                            {
                                if (control.Name == "materialTabControl1")
                                {
                                    foreach (TabPage tabPage in ((MaterialTabControl)control).TabPages)
                                    {
                                        foreach (Control control1 in tabPage.Controls)
                                        {
                                            if (control1.Name == "listBox1")
                                            {
                                                ListBox listBox = (ListBox)control1;
                                                if (listBox.Items.Contains(returnData))
                                                {
                                                    listBox.Items.Remove(returnData);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }));
                    }
                }
                else if (returnData.StartsWith("C"))
                {
                    if (roomFormOpened)
                    {
                        returnData = returnData.Substring(1);
                        roomForm.Invoke(new MethodInvoker(() =>
                        {
                            foreach (Control control in roomForm.Controls)
                            {
                                if (control.Name == "materialTabControl1")
                                {
                                    foreach (TabPage tabPage in ((MaterialTabControl)control).TabPages)
                                    {
                                        foreach (Control control1 in tabPage.Controls)
                                        {
                                            if (control1.Name == "pictureBox1")
                                            {
                                                using (var ms = new MemoryStream(screenShareCompression ? Decompress(Convert.FromBase64String(returnData)) : Convert.FromBase64String(returnData)))
                                                {
                                                    ((PictureBox)control1).BackgroundImage = new Bitmap(ms); 
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
    public static byte[] Decompress(byte[] data)
    {
        MemoryStream input = new MemoryStream(data);
        MemoryStream output = new MemoryStream();
        using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress)) dstream.CopyTo(output);
        return output.ToArray();
    }
    public static byte[] Compress(byte[] data)
    {
        MemoryStream output = new MemoryStream();
        using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal)) dstream.Write(data, 0, data.Length);
        return output.ToArray();
    }
}