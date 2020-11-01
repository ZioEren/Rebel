using System.Collections.Generic;
public class Channel
{
    private string name;
    private int userLimit;
    private List<User> users;
    public Channel(string name)
    {
        this.name = name;
        userLimit = -1;
        users = new List<User>();
    }
    public Channel(string name, int userLimit)
    {
        this.name = name;
        this.userLimit = userLimit;
        users = new List<User>();
    }
    public string getName()
    {
        return name;
    }
    public List<User> getUsers()
    {
        return users;
    }
    public void addUser(User user)
    {
        users.Add(user);
    }
    public void removeUser(User user)
    {
        users.Remove(user);
    }
    public void removeUser(string cpuId)
    {
        foreach (User user in users)
        {
            if (user.getCpuId() == cpuId)
            {
                removeUser(user);
            }
        }
    }
    public int getUserLimit()
    {
        return userLimit;
    }
}