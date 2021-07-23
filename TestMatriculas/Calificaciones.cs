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
            // C�digo para crear el escenario 1
            Escenario01 escenario = new Escenario01();
            EscenarioControl control = new EscenarioControl();
            control.Grabar(escenario);
            // C�digo para crear las matr�culas
            var datosMatr�culas = new DatosMatr�culas();
            datosMatr�culas.Generar();
        }

        [Theory]
        [InlineData("Pedro Infante", false)]
        [InlineData("Jos� Mera", false)]
        [InlineData("Mar�a Brito", false)]
        [InlineData("Karla Castro", true)]
        public void VerificarMatriculas(string strEstudiante, bool resEsperado)
        {
            // Preparaci�n
            bool resReal;
            // Ejecuci�n
            resReal = MatriculaProc.ConsultaYValidaMatriculaPendiente(strEstudiante);
            // Validaci�n
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
