using System;
using System.Collections.Generic;

public static class ScoreManager
{
	public delegate void ScoreChanged(Enums.PlayerIndex id, int score);
	public static event ScoreChanged OnScoreChanged;
	
	static readonly List<int> _scores = new List<int>(4);
    public static void Score(Enums.PlayerIndex id, int score)
	{
        if (id == Enums.PlayerIndex.Unknown)
			return;
		
		_scores[(int)id] += score;
		if (OnScoreChanged != null)
			OnScoreChanged(id, _scores[(int)id]);
	}
}
