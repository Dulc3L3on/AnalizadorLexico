using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practica1_AnalizadorLexico
{
    class ListaEnlazada<T>{
        Nodo<T> primerNodo;
        Nodo<T> ultimoNodo;
        int tamanioLista=0;

        public ListaEnlazada() {
            primerNodo = ultimoNodo = null;
        }

        public void anadirAlFinal(T contenidoNuevo) {
            if (primerNodo == null){
                primerNodo = ultimoNodo = new Nodo<T>(contenidoNuevo, null);
            }else {
                Nodo<T> nodoAuxiliar = primerNodo;

                while (nodoAuxiliar.darSiguiente() !=null) {
                    nodoAuxiliar = nodoAuxiliar.darSiguiente();
                }

                Nodo<T> nuevoNodo = new Nodo<T>(contenidoNuevo, null);
                nodoAuxiliar.establecerSiguiente(nuevoNodo);
                ultimoNodo = nuevoNodo;
            }

            tamanioLista++;
        }

        public Nodo<T> obtnerPrimerNodo() {
            return primerNodo;
        }

        public Boolean estaVacia() {
            return (primerNodo==null);
        }

        public void limpiarLista() {
            primerNodo = ultimoNodo = null;
            tamanioLista = 0;
        }

        public int darTamanio() {
            return tamanioLista;
        }

        
    }
}
