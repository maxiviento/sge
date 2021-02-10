namespace SGE.Importador
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnImportarTerciarios = new System.Windows.Forms.Button();
            this.btnImportarUniversitarios = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnConfirmar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.lblArchivo = new System.Windows.Forms.Label();
            this.pBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnImportarTerciarios
            // 
            this.btnImportarTerciarios.Location = new System.Drawing.Point(13, 12);
            this.btnImportarTerciarios.Name = "btnImportarTerciarios";
            this.btnImportarTerciarios.Size = new System.Drawing.Size(200, 55);
            this.btnImportarTerciarios.TabIndex = 0;
            this.btnImportarTerciarios.Text = "Importar Beneficiarios Terciarios";
            this.btnImportarTerciarios.UseVisualStyleBackColor = true;
            this.btnImportarTerciarios.Click += new System.EventHandler(this.btnImportarTerciarios_Click);
            // 
            // btnImportarUniversitarios
            // 
            this.btnImportarUniversitarios.Location = new System.Drawing.Point(265, 12);
            this.btnImportarUniversitarios.Name = "btnImportarUniversitarios";
            this.btnImportarUniversitarios.Size = new System.Drawing.Size(200, 55);
            this.btnImportarUniversitarios.TabIndex = 1;
            this.btnImportarUniversitarios.Text = "Importar Beneficiarios Universitarios";
            this.btnImportarUniversitarios.UseVisualStyleBackColor = true;
            this.btnImportarUniversitarios.Click += new System.EventHandler(this.btnImportarUniversitarios_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(13, 94);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(949, 429);
            this.dataGridView1.TabIndex = 2;
            // 
            // btnConfirmar
            // 
            this.btnConfirmar.Enabled = false;
            this.btnConfirmar.Location = new System.Drawing.Point(513, 12);
            this.btnConfirmar.Name = "btnConfirmar";
            this.btnConfirmar.Size = new System.Drawing.Size(200, 55);
            this.btnConfirmar.TabIndex = 3;
            this.btnConfirmar.Text = "Confirmar";
            this.btnConfirmar.UseVisualStyleBackColor = true;
            this.btnConfirmar.Click += new System.EventHandler(this.btnConfirmar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Enabled = false;
            this.btnCancelar.Location = new System.Drawing.Point(756, 12);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(200, 55);
            this.btnCancelar.TabIndex = 4;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // lblArchivo
            // 
            this.lblArchivo.AutoSize = true;
            this.lblArchivo.Location = new System.Drawing.Point(13, 73);
            this.lblArchivo.Name = "lblArchivo";
            this.lblArchivo.Size = new System.Drawing.Size(52, 13);
            this.lblArchivo.TabIndex = 5;
            this.lblArchivo.Text = "Archivo:  ";
            // 
            // pBar1
            // 
            this.pBar1.Location = new System.Drawing.Point(460, 73);
            this.pBar1.Name = "pBar1";
            this.pBar1.Size = new System.Drawing.Size(496, 15);
            this.pBar1.TabIndex = 6;
            this.pBar1.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(968, 535);
            this.Controls.Add(this.pBar1);
            this.Controls.Add(this.lblArchivo);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnConfirmar);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnImportarUniversitarios);
            this.Controls.Add(this.btnImportarTerciarios);
            this.Name = "Form1";
            this.Text = "Importador";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnImportarTerciarios;
        private System.Windows.Forms.Button btnImportarUniversitarios;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnConfirmar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Label lblArchivo;
        private System.Windows.Forms.ProgressBar pBar1;
    }
}

