using Microsoft.EntityFrameworkCore;
using Modelo.Escuela;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Procesos
{
    public class MatriculaProc
    {
        public EscuelaContext _context;

        public MatriculaProc(EscuelaContext context)
        {
            _context = context;
        }

        // Consulta la matrícula pendiente del estudiante y la valida
        static public bool ConsultaYValidaMatriculaPendiente(string strEstudiante)
        {
            Matricula matricula;
            using (var db = new EscuelaContext())
            {
                matricula = db.matriculas
                    .Include(matr => matr.Estudiante)
                    .Single(matr => 
                        matr.Estudiante.Nombre == strEstudiante && 
                        matr.Estado == "Pendiente"
                    );
            }
            return MatriculaAprobada(matricula.MatriculaId);
        }

        // Registro de una matrícula
        static public Matricula CreaMatricula(
            EscuelaContext context, string estado,
            string estNombre, string carNombre, 
            DateTime periodoFechaInicio, string[] cursosNombres)
        {
            // 1.- Consulta del estudiante
            Estudiante estudiante = context.estudiantes
                .Single(est => est.Nombre == estNombre);
            // 2.- Consulta de la carrera
            Carrera carrera = context.carreras
                .Single(car => car.Nombre == carNombre);
            // 3.- Consulta del período
            Periodo periodo = context.periodos
                .Single(periodo => periodo.FechaInicio == periodoFechaInicio);
            // 4.- Cabecera de Matrícula
            Matricula matricula = new Matricula()
            {
                Estudiante = estudiante,
                Fecha = periodoFechaInicio, // Se matricula el mismo día que inicia el período
                Carrera = carrera,
                Estado = estado,
                Periodo = periodo
            };
            // 5.- Detalles de la Matrícula
            matricula.Matricula_Dets = new List<Matricula_Det>();
            foreach (var cursoNombre in cursosNombres)
            {
                Curso curso = context.cursos
                    .Single(cur => cur.Nombre == cursoNombre);
                Matricula_Det matricula_Det = new Matricula_Det()
                {
                    Matricula = matricula,
                    Curso = curso
                };
                matricula.Matricula_Dets.Add(matricula_Det);
            }

            return matricula;
        }

        // Registrar las calificaciones de un estudiante
        static public void RegistrarNotas(
            Matricula matricula, Dictionary<string, Calificacion> dicCursosCalifs)
        {
            // Buscar el curso para asignar las calificaciones
            foreach (var det in matricula.Matricula_Dets)
            {
                det.Calificacion = dicCursosCalifs[det.Curso.Nombre];
            }
        }

        public static bool MatriculaAprobada(int matriculaID)
        {
            bool aprobada = true;
            using (var db = new EscuelaContext())
            {
                // Consulta a la configuración
                var configuracion = db.configuracion.Single();
                // Consulta de las matrículas pendientes
                var matricula = db.matriculas
                    .Include(matr => matr.Estudiante)
                    .Include(matr => matr.Matricula_Dets)
                        .ThenInclude(det => det.Curso)
                            .ThenInclude(cur => cur.Materia)
                                .ThenInclude(mat => mat.Malla)
                                    .ThenInclude(malla => malla.PreRequisitos)
                                        .ThenInclude(pre => pre.Materia)
                    .Single(matri => matri.MatriculaId == matriculaID && matri.Estado == "Pendiente");
                // Revisa los prerequisitos
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
                        if (!MateriaAprobada(matricula.Estudiante, materiaPreReq, configuracion))
                        {
                            aprobada = false;
                        }
                    }
                }
            }
            return aprobada;
        }

        public static bool MateriaAprobada(Estudiante estudiante, Materia materia, Configuracion configuracion)
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
                foreach (var matricula in listaMatriculas)
                {
                    foreach (var det in matricula.Matricula_Dets)
                    {
                        var materiaPreReq = det.Curso.Materia;
                        if (det.Calificacion.Aprueba(peso1, peso2, peso3, notaMin)) aprobada = true;
                    }
                }
            }
            return aprobada;
        }

    }
}
