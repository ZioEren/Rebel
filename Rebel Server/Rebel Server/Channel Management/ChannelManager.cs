using System.Collections.Generic;
public class ChannelManager
{
    private List<Channel> channels;
    public ChannelManager()
    {
        channels = new List<Channel>();
    }
    public void addChannel(Channel channel)
    {
        channels.Add(channel);
    }
    public void removeChannel(Channel channel)
    {
        channels.Remove(channel);
    }
    public Channel getChannelByName(string name)
    {
        foreach (Channel channel in channels)
        {
            if (channel.getName() == name)
            {
                return channel;
            }
        }
        return null;
    }
    public Channel getChannelByUser(User user)
    {
        foreach (Channel channel in channels)
        {
            foreach (User userer in channel.getUsers())
            {
                if (user.getCpuId() == userer.getCpuId())
                {
                    return channel;
                }
            }
        }
        return null;
    }
    public Channel getChannelByUser(string cpuId)
    {
        foreach (Channel channel in channels)
        {
            foreach (User userer in channel.getUsers())
            {
                if (cpuId == userer.getCpuId())
                {
                    return channel;
                }
            }
        }
        return null;
    }
    public void addChannel(string name)
    {
        channels.Add(new Channel(name));
    }
    public bool isChannel(string name)
    {
        foreach (Channel channel in channels)
        {
            if (channel.getName() == name)
            {
                return true;
            }
        }
        return false;
    }
    public List<Channel> getChannels()
    {
        return channels;
    }
    public void removeUser(User user)
    {
        foreach (Channel channel in channels)
        {
            foreach (User userino in channel.getUsers())
            {
                if (userino.getCpuId() == user.getCpuId())
                {
                    channel.removeUser(userino);
                }
            }
        }
    }
    public void removeUser(string cpuId)
    {
        foreach (Channel channel in channels)
        {
            foreach (User userino in channel.getUsers())
            {
                if (userino.getCpuId() == cpuId)
                {
                    try
                    {
                        channel.removeUser(userino);
                    }
                    catch (System.Exception ex)
                    {
                    }
                    try
                    {
                        channel.removeUser(userino);
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
            }
        }
    }
    public bool isInChannel(string cpuId)
    {
        foreach (Channel channel in channels)
        {
            foreach (User userino in channel.getUsers())
            {
                if (userino.getCpuId() == cpuId)
                {
                    return true;
                }
            }
        }
        return false;
    }
}