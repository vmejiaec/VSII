using Escenarios;
using Modelo.Escuela;
using Persistencia;
using Procesos;
using Simulacion;
using Xunit;

namespace XUnitTestEscuela
{
    public class Calificacion_EsValida
    {
        public Calificacion_EsValida()
        {
            // Código para crear el escenario 1
            Escenario01 escenario = new Escenario01();
            EscenarioControl control = new EscenarioControl();
            control.Grabar(escenario);
            // Código para crear las matrículas
            var datosMatrículas = new DatosMatrículas();
            datosMatrículas.Generar();
        }


        // Pruebas cualitativas
        [Theory]
        [InlineData(1, "Pedro Diseño" , false)]
        [InlineData(2, "Pedro AdminDB", true)]
        [InlineData(3, "Pedro LogProg", true)]
        [InlineData(4, "Pedro ProdDig", true)]
        [InlineData(5, "Pedro VideoMk", false)]
        [InlineData(6, "Maria Diseño" , false)]
        [InlineData(7, "Maria AdminDB", true)]
        [InlineData(8, "Maria LogProg", true)]
        [InlineData(9, "Maria VideoMk", true)]
        [InlineData(10, "Juan Diseño", true)]
        [InlineData(11, "Juan AdminDB", true)]
        [InlineData(12, "Juan LogProg", true)]
        [InlineData(13, "Juan ProgDig", false)]
        [InlineData(14, "Juan VideoMk", true)]

        public void Calificacion_Calculo_30_30_40(int califId, string estCurso, bool resEsperado)
        {
            bool resultado;
            using (var context = new EscuelaContext())
            {
                Configuracion config = context.configuracion.Find(1);
                config.PesoNota1 = 0.30f;
                config.PesoNota2 = 0.30f;
                config.PesoNota3 = 0.40f;
                context.SaveChanges();

                CalificacionProc opCalif = new CalificacionProc(context);
                resultado = opCalif.Aprobado(califId);
            }
            if (resEsperado)
                Assert.True(resultado, estCurso + " debe estar Aprobado");
            else
                Assert.False(resultado, estCurso + " debe estar Reprobado *");
        }

        [Theory]
        [InlineData(1, "Pedro Diseño", false)]
        [InlineData(2, "Pedro AdminDB", true)]
        [InlineData(3, "Pedro LogProg", true)]
        [InlineData(4, "Pedro ProdDig", true)]
        [InlineData(5, "Pedro VideoMk", false)]
        [InlineData(6, "Maria Diseño", true)]
        [InlineData(7, "Maria AdminDB", true)]
        [InlineData(8, "Maria LogProg", true)]
        [InlineData(9, "Maria VideoMk", true)]
        [InlineData(10, "Juan Diseño", true)]
        [InlineData(11, "Juan AdminDB", true)]
        [InlineData(12, "Juan LogProg", true)]
        [InlineData(13, "Juan ProgDig", false)]
        [InlineData(14, "Juan VideoMk", true)]
        public void Calificacion_Calculo_35_35_30(int califId, string estCurso, bool resEsperado)
        {
            bool resultado;
            using (var context = new EscuelaContext())
            {
                Configuracion config = context.configuracion.Find(1);
                config.PesoNota1 = 0.35f;
                config.PesoNota2 = 0.35f;
                config.PesoNota3 = 0.30f;
                context.SaveChanges();

                CalificacionProc opCalif = new CalificacionProc(context);
                resultado = opCalif.Aprobado(califId);
            }
            if (resEsperado)
                Assert.True(resultado, estCurso + " debe estar Aprobado");
            else
                Assert.False(resultado, estCurso + " debe estar Reprobado *");
        }

        // Pruebas cuantitativas
        [Theory]
        [InlineData(1, "Pedro Diseño", 5.26f)]
        [InlineData(2, "Pedro AdminDB", 7.27f)]
        [InlineData(3, "Pedro LogProg", 8.57f)]
        [InlineData(4, "Pedro ProdDig", 8.49f)]
        [InlineData(5, "Pedro VideoMk", 6.39f)]
        [InlineData(6, "Maria Diseño", 6.95f)]
        [InlineData(7, "Maria AdminDB", 7.48f)]
        [InlineData(8, "Maria LogProg", 7.84f)]
        [InlineData(9, "Maria VideoMk", 7.89f)]
        [InlineData(10, "Juan Diseño", 8.56f)]
        [InlineData(11, "Juan AdminDB", 7.94f)]
        [InlineData(12, "Juan LogProg", 7.05f)]
        [InlineData(13, "Juan ProgDig", 5.79f)]
        [InlineData(14, "Juan VideoMk", 8.34f)]

        public void Calificacion_Calculo_30_30_40_ValorNumerico(int califId, string estCurso, float resEsperado)
        {
            float resultado;
            using (var context = new EscuelaContext())
            {
                Configuracion config = context.configuracion.Find(1);
                config.PesoNota1 = 0.30f;
                config.PesoNota2 = 0.30f;
                config.PesoNota3 = 0.40f;
                context.SaveChanges();

                Calificacion calif = context.calificaciones.Find(califId);

                CalificacionProc opCalif = new CalificacionProc(context);
                resultado = opCalif.NotaFinal(calif);
            }
            Assert.True(resEsperado == resultado, " Esperado " + resEsperado + " != " + resultado + " - " + estCurso);
        }

        [Theory]
        [InlineData(1, "Pedro Diseño", 5.18f)]
        [InlineData(2, "Pedro AdminDB", 7.44f)]
        [InlineData(3, "Pedro LogProg", 8.38f)]
        [InlineData(4, "Pedro ProdDig", 8.49f)]
        [InlineData(5, "Pedro VideoMk", 6.04f)]
        [InlineData(6, "Maria Diseño", 7.00f)]
        [InlineData(7, "Maria AdminDB", 7.35f)]
        [InlineData(8, "Maria LogProg", 7.93f)]
        [InlineData(9, "Maria VideoMk", 7.79f)]
        [InlineData(10, "Juan Diseño", 8.53f)]
        [InlineData(11, "Juan AdminDB", 8.22f)]
        [InlineData(12, "Juan LogProg", 7.00f)]
        [InlineData(13, "Juan ProgDig", 5.84f)]
        [InlineData(14, "Juan VideoMk", 8.50f)]

        public void Calificacion_Calculo_35_35_30_ValorNumerico(int califId, string estCurso, float resEsperado)
        {
            float resultado;
            using (var context = new EscuelaContext())
            {
                Configuracion config = context.configuracion.Find(1);
                config.PesoNota1 = 0.35f;
                config.PesoNota2 = 0.35f;
                config.PesoNota3 = 0.30f;
                context.SaveChanges();

                Calificacion calif = context.calificaciones.Find(califId);

                CalificacionProc opCalif = new CalificacionProc(context);
                resultado = opCalif.NotaFinal(calif);
            }
            Assert.True(resEsperado == resultado, " Esperado " + resEsperado + " != " + resultado + " - " + estCurso);
        }
    }
}
