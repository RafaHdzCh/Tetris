using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHelper : MonoBehaviour
{

    /*

    ESTRUCTURA DE DATOS

    3 |nullnullnullnull
    2 | x  nullnullnull
    1 | x  nullnullnull
    0 | x   x  nullnull
    _ |________________
      | 0 | 1 | 2 | 3 |

    */

    //En el helper todas las variables y metodos son estaticos con el fin de poder ser usadas por otros scripts.
    //No es el codigo de ningun GameObject, complementa otros codigos.

    public static int width = 10, height=30;                            //Variables donde determino el tamano de mi parrilla.
    public static Transform [,] grid = new Transform[width,height];     //Una matriz es un arreglo de arreglos (filas y columnas). Esto generara una parrilla vacia. +4 para que siempre hay sitio para el spawn de la pieza mas larga.
                                                                        //Usamos Transform para guadar las posiciones de cada pieza, no la pieza como tal.
    public static Vector2 RoundVector(Vector2 v)                    
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));     //Devuelve el entero mas cercano. Redondeamos las posiciones en "x" y en "y" de las piezas para que las coordenadas sean exactas.
    }

    public static bool IsInsideBorders(Vector2 pos)                 //No debe ser posible que una ficha se salga de las paredes.
    {
        if(pos.x >= 0 && pos.y >= 0 && pos.x < width)               //Si la posicion en "x" es mayor o igual que cero y la posicion de "y" es mayor o igual a cero y la posicion de "x" es menor que la anchura...
        {
            return true;                                            //...Entonces estas dentro de los bordes.
        }
        else
        {
            return false;                                           //De lo contrario, no se permite hacer el movimiento(Estas en el limite).
        }
    }

    public static void DeleteRow(int y)                             //Una fila(y) debe ser eliminada si esta completa.
    {
        for(int x=0; x<width; x++)                                  //Para cada una de las columnas de la fila actual...
        {
            Destroy(grid[x,y].gameObject);                          //Destruimos el GameObject que se encuentre en cada posicion de la fila
            grid[x,y] = null;                                       //Una vez destruido, el espacio que tenian ahora es nulo.
        }
    }

    public static void DecreaseRow(int y)                           //Una fila(y) debe bajar a partir de la fila que ya fue destruida.
    {
        for(int x=0; x<width; x++)                                  //Para cada una de las columnas de la fila...
        {
            if(grid[x,y]!=null)                                     //Si la posicion a la que quiero ir, no es nula, muevo la ficha a
            {
                grid[x,y-1] = grid[x,y];                            //Si estoy en la "y", tengo que ir a "y-1"
                grid[x,y] = null;                                   //Como hemos bajado la pieza, en la fila anterior ahora no debe haber nada.
                grid[x,y-1].position += new Vector3(0,-1,0);        //Ahora bajamos visualmente las piezas para que esto se vea reflejado en pantalla...
            }                                                       //...Para esto solo hay que bajar la posicion en el eje "y" y mantener la posicion en las otras coordenadas.
        }
    }

    public static void DecreaseRowAbove(int y)                      //Decrementamos las filas(y) que estan arriba a partir de una fila dada.
    {
        for(int i = y; i<height; i++)                               //Para cada una de las filas...
        {
            DecreaseRow(i);                                         //Mandamos llamar el metodo que baja filas gracias al diseno modular del programa.
        }
    }

    public static bool IsRowFull(int y)                             //Es necesario saber si la fila(y) esta llena.-
    {
        for(int x=0; x<width; x++)                                  //Para cada fila, empezando en la columna cero hasta la ultima de lqa caja...
        {
            if(grid[x,y] == null)                                   //...Si encuentro un solo espacio vacio...
            {
                return false;                                       //Entonces la fila no esta llena.
            }
        }                                                           //De lo contrario...
        return true;                                                //La fila esta llena.
    }

    public static void DeleteAllFullRows()                          //Tenemos que borrar todas las filas si vamos a reiniciar el juego o si tenemos que borrar multiples filas.
    {
        for(int y=0; y<height; y++)                                 //Para cada fila...
        {
            if(IsRowFull(y))                                        //Si la fila en la posicion "y" esta llena...
            {
                DeleteRow(y);                                       //Mandamos llamar al metodo que elimina filas para que elimine la fila de esa posicion.
                DecreaseRowAbove(y+1);                              //Una vez eliminada, debemos eliminar la fila que esta encima de la que acabamos de eliminar.
                y--;                                                //Hay que volver a probar desde la misma fila debido a que las que estan encima van a bajar, por lo cual no vamos a revisar una fila completa que fue eliminada.
            }
        }
        CleanPieces();
    }

    private static void CleanPieces()                                               //Elimina los GameObject vacios
    {
        foreach(GameObject piece in GameObject.FindGameObjectsWithTag("Piece"))     //Encuentra el objeto con la etiqueta "Pieza"
        {
            if(piece.transform.childCount == 0)                                     //Si su numero de hijos es 0
            {
                Destroy(piece);                                                     //Entonces podemos eliminar ese gameObject vacio
            }
        }
    }
}
