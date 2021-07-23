using Escenarios;
using Modelo.Escuela;
using Procesos;
using Simulacion;
using System;
using Xunit;

namespace TestMatriculas
{
    public class Calificaciones
    {
        public Calificaciones()
        {
            // Código para crear el escenario 1
            Escenario01 escenario = new Escenario01();
            EscenarioControl control = new EscenarioControl();
            control.Grabar(escenario);
            // Código para crear las matrículas
            var datosMatrículas = new DatosMatrículas();
            datosMatrículas.Generar();
        }

        [Theory]
        [InlineData("Pedro Infante", false)]
        [InlineData("José Mera", false)]
        [InlineData("María Brito", false)]
        [InlineData("Karla Castro", true)]
        public void VerificarMatriculas(string strEstudiante, bool resEsperado)
        {
            // Preparación
            bool resReal;
            // Ejecución
            resReal = MatriculaProc.ConsultaYValidaMatriculaPendiente(strEstudiante);
            // Validación
            if (resEsperado)
            {
                Assert.True(resReal);
            }else
            {
                Assert.False(resReal);
            }
        }        
    }
}
