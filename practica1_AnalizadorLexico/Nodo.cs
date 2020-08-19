using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practica1_AnalizadorLexico
{
    class Nodo<T> {

        T contenido;
        Nodo<T> nodoSiguiente;//ya que solo es enlazada, no requiere conocer a su anterior

        public Nodo(T contenidoAInsertar, Nodo<T> siguiente){
            contenido=contenidoAInsertar;
            nodoSiguiente = siguiente;
        }

        public void establecerSiguiente(Nodo<T> siguiente) {
            nodoSiguiente = siguiente;
        }
        public Nodo<T> darSiguiente() {
            return nodoSiguiente;
        }

        public T darContenido() {
            return contenido;
        }
    }
}
