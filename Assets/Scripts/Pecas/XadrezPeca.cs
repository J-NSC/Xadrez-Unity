using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChessPieceType{
    None = 0,
    Pawn = 1,
    Rook = 2,
    Knight = 3,
    Bishop = 4,
    Queen = 5,
    King = 6
}

public class XadrezPeca : MonoBehaviour
{
    public int team;
    public int currentX;
    public int currentY;
    public ChessPieceType type;


    private Vector3 desiredPosition;
    private Vector3 desiredSacle = Vector3.one;

    private void Start() {
        transform.rotation = Quaternion.Euler((team == 0) ? Vector3.zero : new Vector3(0,180,0));
    }

    private void Update() {
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 10);
        transform.localScale = Vector3.Lerp(transform.localScale, desiredSacle, Time.deltaTime * 10);
    }

    public virtual void setPosition(Vector3 pos, bool force = false){
        desiredPosition = pos;
        if(force){
            transform.position = desiredPosition;
        }
        
    }

    public virtual void setScale(Vector3 scale, bool force = false){
        desiredSacle = scale;
        if(force){
            transform.localScale = desiredSacle;
        }
    }

    public virtual List<Vector2Int> GetAvaliabeMoves(ref XadrezPeca[,] board, int tileCountX, int tileCountY){
        List<Vector2Int> r = new List<Vector2Int>();

        r.Add(new Vector2Int(3,3));
        r.Add(new Vector2Int(3,4));
        r.Add(new Vector2Int(4,3));
        r.Add(new Vector2Int(4,4));

        return r; 
    }

    public virtual SpecialMove GetSpecialMoves(ref XadrezPeca[,] board, ref List<Vector2Int[]> moveList, ref List<Vector2Int> availableMoves){
        return SpecialMove.None;
    }

}


