Using System.Collections.Generic; Using UnityEngine;
public class Bishop: ChessPiece {


public override List<Vector2Int> GetAvailableMoves(ref ChessPiece[,] board, int tileCountX, intvtileCountY)
{
List<Vector2Int> r = new List<Vector2Int> () ;
// Top Right
for (int x = currentX + 1, y = currentY + 1; x < tileCountX && y < tileCountY; X++, y++)
{
if (board[x, y] == null)
r.Add (new Vector2Int(x, y)) ; 
else
{
if (board [x, y].team != team)
r.Add (new Vector2Int (x, y) );
break; }
}
// Top Left
for (int x = currentX - 1, y = currentY + 1; x >=0 && y < tileCountY; X--, y++)
{
if (board[x, y] == null)
r.Add (new Vector2Int(x, y)) ;
 else
{
if (board [x, y].team != team)
r.Add (new Vector2Int (x, y) );
break; }
 }
// Bottom Right
for (int x = currentX + 1, y = currentY - 1; x < tileCountX && y >=0 ; X++, y--)
{
if (board[x, y] == null)
r.Add (new Vector2Int(x, y)) ; 
else
{
if (board [x, y].team != team)
r.Add (new Vector2Int (x, y) );
break; }
}
// Bottom Left
for (int x = currentX - 1, y = currentY - 1; x>=0 && y >=0; X--, y--)
{
if (board[x, y] == null)
r.Add (new Vector2Int(x, y)) ; 
else
{
if (board [x, y].team != team)
r.Add (new Vector2Int (x, y) );
break; }
}
return r; }

}