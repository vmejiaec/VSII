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
                    bool MatriculaAprobada = true;
                    foreach(var det in matricula.Matricula_Dets)
                    {
                        var materia = det.Curso.Materia;
                        // Verificamos si la materia tiene prerequisitos
                        if (materia.Malla.PreRequisitos.Count == 0)
                        {
                            // Esta materia no tiene problemas
                            continue;
                        }
                        foreach(var pre in det.Curso.Materia.Malla.PreRequisitos)
                        {
                            var materiaPre = pre.Materia;
                            // Verificar si la materia de prerequisito ha sido aprobada
                            // por el estudiante en alguna matrícula anterior
                            if (!MateriaAprobada(matricula.Estudiante, materiaPre))
                            {
                                MatriculaAprobada = false;
                                continue;
                            }
                        }
                    }
                    matricula.Estado = MatriculaAprobada ? "Aprobada" : "Rechazada";
                }
            }
        }

        private static bool MateriaAprobada(Estudiante estudiante, Materia materiaPre)
        {
            throw new NotImplementedException();
        }
    }
}
