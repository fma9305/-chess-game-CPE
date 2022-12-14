Using system;
Using UnityEngine;
public class Chessboard : MonoBehaviour
{
[Header("Art stuff")]
[SerializeField] private Material tileMaterial;
[SerializeField] private float tileSize = 1.0f;
[SerializeField] private float yOffset = 0.2f;
[SerializeField] private Vector3 boardcenter = Vector3.zero;
[SerializeField] private float deathSize = 0.5f;
[SerializeField] private float deathSpacing = 0.3725f;
[SerializeField] private float dra.gOffset = 1f;
[SerializeField] private GameObject victoryScreen;
[Header ("Prefabs & Materials")]
[SerializeField] private GameObject[] prefabs;
[SerializeField] private Material[] teamMaterials;

// LOGIC
private ChessPiece[,] chessPieces;
private ChessPiece currentlyDragging;
private List<Vector2Int> availableMoves = new List<Vector2Int> ();
private List<ChessPiece> deadWhites = new List<ChessPiece> ();
private List<ChessPiece> deadBlacks = new List<ChessPiece> ();
private const int TILE _COUNT X = 8;
private const int TILE_COUNT_Y = 8;
private GameObject[,] tiles;
private Vector2Int currentHover;
private Vector3 bounds;
private bool isWhiteTurn;

private void Awake()
{
isWhiteTurn= true;
GenerateAllTiles(tileSize, TILE_COUNT_X, TILE_COUNT _Y);
SpawnAllPieces();
PositionAllPieces();

}
private void Update()
{
if (!currentCamera)
{
currentCamera = Camera.main;
return;
}
RaycastHit info;
Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
If (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile”,“Hover”, “Highlight” )))

{
//get the indexes of the tile i hit//
Vector2Int hitPosition =LookupTileIndex(info.transform.gameObject);

//if were hovering after not hovering//
if (currentHover == -Vector2Int.one)
{
currentHover = hitPosition;
tiles[hitPosition.x, hitPosition.y].layer =LayerMask.NameToLayer ( "Hover");

}
// if were already hovering a tile, change the previous one//
if (currentHover != hitPosition)
{ 
tiles[currentHover.x, currentHover.y].layer =(ContainsValidMove(ref availableMoves, currentHover)) ? LayerMask.NameToLayer ("Highlight") : LayerMask.NameToLayer ("Tile");

currentHover = hitPosition;
tiles[hitPosition.x, hitPosition.y].layer =LayerMask.NameToLayer ( "Hover");

}
// If we press down on the mouse
if (Input.GetMouseButtonDown(0))
{
if (chessPieces [hitPosition.x, hitPosition.y] != null)
{
// Is it our turn?

if ((chessPieces [hitPosition.x, hitPosition.y].team == 0 && isWhiteTurn) || (chessPieces [hitPosition.x, hitPosition.y].team == 1 && !isWhiteTurn))

{
currentlyDragging = chessPieces [hitPosition.x, hitPosition.y];

// Get a list of where i can go, highlight tiles as well

availableMoves = currentlyDragging-GetAvailableMoves(ref chessPieces, TILE_COUNT_X, TILE_COUNT_Y) ;

HighlightTiles ();
}
}
}

// If we are releasing the mouse button
if (currentlyDragging != null && Input.GetMouseButtonUp (0))
{
Vector2Int previousPosition = new Vector2Int(currentlyDragging current, currentlyDragging-currentY) ;

bool validMove = MoveTo (currentlyDragging, hitPosition.x, hitPosition.y);

if (!validMove)
currentlyDragging.setPosition(GetTileCenter(previousPosition.x, previousPosition.y));
currentlyDragging = null;
RemoveHighlightTiles();
}

else
{
If (currentHover != -Vector2Int.one)
{
tiles[currentHover.x, currentHover.y].layer=(ContainsValidMove(ref availableMoves, currentHover)) ? LayerMask.NameToLayer ("Highlight") : LayerMask.NameToLayer ("Tile");

currentHover = -Vector2Int.one;

}
if (currentlyDragging && Input.GetMouseButtonUp(0))
{

currentlyDragging.SetPosition(GetTileCenter(currentlyDragging.currentX, currentlyDragging.currentY));

currentlyDragging = null;
RemoveHighlightTiles();
}
}
// If we're dragging a piece
if (currentlyDragging)
{
Plane horizontalPlane = new Plane(Vector3.up, Vector3.up * yOffset);

float distance = 0.0f;
if (horizontalPlane.Raycast (ray, out distance))


currentlyDragging.SetPosition(ray GetPoint(distance)+ Vector3.up * dragOffset ) ;

}
}
}

//GENERATE BOARD
private void GenerateAllTiles(float tileSize, int tileCountX, int tileCounty)
{
yOffset += transform.position.y:
bounds = new Vector3((tileCountX / 2) * tileSize, 0, (tileCountX / 2) * tileSize) + boardCenter;

tiles = new GameObject[tileCountX, tileCountY];
for (int x = 0; x < tileCountX; x++)
for (int y = 0; y < tileCountY; y++)
tiles[x, y] = GenerateSingleTile(tileSize, x,.y);



}
private GameObject GenerateSingleTile(float tileSize, int x, int y)
{
GameObject tileObject = new GameObject (string.Format("X: (0), Y:(1}", x, y)) ;

tileObject.transform.parent = transform;
Mesh mesh = new Mesh();
tileObject.AddComponent<MeshFilter>().mesh = mesh;
tileobject.AddComponent<MeshRenderer>().material = tileMaterial;
Vector3[ ] vertices = new Vector3[4];
vertices[0] = new Vector3(x * tileSize, yOffset, y * tileSize) - bounds;

vertices[1] = new Vector3(x * tileSize, yOffset, (y+1) * tileSize) - bounds;

vertices [2] = new Vector3((x+1) * tileSize, yOffset, y * tileSize) - bounds;

vertices [3] = new Vector3((x+1)* tileSize, yOffset , (y+1) * tileSize) - bounds;

int[] tris = new int[] { 0, 1, 2, 1, 3, 2 };
mesh.vertices = vertices;
mesh.triangles = tris;
mesh.RecalculateNormals();
tileObject.layer = LayerMask.NameToLayer(“Tile”);
tileObject.AddComponent<BoxCollider>();

return tileObject;
}

//SPAWNING OF THE PIECES
private void SpawnAllPieces ()
{
chessPieces = new ChessPiece [TILE_COUNT_X, TILE_COUNT_Y];

int whiteTeam = 0, blackTeam = 1;
// White team
chessPieces [0, 0] = SpawnSinglePiece(ChessPieceType.Rook, whiteTeam) ;

chessPieces [1, 0] = SpawnSinglePiece(ChessPieceType.Knight, whiteTeam);

chessPieces [2, 0] = SpawnSinglePiece(ChessPieceType.Bishop, whiteTeam);

chessPieces [4, 0] = SpawnSinglePiece(ChessPieceType.King, whiteTeam);

chessPieces [3, 0] = SpawnSinglePiece(ChessPieceType.Queen, whiteTeam);

chessPieces [5, 0] = SpawnSinglePiece(ChessPieceType.Bishop, whiteTeam);

chessPieces [6, 0] = SpawnSinglePiece(ChessPieceType.Knight, whiteTeam);

chessPieces [7, 0] = SpawnSinglePiece(ChessPieceType.Rook, whiteTeam) ;

for (int i = 0; 1 < TILE_COUNT_X; i++)
chessPieces [i, 1] = SpawnSinglePiece(ChessPieceType.Pawn, whiteTeam) ;

// Black team
chessPieces [0, 7] = SpawnSinglePiece(ChessPieceType.Rook,blackTeam) ;

chessPieces [1, 7] = SpawnSinglePiece(ChessPieceType.Knight,blackTeam);

chessPieces [2, 7] = SpawnSinglePiece(ChessPieceType.Bishop, blackTeam);

chessPieces [4, 7] = SpawnSinglePiece(ChessPieceType.King,blackTeam);

chessPieces [3, 7] = SpawnSinglePiece(ChessPieceType.Queen,blackTeam);

chessPieces [5, 7] = SpawnSinglePiece(ChessPieceType.Bishop,blackTeam);

chessPieces [6, 7] = SpawnSinglePiece(ChessPieceType.Knight,blackTeam);

chessPieces [7, 7] = SpawnSinglePiece(ChessPieceType.Rook,blackTeam) ;

for (int i = 0; 1 < TILE_COUNT_X; i++)

chessPieces [i, 6] = SpawnSinglePiece(ChessPieceType.Pawn, blackTeam) ;
}

private ChessPiece SpawnSinglePiece (ChessPieceType type, int team)
{
ChessPiece cp = Instantiate(prefabs [ (int)type - 1], transform).GetComponent<ChessPiece>();
cp.type = type;
cp.team = team;
cp.GetComponent‹MeshRenderer›().material = teamMaterials [ ( (team == 0)? 0 : 6) + ( (int)tlpe - 1)1;
return cp;
}

// POSITIONING
private void PositionAllPieces ()
{
for (int x = 0; x < TILE_COUNT_X; X++)
for (int y = 0; y < TILE_COUNT_Y; y++)
if (chessPieces [x, y] != null)
PositionSinglePiece(X, y, true);

}
private void PositionSinglePiece(int x, int y, bool force = false)
{
chessPieces [x, y].currentX = x;
chessPieces [x, y]-currentY = y;
chessPieces [x, y].setPosition(GetTileCenter( x, y), force);
}
private Vector GetTileCenter(int x, int y)
{
return new Vector3(x * tileSize, yOffset, y * tileSize) - bounds + new Vector3(tileSize / 2, 0, tileSize / 2);
}

// HIGHLIGHT TILES
private void HighlightTiles ()
{
for (int i = 0; i < availableMoves.Count; i++)
tiles [availableMoves [i] x, availableMoves [i] y].layer =LayerMask.NameToLayer("Highlight");
}
private void RemoveHighlightTiles ()
{
for (int i = 0; i ‹ availableMoves.Count; i++)
tiles[availableMoves[i].x, availableMoves [i].y].layer =LayerMask.NameToLayer ("Tile");
availableMoves.Clear ();
}

//CHECKMATE
private void CheckMate (int team)
{
DisplayVictory(team);
}
private void DisplayVictory(int winningTeam)
{
victoryScreen.setActive(true);

victoryScreen.transform.GetChild(winningTeam).gameObject.SetActive(true);
}
public void OnResetButton ( )
{
// UI
victoryScreen.transform.GetChild(0).gameObject.SetActive(false);
victoryScreen.transform.GetChild(1)-gameObject.SetActive(false);
victoryScreen.SetActive(false);
// Fields reset
currentlyDragging = null;
availableMoves = new List<Vector2Int> () ;

// Clean up
for (int x = 0; × < TILE_COUNT_X; X++)
{
for (int y = 0; y < TILE_COUNT_Y; y++)
{
if (chessPieces [x, y] != null)
Destroy (chessPieces [x, y] gameObject);
chessPieces[x, y] = null;
}
}
for (int i = 0; i < deadWhites.Count; i++)
Destroy (deadWhites [i]-gameObject);
for (int i = 0; i ‹ deadBlacks.Count; i++)
Destroy(deadBlacks[i]-gameObject);
deadWhites.Clear();
deadBlacks.Clear();
SpawnAllPieces () ;
PositionAllPieces();
isWhiteTurn = true;
}
public void OnExitButton()
{
Application.Quit();
}

//OPERATIONS
private bool ContainsValidMove(ref List<Vector2Int> moves, Vectorpos)
{
for (int 1 = 0; 1 < moves.Count; i++)
if (moves [i].x == pos.× && moves [i].y == pos.y)
return true;

return false;
}

private bool MoveTo (ChessPiece cp, int x, int y)
{
if (! ContainsValidMove(ref availableMoves, new Vector2(x, y)))
return false;
Vector2Int previousPosition = new VectorInt (cp.currentX, cp.currentY) ;

// Is there another piece on the target position?
if (chessPieces [x, y] != null)
{
ChessPiece op = chessPieces [x, y];
if (cp.team == ocp.team)
return false;
// If its the enemy team
if (ocp.team == 0)
{
if (ocp.type == ChessPieceType.King)
CheckMate (1) ;
deadWhites.Add(ocp);
ocp.SetScale(Vector3.one * deathSize);
ocp.SetPosition(new Vector3(8 * tileSize, yOffset, -1 * tileSize)
- bounds
+ new Vector3(tileSize / 2, 0, tileSize / 2)
+ (Vector3.forward * deathSpacing) *deadWhites.Count);
}
else
{
if (ocp.type == ChessPieceType.King)
CheckMate (0) ;
deadBlacks.Add(op);
op.SetScale(Vector3.one * deathSize);
ocp.SetPosition(new Vector3(-1 * tileSize, yOffset, 8 * tileSize)
- bounds
+ new Vector3(tileSize / 2, 0, tileSize / 2)

+ (Vector3.Black * deathSpacing) * deadBlacks.Count);
}
}
chessPieces [x, y] = cp;
chessPieces [previousPosition.x, previousPosition.y] = null;
PositionSinglePiece(x, y) ;
isWhiteTurn=! isWhiteTurn ;

return true;
}
privateVector2Int LookupTileIndex(GameObject hitInfo)
{
for (int x = 0; x < TILE_COUNT_X; x++)
for (int y = 0; y < TILE_COUNT_Y; y++)
if (tiles[x, y]== hitInfo)
return new Vector2Int(x, y);

return -Vector2Int.one; // Invalid
}

}