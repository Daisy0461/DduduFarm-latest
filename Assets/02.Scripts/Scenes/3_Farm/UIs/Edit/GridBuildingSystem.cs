using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem current;
 
    public GridLayout gridLayout;
    public Tilemap MainTilemap;
    public Tilemap TempTilemap;

    private static Dictionary<TileType, TileBase> tileBases = new Dictionary<TileType, TileBase>();
    
    private Building temp;
    private Vector3 prevPos;
    private BoundsInt prevArea;

    #region Unity Methods

    private void Awake() 
    {
        current = this;
    }

    private void Start() 
    {
        if (tileBases.Count <= 0)
        {
            string tilePath = @"Tiles\";
            tileBases.Add(TileType.Empty, null);
            tileBases.Add(TileType.White, Resources.Load<TileBase>(path:tilePath + "white"));
            tileBases.Add(TileType.Green, Resources.Load<TileBase>(path:tilePath + "green"));
            tileBases.Add(TileType.Yellow, Resources.Load<TileBase>(path:tilePath + "yellow"));    
            tileBases.Add(TileType.Red, Resources.Load<TileBase>(path:tilePath + "red"));
        }
        gameObject.SetActive(false);
    }

    private void Update() 
    {
        if (!temp)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            if (EventSystem.current.currentSelectedGameObject?.name[0] == 'B')
            {// 편집 모드 버튼만 감지
                return;
            }

            if (!temp.Placed)
            {
                Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPos = gridLayout.LocalToCell(touchPos);

                if (prevPos != cellPos)
                {
                    temp.transform.localPosition = gridLayout.CellToLocalInterpolated(cellPos);
                    prevPos = cellPos;
                    FollowBuilding();
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClearArea();
            Destroy(temp.gameObject);
        }
    }

    #endregion

    #region  Tilemap Management

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, z:0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    private static void SetTilesBlock(BoundsInt area, TileType type, Tilemap tilemap)
    {
        int size = area.size.x * area.size.y * area.size.z;
        TileBase[] tileArray = new TileBase[size];
        FillTiles(tileArray, type);
        tilemap.SetTilesBlock(area, tileArray);
    }

    private static void FillTiles(TileBase[] arr, TileType type)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = tileBases[type];
        }
    }
    #endregion

    #region Building Placement
    
    public GameObject InitializeWithBuilding(GameObject building)
    {
        temp = Instantiate(building, Camera.main.transform.position + new Vector3(0,0,10), Quaternion.identity).GetComponentInChildren<Building>();
        FollowBuilding();
        
        return temp.gameObject;
    }

    private void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, TileType.Empty);
        TempTilemap.SetTilesBlock(prevArea, toClear);
    }

    private void FollowBuilding()
    {
        ClearArea();

        temp.area.position = gridLayout.WorldToCell(temp.gameObject.transform.position);
        BoundsInt buildingArea = temp.area;

        TileBase[] baseArray = GetTilesBlock(buildingArea, MainTilemap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == tileBases[TileType.White] || 
                baseArray[i] == tileBases[TileType.Yellow])
            {
                tileArray[i] = tileBases[TileType.Green];
            }
            else
            {
                FillTiles(tileArray, TileType.Red);
                break;
            }
        }

        TempTilemap.SetTilesBlock(buildingArea, tileArray);
        prevArea = buildingArea;
    }

    public void SetBuilding()
    {
        if (temp.CanBePlaced())
        {
            temp.Place();
        }
    }

    public void CancelBuilding()
    {
        ClearArea();
        Destroy(temp.gameObject);
    }

    public bool CanTakeArea(BoundsInt area)
    {
        TileBase[] baseArray = GetTilesBlock(area, MainTilemap);
        foreach (var b in baseArray)
        {
            if (b != tileBases[TileType.White] &&
                b != tileBases[TileType.Yellow])
            {
                Debug.Log("Cannot place here");
                return false;
            }
        }

        return true;
    }

    public void TakeArea(BoundsInt area)
    {
        SetTilesBlock(area, TileType.Empty, TempTilemap);
        SetTilesBlock(area, TileType.Red, MainTilemap);
    }

    #endregion

    public enum TileType
    {
        Empty,
        White,
        Green,
        Yellow,
        Red,
    }
}
