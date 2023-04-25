using UnityEngine;
using UnityEditor;

namespace FaceSync
{
	[CreateAssetMenu(fileName ="BlendShapeID", menuName = "PlanetMaenad/FaceSync/BlendShape ID")]
	public class FaceSyncBlendShapeID : ScriptableObject
	{
		public string Label;
		public string Identifier;
	}
}
