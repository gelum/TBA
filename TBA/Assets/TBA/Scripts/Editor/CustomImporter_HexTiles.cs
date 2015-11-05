using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[Tiled2Unity.CustomTiledImporter]
public class CustomImporter_HexTiles : Tiled2Unity.ICustomTiledImporter {

	public void HandleCustomProperties(GameObject gameObject,
	                                   IDictionary<string, string> customProperties)
	{
		if (customProperties.ContainsKey("Type"))
		{
			// Add the terrain tile game object
			HexTile tile = gameObject.AddComponent<HexTile>();
			tile.Type = customProperties["Type"];
		}
	}
	
	public void CustomizePrefab(GameObject prefab)
	{
		// Do nothing
	}
}
