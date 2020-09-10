using System.Collections;
using System.Collections.Generic;
using System;

public struct LeaderBoardEntries
{
    public List<LeaderBoardEntry> leaderBoardEntries;

    public LeaderBoardEntries(List<LeaderBoardEntry> leaderBoardEntries){
        this.leaderBoardEntries = leaderBoardEntries;
    }
}
