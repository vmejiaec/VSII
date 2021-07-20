using Persistencia;
using System.Linq;
using System;
using Modelo.Escuela;
using Escenarios;
using Reportes;
using Procesos;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Simulacion
{
    class Program
    {
        static void Main(string[] args)
        {
            // Código para crear el escenario 1
            Escenario01 escenario = new Escenario01();
            EscenarioControl control = new EscenarioControl();
            control.Grabar(escenario);
            // Código para crear las matrículas
            var datosMatrículas = new DatosMatrículas();
            datosMatrículas.Generar();
            // Regla del negocio
            // 1. Listar las matrículas pendientes  
            // 2. Para cada matrícula pendiente, analizamos sus materias
            // 3. Para cada materia presente en los detalles de la matícula, averiguamos sus prerequisitos
            // 4. Para cada materia presente en los prerequisaitos, averiguamos si ha sido aprobada previamente

            // Listar las matrículas pendientes  
            using (var db = new EscuelaContext())
            {
                var listaMatriculas = db.matriculas
                    .Include(matr => matr.Estudiante)
                    .Include(matr => matr.Matricula_Dets)
                        .ThenInclude(det => det.Curso)
                            .ThenInclude(cur => cur.Materia)
                                .ThenInclude(mat => mat.Malla)
                                    .ThenInclude(malla => malla.PreRequisitos)
                                        .ThenInclude(pre => pre.Materia)
                    .Where(matr => matr.Estado == "Pendiente")
                    .ToList();
                // Revisamos las matrículas
                foreach(var matricula in listaMatriculas)
                {
                    Console.WriteLine("Matrícula ID:" +matricula.MatriculaId + " Estudiante:" + matricula.Estudiante.Nombre);
                    foreach(var det in matricula.Matricula_Dets)
                    {
                        Console.WriteLine("\tCurso: "+det.Curso.Nombre);
                        Console.WriteLine("\t  Materia: " + det.Curso.Materia.Nombre);
                        Console.WriteLine("\t  Lista de Prerequisitos");
                        foreach(var pre in det.Curso.Materia.Malla.PreRequisitos)
                        {
                            Console.WriteLine("  "+pre.Materia.Nombre);
                        }
                    }
                }
            }
        }


    }
}
