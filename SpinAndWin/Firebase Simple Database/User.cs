using System;

public class User
{   
    public int level;
    public int xp;
    public int gold;
    public int gem;

    public User(int level, int xp, int gold, int gem)
    {
        this.level = level;
        this.xp = xp;
        this.gold = gold;
        this.gem = gem;
    }
}