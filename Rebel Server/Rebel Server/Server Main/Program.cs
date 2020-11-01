using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime;
public class Program
{
    [DllImport("psapi.dll")]
    static extern int EmptyWorkingSet(IntPtr hwProc);
    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool SetProcessWorkingSetSize(IntPtr process, UIntPtr minimumWorkingSetSize, UIntPtr maximumWorkingSetSize);
    public static string[] settingsFile;
    public static ChannelManager channelManager;
    private static UdpClient receivingUdpClient;
    private static IPEndPoint remoteIpEndPoint;
    private static UserManager userManager;
    public static void Main()
    {
        LogInfo("Starting server...");
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
        if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\settings.ini"))
        {
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\settings.ini", "SERVER_IP = ALL" + Environment.NewLine + "SERVER_PORT = 8080" + Environment.NewLine + "SERVER_NAME = Test" + Environment.NewLine + "ANTI_SPAM = TRUE" + Environment.NewLine + "ANTI_SPAM_DELAY = 4000" + Environment.NewLine + "VOICE_COMPRESSION = FALSE" + Environment.NewLine + "VIDEO_SCREEN_SHARE_COMPRESSION = FALSE");
            LogWarn("Created settings file (settings.ini) with all the default settings.");
        }
        if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\structure.dat"))
        {
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\structure.dat", "General Chat");
            LogWarn("Created structure file (structure.dat) with 1 channel.");
        }
        settingsFile = System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\settings.ini");
        string ipLol = "";
        if (getProperty("SERVER_IP").ToLower() == "all") ipLol = "127.0.0.1"; else ipLol = getProperty("SERVER_IP");
        channelManager = new ChannelManager();
        if (System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\structure.dat").Replace(" ", "") == "")
        {
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "\\structure.dat", "General Chat");
            channelManager.addChannel(new Channel("General Chat"));
            LogWarn("Created structure file (structure.dat) with 1 channel.");
        }
        else
        {
            int channels = 0;
            foreach (string line in System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\structure.dat"))
            {
                if (!(line.Replace(" ", "") == ""))
                {
                    if (line.Contains("|||"))
                    {
                        string[] splitter = line.Split(new string[] { "|||" }, StringSplitOptions.None);
                        if (!channelManager.isChannel(splitter[0])) channelManager.addChannel(new Channel(splitter[0], int.Parse(splitter[1])));
                    }
                    else if (!channelManager.isChannel(line)) channelManager.addChannel(line);
                    channels++;
                }
            }
            LogInfo("Succesfully loaded " + channels.ToString() + " channels.");
        }
        try
        {
            receivingUdpClient = new UdpClient(int.Parse(getProperty("SERVER_PORT")));
            if (getProperty("SERVER_IP").ToLower() == "all") remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            else remoteIpEndPoint = new IPEndPoint(IPAddress.Parse(getProperty("SERVER_IP")), 0);
            userManager = new UserManager();
            Thread childThread = new Thread(new ThreadStart(receiveAll));
            childThread.Start();
            LogInfo("Started receiving UDP system.");
            Thread childThread1 = new Thread(new ThreadStart(keepAliveSystem));
            childThread1.Start();
            Thread childThread2 = new Thread(new ThreadStart(clearRam));
            childThread2.Start();
            LogInfo("Started keep alive system.");
            Console.Title = getProperty("SERVER_NAME");
            LogInfo("Server succesfully started at " + ipLol + ":" + getProperty("SERVER_PORT") + ".");
        }
        catch (Exception ex)
        {
            LogError("Failed to start the server.");
        }
    }
    public static string getProperty(string property)
    {
        foreach (string line in settingsFile) if (line.ToLower().StartsWith(property.ToLower() + " = ")) return line.Substring((property.ToLower() + " = ").Length);
        return "";
    }
    public static void LogLine(string line)
    {
        Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] " + line);
    }
    public static void LogInfo(string line)
    {
        Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [INFO] " + line);
    }
    public static void LogWarn(string line)
    {
        Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [WARN] " + line);
    }
    public static void LogError(string line)
    {
        Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [ERROR] " + line);
    }
    public static void receiveAll()
    {
        while (true)
        {
            try
            {
                string returnData = System.Text.Encoding.Unicode.GetString(receivingUdpClient.Receive(ref remoteIpEndPoint));
                string ip = remoteIpEndPoint.Address.ToString();
                if (returnData.StartsWith("0"))
                {
                    returnData = returnData.Substring(1);
                    string[] spaces = returnData.Split(new char[0]);
                    if (userManager.isIDConnected(spaces[0]))
                    {
                        User user = userManager.getUserById(spaces[0]);
                        if (channelManager.isInChannel(spaces[0])) channelManager.removeUser(spaces[0]);
                        userManager.sendAllPacket("3" + user.getUsername());
                        user.disconnect();
                        userManager.removeUser(user);
                    }
                    if (!System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\data")) System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\data");
                    if (!System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\data\\" + spaces[0])) System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\data\\" + spaces[0]);
                    User userino = new User(ip, spaces[1], spaces[0]);
                    userManager.addUser(userino);
                    string channels = "";
                    string channelInitialize = "";
                    foreach (Channel channel in channelManager.getChannels())
                    {
                        if (channels == "") channels = channel.getName();
                        else channels += "|||" + channel.getName();
                        if (channel.getUsers().Count > 0)
                        {
                            string userList = "";
                            foreach (User user in channel.getUsers())
                            {
                                if (userList == "") userList = user.getUsername();
                                else userList += "---" + user.getUsername();
                            }
                            if (channelInitialize == "") channelInitialize = channel.getName() + "---" + userList;
                            else channelInitialize += "|||" + channel.getName() + "---" + userList;
                        }
                    }
                    userino.sendString("0" + getProperty("SERVER_NAME") + "---" + channels);
                    userino.sendString("7" + getProperty("ANTI_SPAM").ToLower() + " " + getProperty("ANTI_SPAM_DELAY") + " " + getProperty("VOICE_COMPRESSION").ToLower() + " " + getProperty("VOICE_FORCED_BITS").ToLower() + " " + getProperty("VOICE_FORCED_BITS_VALUE") + " " + getProperty("VOICE_FORCED_CHANNELS").ToLower() + " " + getProperty("VOICE_FORCED_CHANNELS_VALUE") + " " + getProperty("VOICE_FORCED_FREQUENCY") + " " + getProperty("VOICE_FORCED_FREQUENCY_VALUE") + " " + getProperty("VIDEO_SCREEN_SHARE_COMPRESSION").ToLower());
                    if (channelInitialize != "") userino.sendString("6" + channelInitialize);
                    userino.sendString("9");
                    LogInfo("A new user has been connected to the server: [IP] " + ip + ", [USERNAME] " + spaces[1] + ", [ID] " + spaces[0] + ".");
                }
                else if (returnData.Equals("2"))
                {
                    if (userManager.isIPConnected(ip))
                    {
                        User user = userManager.getUserByIP(ip);
                        if (channelManager.isInChannel(user.getCpuId())) channelManager.removeUser(user.getCpuId());
                        userManager.sendAllPacket("3" + user.getUsername());
                        user.disconnect();
                        userManager.removeUser(user);
                        LogInfo("A user has been disconnected from the server: [IP] " + ip + ", [USERNAME] " + user.getUsername() + ", [ID] " + user.getCpuId() + ".");
                    }
                }
                else if (returnData.StartsWith("3"))
                {
                    User user = userManager.getUserByIP(ip);
                    if (returnData[1] == '1') user.setMicMuted(true); else user.setMicMuted(false);
                    if (returnData[2] == '1') user.setAudioMuted(true); else user.setAudioMuted(false);
                    if (returnData[3] == '1') user.setHearVoice(true); else user.setHearVoice(false);
                    LogInfo("The user '" + user.getUsername() + "' {" + ip + "} has changed his/her audio status: Microphone [" + (user.isMicMuted() ? "Muted" : "Unmuted") + "], Audio [" + (user.isAudioMuted() ? "Muted" : "Unmuted") + "], Hear Voice [" + (user.isHearVoice() ? "Enabled" : "Disabled") + "].");
                }
                else if (returnData.StartsWith("4"))
                {
                    User user = userManager.getUserByIP(ip);
                    string message = returnData.Substring(1);
                    userManager.sendAllPacket("1" + user.getUsername() + " > " + message);
                    LogInfo("The user '" + user.getUsername() + "' {" + ip + "} has sent a message in Global Chat: '" + message + "'.");
                }
                else if (returnData.StartsWith("5"))
                {
                    User user = userManager.getUserByIP(ip);
                    string channelName = returnData.Substring(2);
                    Channel channel = channelManager.getChannelByName(channelName);
                    Channel myChannel = channelManager.getChannelByUser(user.getCpuId());
                    if (channel != null)
                    {
                        string userList = user.getUsername();
                        foreach (User userino in channelManager.getChannelByName(channelName).getUsers()) userList += "|||" + userino.getUsername();
                        if (myChannel != null)
                        {
                            if (myChannel.getName() == channel.getName())
                            {
                                LogInfo("The user '" + user.getUsername() + "' {" + ip + "} has tried to join to channel '" + channelName + "' but it is already in it.");
                            }
                            else
                            {
                                for (int i = 0; i < 2; i++)
                                {
                                    try
                                    {
                                        channelManager.getChannelByName(myChannel.getName()).removeUser(user.getCpuId());
                                        if (channelManager.getChannelByName(channelName).getUserLimit() != -1)
                                        {
                                            if (channelManager.getChannelByName(channelName).getUsers().Count != channelManager.getChannelByName(channelName).getUserLimit())
                                            {
                                                user.setInScreenshare(false);
                                                user.setCurrentScreen("");
                                                channelManager.getChannelByName(channelName).addUser(user);
                                                LogInfo("The user '" + user.getUsername() + "' {" + ip + "} has been switched to channel '" + channelName + "'.");
                                                userManager.sendAllPacket("2" + user.getUsername() + "---" + channelName);
                                                if (userList != "") userManager.sendAllPacket("8" + userList);
                                            }
                                        }
                                        else
                                        {
                                            user.setInScreenshare(false);
                                            user.setCurrentScreen("");
                                            channelManager.getChannelByName(channelName).addUser(user);
                                            LogInfo("The user '" + user.getUsername() + "' {" + ip + "} has been switched to channel '" + channelName + "'.");
                                            userManager.sendAllPacket("2" + user.getUsername() + "---" + channelName);
                                            if (userList != "") userManager.sendAllPacket("8" + userList);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (channelManager.getChannelByName(channelName).getUserLimit() != -1)
                            {
                                if (!(channelManager.getChannelByName(channelName).getUsers().Count >= channelManager.getChannelByName(channelName).getUserLimit()))
                                {
                                    user.setInScreenshare(false);
                                    user.setCurrentScreen("");
                                    channelManager.getChannelByName(channelName).addUser(user);
                                    userManager.sendAllPacket("2" + user.getUsername() + "---" + channelName);
                                    LogInfo("The user '" + user.getUsername() + "' {" + ip + "} has been connected to channel '" + channelName + "'.");
                                    if (userList != "") userManager.sendAllPacket("8" + userList);
                                }
                            }
                            else
                            {
                                user.setInScreenshare(false);
                                user.setCurrentScreen("");
                                channelManager.getChannelByName(channelName).addUser(user);
                                userManager.sendAllPacket("2" + user.getUsername() + "---" + channelName);
                                LogInfo("The user '" + user.getUsername() + "' {" + ip + "} has been connected to channel '" + channelName + "'.");
                                if (userList != "") userManager.sendAllPacket("8" + userList);
                            }
                        }
                    }
                }
                else if (returnData.Equals("6"))
                {
                    User user = userManager.getUserByIP(ip);
                    user.setInScreenshare(false);
                    user.setCurrentScreen("");
                    Channel channel = channelManager.getChannelByUser(user.getCpuId());
                    if (channel != null)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            try
                            {
                                channelManager.getChannelByUser(user.getCpuId()).removeUser(user);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        userManager.sendAllPacket("3" + user.getUsername());
                        LogInfo("The user '" + user.getUsername() + "' {" + ip + "} has been disconnected from channel '" + channel.getName() + "'.");
                    }
                }
                else if (returnData.StartsWith("7"))
                {
                    User user = userManager.getUserByIP(ip);
                    Channel channel = channelManager.getChannelByUser(user.getCpuId());
                    if (channel != null)
                    {
                        string message = returnData.Substring(1);
                        foreach (User userino in channel.getUsers()) userino.sendString("4" + user.getUsername() + " > " + message);
                        LogInfo("The user '" + user.getUsername() + "' {" + ip + "} has sent a message in a channel chat [" + channel.getName() + "]: '" + message + "'.");
                    }
                }
                else if (returnData.StartsWith("8"))
                {
                    User user = userManager.getUserByIP(ip);
                    Channel channel = channelManager.getChannelByUser(user.getCpuId());
                    if (!user.isAudioMuted() && !user.isMicMuted())
                    {
                        if (channel != null)
                        {
                            returnData = returnData.Substring(1);
                            foreach (User userino in channel.getUsers())
                            {
                                if (!userino.isAudioMuted())
                                {
                                    bool can = true;
                                    if (userino.getCpuId() == user.getCpuId() && !user.isHearVoice()) can = false;
                                    if (can) userino.sendString("5" + returnData);
                                }
                            }
                        }
                    }
                }
                else if (returnData.Equals("9"))
                {
                    userManager.getUserByIP(ip).setKeepAlived(true);
                }
                else if (returnData.Equals("A"))
                {
                    User user = userManager.getUserByIP(ip);
                    Channel channel = channelManager.getChannelByUser(user.getCpuId());
                    if (!user.isInScreenShare())
                    {
                        if (channel != null)
                        {
                            user.setInScreenshare(true);
                            foreach (User userino in channel.getUsers())
                            {
                                userino.sendString("A" + user.getUsername());
                            }
                            LogInfo("The user " + user.getUsername() + " {" + user.getIp() + "} has turned on his screen share in channel ['" + channel.getName() + "'].");
                        }
                    }
                }
                else if (returnData.Equals("B"))
                {
                    User user = userManager.getUserByIP(ip);
                    Channel channel = channelManager.getChannelByUser(user.getCpuId());
                    if (user.isInScreenShare())
                    {
                        if (channel != null)
                        {
                            user.setInScreenshare(false);
                            foreach (User userino in channel.getUsers())
                            {
                                userino.sendString("B" + user.getUsername());
                            }
                            LogInfo("The user " + user.getUsername() + " {" + user.getIp() + "} has turned off his screen share in channel ['" + channel.getName() + "'].");
                        }
                    }
                }
                else if (returnData.StartsWith("C"))
                {
                    returnData = returnData.Substring(1);
                    User user = userManager.getUserByIP(ip);
                    user.setCurrentScreen(returnData);
                    if (returnData == "")
                    {
                        LogInfo("The user " + user.getUsername() + " {" + user.getIp() + "} is now seeing no one user.");
                    }
                    else
                    {
                        LogInfo("The user " + user.getUsername() + " {" + user.getIp() + "} is now seeing " + returnData + ".");
                    }
                }
                else if (returnData.StartsWith("D"))
                {
                    returnData = returnData.Substring(1);
                    User user = userManager.getUserByIP(ip);
                    Channel channel = channelManager.getChannelByUser(user.getCpuId());
                    if (user.isInScreenShare())
                    {
                        if (channel != null)
                        {
                            foreach (User userino in channel.getUsers())
                            {
                                if (userino.getCurrentScreen().Equals(user.getUsername()))
                                {
                                    userino.sendString("C" + returnData);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
    public static void keepAliveSystem()
    {
        while (true)
        {
            try
            {
                Thread.Sleep(4000);
                foreach (User user in userManager.getallUsers())
                {
                    user.sendString("9");
                    if (!user.isKeepAlived())
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            try
                            {
                                if (channelManager.isInChannel(user.getCpuId())) channelManager.removeUser(user.getCpuId());
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        userManager.sendAllPacket("3" + user.getUsername());
                        user.disconnect();
                        userManager.removeUser(user);
                        LogInfo("A user has been disconnected from the server: [IP] " + user.getIp() + ", [USERNAME] " + user.getUsername() + ", [ID] " + user.getCpuId() + ".");
                    }
                    else user.setKeepAlived(false);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
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
}