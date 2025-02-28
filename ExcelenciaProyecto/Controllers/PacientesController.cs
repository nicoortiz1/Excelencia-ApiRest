using ExcelenciaProyecto.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using MySql.Data.MySqlClient; // Usar MySQL

namespace ExcelenciaProyecto.Controllers
{
    public class PacientesController : Controller
    {
        private static string conexion = ConfigurationManager.ConnectionStrings["cadena"].ToString();
        private static List<Pacientes> olista = new List<Pacientes>();

        // GET: Pacientes
        public ActionResult Inicio()
        {
            olista.Clear();

            using (MySqlConnection oconexion = new MySqlConnection(conexion))
            {
                try
                {
                    oconexion.Open();
                    Console.WriteLine("✅ Conexión exitosa a la base de datos");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("❌ Error de conexión: " + ex.Message);
                }
            }


            using (MySqlConnection oconexion = new MySqlConnection(conexion))
            {
                string query = "SELECT * FROM pacientes";
                MySqlCommand cmd = new MySqlCommand(query, oconexion);

                oconexion.Open();

                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Pacientes oPaciente = new Pacientes
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Nombre = dr["Nombre"].ToString(),
                            Apellido = dr["Apellido"].ToString(),
                            DNI = dr["Dni"].ToString(),
                            FechaNacimiento = Convert.ToDateTime(dr["Fecha_nacimiento"]),
                            Telefono = dr["Telefono"].ToString()
                        };

                        olista.Add(oPaciente);
                    }
                }
            }

            return View(olista);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Pacientes pacientes)
        {
            if (!ModelState.IsValid)
            {
                return View(pacientes);
            }

            try
            {
                int filasAfectadas = 0; // Variable para verificar el guardado

                using (MySqlConnection oconexion = new MySqlConnection(conexion))
                {
                    string query = @"INSERT INTO pacientes 
                             (Nombre, Apellido, Dni, FechaNacimiento, Telefono, Email, Direccion) 
                             VALUES 
                             (@Nombre, @Apellido, @Dni, @Fecha_nacimiento, @Telefono, @Email, @Direccion)";

                    MySqlCommand cmd = new MySqlCommand(query, oconexion);

                    cmd.Parameters.AddWithValue("@Nombre", pacientes.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", pacientes.Apellido);
                    cmd.Parameters.AddWithValue("@Dni", pacientes.DNI);
                    cmd.Parameters.AddWithValue("@Fecha_nacimiento", pacientes.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Telefono", pacientes.Telefono);
                    cmd.Parameters.AddWithValue("@Email", pacientes.Email ?? string.Empty);
                    cmd.Parameters.AddWithValue("@Direccion", pacientes.Direccion ?? string.Empty);

                    oconexion.Open();
                    filasAfectadas = cmd.ExecuteNonQuery(); // Guardamos el número de filas afectadas
                }

                if (filasAfectadas > 0)
                {
                    TempData["MensajeExito"] = "Paciente registrado correctamente.";
                }
                else
                {
                    TempData["MensajeError"] = "No se pudo registrar el paciente.";
                }

                return RedirectToAction("Inicio");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al registrar el paciente: " + ex.Message);
                return View(pacientes);
            }
        }


    }
}
