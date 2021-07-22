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
