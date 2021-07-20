using Modelo.Escuela;
using Persistencia;
using Procesos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulacion
{
    public class DatosMatrículas
    {
        // Estados de la matrícula
        public enum MatriculaEstados { Pendiente, Aprobada, Rechazada };
        public void Generar()
        {   
            // Estudiante y carrera
            string estNombre = "Pedro Infante";
            string carNombre = "Comercio Electrónico";
            // Matrícula de segundo nivel
            Matricula matrPedroNivel2;
            DateTime dt2020_PAO2 = new DateTime(2020, 9, 1);
            string[] Nivel2cursos = new string[] {
                "Nivel 2 Diurna de Diseño Web",
                "Nivel 2 Diurno de Administración BBDD" };
            // Notas de segundo Nivel
            Dictionary<string, Calificacion> dicPedroCursosCalifsNivel2 = new()
            {
                {
                    Nivel2cursos[0],
                    new Calificacion() { Nota1 = 5.55f, Nota2 = 4.34f, Nota3 = 5.74f }
                },
                {
                    Nivel2cursos[1],
                    new Calificacion() { Nota1 = 8.78f, Nota2 = 7.12f, Nota3 = 6.25f }
                }
            };
            // Matrícula de tercer nivel
            Matricula matrPedroNivel3;
            DateTime dt2021_PAO1 = new DateTime(2021, 4, 1);
            string[] Nivel3cursos = new string[] {
                "Nivel 3 Diurno de Lógica de Programación",
                "Nivel 3 Diurno de Productos Digitales",
                "Nivel 3 Diurno de Video Marketing" };
            // Notas de tercer Nivel
            Dictionary<string, Calificacion> dicPedroCursosCalifsNivel3 = new()
            {
                {
                    Nivel3cursos[0],
                    new Calificacion() { Nota1 = 6.65f, Nota2 = 8.94f, Nota3 = 9.74f }
                },
                {
                    Nivel3cursos[1],
                    new Calificacion() { Nota1 = 7.84f, Nota2 = 9.12f, Nota3 = 8.50f }
                },
                {
                    Nivel3cursos[2],
                    new Calificacion() { Nota1 = 4.84f, Nota2 = 5.12f, Nota3 = 8.50f }
                }
            };
            //----------------------------------------------------------------------------------------------
            // Matrícula de cuarto nivel
            Matricula matrPedroNivel4;
            DateTime dt2021_PAO2 = new DateTime(2021, 9, 1);
            string[] Nivel4cursos = new string[] {
                "Nivel 4 Diurno de Programación Web",
                "Nivel 4 Diurno de E-Learning"};
            //----------------------------------------------------------------------------------------------
            // Persistencia de Pedro
            using (var db = new EscuelaContext())
            {
                matrPedroNivel2 = MatriculaProc.CreaMatricula(db,
                    MatriculaEstados.Aprobada.ToString(), estNombre, carNombre, dt2020_PAO2, Nivel2cursos) ;
                MatriculaProc.RegistrarNotas(matrPedroNivel2, dicPedroCursosCalifsNivel2);
                matrPedroNivel3 = MatriculaProc.CreaMatricula(db,
                    MatriculaEstados.Aprobada.ToString(), estNombre, carNombre, dt2021_PAO1, Nivel3cursos);
                MatriculaProc.RegistrarNotas(matrPedroNivel3, dicPedroCursosCalifsNivel3);
                matrPedroNivel4 = MatriculaProc.CreaMatricula(db,
                    MatriculaEstados.Pendiente.ToString(), estNombre, carNombre, dt2021_PAO2, Nivel4cursos);
                db.matriculas.Add(matrPedroNivel2);
                db.matriculas.Add(matrPedroNivel3);
                db.matriculas.Add(matrPedroNivel4);
                db.SaveChanges();
            }
            //----------------------------------------------------------------------------------------------

            // Matriculas de Maria
            estNombre = "María Brito";
            // Matrícula de segundo nivel
            Matricula matrMariaNivel2;
            Nivel2cursos = new string[] { "Nivel 2 Diurna de Diseño Web", "Nivel 2 Diurno de Administración BBDD" };
            Dictionary<string, Calificacion> dicMariaCursosCalifsNivel2 = new()
            {
                {
                    Nivel2cursos[0],
                    new Calificacion() { Nota1 = 7.02f, Nota2 = 7.24f, Nota3 = 6.69f }
                },
                {
                    Nivel2cursos[1],
                    new Calificacion() { Nota1 = 7.48f, Nota2 = 6.42f, Nota3 = 8.27f }
                }
            };
            // Matrícula de tercer nivel
            Matricula matrMariaNivel3;
            Nivel3cursos = new string[] { "Nivel 3 Diurno de Lógica de Programación", "Nivel 3 Diurno de Video Marketing" };
            // Notas de tercer Nivel
            Dictionary<string, Calificacion> dicMariaCursosCalifsNivel3 = new()
            {
                {
                    Nivel3cursos[0],
                    new Calificacion() { Nota1 = 9.12f, Nota2 = 7.33f, Nota3 = 7.25f }
                },
                {
                    Nivel3cursos[1],
                    new Calificacion() { Nota1 = 7.84f, Nota2 = 7.12f, Nota3 = 8.50f }
                }
            };
            // Matrícula de cuarto nivel
            Matricula matrMariaNivel4;
            Nivel4cursos = new string[] { "Nivel 4 Diurno de Programación Web", "Nivel 4 Diurno de E-Learning" };
            //----------------------------------------------------------------------------------------------
            // Persistencia de Maria
            using (var db = new EscuelaContext())
            {
                matrMariaNivel2 = MatriculaProc.CreaMatricula(db,
                    MatriculaEstados.Aprobada.ToString(), estNombre, carNombre, dt2020_PAO2, Nivel2cursos);
                MatriculaProc.RegistrarNotas(matrMariaNivel2, dicMariaCursosCalifsNivel2);
                matrMariaNivel3 = MatriculaProc.CreaMatricula(db,
                    MatriculaEstados.Aprobada.ToString(), estNombre, carNombre, dt2021_PAO1, Nivel3cursos);
                MatriculaProc.RegistrarNotas(matrMariaNivel3, dicMariaCursosCalifsNivel3);
                matrMariaNivel4 = MatriculaProc.CreaMatricula(db,
                    MatriculaEstados.Pendiente.ToString(), estNombre, carNombre, dt2021_PAO2, Nivel4cursos);
                db.matriculas.Add(matrMariaNivel2);
                db.matriculas.Add(matrMariaNivel3);
                db.matriculas.Add(matrMariaNivel4);
                db.SaveChanges();
            }
            //----------------------------------------------------------------------------------------------

            // Matriculas de Juan
            estNombre = "José Mera";
            // Matrícula de segundo nivel
            Matricula matrJuanNivel2;
            Nivel2cursos = new string[] { "Nivel 2 Diurna de Diseño Web", "Nivel 2 Diurno de Administración BBDD" };
            // Notas de tercer Nivel
            Dictionary<string, Calificacion> dicJuanCursosCalifsNivel2 = new()
            {
                {
                    Nivel2cursos[0],
                    new Calificacion() { Nota1 = 8.55f, Nota2 = 8.34f, Nota3 = 8.74f }
                },
                {
                    Nivel2cursos[1],
                    new Calificacion() { Nota1 = 8.78f, Nota2 = 9.33f, Nota3 = 6.27f }
                }
            };
            // Matrícula de tercer nivel
            Matricula matrJuanNivel3;
            Nivel3cursos = new string[] { "Nivel 3 Diurno de Lógica de Programación", "Nivel 3 Diurno de Productos Digitales", "Nivel 3 Diurno de Video Marketing" };
            // Notas de tercer Nivel
            Dictionary<string, Calificacion> dicJuanCursosCalifsNivel3 = new()
            {
                {
                    Nivel3cursos[0],
                    new Calificacion() { Nota1 = 6.89f, Nota2 = 6.82f, Nota3 = 7.34f }
                },
                {
                    Nivel3cursos[1],
                    new Calificacion() { Nota1 = 7.84f, Nota2 = 4.12f, Nota3 = 5.50f }
                },
                {
                    Nivel3cursos[2],
                    new Calificacion() { Nota1 = 9.84f, Nota2 = 8.12f, Nota3 = 7.37f }
                }
            };
            // Matrícula de cuarto nivel
            Matricula matrJuanNivel4;
            Nivel4cursos = new string[] { "Nivel 4 Diurno de Programación Web", "Nivel 4 Diurno de E-Learning" };
            //----------------------------------------------------------------------------------------------
            // Persistencia de Juan
            using (var db = new EscuelaContext())
            {
                matrJuanNivel2 = MatriculaProc.CreaMatricula(db,
                    MatriculaEstados.Aprobada.ToString(), estNombre, carNombre, dt2020_PAO2, Nivel2cursos);
                MatriculaProc.RegistrarNotas(matrJuanNivel2, dicJuanCursosCalifsNivel2);
                matrJuanNivel3 = MatriculaProc.CreaMatricula(db,
                    MatriculaEstados.Aprobada.ToString(), estNombre, carNombre, dt2021_PAO1, Nivel3cursos);
                MatriculaProc.RegistrarNotas(matrJuanNivel3, dicJuanCursosCalifsNivel3);
                matrJuanNivel4 = MatriculaProc.CreaMatricula(db,
                    MatriculaEstados.Pendiente.ToString(), estNombre, carNombre, dt2021_PAO2, Nivel4cursos);
                db.matriculas.Add(matrJuanNivel2);
                db.matriculas.Add(matrJuanNivel3);
                db.matriculas.Add(matrJuanNivel4);
                db.SaveChanges();
            }
        }
    }
}
