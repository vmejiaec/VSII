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
            // Matricula en estado pendiente, debe ser sometida a un proceso de validación
            // Consiste en verificar que cada matria presente en el curso del detalle de la matricula
            // tenga sus materias prerequisitos aprobadas. Caso contrario la matrícula pasa a estado rechazada
            // Consultar las matrículas pendientes
            using (var db = new EscuelaContext())
            {
                // Consulta a la configuración
                var configuracion = db.configuracion.Single();
                // Consulta de las matrículas pendientes
                var listaMatriculas = db.matriculas
                    .Include(matr => matr.Estudiante)
                    .Include(matr => matr.Matricula_Dets)
                        .ThenInclude(det => det.Curso)
                            .ThenInclude(cur => cur.Materia)
                                .ThenInclude(mat => mat.Malla)
                                    .ThenInclude(malla => malla.PreRequisitos)
                                        .ThenInclude(pre => pre.Materia)
                    .Where(matri => matri.Estado == "Pendiente")
                    .ToList();
                // Recorrer las instancias consultadas
                foreach(var matricula in listaMatriculas)
                {
                    bool MatriculaAprobada = true;
                    foreach (var det in matricula.Matricula_Dets)
                    {
                        var materia = det.Curso.Materia;
                        // Si la materia no tiene malla, entonces OK
                        if (materia.Malla is null) continue;
                        // Si la lista de prerequisitos está vacia entonces OK.
                        if (materia.Malla.PreRequisitos.Count == 0) continue;
                        // Verificación de prerequisitos
                        foreach (var prerequisito in materia.Malla.PreRequisitos)
                        {
                            var materiaPreReq = prerequisito.Materia;
                            // El estudiante habrá aprobado la materiaPreReq?
                            if(!MateriaAprobada(matricula.Estudiante, materiaPreReq, configuracion))
                            {
                                MatriculaAprobada = false;
                            }
                        }
                    }
                    matricula.Estado = MatriculaAprobada ? "Aprobada" : "Rechazada";
                    Console.WriteLine("Matrícula: "+matricula.MatriculaId + " "+matricula.Estado);
                }
            }
        }

        private static bool MateriaAprobada(Estudiante estudiante, Materia materia, Configuracion configuracion)
        {
            bool aprobada = false;
            float peso1 = configuracion.PesoNota1;
            float peso2 = configuracion.PesoNota2;
            float peso3 = configuracion.PesoNota3;
            float notaMin = configuracion.NotaMinima;
            // Consultar las matrículas del estudiante en estado Aprobadas
            using (var db = new EscuelaContext())
            {
                var listaMatriculas = db.matriculas
                    .Include(matr => matr.Matricula_Dets)
                        .ThenInclude(det => det.Calificacion)
                    .Include(matr => matr.Matricula_Dets.Where(det => det.Curso.MateriaId == materia.MateriaId))
                        .ThenInclude(det => det.Curso)
                            .ThenInclude(cur => cur.Materia)
                    .Where(matr => 
                        matr.EstudianteId == estudiante.EstudianteId && 
                        matr.Estado == "Aprobada"
                    )
                    .ToList();
                // Debbuger
                Console.WriteLine("-----------------------------------------------");
                Console.WriteLine(" " + estudiante.Nombre +" " + materia.Nombre);
                foreach(var matricula in listaMatriculas)
                {
                    Console.WriteLine("\tMatrícula ID:" + matricula.MatriculaId);
                    if (matricula.Matricula_Dets.Count == 0) 
                        Console.WriteLine("----> La matrícula no tiene detalles");
                    foreach(var det in matricula.Matricula_Dets)
                    {
                        var materiaPreReq = det.Curso.Materia;
                        Console.WriteLine("   \t" + materiaPreReq.Nombre + " " +
                            det.Calificacion.Nota1 + " " +
                            det.Calificacion.Nota2 + " " +
                            det.Calificacion.Nota3 + " " +
                            (det.Calificacion.Aprueba(peso1, peso2, peso3, notaMin) ? "Aprueba":"Reprueba")
                        );
                        if( det.Calificacion.Aprueba(peso1, peso2, peso3, notaMin))
                            aprobada = true;
                    }
                }
            }
            return aprobada;
        }
    }
}
