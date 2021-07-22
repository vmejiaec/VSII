using System;
using Xunit;

namespace TestProject1
{
    

    public class UnitTest1
    {
        [Theory]
        [InlineData(1, 1, 2, true)]
        [InlineData(2, 1, 3, true)]
        [InlineData(1, 2, 2, false)]
        [InlineData(2, 2, 5, false)]
        public void Test1(int a, int b,int c,  bool resultadoEsperado)
        {
            // Preparar la prueba
            bool resutadoObtenido = ConfirmaSuma(a, b,c);  // Error de programación
            // Confirmación
            if (resultadoEsperado)
            {
                Assert.True(resutadoObtenido);
            } 
            else
            {
                Assert.False(resutadoObtenido);
            }
        }

        bool ConfirmaSuma(int a, int b, int c)
        {
            if (a == 1) a = 0;
            int x = a + b - c;
            bool res = x == 0;
            return res;
        }
    }
}
