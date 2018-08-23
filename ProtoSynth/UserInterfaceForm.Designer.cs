namespace ProtoSynth
{
    partial class UserInterfaceForm
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
            this.lblFrequency = new System.Windows.Forms.Label();
            this.txtFrequency = new System.Windows.Forms.TextBox();
            this.barVolume = new System.Windows.Forms.TrackBar();
            this.lblVolume = new System.Windows.Forms.Label();
            this.btnSineBuffer = new System.Windows.Forms.Button();
            this.boxWaveType = new System.Windows.Forms.ComboBox();
            this.lblWaveType = new System.Windows.Forms.Label();
            this.btnWrite = new System.Windows.Forms.Button();
            this.lblMulti = new System.Windows.Forms.Label();
            this.barMulti = new System.Windows.Forms.TrackBar();
            this.lblPhase = new System.Windows.Forms.Label();
            this.barPhase = new System.Windows.Forms.TrackBar();
            this.barAttack = new System.Windows.Forms.TrackBar();
            this.barDecay = new System.Windows.Forms.TrackBar();
            this.barSustain = new System.Windows.Forms.TrackBar();
            this.barRelease = new System.Windows.Forms.TrackBar();
            this.lblAttack = new System.Windows.Forms.Label();
            this.lblDecay = new System.Windows.Forms.Label();
            this.lblSustain = new System.Windows.Forms.Label();
            this.lblRelease = new System.Windows.Forms.Label();
            this.btnRetrigger = new System.Windows.Forms.Button();
            this.btnRelease = new System.Windows.Forms.Button();
            this.barDist = new System.Windows.Forms.TrackBar();
            this.lblDist = new System.Windows.Forms.Label();
            this.panWave = new System.Windows.Forms.Panel();
            this.dataNotes = new System.Windows.Forms.DataGridView();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSequence = new System.Windows.Forms.Button();
            this.chkTone = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.barVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barMulti)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barPhase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAttack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barDecay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barSustain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barRelease)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barDist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataNotes)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFrequency
            // 
            this.lblFrequency.AutoSize = true;
            this.lblFrequency.Location = new System.Drawing.Point(12, 17);
            this.lblFrequency.Name = "lblFrequency";
            this.lblFrequency.Size = new System.Drawing.Size(60, 13);
            this.lblFrequency.TabIndex = 1;
            this.lblFrequency.Text = "Frequency:";
            // 
            // txtFrequency
            // 
            this.txtFrequency.Location = new System.Drawing.Point(84, 12);
            this.txtFrequency.Name = "txtFrequency";
            this.txtFrequency.Size = new System.Drawing.Size(80, 20);
            this.txtFrequency.TabIndex = 2;
            this.txtFrequency.Text = "440";
            this.txtFrequency.TextChanged += new System.EventHandler(this.txtFrequency_TextChanged);
            // 
            // barVolume
            // 
            this.barVolume.Location = new System.Drawing.Point(431, 28);
            this.barVolume.Name = "barVolume";
            this.barVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.barVolume.Size = new System.Drawing.Size(45, 420);
            this.barVolume.TabIndex = 3;
            this.barVolume.Value = 1;
            this.barVolume.ValueChanged += new System.EventHandler(this.barVolume_ValueChanged);
            // 
            // lblVolume
            // 
            this.lblVolume.AutoSize = true;
            this.lblVolume.Location = new System.Drawing.Point(429, 12);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(42, 13);
            this.lblVolume.TabIndex = 4;
            this.lblVolume.Text = "Volume";
            // 
            // btnSineBuffer
            // 
            this.btnSineBuffer.Location = new System.Drawing.Point(15, 425);
            this.btnSineBuffer.Name = "btnSineBuffer";
            this.btnSineBuffer.Size = new System.Drawing.Size(75, 23);
            this.btnSineBuffer.TabIndex = 10;
            this.btnSineBuffer.Text = "Play";
            this.btnSineBuffer.UseVisualStyleBackColor = true;
            this.btnSineBuffer.Click += new System.EventHandler(this.btnSineBuffer_Click);
            // 
            // boxWaveType
            // 
            this.boxWaveType.FormattingEnabled = true;
            this.boxWaveType.Location = new System.Drawing.Point(84, 39);
            this.boxWaveType.Name = "boxWaveType";
            this.boxWaveType.Size = new System.Drawing.Size(137, 21);
            this.boxWaveType.TabIndex = 11;
            this.boxWaveType.Text = "Select a wave";
            this.boxWaveType.SelectedIndexChanged += new System.EventHandler(this.boxWaveType_SelectedIndexChanged);
            // 
            // lblWaveType
            // 
            this.lblWaveType.AutoSize = true;
            this.lblWaveType.Location = new System.Drawing.Point(12, 42);
            this.lblWaveType.Name = "lblWaveType";
            this.lblWaveType.Size = new System.Drawing.Size(66, 13);
            this.lblWaveType.TabIndex = 12;
            this.lblWaveType.Text = "Wave Type:";
            // 
            // btnWrite
            // 
            this.btnWrite.Location = new System.Drawing.Point(259, 425);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(75, 23);
            this.btnWrite.TabIndex = 13;
            this.btnWrite.Text = "Write";
            this.btnWrite.UseVisualStyleBackColor = true;
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // lblMulti
            // 
            this.lblMulti.AutoSize = true;
            this.lblMulti.Location = new System.Drawing.Point(12, 117);
            this.lblMulti.Name = "lblMulti";
            this.lblMulti.Size = new System.Drawing.Size(32, 13);
            this.lblMulti.TabIndex = 16;
            this.lblMulti.Text = "Multi:";
            // 
            // barMulti
            // 
            this.barMulti.LargeChange = 1;
            this.barMulti.Location = new System.Drawing.Point(58, 117);
            this.barMulti.Maximum = 100;
            this.barMulti.Name = "barMulti";
            this.barMulti.Size = new System.Drawing.Size(163, 45);
            this.barMulti.TabIndex = 1;
            this.barMulti.TickStyle = System.Windows.Forms.TickStyle.None;
            this.barMulti.Scroll += new System.EventHandler(this.barMulti_Scroll);
            // 
            // lblPhase
            // 
            this.lblPhase.AutoSize = true;
            this.lblPhase.Location = new System.Drawing.Point(12, 66);
            this.lblPhase.Name = "lblPhase";
            this.lblPhase.Size = new System.Drawing.Size(40, 13);
            this.lblPhase.TabIndex = 17;
            this.lblPhase.Text = "Phase:";
            // 
            // barPhase
            // 
            this.barPhase.LargeChange = 1;
            this.barPhase.Location = new System.Drawing.Point(58, 68);
            this.barPhase.Maximum = 100;
            this.barPhase.Name = "barPhase";
            this.barPhase.Size = new System.Drawing.Size(163, 45);
            this.barPhase.TabIndex = 100;
            this.barPhase.TickStyle = System.Windows.Forms.TickStyle.None;
            this.barPhase.Scroll += new System.EventHandler(this.barPhase_Scroll);
            // 
            // barAttack
            // 
            this.barAttack.LargeChange = 1;
            this.barAttack.Location = new System.Drawing.Point(227, 28);
            this.barAttack.Maximum = 100;
            this.barAttack.Name = "barAttack";
            this.barAttack.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.barAttack.Size = new System.Drawing.Size(45, 134);
            this.barAttack.TabIndex = 101;
            this.barAttack.TickStyle = System.Windows.Forms.TickStyle.None;
            this.barAttack.Scroll += new System.EventHandler(this.barAttack_Scroll);
            // 
            // barDecay
            // 
            this.barDecay.Location = new System.Drawing.Point(278, 28);
            this.barDecay.Maximum = 100;
            this.barDecay.Name = "barDecay";
            this.barDecay.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.barDecay.Size = new System.Drawing.Size(45, 134);
            this.barDecay.TabIndex = 101;
            this.barDecay.TickStyle = System.Windows.Forms.TickStyle.None;
            this.barDecay.Scroll += new System.EventHandler(this.barDecay_Scroll);
            // 
            // barSustain
            // 
            this.barSustain.Location = new System.Drawing.Point(329, 28);
            this.barSustain.Maximum = 100;
            this.barSustain.Name = "barSustain";
            this.barSustain.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.barSustain.Size = new System.Drawing.Size(45, 134);
            this.barSustain.TabIndex = 101;
            this.barSustain.TickStyle = System.Windows.Forms.TickStyle.None;
            this.barSustain.Value = 100;
            this.barSustain.Scroll += new System.EventHandler(this.barSustain_Scroll);
            // 
            // barRelease
            // 
            this.barRelease.Location = new System.Drawing.Point(380, 28);
            this.barRelease.Maximum = 100;
            this.barRelease.Name = "barRelease";
            this.barRelease.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.barRelease.Size = new System.Drawing.Size(45, 134);
            this.barRelease.TabIndex = 101;
            this.barRelease.TickStyle = System.Windows.Forms.TickStyle.None;
            this.barRelease.Scroll += new System.EventHandler(this.barRelease_Scroll);
            // 
            // lblAttack
            // 
            this.lblAttack.AutoSize = true;
            this.lblAttack.Location = new System.Drawing.Point(224, 12);
            this.lblAttack.Name = "lblAttack";
            this.lblAttack.Size = new System.Drawing.Size(38, 13);
            this.lblAttack.TabIndex = 102;
            this.lblAttack.Text = "Attack";
            // 
            // lblDecay
            // 
            this.lblDecay.AutoSize = true;
            this.lblDecay.Location = new System.Drawing.Point(275, 12);
            this.lblDecay.Name = "lblDecay";
            this.lblDecay.Size = new System.Drawing.Size(38, 13);
            this.lblDecay.TabIndex = 102;
            this.lblDecay.Text = "Decay";
            // 
            // lblSustain
            // 
            this.lblSustain.AutoSize = true;
            this.lblSustain.Location = new System.Drawing.Point(326, 12);
            this.lblSustain.Name = "lblSustain";
            this.lblSustain.Size = new System.Drawing.Size(42, 13);
            this.lblSustain.TabIndex = 102;
            this.lblSustain.Text = "Sustain";
            // 
            // lblRelease
            // 
            this.lblRelease.AutoSize = true;
            this.lblRelease.Location = new System.Drawing.Point(377, 12);
            this.lblRelease.Name = "lblRelease";
            this.lblRelease.Size = new System.Drawing.Size(46, 13);
            this.lblRelease.TabIndex = 102;
            this.lblRelease.Text = "Release";
            // 
            // btnRetrigger
            // 
            this.btnRetrigger.Location = new System.Drawing.Point(96, 425);
            this.btnRetrigger.Name = "btnRetrigger";
            this.btnRetrigger.Size = new System.Drawing.Size(75, 23);
            this.btnRetrigger.TabIndex = 13;
            this.btnRetrigger.Text = "Retrigger";
            this.btnRetrigger.UseVisualStyleBackColor = true;
            this.btnRetrigger.Click += new System.EventHandler(this.btnRetrigger_Click);
            // 
            // btnRelease
            // 
            this.btnRelease.Location = new System.Drawing.Point(178, 425);
            this.btnRelease.Name = "btnRelease";
            this.btnRelease.Size = new System.Drawing.Size(75, 23);
            this.btnRelease.TabIndex = 103;
            this.btnRelease.Text = "Release";
            this.btnRelease.UseVisualStyleBackColor = true;
            this.btnRelease.Click += new System.EventHandler(this.btnRelease_Click);
            // 
            // barDist
            // 
            this.barDist.LargeChange = 1;
            this.barDist.Location = new System.Drawing.Point(58, 168);
            this.barDist.Maximum = 100;
            this.barDist.Name = "barDist";
            this.barDist.Size = new System.Drawing.Size(163, 45);
            this.barDist.TabIndex = 1;
            this.barDist.TickStyle = System.Windows.Forms.TickStyle.None;
            this.barDist.Scroll += new System.EventHandler(this.barDist_Scroll);
            // 
            // lblDist
            // 
            this.lblDist.AutoSize = true;
            this.lblDist.Location = new System.Drawing.Point(12, 168);
            this.lblDist.Name = "lblDist";
            this.lblDist.Size = new System.Drawing.Size(28, 13);
            this.lblDist.TabIndex = 16;
            this.lblDist.Text = "Dist:";
            // 
            // panWave
            // 
            this.panWave.Location = new System.Drawing.Point(15, 219);
            this.panWave.Name = "panWave";
            this.panWave.Size = new System.Drawing.Size(390, 200);
            this.panWave.TabIndex = 104;
            this.panWave.Paint += new System.Windows.Forms.PaintEventHandler(this.panWave_Paint);
            // 
            // dataNotes
            // 
            this.dataNotes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataNotes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Value,
            this.Length});
            this.dataNotes.Location = new System.Drawing.Point(482, 12);
            this.dataNotes.Name = "dataNotes";
            this.dataNotes.Size = new System.Drawing.Size(173, 436);
            this.dataNotes.TabIndex = 105;
            // 
            // Value
            // 
            this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Value.HeaderText = "Value";
            this.Value.Name = "Value";
            this.Value.Width = 59;
            // 
            // Length
            // 
            this.Length.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Length.HeaderText = "Length";
            this.Length.Name = "Length";
            this.Length.Width = 65;
            // 
            // btnSequence
            // 
            this.btnSequence.Location = new System.Drawing.Point(661, 12);
            this.btnSequence.Name = "btnSequence";
            this.btnSequence.Size = new System.Drawing.Size(93, 23);
            this.btnSequence.TabIndex = 106;
            this.btnSequence.Text = "Play Sequence";
            this.btnSequence.UseVisualStyleBackColor = true;
            // 
            // chkTone
            // 
            this.chkTone.AutoSize = true;
            this.chkTone.Location = new System.Drawing.Point(170, 14);
            this.chkTone.Name = "chkTone";
            this.chkTone.Size = new System.Drawing.Size(51, 17);
            this.chkTone.TabIndex = 107;
            this.chkTone.Text = "Tone";
            this.chkTone.UseVisualStyleBackColor = true;
            this.chkTone.CheckedChanged += new System.EventHandler(this.chkTone_CheckedChanged);
            // 
            // UserInterfaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1046, 462);
            this.Controls.Add(this.chkTone);
            this.Controls.Add(this.btnSequence);
            this.Controls.Add(this.dataNotes);
            this.Controls.Add(this.panWave);
            this.Controls.Add(this.btnRelease);
            this.Controls.Add(this.lblRelease);
            this.Controls.Add(this.lblSustain);
            this.Controls.Add(this.lblDecay);
            this.Controls.Add(this.lblAttack);
            this.Controls.Add(this.barRelease);
            this.Controls.Add(this.barSustain);
            this.Controls.Add(this.barDecay);
            this.Controls.Add(this.barAttack);
            this.Controls.Add(this.barPhase);
            this.Controls.Add(this.lblPhase);
            this.Controls.Add(this.barDist);
            this.Controls.Add(this.barMulti);
            this.Controls.Add(this.lblDist);
            this.Controls.Add(this.lblMulti);
            this.Controls.Add(this.btnRetrigger);
            this.Controls.Add(this.btnWrite);
            this.Controls.Add(this.lblWaveType);
            this.Controls.Add(this.boxWaveType);
            this.Controls.Add(this.btnSineBuffer);
            this.Controls.Add(this.lblVolume);
            this.Controls.Add(this.barVolume);
            this.Controls.Add(this.txtFrequency);
            this.Controls.Add(this.lblFrequency);
            this.Name = "UserInterfaceForm";
            this.Text = "ProtoSynth";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UserInterfaceForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.barVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barMulti)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barPhase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAttack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barDecay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barSustain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barRelease)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barDist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataNotes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblFrequency;
        private System.Windows.Forms.TextBox txtFrequency;
        private System.Windows.Forms.TrackBar barVolume;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.Button btnSineBuffer;
        private System.Windows.Forms.ComboBox boxWaveType;
        private System.Windows.Forms.Label lblWaveType;
        private System.Windows.Forms.Button btnWrite;
        private System.Windows.Forms.Label lblMulti;
        private System.Windows.Forms.TrackBar barMulti;
        private System.Windows.Forms.Label lblPhase;
        private System.Windows.Forms.TrackBar barPhase;
        private System.Windows.Forms.TrackBar barAttack;
        private System.Windows.Forms.TrackBar barDecay;
        private System.Windows.Forms.TrackBar barSustain;
        private System.Windows.Forms.TrackBar barRelease;
        private System.Windows.Forms.Label lblAttack;
        private System.Windows.Forms.Label lblDecay;
        private System.Windows.Forms.Label lblSustain;
        private System.Windows.Forms.Label lblRelease;
        private System.Windows.Forms.Button btnRetrigger;
        private System.Windows.Forms.Button btnRelease;
        private System.Windows.Forms.TrackBar barDist;
        private System.Windows.Forms.Label lblDist;
        private System.Windows.Forms.Panel panWave;
        private System.Windows.Forms.DataGridView dataNotes;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn Length;
        private System.Windows.Forms.Button btnSequence;
        private System.Windows.Forms.CheckBox chkTone;
    }
}

