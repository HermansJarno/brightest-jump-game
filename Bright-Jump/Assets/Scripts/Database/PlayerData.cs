using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public struct PlayerData 
{
    public string name;
    public long score;

    public PlayerData(string name, long score){
        this.name = name;
        this.score = score;
    }
}
