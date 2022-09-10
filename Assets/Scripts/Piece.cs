using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    float lastFall = 0.0f;                                                          //Miliseundos desde la ultima caida.

    void Start()                                                                    //Al empezar...
    {
        if(!IsValidPiecePosition())                                                 //Si la posicion no es valida...
        {
            Debug.Log("Game Over");                                                 //Mostrar letrero por consola
            Destroy(this.gameObject);                                               //Destruir el gameobject
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))                                     //Si pulso la flecha izquierda...
        {
            MovePieceHorizontally(-1);                                              //Muevo la pieza a la izquierda.
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))                               //Si pulso la flecha derecha...
        {
            MovePieceHorizontally(1);                                               //Muevo la pieza a la derecha.
        }   
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {                                 //Si pulso la lecha hacia arriba...
                this.transform.Rotate(0,0,-90);                                     //Roto en sentido antihorariio 90 grados.
                if(IsValidPiecePosition())                                          //Si la posicion es valida...
                {
                    UpdateGrid();                                                   //Actualizo la parrilla visualmente.
                }
                else                                                                //De otra forma...
                {
                    this.transform.Rotate(0,0,90);                                  //Contrarresto el movimiento.
                }
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow)||(Time.time-lastFall)>1.0f)     //Si se presiona la flecha hacia abajo o el tiempo que ha pasado desde que inicio la partida menos la ultima caida es mayor a un segundo...
        {
            this.transform.position += new Vector3(0,-1,0);                         //Muevo la pieza una posicion hacia abajo.
            if(IsValidPiecePosition())                                              //Si la posicion es valida...
            {
                UpdateGrid();                                                       //Actualizo la parrilla visualmente.
            }
            else                                                                    //De otra forma...
            {
                this.transform.position += new Vector3(0,1,0);                      //Contrarresto el movimiento.
                GridHelper.DeleteAllFullRows();                                     //Mando llamar al metodo que elimina todas las filas que se hayan podido completar al bajar esta pieza.
                FindObjectOfType<PieceSpawner>().SpawnNextPiece();                  //Encuentro el unico objeto del tipo piecespawner para que ejecute el codigo que invoca la siguiente pieza.
                this.enabled=false;                                                 //Deshabilito el script para esta pieza para que no se mueva cuando aparezca la siguiente.
            }
            lastFall = Time.time;                                                   //"Toma una foto" del instante exacto donde la pieza cayo.
        }
    }

    void MovePieceHorizontally(int direction)
    {
        this.transform.position += new Vector3(direction,0,0);                      //Muevo la pieza una posicion a la izquierda.
            if(IsValidPiecePosition())                                              //Si la posicion es valida
            {
                UpdateGrid();                                                       //Actualizo la parrilla (visualmente)
            }
            else                                                                    //De otra forma...
            {
                this.transform.position += new Vector3(-direction,0,0);             //Contrarresto el movimiento.
            }
    }

    bool IsValidPiecePosition()                                                     //Metodo para verificar si cada bloque de la pieza puede pasar a la siguiente posicion
    {
        foreach(Transform block in this.transform)                                  //Para cada uno de los hijos del transform...
        {
            Vector2 pos = GridHelper.RoundVector(block.position);                   //Redondeo la posicion de cada uno de los bloques.
            if(!GridHelper.IsInsideBorders(pos))                                    //Si la pieza esta fuera dentro de los bordes...
            {
                return false;                                                       //Esta posicion no es valida.
            }
            Transform possibleObject = GridHelper.grid[(int)pos.x,(int)pos.y];      //Variable donde convierto las coordenadas en enteros.
            if(possibleObject != null && possibleObject.parent != this.transform)   //Si no hay un espacio nulo abajo de mi pieza y si el padre del objeto posible no es la propia pieza...
            {
                Debug.Log("La posicion no es valida.");                             //(NOTA: primero se tiene que verificar que posibleObject no sea null, primero tiene que existir para despues verificar si dicho objeto pertenece a la propia pieza.)
                return false;                                                       //Entonces la pieza no puede bajar mas.
            }                                        
        }
        return true;                                                                //Si el bucle se completa satisfactoriamente, entonces la pieza si puede bajar a la siguiente posicion.
    }
                                                                                    //ESTE METODO FUNCIONA IGUAL PARA LA CANTIDAD DE PIEZAS QUE SEAN > o < a 4.
    void UpdateGrid()                                                               //Metodo que actualiza la parrilla una vez que es posible mover la pieza (Un bucle para borrar la pieza, y otro para aparecer la pieza una posicion en y mas abajo)
    {
        for(int y=0; y<GridHelper.height; y++)                                      //Bucle para cada bloque de las filas desde la primera hasta la ultima...
        {
            for(int x=0; x<GridHelper.width; x++)                                   //Bucle para cada bloque de las columnas desde la primera hasta la ultima...
            {
                if(GridHelper.grid[x,y] != null)                                    //...Comprobamos SI el espacio es diferente de nulo...
                {
                    if(GridHelper.grid[x,y].parent == this.transform)               //...SI el padre del bloque es la pieza del propio script...
                    {
                        GridHelper.grid[x,y] = null;                                //Entonces ahora esos X espacios estan vacios. (X porque voy a desaparecer las X piezas guardadas en la parrilla de esta pieza)
                    }
                }
            }
        }

        foreach(Transform block in this.transform)                                  //Bucle para cada uno de los bloques del objeto actual
        {
            Vector2 pos = GridHelper.RoundVector(block.position);                   //Redondeo la posicion de los X hijos(bloques) por si hubiera algun decimal.
            GridHelper.grid[(int)pos.x, (int)pos.y] = block;                        //Y una vez redondeada, uso la informacion para guardar en la fila y columna correspondiente esos bloques.
        }
    }
}
