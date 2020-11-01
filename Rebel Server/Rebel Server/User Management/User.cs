using System;
using System.Net;
using System.Net.Sockets;
public class User
{
    private string ip, username, cpuId, currentScreen = "";
    private bool micMuted, audioMuted, hearVoice, keepAlived, screenShare;
    private UdpClient udpClient = new UdpClient();
    public User(string ip, string username, string cpuId)
    {
        this.ip = ip;
        this.username = username;
        this.cpuId = cpuId;
        try
        {
            udpClient.Connect(IPAddress.Parse(ip), 53);
        }
        catch (Exception ex)
        {
        }
    }
    public string getIp()
    {
        return ip;
    }
    public string getUsername()
    {
        return username;
    }
    public string getCpuId()
    {
        return cpuId;
    }
    public void sendBytes(byte[] command)
    {
        try
        {
            udpClient.Send(command, command.Length);
        }
        catch (Exception ex)
        {
        }
    }
    public void sendString(string command)
    {
        sendBytes(System.Text.Encoding.Unicode.GetBytes(command));
    }
    public void disconnect()
    {
        udpClient.Close();
    }
    public bool isMicMuted()
    {
        return micMuted;
    }
    public bool isAudioMuted()
    {
        return audioMuted;
    }
    public void setMicMuted(bool muted)
    {
        micMuted = muted;
    }
    public void setAudioMuted(bool muted)
    {
        audioMuted = muted;
    }
    public bool isHearVoice()
    {
        return hearVoice;
    }
    public void setHearVoice(bool confirm)
    {
        hearVoice = confirm;
    }
    public void setKeepAlived(bool confirm)
    {
        keepAlived = confirm;
    }
    public bool isKeepAlived()
    {
        return keepAlived;
    }
    public void setInScreenshare(bool confirm)
    {
        screenShare = confirm;
    }
    public bool isInScreenShare()
    {
        return screenShare;
    }
    public void setCurrentScreen(string to)
    {
        currentScreen = to;
    }
    public string getCurrentScreen()
    {
        return currentScreen;
    }
}