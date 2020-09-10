using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public struct LeaderBoardEntry {
    public string name;
    public long score;
    private string uid;

    public LeaderBoardEntry(string uid, long score, string name) {
        this.uid = uid;
        this.score = score;
        this.name = name;
    }

    public Dictionary<string, Object> ToDictionary() {
        Dictionary<string, Object> result = new Dictionary<string, Object>();
        result["uid"] = uid;
        result["score"] = score;
        result["name"] = name;

        return result;
    }
}