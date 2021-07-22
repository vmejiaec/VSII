using Escenarios;
using Procesos;
using Simulacion;
using System;
using Xunit;

namespace xUnitTestMatriculas
{
    public class UnitTest1
    {
        public UnitTest1()
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
        public void Test1(string strEstudiante, bool resultadoEsperado)
        {            
            bool resultadoReal = MatriculaProc.ConsultaYValidaMatriculaPendiente(strEstudiante);
            if (resultadoEsperado)
            {
                Assert.True(resultadoReal, strEstudiante + "Mensaje 1 ");
            }
            else
            {
                Assert.False(resultadoReal, strEstudiante + "Mensaje 2");
            }
        }
    }
}
