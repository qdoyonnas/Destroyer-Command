using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Team", menuName = "ScriptableObjects/Team", order = 2)]
public class Team : ScriptableObject
{
	public int teamNumber = 0;
	public Color hullColor;
	public Gradient trailColor;
}
