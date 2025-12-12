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
using VideoGameCatalog.Database;
using VideoGameCatalog.Models;

namespace VideoGameCatalog
{
    public partial class MainWindow : Window
    {
        private DatabaseManager _db;
        private Plataforma _plataformaSeleccionada;
        private Juego _juegoSeleccionado;

        public MainWindow()
        {
            InitializeComponent();
            _db = new DatabaseManager();
            _db.InitializeDatabase();
            CargarPlataformas();
            CargarComboPlataformas();
        }

      
        private void CargarPlataformas()
        {
            try
            {
                List<Plataforma> plataformas = _db.ObtenerPlataformas();
                lbPlataformas.ItemsSource = null; 
                lbPlataformas.ItemsSource = plataformas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void CargarComboPlataformas()
        {
            try
            {
                List<Plataforma> plataformas = _db.ObtenerPlataformas();
                cbPlataformasJuego.ItemsSource = plataformas;
                cbPlataformasJuego.DisplayMemberPath = "Nombre";
                cbPlataformasJuego.SelectedValuePath = "Id";     
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private void LbPlataformas_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lbPlataformas.SelectedItem is Plataforma plataforma)
            {
                _plataformaSeleccionada = plataforma;
                CargarJuegosPorPlataforma(plataforma.Id);
                txtNombrePlataforma.Text = plataforma.Nombre;
                LimpiarFormularioJuego();
            }
        }

        private void BtnGuardarPlataforma_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nombre = txtNombrePlataforma.Text.Trim();

                if (string.IsNullOrEmpty(nombre))
                {
                    MessageBox.Show("Nombre obligatorio");
                    return;
                }

                if (_plataformaSeleccionada != null)
                {
                    _db.ActualizarPlataforma(_plataformaSeleccionada.Id, nombre);
                }

                else
                {
                    _db.InsertarPlataforma(nombre);
                }

                LimpiarFormularioPlataforma();
                CargarPlataformas();
                CargarComboPlataformas();
                MessageBox.Show("Guardado");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private void BtnEliminarPlataforma_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_plataformaSeleccionada == null)
                {
                    MessageBox.Show("Selecciona plataforma");
                    return;
                }


                MessageBoxResult result = MessageBox.Show(
                    $"¿Eliminar {_plataformaSeleccionada.Nombre}?",
                    "Confirmar",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    _db.EliminarPlataforma(_plataformaSeleccionada.Id);
                    LimpiarFormularioPlataforma();
                    CargarPlataformas();
                    CargarComboPlataformas();
                    MessageBox.Show("Eliminado");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void BtnLimpiarPlataforma_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormularioPlataforma();
        }


        private void LimpiarFormularioPlataforma()
        {
            txtNombrePlataforma.Text = "";
            lbPlataformas.SelectedIndex = -1;
            _plataformaSeleccionada = null;
        }


        private void CargarJuegosPorPlataforma(int plataformaId)
        {
            try
            {
                List<Juego> juegos = _db.ObtenerJuegosPorPlataforma(plataformaId);
                lbJuegos.ItemsSource = null;
                lbJuegos.ItemsSource = juegos;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void LbJuegos_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lbJuegos.SelectedItem is Juego juego)
            {
                _juegoSeleccionado = juego;
                txtTituloJuego.Text = juego.Titulo;
                txtGeneroJuego.Text = juego.Genero;
                txtNotaJuego.Text = juego.Nota.ToString();
                cbPlataformasJuego.SelectedValue = juego.PlataformaId;
            }
        }


        private void BtnGuardarJuego_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string titulo = txtTituloJuego.Text.Trim();
                string genero = txtGeneroJuego.Text.Trim();
                string notaStr = txtNotaJuego.Text.Trim();

                if (string.IsNullOrEmpty(titulo) || string.IsNullOrEmpty(genero))
                {
                    MessageBox.Show("Título y Género obligatorios");
                    return;
                }

                if (!decimal.TryParse(notaStr, out decimal nota))
                {
                    MessageBox.Show("Nota debe ser número");
                    return;
                }

                if (cbPlataformasJuego.SelectedValue == null)
                {
                    MessageBox.Show("Selecciona plataforma");
                    return;
                }

                int plataformaId = (int)cbPlataformasJuego.SelectedValue;

                if (_juegoSeleccionado != null)
                {
                    _db.ActualizarJuego(_juegoSeleccionado.Id, titulo, genero, nota, plataformaId);
                }
                else
                {
                    _db.InsertarJuego(titulo, genero, nota, plataformaId);
                }

                LimpiarFormularioJuego();
                CargarJuegosPorPlataforma(_plataformaSeleccionada.Id);
                MessageBox.Show("Guardado");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void BtnEliminarJuego_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_juegoSeleccionado == null)
                {
                    MessageBox.Show("Selecciona juego");
                    return;
                }

                MessageBoxResult result = MessageBox.Show(
                    $"¿Eliminar {_juegoSeleccionado.Titulo}?",
                    "Confirmar",
                    MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    _db.EliminarJuego(_juegoSeleccionado.Id);
                    LimpiarFormularioJuego();
                    CargarJuegosPorPlataforma(_plataformaSeleccionada.Id);
                    MessageBox.Show("Eliminado");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }


        private void BtnLimpiarJuego_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFormularioJuego();
        }

        private void LimpiarFormularioJuego()
        {
            txtTituloJuego.Text = "";
            txtGeneroJuego.Text = "";
            txtNotaJuego.Text = "";
            cbPlataformasJuego.SelectedIndex = -1;
            lbJuegos.SelectedIndex = -1;
            _juegoSeleccionado = null;
        }
    }
}
