using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour
{
    public GameObject[] levelPieces;                                                            //Array que contiene los prefabs de las piezas.
    public GameObject currentPiece, nextPiece;                                                  //Variable de pieza actual y la siguiente pieza.        

    private void Start()                                                                        //Al iniciar la partida...
    {
        nextPiece = Instantiate(levelPieces[0], this.transform.position, Quaternion.identity);  //Se instancia la pieza en la posicion 0 del arreglo
        SpawnNextPiece();                                                                       //...Aparecer una nueva pieza.
    }

    public void SpawnNextPiece()                                                                //Metodo para aparecer la siguiente pieza.
    {
        currentPiece = nextPiece;                                                               //La pieza actual es la siguiente
        currentPiece.GetComponent<Piece>().enabled = true;                                      //De la pieza actual se obtiene el componente pieza y se habilita.
        StartCoroutine("PrepareNextPiece");                                                     //Empieza la corutina de preparacion para la siguiente pieza        
    }

    IEnumerator PrepareNextPiece()
    {
        yield return new WaitForSecondsRealtime(3.0f);                                          //Se espera 3 segundos antes de spawnear la siguiente pieza
        int i = Random.Range(0, levelPieces.Length);                                            //Variable que guardara un valor entre 0 y la cantidad de piezas.
        nextPiece = Instantiate(levelPieces[i], this.transform.position, Quaternion.identity);  //Instanciar la pieza que corresponde al valor de la variable en la posicion del objeto PieceSpawner.
        nextPiece.GetComponent<Piece>().enabled = false;                                        //La pieza que esta en espera para ser utilizada se deshabilita.
    }

}
