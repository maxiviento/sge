using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SGE.Model.Comun;
using SGE.Model.Entidades;
using SGE.Model.Entidades.Interfaces;
using SGE.Model.Repositorio;
using SGE.Repositorio;

namespace SGE.Test
{
    [TestFixture]
    public class Test
    {

        [Test]
        public void GetControlDatos()
        {
            string asdasd = "   IMPUTADO                                ";

            int aaa = asdasd.Length;



            IControlDatosRepositorio obj = new ControlDatosRepositorio();

            IList<IControlDatos> ob = obj.GetBeneficiariosMonedaIncorrecta();


            Assert.IsTrue(ob.Count > 1);

            //Assert.IsTrue(a != null);

        }


        [Test]
        public void GetFichas()
        {
            IFichaRepositorio obj = new FichaRepositorio();

            IList<IFicha> objFichas = null;
                                                   

            Assert.IsTrue(objFichas.Count > 1);

            //Assert.IsTrue(a != null);

        }

        [Test]
        public void GetInstituciones()
        {
            IInstitucionRepositorio obj = new InstitucionRepositorio();

            IList<IInstitucion> objFichas = obj.GetInstitucionesBySectorProductivo(3);

            Assert.IsTrue(objFichas.Count > 1);

            //Assert.IsTrue(a != null);

        }


        [Test]
        public void GetArchivos()
        {

            var fechasolicitud = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));

            Assert.IsTrue(true);

            //Assert.IsTrue(a != null);

        }



          [Test]
        public void AgregarFR()
          {
              IFichaRechazoRepositorio obj = new FichaRechazoRepositorio();

              IFichaRechazoRepositorio obj2 = new FichaRechazoRepositorio();

              IFichaRechazo objfc = new FichaRechazo { IdFicha = 244081, IdTipoRechazo = 1 };

              IFichaRechazo objfc2 = new FichaRechazo { IdFicha = 244081, IdTipoRechazo = 1 };

              obj.DeleteFichaTipoRechazo(objfc);


              obj.AddFichaTipoRechazo(objfc2);

              

              Assert.IsTrue(true);

             //Assert.IsTrue(a != null);

        }

        

         [Test]
         public void Localidad()
         {
             ILocalidadRepositorio obj = new LocalidadRepositorio();

             IList<ILocalidad> lista = obj.GetSucursalesforLocalidades();

            
             


             Assert.IsTrue(true);

             //Assert.IsTrue(a != null);

         }


         [Test]
         public void GetLiquidacionExcluidos()
         {
             IBeneficiarioRepositorio obj = new BeneficiarioRepositorio();

             IList<IBeneficiario> objben = obj.GetBeneficiarioForLiquidacionExcluidos(1, 544, System.DateTime.Now);


             Assert.IsTrue(objben.Count > 1);


         }


    }
}
