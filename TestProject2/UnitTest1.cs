using Modelo.Escuela;
using System;
using Xunit;

namespace TestProject2
{
    public class UnitTest1
    {
        [Theory]
        [InlineData(6.99, 6.99, 7.59, true)]
        public void Test1(float n1, float n2, float n3, bool resultadoEsperado)
        {
            // Preparar la clases calificación
            bool resReal;
            Calificacion calificacion = new Calificacion()
                { Nota1=n1, Nota2=n2, Nota3=n3 };
            float peso1 = 0.30f;
            float peso2 = 0.30f;
            float peso3 = 0.40f;
            float notaMin = 7.00f;
            // Ejecución
            resReal = calificacion.Aprueba(peso1,peso2,peso3,notaMin);
            // Verificación
            if (resultadoEsperado)
            {
                Assert.True(resReal);
            }
            else
            {
                Assert.False(resReal);
            }
        }
    }
}
