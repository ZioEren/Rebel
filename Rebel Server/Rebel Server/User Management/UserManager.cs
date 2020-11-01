using System.Collections.Generic;
public class UserManager
{
    private List<User> userList;
    public UserManager()
    {
        userList = new List<User>();
    }
    public void addUser(User user)
    {
        userList.Add(user);
    }
    public void removeUser(User user)
    {
        userList.Remove(user);
    }
    public List<User> getallUsers()
    {
        return userList;
    }
    public User getUserById(string cpuId)
    {
        foreach (User user in userList)
        {
            if (user.getCpuId() == cpuId)
            {
                return user;
            }    
        }
        return null;
    }
    public User getUserByIP(string ip)
    {
        foreach (User user in userList)
        {
            if (user.getIp() == ip)
            {
                return user;
            }
        }
        return null;
    }
    public bool isIPConnected(string ip)
    {
        foreach (User user in userList)
        {
            if (user.getIp() == ip)
            {
                return true;
            }
        }
        return false;
    }
    public bool isIDConnected(string cpuId)
    {
        foreach (User user in userList)
        {
            if (user.getCpuId() == cpuId)
            {
                return true;
            }
        }
        return false;
    }
    public void sendAllPacket(string packet)
    {
        foreach (User user in userList)
        {
            user.sendString(packet);
        }
    }
    public void sendAllPacket(string packet, User owner)
    {
        foreach (User user in userList)
        {
            if (!user.getCpuId().Equals(owner.getCpuId()))
            {
                user.sendString(packet);
            }
        }
    }
}