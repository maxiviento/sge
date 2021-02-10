using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SGE.Importador.Datos;

namespace SGE.Importador
{
    public partial class Form1 : Form
    {
        private DataSet ds;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void btnImportarTerciarios_Click(object sender, EventArgs e)
        {
            string path = String.Empty;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\temp";
            openFileDialog1.Filter = "Excel Files (*.xls;*.xlsx;)|*.xls;*.xlsx;";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    path = openFileDialog1.FileName;

                    lblArchivo.Text = "Archivo: " + path;

                    string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";

                    DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");

                    using (DbConnection connection = factory.CreateConnection())
                    {
                        connection.ConnectionString = connectionString;
                        connection.Open();

                        ds = new DataSet();

                        using (DbCommand command = connection.CreateCommand())
                        {
                            DbDataAdapter dbDataAdapter = factory.CreateDataAdapter();
                            command.CommandText = "SELECT * FROM [Sheet1$]";
                            dbDataAdapter.SelectCommand = command;
                            dbDataAdapter.Fill(ds);

                            dataGridView1.DataSource = ds.Tables[0];
                        }
                    }

                    btnImportarTerciarios.Enabled = false;
                    btnImportarUniversitarios.Enabled = false;
                    btnConfirmar.Enabled = true;
                    btnConfirmar.Text = "Confirmar [Terciarios]";
                    btnCancelar.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: No se pudo leer el archivo desde el disco. Error original: " + ex.Message);
                }
            }
        }

        private void btnImportarUniversitarios_Click(object sender, EventArgs e)
        {
            string path = String.Empty;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\temp";
            openFileDialog1.Filter = "Excel Files (*.xls;*.xlsx;)|*.xls;*.xlsx;";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    path = openFileDialog1.FileName;

                    lblArchivo.Text = "Archivo: " + path;

                    string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=YES;\"";

                    DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");

                    using (DbConnection connection = factory.CreateConnection())
                    {
                        connection.ConnectionString = connectionString;
                        connection.Open();

                        ds = new DataSet();

                        using (DbCommand command = connection.CreateCommand())
                        {
                            DbDataAdapter dbDataAdapter = factory.CreateDataAdapter();
                            command.CommandText = "SELECT * FROM [Sheet1$]";
                            dbDataAdapter.SelectCommand = command;
                            dbDataAdapter.Fill(ds);

                            dataGridView1.DataSource = ds.Tables[0];
                        }
                    }

                    btnImportarTerciarios.Enabled = false;
                    btnImportarUniversitarios.Enabled = false;
                    btnConfirmar.Enabled = true;
                    btnConfirmar.Text = "Confirmar [Universitarios]";
                    btnCancelar.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: No se pudo leer el archivo desde el disco. Error original: " + ex.Message);
                }
            }
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            btnCancelar.Enabled = false;
            btnConfirmar.Enabled = false;

            pBar1.Visible = true;
            // Set Minimum to 1 to represent the first file being copied.
            pBar1.Minimum = 1;
            // Set the initial value of the ProgressBar.
            pBar1.Value = 1;
            // Set the Step property to a value of 1 to represent each file being copied.
            pBar1.Step = 1;
           
            //Modelo.GE mdb = new Modelo.GE();
            Datos.GEContainer mdb = new Datos.GEContainer();

            // Create a writer and open the file:
            StreamWriter log;
            DateTime ahora = DateTime.Now;

            string logfile = "c:\\temp\\Imp_" + ahora.Year.ToString() + "_" + ahora.Month.ToString() + "_" + ahora.Day.ToString() + "_"
                + ahora.Hour.ToString() + "_" + ahora.Minute.ToString() + "_" + ahora.Second.ToString() + ".txt";

            if (!File.Exists(logfile))
            {
                log = new StreamWriter(logfile);
            }
            else
            {
                log = File.AppendText(logfile);
            }

            try
            {
                // Write to the file:
                log.WriteLine(ahora);

                int fila = 0;
                int id_ficha = 0;
                const int id_estado_ficha = 3; // BENEFICIARIO

                switch (btnConfirmar.Text)
                {
                    case "Confirmar [Terciarios]":
                        log.WriteLine("Importación de TERCIARIOS");
                        log.WriteLine();
                        // Set Maximum to the total number of files to copy.
                        pBar1.Maximum = ds.Tables[0].Rows.Count;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string nro_documento = row["Número de documento"].ToString();
                            string cod_sucursal = row["SUCbco"].ToString();
                            string email = row["Correo electrónico"].ToString();
                            string mensaje = String.Empty;

                            var fichas = mdb.T_FICHAS.ToList().Where(c => c.NUMERO_DOCUMENTO == nro_documento);

                            if (fichas != null)
                            {
                                bool existe = false;
                                
                                foreach (var ficha in fichas)
                                {
                                    id_ficha = ficha.ID_FICHA;

                                    if (ficha.TIPO_FICHA == 1)
                                    {
                                        existe = true;

                                        int id_beneficiario =
                                        (from b in mdb.T_BENEFICIARIOS where b.ID_FICHA == ficha.ID_FICHA select b.ID_BENEFICIARIO)
                                            .FirstOrDefault();

                                        if (id_beneficiario == 0)
                                        {
                                            var obj = mdb.T_FICHAS.FirstOrDefault(c => c.ID_FICHA == ficha.ID_FICHA);
                                            obj.ID_ESTADO_FICHA = id_estado_ficha;

                                            var beneficiario = new T_BENEFICIARIOS
                                            {
                                                ID_BENEFICIARIO = EFSequence.GetNextValue(),
                                                ID_FICHA = obj.ID_FICHA,
                                                ID_PROGRAMA = 1, // TERCIARIO
                                                ID_ESTADO = 2, // ACTIVO
                                                FEC_ALTA = DateTime.Now,
                                                EMAIL = email
                                            };

                                            mdb.T_BENEFICIARIOS.AddObject(beneficiario);

                                            var sucursal = mdb.T_TABLAS_BCO_CBA.FirstOrDefault(c => c.COD_BCO_CBA == cod_sucursal);

                                            if (sucursal != null)
                                            {
                                                var cuenta_banco = new T_CUENTAS_BANCO
                                                {
                                                    ID_CUENTA_BANCO = EFSequence.GetNextValue(),
                                                    ID_BENEFICIARIO = beneficiario.ID_BENEFICIARIO,
                                                    ID_SUCURSAL = sucursal.ID_TABLA_BCO_CBA
                                                };

                                                mdb.T_CUENTAS_BANCO.AddObject(cuenta_banco);

                                                mdb.SaveChanges();

                                                mensaje = "Registro actualizado.";
                                            }
                                            else
                                            {
                                                mensaje = "Registro NO actualizado porque no se encontró la Sucursal de Bancor con Id = " + cod_sucursal + ".";
                                            }
                                        }
                                        else
                                        {
                                            mensaje = "Registro NO actualizado porque ya existe un Beneficiario con ese documento.";
                                        }
                                        break;
                                    }
                                }

                                if (!existe)
                                {
                                    mensaje = "Registro NO actualizado porque el Tipo de Ficha no corresponde a Terciario.";
                                }
                            }
                            else
                            {
                                mensaje = "Registro NO actualizado porque no se encontró la FICHA con ese DNI.";
                            }


                            log.WriteLine((++fila).ToString() + " := id_ficha: " + id_ficha.ToString() + "; nro_documento: " + nro_documento.ToString() + "; " + mensaje);
                            log.WriteLine();

                            // Perform the increment on the ProgressBar.
                            pBar1.PerformStep();
                        }
                        break;
                    case "Confirmar [Universitarios]":
                        log.WriteLine("Importación de UNIVERSITARIOS");
                        log.WriteLine();
                        // Set Maximum to the total number of files to copy.
                        pBar1.Maximum = ds.Tables[0].Rows.Count;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string nro_documento = row["Número de documento"].ToString();
                            string cod_sucursal = row["SUCbco3"].ToString();
                            string email = row["Correo electrónico"].ToString();
                            string mensaje = String.Empty;

                            var fichas = mdb.T_FICHAS.ToList().Where(c => c.NUMERO_DOCUMENTO == nro_documento);

                            if (fichas != null)
                            {
                                bool existe = false;

                                foreach (var ficha in fichas)
                                {
                                    id_ficha = ficha.ID_FICHA;

                                    if (ficha.TIPO_FICHA == 2)
                                    {
                                        existe = true;

                                        int id_beneficiario =
                                        (from b in mdb.T_BENEFICIARIOS where b.ID_FICHA == ficha.ID_FICHA select b.ID_BENEFICIARIO)
                                            .FirstOrDefault();

                                        if (id_beneficiario == 0)
                                        {
                                            var obj = mdb.T_FICHAS.FirstOrDefault(c => c.ID_FICHA == ficha.ID_FICHA);
                                            obj.ID_ESTADO_FICHA = id_estado_ficha;

                                            var beneficiario = new T_BENEFICIARIOS
                                            {
                                                ID_BENEFICIARIO = EFSequence.GetNextValue(),
                                                ID_FICHA = obj.ID_FICHA,
                                                ID_PROGRAMA = 2, // UNIVERSITARIO
                                                ID_ESTADO = 2, // ACTIVO
                                                FEC_ALTA = DateTime.Now,
                                                EMAIL = email
                                            };

                                            mdb.T_BENEFICIARIOS.AddObject(beneficiario);

                                            var sucursal = mdb.T_TABLAS_BCO_CBA.FirstOrDefault(c => c.COD_BCO_CBA == cod_sucursal);

                                            if (sucursal != null)
                                            {
                                                var cuenta_banco = new T_CUENTAS_BANCO
                                                {
                                                    ID_CUENTA_BANCO = EFSequence.GetNextValue(),
                                                    ID_BENEFICIARIO = beneficiario.ID_BENEFICIARIO,
                                                    ID_SUCURSAL = sucursal.ID_TABLA_BCO_CBA
                                                };

                                                mdb.T_CUENTAS_BANCO.AddObject(cuenta_banco);

                                                mdb.SaveChanges();

                                                mensaje = "Registro actualizado.";
                                            }
                                            else
                                            {
                                                mensaje = "Registro NO actualizado porque no se encontró la Sucursal de Bancor con Id = " + cod_sucursal + ".";
                                            }
                                        }
                                        else
                                        {
                                            mensaje = "Registro NO actualizado porque ya existe un Beneficiario con ese documento.";
                                        }
                                        break;
                                    }
                                }

                                if (!existe)
                                {
                                    mensaje = "Registro NO actualizado porque el Tipo de Ficha no corresponde a Universitario.";
                                }
                            }
                            else
                            {
                                mensaje = "Registro NO actualizado porque no se encontró la FICHA con ese DNI.";
                            }


                            log.WriteLine((++fila).ToString() + " := id_ficha: " + id_ficha.ToString() + "; nro_documento: " + nro_documento.ToString() + "; " + mensaje);
                            log.WriteLine();

                            // Perform the increment on the ProgressBar.
                            pBar1.PerformStep();
                        }
                        break;
                    default:
                        log.WriteLine("Tipo de Ficha no válido.");
                        log.WriteLine();
                        break;
                }

                MessageBox.Show("La importación se realizó correctamente, verifique el archivo: " + logfile + ", para mayor información.");
            }
            catch (Exception exc)
            {
                log.WriteLine("Mensaje de Error: " + exc.Message);
                log.WriteLine();
                log.WriteLine("Inner del Error: " + exc.InnerException);
                log.WriteLine();

                MessageBox.Show("Error: " + exc.Message);
            }
            finally
            {
                dataGridView1.DataSource = null;

                // Close the stream:
                log.Close();

                pBar1.Visible = false;
            }

            btnImportarTerciarios.Enabled = true;
            btnImportarUniversitarios.Enabled = true;
            btnConfirmar.Enabled = false;
            btnConfirmar.Text = "Confirmar";
            btnCancelar.Enabled = false;
            lblArchivo.Text = "Archivo: ";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            btnImportarTerciarios.Enabled = true;
            btnImportarUniversitarios.Enabled = true;
            btnConfirmar.Enabled = false;
            btnConfirmar.Text = "Confirmar";
            btnCancelar.Enabled = false;
            lblArchivo.Text = "Archivo: ";

            dataGridView1.DataSource = null;
        }
    }
}