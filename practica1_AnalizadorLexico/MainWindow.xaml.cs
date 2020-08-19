using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace practica1_AnalizadorLexico
{
	/// <summary>
	/// Lógica de interacción para MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Analizador analizador = new Analizador();		
		
		public MainWindow()
		{			
			InitializeComponent();						
		}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
			lbl_resultado.Content = "";
			if (txt_informacion.Text!=null) {
				analizador.analizadorLexico(txt_informacion.Text);
				lbl_resultado.Content = analizador.mostrarResultado();
			}						
		}

    }
}
