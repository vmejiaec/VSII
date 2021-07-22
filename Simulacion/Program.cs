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
            // Regla del negocio: validación de prerequisitos
            using (var db = new EscuelaContext())
            {
                var listaMatriculas = db.matriculas
                    .Include(matr => matr.Estudiante)
                    .Where(matr => matr.Estado=="Pendiente")
                    .ToList();
                foreach (var matricula in listaMatriculas)
                {
                    Console.WriteLine(
                        String.Format(
                            "  - {0} Matricula Id: {1} Estado: {2}",
                            matricula.Estudiante.Nombre,
                            matricula.MatriculaId,
                            MatriculaProc.MatriculaAprobada(matricula.MatriculaId)?"Aprobada":"Rechazada"
                        )
                    );
                }
            }
        }

        
    }
}
