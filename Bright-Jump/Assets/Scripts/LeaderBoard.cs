using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class LeaderBoard
{
    private int maxScores = 10;

    public void AddScoreToLeaders(string name, long score, DatabaseReference dbReference) {
        dbReference.RunTransaction(mutableData => {
            List<object> highscores = mutableData.Value as List<object>;

            if (highscores == null) {
                highscores = new List<object>();
            } else if (mutableData.ChildrenCount >= maxScores) {
                long minScore = long.MaxValue;
                object minVal = null;
                foreach (var child in highscores) {
                    if (!(child is Dictionary<string, object>)) continue;
                    long childScore = (long)((Dictionary<string, object>)child)["score"];
                    if (childScore < minScore) {
                        minScore = childScore;
                        minVal = child;
                    }
                }
                if (minScore > score) {
                // The new score is lower than the existing 5 scores, abort.
                return TransactionResult.Abort();
                }
 
                // Remove the lowest score.
                highscores.Remove(minVal);
            }

            // Add the new high score.
            Dictionary<string, object> newScoreMap = new Dictionary<string, object>();
            newScoreMap["score"] = score;
            newScoreMap["name"] = name;
            highscores.Add(newScoreMap);
            mutableData.Value = highscores;
            return TransactionResult.Success(mutableData);
        });
    }
}
