using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SGE.Servicio.Servicio;
using SGE.Servicio.ServicioInterfaces;

namespace SGE.Test
{
    [TestFixture]
    public class TestServicio
    {

        [Test]
        public void GetFichas()
        {
            IFichaServicio obj = new FichaServicio();

            Assert.IsTrue(obj.GetFicha(521, "A").IdLocalidad > 1);
        }


        [Test]
        public void GetLiquidacion()
        {
            ILiquidacionServicio obj = new LiquidacionServicio();

            Assert.IsTrue(true);
        }

        [Test]
        public void GetBeneficiarios()
        {
            IBeneficiarioServicio obj = new BeneficiarioServicio();

            Assert.IsTrue(true);
        }



       

    }
}
