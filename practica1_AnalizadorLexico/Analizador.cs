using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace practica1_AnalizadorLexico
{
    class Analizador
    {
        ListaEnlazada<String> listaPalabras = new ListaEnlazada<string>();
        ListaEnlazada<String> listaMonedas = new ListaEnlazada<string>();
        ListaEnlazada<String> listaEnteros = new ListaEnlazada<string>();
        ListaEnlazada<String> listaDecimales = new ListaEnlazada<string>();
        ListaEnlazada<String> listaErroneas = new ListaEnlazada<string>();
     


        public void analizadorLexico(String objetoDeAnalisis) {
            limpiarListas();
            anadirTitulos();
            String[] conjuntoPalabras = agrupador(objetoDeAnalisis);

            for (int conjuntoActual = 0; conjuntoActual < conjuntoPalabras.Length; conjuntoActual++) {
                agruparConjuntos(clasificador(separadorCaracteres(conjuntoPalabras[conjuntoActual])),
                    conjuntoPalabras[conjuntoActual]);
            }
        }


        /*
         Este método es el encargado de separar
         a los conjuntos de caracteres que se en-
         cuentren en una oración ingresada por el 
         usuario
         */
        private String[] agrupador(String oracion) {
            oracion = oracion.TrimStart();
            String[] conjuntosPalabras = oracion.Split(' ');

            return conjuntosPalabras;
        }

        /*
         Encargado de separar en las más mínimas 
         partes a una palabra en específico, es 
         decir en sus caracteres
         */

        private char[] separadorCaracteres(String palabra) {
            char[] caracteres = palabra.ToCharArray(0, palabra.Length);

            return caracteres;
        }

        /*
         Este será el método encargado de dar el tipo del
         caracter según el intervalo en el que se encuentre
         su valor ASCII correspondiente
          puede devolver
             l: letra, n: numero, p: punto, i: invalido
         */
        private char determinardorTipoCaracter(char caracter) {
            char tipoCaracter = 'i';
            //subgrupo del alfabeto compuesto por las letras, digitos y el punto "."
            if (((int)caracter >= 65 && (int)caracter <= 90) || ((int)caracter >= 97 && (int)caracter <= 122)) {//o hubiera podido convertir todo a minus o mayus y luego cuando quisieras lo de la Q entonces solo comparaías con ese caracter en concreto xD :v
                tipoCaracter = 'l';
            }

            if ((int)caracter >= 48 && (int)caracter <= 57) {
                tipoCaracter = 'n';
            }

            if ((int)caracter == 46) {
                tipoCaracter = 'p';
            }

            return tipoCaracter;
        }

        /*
          Este método será al que se le pasará una palabra 
          por vez hasta que se terminen, de tal manera que
          pueda analizar una por una y así determinar la 
          clasificación a la que corresponde
         */
        private String clasificador(char[] palabraDeAnalisis) {
            String clasificacionDeGrupo = "invalida";
            String numero = "entero";
            Boolean necesitaMas = false;
            char tipoDeberiaVenir;

            //estudio el primer caracter de forma diferente porque si inicio mal la palabra... para que voy a gastar esfuerzos xD
            clasificacionDeGrupo= definirClasificacionInicial(tipoDeberiaVenir = determinardorTipoCaracter(palabraDeAnalisis[0]));//pues si es una letra o número lo primero que se esperaría es que viniera una letra ó número xD

            if (tipoDeberiaVenir.CompareTo('i') != 0 && tipoDeberiaVenir.CompareTo('p') != 0) {
                for (int caracterActual = 1; caracterActual < palabraDeAnalisis.Length; caracterActual++)//pues ya se evaluó el primer caracter y yo quiero saber que caracter es el que debe venir
                {
                    switch (tipoDeberiaVenir) {
                        case 'l':
                            tipoDeberiaVenir = determinardorTipoCaracter(palabraDeAnalisis[caracterActual]);//no puedo colocarlo arriba, sino estaría tomaría el tipo del segundo caracter y no puedo estudiar al caracter en la posición 0 porque sino la estructura del análisis hecho cambia 

                            if (tipoDeberiaVenir == 'l')
                            {
                                clasificacionDeGrupo = "palabra";
                                tipoDeberiaVenir = 'l';//este es el valor esperado para el caracter siguiente, el cual es verificado, en la ronda siguiente por estos dos bloques

                            } else if (caracterActual == 1 && palabraDeAnalisis[0] == 'Q' && //lo del caracter actual es porque solo en el segundo espacio es posible que aparezca un caracter diferente a la letra que sea un # o el ".", de lo contrario ya no es palabra... se arruinó xD 
                             (tipoDeberiaVenir == 'n' || tipoDeberiaVenir == 'p'))
                            {
                                clasificacionDeGrupo = "moneda";
                                tipoDeberiaVenir = 'n';
                                necesitaMas = true;
                            }
                            else {
                                clasificacionDeGrupo = "invalida";
                                caracterActual = palabraDeAnalisis.Length;//pues ya se arruinó todo para ese conjunto xD, asi que ya no hay que seguir xD
                            }

                            break;

                        case 'n'://recuerda que en esta situación el único caracter diferente a número puede ser un punto SI es que no se había catalogado al número como decimal
                            tipoDeberiaVenir = determinardorTipoCaracter(palabraDeAnalisis[caracterActual]);

                            if (tipoDeberiaVenir == 'n') {
                                if (clasificacionDeGrupo.CompareTo("invalida") == 0) {
                                    clasificacionDeGrupo = numero;//pues si es moneda se complica un poquis estarlo asignando desde aquí, mejor hasta el final.. solo existen 2 casos en los que la clasif llevará número, cuando desde el inicio se tenga al caracter numérico, cuando sea moneda...[ya sea ent o deci pero tendrá concat a numero]
                                }
                                necesitaMas = false;
                                tipoDeberiaVenir = 'n';//solo esto es necesario porque al terminar el ciclo se revisará que se tiene en la clasificación, si es moneda entonces se concatenará lo que en numero se encuentre y 
                            } else if (tipoDeberiaVenir == 'p' && numero.CompareTo("decimal") != 0) {//pues quiere decir que en ningún momento se ha asignado otro "." y si llegó hasta aquí, la estructura va bien así que no hay nada más por revisar que esto...
                                numero = "decimal";
                                tipoDeberiaVenir = 'n';
                                necesitaMas = true;
                                if (clasificacionDeGrupo.CompareTo("entero") == 0) {//pues solo puede estar deci en la clasificación por dos situaciones, porque el número al final era decimal, ó porque la moneda es decimal... pero recuerda que lo de moneda se asigna hasta el final, porque sino se estaría concat a lo concat y ahí se arruinaría todo lo de moneda
                                    clasificacionDeGrupo = numero;
                                }
                            } else {//ya sea que venga letra o un punto, que en este caso no son aceptados... ya sea porque no llegaron a tiempo o porque no tienen cabida...
                                clasificacionDeGrupo = "invalida";
                                caracterActual = palabraDeAnalisis.Length;
                            }
                            break;

                        default://esto lo tengo aquí por el hecho de que pueden aparecer más tipos de caracteres que no permitirían entrar a ninguno de los casos aceptados
                            clasificacionDeGrupo = "invalida";
                            caracterActual = palabraDeAnalisis.Length;
                            break;
                    }
                }
            }

            if (clasificacionDeGrupo.CompareTo("moneda") == 0) {
                clasificacionDeGrupo += " " + numero;
            }

            if (necesitaMas == true) {
                clasificacionDeGrupo = "invalida";//hubiera sido bonito que hubiera una sola var para que dependiendo de donde estaba se le asignara inválida o válida y que dependiendo de este valor se agrupara en la clasificaicon de erradas o en la que se había almacenado hasta el momento...
            }
            return clasificacionDeGrupo;
        }

        /*
          Este método se encarga de colocar a los conjuntos de 
          caracteres que se encontraron el la información ingre
          sada en el arreglo que corresponde a su tipo hallado
         */
        private void agruparConjuntos(String tipoLista, String dato) {

            if (tipoLista.StartsWith("moneda")){
                //aquí iría lo de moneda, y recuerda que debes separar a la cantidad para pasarla al listado de númeos correspondiente y además mandarla a moneda sin importar que tipo de número es... debes usar algo como startsWith
                String listaNumero = tipoLista.Substring(7);
                listaMonedas.anadirAlFinal(dato);
                tipoLista = listaNumero;

                if (dato.Contains(".")){
                    dato = dato.Substring(2);
                }
                else {
                    dato = dato.Substring(1);
                }
            }
            if (tipoLista.CompareTo("palabra") == 0) {
                listaPalabras.anadirAlFinal(dato);

            } else if (tipoLista.CompareTo("entero") == 0) {
                listaEnteros.anadirAlFinal(dato);
            } else if (tipoLista.CompareTo("decimal") == 0) {
                listaDecimales.anadirAlFinal(dato);

            } else if (tipoLista.CompareTo("invalida")==0) {
                listaErroneas.anadirAlFinal(dato);

            }


        }//fin del método agrupador

        public String definirClasificacionInicial(char tipo) {
            if (tipo=='l') {
                return "palabra";
            }
            if (tipo == 'n') {
                return "entero";
            }

            return "invalida";
        }

        public String mostrarResultado(){
            Nodo<String> nodoAuxiliar1 = listaPalabras.obtnerPrimerNodo();
            Nodo<String> nodoAuxiliar2 = listaMonedas.obtnerPrimerNodo();
            Nodo<String> nodoAuxiliar3 = listaEnteros.obtnerPrimerNodo();
            Nodo<String> nodoAuxiliar4 = listaDecimales.obtnerPrimerNodo();
            Nodo<String> nodoAuxiliar5 = listaErroneas.obtnerPrimerNodo();

            String Resultado="";
            for (int dato = 0; dato < listaPalabras.darTamanio(); dato++)
            {
                if (listaPalabras.darTamanio()>=2)
                {                    
                    Resultado += nodoAuxiliar1.darContenido()+", ";
                    nodoAuxiliar1 = nodoAuxiliar1.darSiguiente();
                    if (dato == (listaPalabras.darTamanio() - 1))
                    {
                        Resultado += "\n\n";
                    }
                }
                
            }


            for (int dato = 0; dato < listaMonedas.darTamanio(); dato++)
            {
                if (listaMonedas.darTamanio()>=2) {
                    Resultado += nodoAuxiliar2.darContenido() + ", ";
                    nodoAuxiliar2 = nodoAuxiliar2.darSiguiente();
                    if (dato == (listaMonedas.darTamanio() - 1))
                    {
                        Resultado += "\n\n";
                    }
                }
            
            }

            for (int dato = 0; dato < listaEnteros.darTamanio(); dato++)
            {
                if (listaEnteros.darTamanio()>=2) {
                    Resultado += nodoAuxiliar3.darContenido() + ", "; 
                    nodoAuxiliar3 = nodoAuxiliar3.darSiguiente();
                    if (dato == (listaEnteros.darTamanio() - 1))
                    {
                        Resultado += "\n\n";
                    }
                }
              
            }

            for (int dato = 0; dato < listaDecimales.darTamanio(); dato++)
            {
                if(listaDecimales.darTamanio()>=2)
                {
                    Resultado += nodoAuxiliar4.darContenido() + ", ";
                    nodoAuxiliar4 = nodoAuxiliar4.darSiguiente();
                    if (dato == (listaDecimales.darTamanio() - 1))
                    {
                        Resultado += "\n\n";
                    }
                }
              
            }

            for (int dato = 0; dato < listaErroneas.darTamanio(); dato++)
            {
                if (listaErroneas.darTamanio()>=2) {
                    Resultado += nodoAuxiliar5.darContenido() + ", ";
                    nodoAuxiliar5 = nodoAuxiliar5.darSiguiente();
                    if (dato == (listaErroneas.darTamanio() - 1))
                    {
                        Resultado += "\n\n";
                    }
                }               
            }

            return Resultado;
        }


        public void anadirTitulos()
        {
            listaPalabras.anadirAlFinal("-> Palabras");
            listaMonedas.anadirAlFinal("->Monedas");
            listaEnteros.anadirAlFinal("-> Enteros");
            listaDecimales.anadirAlFinal("-> Decimales");
            listaErroneas.anadirAlFinal("-> Erroneas");
        }

        public void limpiarListas()
        {
            listaPalabras.limpiarLista();
            listaMonedas.limpiarLista();
            listaEnteros.limpiarLista();
            listaDecimales.limpiarLista();
            listaErroneas.limpiarLista();

        }

    }
}
